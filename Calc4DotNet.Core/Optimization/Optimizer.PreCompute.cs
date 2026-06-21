using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization;

public static partial class Optimizer
{
    private sealed class PreComputeSession<TNumber>
        where TNumber : INumber<TNumber>
    {
        public Dictionary<SpecializationCacheKey<TNumber>, SpecializationCacheEntry<TNumber>?> SpecializationCache { get; } = [];
        public Dictionary<SpecializationCacheKey<TNumber>, LetInlineCacheEntry<TNumber>> LetInlineCache { get; } = [];
        public Dictionary<IOperator, ImmutableArray<int>> ExistingLetLocalIndicesCache { get; } = new(ReferenceEqualityComparer.Instance);
    }

    private readonly record struct SpecializationBinding<TNumber>(int Index, TNumber Value)
        where TNumber : INumber<TNumber>;

    private sealed record SpecializationCacheEntry<TNumber>(SpecializationResult<TNumber> Specialization,
                                                            ImmutableArray<int> GeneratedLocalIndices)
        where TNumber : INumber<TNumber>;

    private sealed record LetInlineCacheEntry<TNumber>(SpecializationResult<TNumber> Specialization,
                                                       ImmutableDictionary<int, int> UsageCounts,
                                                       ImmutableArray<int> GeneratedLocalIndices)
        where TNumber : INumber<TNumber>;

    private readonly struct SpecializationCacheKey<TNumber> : IEquatable<SpecializationCacheKey<TNumber>>
        where TNumber : INumber<TNumber>
    {
        private readonly string operatorName;
        private readonly IOperator operatorBody;
        private readonly ImmutableArray<SpecializationBinding<TNumber>> bindings;
        private readonly int hashCode;

        public SpecializationCacheKey(string operatorName,
                                      IOperator operatorBody,
                                      ImmutableArray<SpecializationBinding<TNumber>> bindings)
        {
            this.operatorName = operatorName;
            this.operatorBody = operatorBody;
            this.bindings = bindings;

            HashCode hash = new();
            hash.Add(operatorName);
            hash.Add(RuntimeHelpers.GetHashCode(operatorBody));
            foreach (var binding in bindings)
            {
                hash.Add(binding);
            }

            hashCode = hash.ToHashCode();
        }

        public bool Equals(SpecializationCacheKey<TNumber> other)
        {
            if (operatorName != other.operatorName
                || !ReferenceEquals(operatorBody, other.operatorBody)
                || bindings.Length != other.bindings.Length)
            {
                return false;
            }

            for (int i = 0; i < bindings.Length; i++)
            {
                if (!bindings[i].Equals(other.bindings[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            return obj is SpecializationCacheKey<TNumber> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }

    private sealed class PreComputeVisitor<TNumber> : IOperatorVisitor<PartialEvaluationResult<TNumber>, PreComputeFrame<TNumber>>
        where TNumber : INumber<TNumber>
    {
        private const int NoTemporaryLetLocalIndex = 0;
        private readonly CompilationContext compilationContext;
        private readonly ImmutableDictionary<string, PotentialEffects> effectsByOperators;
        private readonly PreComputeSession<TNumber> session;

        // Names of operators currently being specialized. Used to break infinite chains
        // when a recursive operator's body calls itself with a different binding pattern.
        // Without this guard, specializing op(5, 1, 0) whose body calls op(x-1, 1, 1) would
        // recursively specialize op again, and so on indefinitely for recursions whose
        // argument values drift (e.g., fibImpl's accumulators).
        private readonly HashSet<string> specializationInProgress = [];

        private int nextLetLocalIndex;

        public PreComputeVisitor(CompilationContext context,
                                 ImmutableDictionary<string, PotentialEffects> effects,
                                 PreComputeSession<TNumber> session,
                                 int nextLetLocalIndex)
        {
            compilationContext = context ?? throw new ArgumentNullException(nameof(context));
            effectsByOperators = effects ?? throw new ArgumentNullException(nameof(effects));
            this.session = session ?? throw new ArgumentNullException(nameof(session));
            this.nextLetLocalIndex = nextLetLocalIndex;
        }

        public int NextLetLocalIndex => nextLetLocalIndex;

        public PartialEvaluationResult<TNumber> Evaluate(IOperator op, PreComputeFrame<TNumber> frame)
        {
            return op.Accept(this, frame);
        }

        public PartialEvaluationResult<TNumber> Visit(ZeroOperator op, PreComputeFrame<TNumber> frame)
        {
            return Constant(new PreComputedOperator(TNumber.Zero), frame.State, TNumber.Zero);
        }

        public PartialEvaluationResult<TNumber> Visit(PreComputedOperator op, PreComputeFrame<TNumber> frame)
        {
            return Constant(op, frame.State, (TNumber)op.Value);
        }

        public PartialEvaluationResult<TNumber> Visit(ArgumentOperator op, PreComputeFrame<TNumber> frame)
        {
            return frame.State.TryGetArgument(op.Index, out var value)
                ? Constant(new PreComputedOperator(value), frame.State, value)
                : Unknown(op, frame.State);
        }

        public PartialEvaluationResult<TNumber> Visit(LetVariableOperator op, PreComputeFrame<TNumber> frame)
        {
            return frame.LetValues.TryGetValue(op.LocalIndex, out var value)
                ? Constant(new PreComputedOperator(value), frame.State, value)
                : Unknown(op, frame.State);
        }

        public PartialEvaluationResult<TNumber> Visit(DefineOperator op, PreComputeFrame<TNumber> frame)
        {
            return Constant(new PreComputedOperator(TNumber.Zero), frame.State, TNumber.Zero);
        }

        public PartialEvaluationResult<TNumber> Visit(LoadVariableOperator op, PreComputeFrame<TNumber> frame)
        {
            return frame.State.TryGetVariable(op.VariableName, out var value)
                ? Constant(new PreComputedOperator(value), frame.State, value)
                : Unknown(op, frame.State);
        }

        public PartialEvaluationResult<TNumber> Visit(InputOperator op, PreComputeFrame<TNumber> frame)
        {
            return Unknown(op, frame.State);
        }

        public PartialEvaluationResult<TNumber> Visit(LoadArrayOperator op, PreComputeFrame<TNumber> frame)
        {
            var index = Evaluate(op.Index, frame);
            var rewrittenOperator = op with { Index = index.Operator };

            if (TryEvaluateExactly(rewrittenOperator, frame, out var exactResult))
            {
                return exactResult;
            }

            if (index is ConstantEvaluationResult<TNumber> constantIndex
                && constantIndex.ExitState.TryGetArrayValue(constantIndex.ConstantValue, out var value))
            {
                return WithConstantValue(constantIndex, value);
            }

            return Unknown(rewrittenOperator, index.ExitState);
        }

        public PartialEvaluationResult<TNumber> Visit(PrintCharOperator op, PreComputeFrame<TNumber> frame)
        {
            var character = Evaluate(op.Character, frame);
            var rewrittenOperator = op with { Character = character.Operator };

            if (TryEvaluateExactly(rewrittenOperator, frame, out var exactResult))
            {
                return exactResult;
            }

            return character is ConstantEvaluationResult<TNumber>
                ? Constant(rewrittenOperator, character.ExitState, TNumber.Zero)
                : Unknown(rewrittenOperator, character.ExitState);
        }

        public PartialEvaluationResult<TNumber> Visit(ParenthesisOperator op, PreComputeFrame<TNumber> frame)
        {
            var currentState = frame.State;
            var rewrittenOperators = ImmutableArray.CreateBuilder<IOperator>(op.Operators.Length);
            PartialEvaluationResult<TNumber>? lastResult = null;

            foreach (var child in op.Operators)
            {
                var result = Evaluate(child, frame with { State = currentState });
                rewrittenOperators.Add(result.Operator);
                currentState = result.ExitState;
                lastResult = result;
            }

            Debug.Assert(lastResult is not null);

            var rewrittenOperator = BuildSequence(rewrittenOperators.DrainToImmutable());

            if (TryEvaluateExactly(rewrittenOperator, frame, out var exactResult))
            {
                return exactResult;
            }

            return lastResult is ConstantEvaluationResult<TNumber> constantLast
                ? Constant(rewrittenOperator, currentState, constantLast.ConstantValue)
                : Unknown(rewrittenOperator, currentState);
        }

        public PartialEvaluationResult<TNumber> Visit(DecimalOperator op, PreComputeFrame<TNumber> frame)
        {
            var operand = Evaluate(op.Operand, frame);
            var rewrittenOperator = op with { Operand = operand.Operator };

            if (TryEvaluateExactly(rewrittenOperator, frame, out var exactResult))
            {
                return exactResult;
            }

            if (operand is ConstantEvaluationResult<TNumber> constantOperand)
            {
                var value = constantOperand.ConstantValue * TNumber.CreateTruncating(10) + TNumber.CreateTruncating(op.Value);
                return WithConstantValue(constantOperand, value);
            }

            return Unknown(rewrittenOperator, operand.ExitState);
        }

        public PartialEvaluationResult<TNumber> Visit(StoreVariableOperator op, PreComputeFrame<TNumber> frame)
        {
            var operand = Evaluate(op.Operand, frame);
            var rewrittenOperator = op with { Operand = operand.Operator };

            if (TryEvaluateExactly(rewrittenOperator, frame, out var exactResult))
            {
                return exactResult;
            }

            if (operand is ConstantEvaluationResult<TNumber> constantOperand)
            {
                var nextState = operand.ExitState.SetVariable(op.VariableName, constantOperand.ConstantValue);
                return Constant(rewrittenOperator, nextState, constantOperand.ConstantValue);
            }

            return Unknown(rewrittenOperator, operand.ExitState.UnsetVariable(op.VariableName));
        }

        public PartialEvaluationResult<TNumber> Visit(StoreArrayOperator op, PreComputeFrame<TNumber> frame)
        {
            var value = Evaluate(op.Value, frame);
            var index = Evaluate(op.Index, frame with { State = value.ExitState });
            PreComputeState<TNumber> nextState;

            if (index is ConstantEvaluationResult<TNumber> constantIndex)
            {
                if (value is ConstantEvaluationResult<TNumber> constantValue)
                {
                    nextState = index.ExitState.SetArrayValue(constantIndex.ConstantValue, constantValue.ConstantValue);
                }
                else
                {
                    nextState = index.ExitState.UnsetArrayValue(constantIndex.ConstantValue);
                }
            }
            else
            {
                nextState = index.ExitState.InvalidateAllArrayElements();
            }

            var rewrittenOperator = op with { Value = value.Operator, Index = index.Operator };
            if (TryEvaluateExactly(rewrittenOperator, frame, out var exactResult))
            {
                return exactResult;
            }

            return value is ConstantEvaluationResult<TNumber> finalConstantValue
                ? Constant(rewrittenOperator, nextState, finalConstantValue.ConstantValue)
                : Unknown(rewrittenOperator, nextState);
        }

        public PartialEvaluationResult<TNumber> Visit(BinaryOperator op, PreComputeFrame<TNumber> frame)
        {
            var left = Evaluate(op.Left, frame);

            if (op.Type is BinaryType.LogicalAnd or BinaryType.LogicalOr)
            {
                return VisitShortCircuit(op, frame, left, isLogicalOr: op.Type == BinaryType.LogicalOr);
            }

            var right = Evaluate(op.Right, frame with { State = left.ExitState });
            var rewrittenOperator = op with { Left = left.Operator, Right = right.Operator };

            if (TryEvaluateExactly(rewrittenOperator, frame, out var exactResult))
            {
                return exactResult;
            }

            if (left is ConstantEvaluationResult<TNumber> constantLeft
                && right is ConstantEvaluationResult<TNumber> constantRight
                && TryEvaluateBinary(op.Type, constantLeft.ConstantValue, constantRight.ConstantValue, out var value))
            {
                return Constant(BuildSequence(left.Operator, right.Operator, new PreComputedOperator(value)),
                                right.ExitState,
                                value);
            }

            if (TrySimplifyBinary(left, right, op.Type, out var simplified))
            {
                return simplified;
            }

            return Unknown(rewrittenOperator, right.ExitState);
        }

        public PartialEvaluationResult<TNumber> Visit(ConditionalOperator op, PreComputeFrame<TNumber> frame)
        {
            var condition = Evaluate(op.Condition, frame);

            if (condition is ConstantEvaluationResult<TNumber> constantCondition)
            {
                var chosen = TNumber.IsZero(constantCondition.ConstantValue)
                    ? Evaluate(op.IfFalse, frame with { State = condition.ExitState })
                    : Evaluate(op.IfTrue, frame with { State = condition.ExitState });

                var rewrittenOperator = BuildSequence(condition.Operator, chosen.Operator);
                if (TryEvaluateExactly(rewrittenOperator, frame, out var exactResult))
                {
                    return exactResult;
                }

                return chosen is ConstantEvaluationResult<TNumber> constantChosen
                    ? Constant(rewrittenOperator, chosen.ExitState, constantChosen.ConstantValue)
                    : Unknown(rewrittenOperator, chosen.ExitState);
            }

            var ifTrue = Evaluate(op.IfTrue, frame with { State = condition.ExitState });
            var ifFalse = Evaluate(op.IfFalse, frame with { State = condition.ExitState });

            // If both branches rewrite to the same operator tree, they produce the same
            // value and the same state changes. Collapse to (condition, branch).
            if (ifTrue.Operator.Equals(ifFalse.Operator))
            {
                var rewritten = BuildSequence(condition.Operator, ifTrue.Operator);
                if (TryEvaluateExactly(rewritten, frame, out var exactResult))
                {
                    return exactResult;
                }

                return ifTrue is ConstantEvaluationResult<TNumber> constantIfTrue
                    ? Constant(rewritten, ifTrue.ExitState, constantIfTrue.ConstantValue)
                    : Unknown(rewritten, ifTrue.ExitState);
            }

            var mergedState = PreComputeState<TNumber>.Merge(ifTrue.ExitState, ifFalse.ExitState);
            var rewrittenConditional = op with
            {
                Condition = condition.Operator,
                IfTrue = ifTrue.Operator,
                IfFalse = ifFalse.Operator,
            };

            if (TryEvaluateExactly(rewrittenConditional, frame, out var conditionalExactResult))
            {
                return conditionalExactResult;
            }

            return Unknown(rewrittenConditional, mergedState);
        }

        public PartialEvaluationResult<TNumber> Visit(LetOperator op, PreComputeFrame<TNumber> frame)
        {
            var value = Evaluate(op.Value, frame);
            var letValues = value is ConstantEvaluationResult<TNumber> constantValue
                ? frame.LetValues.SetItem(op.LocalIndex, constantValue.ConstantValue)
                : frame.LetValues.Remove(op.LocalIndex);
            var body = Evaluate(op.Body, frame with { State = value.ExitState, LetValues = letValues });
            var referenceCounts = CountLetVariableReferences(body.Operator, [op.LocalIndex]);
            var rewrittenOperator = referenceCounts.ContainsKey(op.LocalIndex)
                ? op with { Value = value.Operator, Body = body.Operator }
                : BuildSequence(value.Operator, body.Operator);

            if (TryEvaluateExactly(rewrittenOperator, frame, out var exactResult))
            {
                return exactResult;
            }

            return body is ConstantEvaluationResult<TNumber> constantBody
                ? Constant(rewrittenOperator, body.ExitState, constantBody.ConstantValue)
                : Unknown(rewrittenOperator, body.ExitState);
        }

        public PartialEvaluationResult<TNumber> Visit(UserDefinedOperator op, PreComputeFrame<TNumber> frame)
        {
            var currentState = frame.State;
            var operandResults = new PartialEvaluationResult<TNumber>[op.Operands.Length];
            bool hasConstantOperand = false;
            bool hasNonConstantOperand = false;

            for (int i = 0; i < op.Operands.Length; i++)
            {
                operandResults[i] = Evaluate(op.Operands[i], frame with { State = currentState });
                currentState = operandResults[i].ExitState;
                if (operandResults[i] is ConstantEvaluationResult<TNumber>)
                {
                    hasConstantOperand = true;
                }
                else
                {
                    hasNonConstantOperand = true;
                }
            }

            var rewrittenOperator = op with { Operands = operandResults.Select(result => result.Operator).ToImmutableArray() };

            if (TryEvaluateExactly(rewrittenOperator, frame, out var exactResult))
            {
                return exactResult;
            }

            if (hasConstantOperand || op.Operands.Length == 0)
            {
                if (TryPartiallySpecialize(op, operandResults, currentState, frame.Budget, out var specialized))
                {
                    return specialized;
                }
            }

            if (hasNonConstantOperand && TryInlineWithLet(op, operandResults, currentState, frame.Budget, out var inlined))
            {
                return inlined;
            }

            var nextState = currentState.Apply(LookupEffects(op.Definition.Name));
            return Unknown(rewrittenOperator, nextState);
        }

        private bool TryPartiallySpecialize(UserDefinedOperator op,
                                            PartialEvaluationResult<TNumber>[] operandResults,
                                            PreComputeState<TNumber> currentState,
                                            UserDefinedCallBudget budget,
                                            [NotNullWhen(true)] out PartialEvaluationResult<TNumber>? result)
        {
            if (specializationInProgress.Contains(op.Definition.Name))
            {
                result = null;
                return false;
            }

            if (!budget.TryConsume(out var bodyBudget))
            {
                result = null;
                return false;
            }

            var bindings = ImmutableDictionary.CreateBuilder<int, TNumber>();
            var keyBindings = ImmutableArray.CreateBuilder<SpecializationBinding<TNumber>>(operandResults.Length);
            for (int i = 0; i < operandResults.Length; i++)
            {
                if (operandResults[i] is ConstantEvaluationResult<TNumber> constantOperand)
                {
                    bindings[i] = constantOperand.ConstantValue;
                    keyBindings.Add(new SpecializationBinding<TNumber>(i, constantOperand.ConstantValue));
                }
            }

            if (bindings.Count == 0 && op.Operands.Length != 0)
            {
                result = null;
                return false;
            }

            var operatorBody = LookupOperatorBody(op.Definition.Name);
            var key = new SpecializationCacheKey<TNumber>(op.Definition.Name, operatorBody, keyBindings.DrainToImmutable());

            bool cacheHit;
            SpecializationCacheEntry<TNumber>? cachedEntry;
            if (session.SpecializationCache.TryGetValue(key, out var found))
            {
                cacheHit = true;
                cachedEntry = found;
            }
            else
            {
                cacheHit = false;
                specializationInProgress.Add(op.Definition.Name);
                try
                {
                    cachedEntry = ComputeSpecialization(operatorBody, bindings.ToImmutable(), bodyBudget);
                }
                finally
                {
                    specializationInProgress.Remove(op.Definition.Name);
                }

                session.SpecializationCache[key] = cachedEntry;
            }

            if (cachedEntry is null)
            {
                result = null;
                return false;
            }

            var cached = cacheHit
                ? RemapGeneratedLocalIndices(cachedEntry.Specialization, cachedEntry.GeneratedLocalIndices)
                : cachedEntry.Specialization;

            // Keep any operand whose evaluation is still observable at the call site.
            var pieces = new List<IOperator>();
            for (int i = 0; i < operandResults.Length; i++)
            {
                var rewrittenOperand = operandResults[i].Operator;
                if (operandResults[i] is not ConstantEvaluationResult<TNumber>
                    || !IsPure<TNumber>(rewrittenOperand, effectsByOperators))
                {
                    pieces.Add(rewrittenOperand);
                }
            }

            pieces.Add(cached.Body);
            var rewritten = pieces.Count == 1 ? pieces[0] : BuildSequence(pieces);

            var nextState = currentState.Apply(cached.ExitDelta);

            result = cached is ConstantSpecializationResult<TNumber> constantSpecialization
                ? Constant(rewritten, nextState, constantSpecialization.ConstantValue)
                : Unknown(rewritten, nextState);
            return true;
        }

        private bool TryInlineWithLet(UserDefinedOperator op,
                                      PartialEvaluationResult<TNumber>[] operandResults,
                                      PreComputeState<TNumber> currentState,
                                      UserDefinedCallBudget budget,
                                      [NotNullWhen(true)] out PartialEvaluationResult<TNumber>? result)
        {
            if (specializationInProgress.Contains(op.Definition.Name))
            {
                result = null;
                return false;
            }

            if (!budget.TryConsume(out var bodyBudget))
            {
                result = null;
                return false;
            }

            int savedNextLetLocalIndex = nextLetLocalIndex;
            var replacements = ImmutableDictionary.CreateBuilder<int, IOperator>();
            var temporaryLocalIndices = Enumerable.Repeat(NoTemporaryLetLocalIndex, operandResults.Length).ToArray();
            var prefixOperators = new IOperator?[operandResults.Length];
            var keyBindings = ImmutableArray.CreateBuilder<SpecializationBinding<TNumber>>(operandResults.Length);
            bool hasLetBinding = false;

            for (int i = 0; i < operandResults.Length; i++)
            {
                var rewrittenOperand = operandResults[i].Operator;
                if (operandResults[i] is ConstantEvaluationResult<TNumber> constantOperand)
                {
                    replacements[i] = new PreComputedOperator(constantOperand.ConstantValue);
                    keyBindings.Add(new SpecializationBinding<TNumber>(i, constantOperand.ConstantValue));
                    if (!IsPure<TNumber>(rewrittenOperand, effectsByOperators))
                    {
                        prefixOperators[i] = rewrittenOperand;
                    }
                }
                else
                {
                    int localIndex = GetCanonicalTemporaryLetLocalIndex(i);
                    temporaryLocalIndices[i] = localIndex;
                    replacements[i] = new LetVariableOperator(localIndex);
                    hasLetBinding = true;
                }
            }

            if (!hasLetBinding)
            {
                result = null;
                return false;
            }

            var trackedLocals = temporaryLocalIndices.Where(localIndex => localIndex < 0)
                                                     .ToImmutableHashSet();
            var operatorBody = LookupOperatorBody(op.Definition.Name);
            var key = new SpecializationCacheKey<TNumber>(op.Definition.Name, operatorBody, keyBindings.DrainToImmutable());
            bool cacheHit;
            LetInlineCacheEntry<TNumber> cached;
            if (session.LetInlineCache.TryGetValue(key, out var found))
            {
                cacheHit = true;
                cached = found;
            }
            else
            {
                cacheHit = false;

                specializationInProgress.Add(op.Definition.Name);
                try
                {
                    cached = ComputeLetInlineBody(operatorBody,
                                                  replacements.ToImmutable(),
                                                  trackedLocals,
                                                  bodyBudget);
                }
                finally
                {
                    specializationInProgress.Remove(op.Definition.Name);
                }

                session.LetInlineCache[key] = cached;
            }

            var specialized = cacheHit
                ? RemapGeneratedLocalIndices(cached.Specialization, cached.GeneratedLocalIndices)
                : cached.Specialization;
            var usageCounts = cached.UsageCounts;
            var bodyReplacements = ImmutableDictionary.CreateBuilder<int, IOperator>();
            var letLocalIndices = Enumerable.Repeat(-1, operandResults.Length).ToArray();

            for (int i = 0; i < operandResults.Length; i++)
            {
                int temporaryLocalIndex = temporaryLocalIndices[i];

                if (temporaryLocalIndex == NoTemporaryLetLocalIndex)
                {
                    continue;
                }

                usageCounts.TryGetValue(temporaryLocalIndex, out int useCount);
                var rewrittenOperand = operandResults[i].Operator;

                if (useCount == 0)
                {
                    if (!IsPure<TNumber>(rewrittenOperand, effectsByOperators))
                    {
                        prefixOperators[i] = rewrittenOperand;
                    }
                }
                else if (useCount == 1 && CanInlineAtUseSite<TNumber>(rewrittenOperand))
                {
                    bodyReplacements[temporaryLocalIndex] = rewrittenOperand;
                }
                else
                {
                    int localIndex = AllocateLetLocalIndex();
                    letLocalIndices[i] = localIndex;
                    bodyReplacements[temporaryLocalIndex] = new LetVariableOperator(localIndex);
                }
            }

            IOperator rewritten = ReplaceLetVariables(specialized.Body, bodyReplacements.ToImmutable());
            for (int i = operandResults.Length - 1; i >= 0; i--)
            {
                if (letLocalIndices[i] >= 0)
                {
                    rewritten = new LetOperator(letLocalIndices[i], operandResults[i].Operator, rewritten);
                }
                else if (prefixOperators[i] is { } prefix)
                {
                    rewritten = BuildSequence(prefix, rewritten);
                }
            }

            if (HasMoreThanNodes(rewritten, MaxInlineSize))
            {
                nextLetLocalIndex = savedNextLetLocalIndex;
                result = null;
                return false;
            }

            var nextState = currentState.Apply(specialized.ExitDelta);

            result = specialized is ConstantSpecializationResult<TNumber> constantSpecialization
                ? Constant(rewritten, nextState, constantSpecialization.ConstantValue)
                : Unknown(rewritten, nextState);
            return true;
        }

        private SpecializationCacheEntry<TNumber>? ComputeSpecialization(IOperator operatorBody,
                                                                         ImmutableDictionary<int, TNumber> bindings,
                                                                         UserDefinedCallBudget bodyBudget)
        {
            var argumentOperators = ImmutableDictionary.CreateBuilder<int, IOperator>();
            foreach (var (index, value) in bindings)
            {
                argumentOperators[index] = new PreComputedOperator(value);
            }

            int generatedLocalStart = nextLetLocalIndex;
            var remappedBody = RemapExistingLetLocalIndices(operatorBody);
            var substituted = SubstituteArguments(remappedBody, argumentOperators.ToImmutable());

            // Optimize the substituted body in a fresh state: variables start unknown and
            // arrays cannot be assumed zero because we are mid-program.
            var fresh = PreComputeState<TNumber>.Create(arraysZeroInitialized: false);
            var frame = new PreComputeFrame<TNumber>(fresh, bodyBudget, []);
            var specResult = Evaluate(substituted, frame);
            int generatedLocalEnd = nextLetLocalIndex;

            // The body must be self-contained and small enough to inline without blowing up code size.
            if (ContainsUnresolvedArgument(specResult.Operator))
            {
                nextLetLocalIndex = generatedLocalStart;
                return null;
            }

            if (HasMoreThanNodes(specResult.Operator, MaxInlineSize))
            {
                nextLetLocalIndex = generatedLocalStart;
                return null;
            }

            var exitDelta = CreateSpecializationStateDelta(specResult);
            var generatedLocalIndices = CollectLetLocalIndices(specResult.Operator, generatedLocalStart, generatedLocalEnd);
            return specResult is ConstantEvaluationResult<TNumber> constantSpecResult
                ? new SpecializationCacheEntry<TNumber>(
                    new ConstantSpecializationResult<TNumber>(specResult.Operator, exitDelta, constantSpecResult.ConstantValue),
                    generatedLocalIndices)
                : new SpecializationCacheEntry<TNumber>(
                    new UnknownSpecializationResult<TNumber>(specResult.Operator, exitDelta),
                    generatedLocalIndices);
        }

        private LetInlineCacheEntry<TNumber> ComputeLetInlineBody(IOperator operatorBody,
                                                                  ImmutableDictionary<int, IOperator> replacements,
                                                                  ImmutableHashSet<int> trackedLocalIndices,
                                                                  UserDefinedCallBudget bodyBudget)
        {
            int generatedLocalStart = nextLetLocalIndex;
            var remappedBody = RemapExistingLetLocalIndices(operatorBody);
            var substituted = SubstituteArguments(remappedBody, replacements);
            var fresh = PreComputeState<TNumber>.Create(arraysZeroInitialized: false);
            var frame = new PreComputeFrame<TNumber>(fresh, bodyBudget, []);
            var specResult = Evaluate(substituted, frame);
            int generatedLocalEnd = nextLetLocalIndex;

            // All call arguments were replaced before evaluation, so unresolved arguments indicate an invalid tree.
            Debug.Assert(!ContainsUnresolvedArgument(specResult.Operator));

            var exitDelta = CreateSpecializationStateDelta(specResult);
            var usageCounts = CountLetVariableReferences(specResult.Operator, trackedLocalIndices);
            var generatedLocalIndices = CollectLetLocalIndices(specResult.Operator, generatedLocalStart, generatedLocalEnd);
            return specResult is ConstantEvaluationResult<TNumber> constantSpecResult
                ? new LetInlineCacheEntry<TNumber>(
                    new ConstantSpecializationResult<TNumber>(specResult.Operator, exitDelta, constantSpecResult.ConstantValue),
                    usageCounts,
                    generatedLocalIndices)
                : new LetInlineCacheEntry<TNumber>(
                    new UnknownSpecializationResult<TNumber>(specResult.Operator, exitDelta),
                    usageCounts,
                    generatedLocalIndices);
        }

        private SpecializationStateDelta<TNumber> CreateSpecializationStateDelta(PartialEvaluationResult<TNumber> result)
        {
            PotentialEffectsBuilder effectsBuilder = new();
            CollectPotentialEffects(result.Operator, effectsBuilder);
            var effects = effectsBuilder.ToImmutable();
            var knownVariables = ImmutableDictionary.CreateBuilder<ValueBox<string>, TNumber>();
            var unknownVariables = ImmutableHashSet.CreateBuilder<ValueBox<string>>();

            foreach (var variableName in effects.WrittenVariables)
            {
                var key = ValueBox.Create(variableName);
                if (result.ExitState.TryGetVariable(variableName, out var value))
                {
                    knownVariables[key] = value;
                }
                else
                {
                    unknownVariables.Add(key);
                }
            }

            return new SpecializationStateDelta<TNumber>(
                knownVariables.ToImmutable(),
                unknownVariables.ToImmutable(),
                result.ExitState.HasInvalidatedArrayElements,
                result.ExitState.EnumerateKnownArrayValues().ToImmutableDictionary(),
                result.ExitState.EnumerateUnknownArrayIndices().ToImmutableHashSet());
        }

        private void CollectPotentialEffects(IOperator op, PotentialEffectsBuilder effects)
        {
            CollectEffectsFromTree<TNumber>(op,
                                            effects,
                                            userDefined => effects.AddFrom(LookupEffects(userDefined.Definition.Name)));
        }

        private int AllocateLetLocalIndex()
        {
            return nextLetLocalIndex++;
        }

        private IOperator RemapExistingLetLocalIndices(IOperator operatorBody)
        {
            var existingLocalIndices = GetExistingLetLocalIndices(operatorBody);
            if (existingLocalIndices.IsDefaultOrEmpty)
            {
                return operatorBody;
            }

            var replacements = ImmutableDictionary.CreateBuilder<int, int>();
            foreach (int localIndex in existingLocalIndices)
            {
                replacements[localIndex] = AllocateLetLocalIndex();
            }

            return ReplaceLetLocalIndices(operatorBody, replacements.ToImmutable());
        }

        private ImmutableArray<int> GetExistingLetLocalIndices(IOperator operatorBody)
        {
            if (session.ExistingLetLocalIndicesCache.TryGetValue(operatorBody, out var existingLocalIndices))
            {
                return existingLocalIndices;
            }

            existingLocalIndices = CollectLetLocalIndices(operatorBody);
            session.ExistingLetLocalIndicesCache[operatorBody] = existingLocalIndices;
            return existingLocalIndices;
        }

        private SpecializationResult<TNumber> RemapGeneratedLocalIndices(SpecializationResult<TNumber> specialization,
                                                                         ImmutableArray<int> generatedLocalIndices)
        {
            if (generatedLocalIndices.IsDefaultOrEmpty)
            {
                return specialization;
            }

            var replacements = ImmutableDictionary.CreateBuilder<int, int>();
            foreach (int localIndex in generatedLocalIndices)
            {
                replacements[localIndex] = AllocateLetLocalIndex();
            }

            var body = ReplaceLetLocalIndices(specialization.Body, replacements.ToImmutable());
            return specialization is ConstantSpecializationResult<TNumber> constant
                ? new ConstantSpecializationResult<TNumber>(body, constant.ExitDelta, constant.ConstantValue)
                : new UnknownSpecializationResult<TNumber>(body, specialization.ExitDelta);
        }

        private static int GetCanonicalTemporaryLetLocalIndex(int operandIndex)
        {
            return -operandIndex - 1;
        }

        private static ImmutableArray<int> CollectLetLocalIndices(IOperator op)
        {
            HashSet<int> indices = [];
            CollectLetLocalIndices(op, indices);
            return indices.Order().ToImmutableArray();
        }

        private static void CollectLetLocalIndices(IOperator op, HashSet<int> indices)
        {
            switch (op)
            {
                case LetOperator let:
                    indices.Add(let.LocalIndex);
                    break;
                case LetVariableOperator letVariable:
                    indices.Add(letVariable.LocalIndex);
                    break;
                default:
                    break;
            }

            if (op is ParenthesisOperator parenthesis)
            {
                foreach (var inner in parenthesis.Operators)
                {
                    CollectLetLocalIndices(inner, indices);
                }
            }

            op.ForEachOperand(operand => CollectLetLocalIndices(operand, indices));
        }

        private static ImmutableArray<int> CollectLetLocalIndices(IOperator op, int minInclusive, int maxExclusive)
        {
            if (minInclusive >= maxExclusive)
            {
                return [];
            }

            HashSet<int> indices = [];
            CollectLetLocalIndices(op, minInclusive, maxExclusive, indices);
            return indices.Order().ToImmutableArray();
        }

        private static void CollectLetLocalIndices(IOperator op,
                                                   int minInclusive,
                                                   int maxExclusive,
                                                   HashSet<int> indices)
        {
            switch (op)
            {
                case LetOperator let when let.LocalIndex >= minInclusive && let.LocalIndex < maxExclusive:
                    indices.Add(let.LocalIndex);
                    break;
                case LetVariableOperator letVariable when letVariable.LocalIndex >= minInclusive && letVariable.LocalIndex < maxExclusive:
                    indices.Add(letVariable.LocalIndex);
                    break;
                default:
                    break;
            }

            if (op is ParenthesisOperator parenthesis)
            {
                foreach (var inner in parenthesis.Operators)
                {
                    CollectLetLocalIndices(inner, minInclusive, maxExclusive, indices);
                }
            }

            op.ForEachOperand(operand => CollectLetLocalIndices(operand, minInclusive, maxExclusive, indices));
        }

        private static bool ContainsUnresolvedArgument(IOperator op)
        {
            if (op is ArgumentOperator)
            {
                return true;
            }

            if (op is ParenthesisOperator parenthesis)
            {
                foreach (var inner in parenthesis.Operators)
                {
                    if (ContainsUnresolvedArgument(inner))
                    {
                        return true;
                    }
                }
            }

            bool contains = false;
            op.ForEachOperand(operand =>
            {
                if (!contains && ContainsUnresolvedArgument(operand))
                {
                    contains = true;
                }
            });

            return contains;
        }

        private IOperator LookupOperatorBody(string operatorName)
        {
            var implement = compilationContext.LookupOperatorImplement(operatorName);
            Debug.Assert(implement.Operator is not null);
            return implement.Operator;
        }

        private PotentialEffects LookupEffects(string operatorName)
        {
            Debug.Assert(effectsByOperators.ContainsKey(operatorName));
            return effectsByOperators[operatorName];
        }

        private bool TryEvaluateExactly(IOperator op,
                                        PreComputeFrame<TNumber> frame,
                                        [NotNullWhen(true)] out PartialEvaluationResult<TNumber>? result)
        {
            var variableSource = new RecordingKnownVariableSource<TNumber>(frame.State);
            var arraySource = new RecordingKnownArraySource<TNumber>(frame.State);
            var ioService = new MemoryIOService();

            if (Evaluator.TryEvaluate(op,
                                      compilationContext,
                                      new SimpleEvaluationState<TNumber>(variableSource,
                                                                         arraySource,
                                                                         ioService),
                                      frame.Budget.RemainingUserDefinedCalls,
                                      out var value))
            {
                result = CreateExactEvaluationResult(frame.State, variableSource, arraySource, ioService.GetHistory(), value);
                return true;
            }

            result = null;
            return false;
        }

        private PartialEvaluationResult<TNumber> CreateExactEvaluationResult(PreComputeState<TNumber> initialState,
                                                                             RecordingKnownVariableSource<TNumber> variableSource,
                                                                             RecordingKnownArraySource<TNumber> arraySource,
                                                                             string output,
                                                                             TNumber value)
        {
            var nextState = initialState;
            List<(IOperator Operator, TNumber Result)> effectOperators = [];

            foreach (var (variableName, variableValue) in variableSource.EnumerateWrittenVariables())
            {
                nextState = nextState.SetVariable(variableName, variableValue);

                if (!initialState.TryGetVariable(variableName, out var initialValue) || initialValue != variableValue)
                {
                    effectOperators.Add((new StoreVariableOperator(new PreComputedOperator(variableValue), variableName), variableValue));
                }
            }

            foreach (var (index, arrayValue) in arraySource.EnumerateWrittenArrayValues())
            {
                nextState = nextState.SetArrayValue(index, arrayValue);

                if (!initialState.TryGetArrayValue(index, out var initialValue) || initialValue != arrayValue)
                {
                    effectOperators.Add((new StoreArrayOperator(new PreComputedOperator(arrayValue), new PreComputedOperator(index)), arrayValue));
                }
            }

            foreach (var character in output)
            {
                effectOperators.Add((new PrintCharOperator(new PreComputedOperator(TNumber.CreateTruncating(character))), TNumber.Zero));
            }

            List<IOperator> operators = effectOperators.Select(static x => x.Operator).ToList();
            if (effectOperators.Count == 0 || effectOperators[^1].Result != value)
            {
                operators.Add(new PreComputedOperator(value));
            }

            return Constant(BuildSequence(operators), nextState, value);
        }

        // Algebraic simplification for binary operators when only one side (or neither) is
        // a constant. Returns false if no rewrite applies. Caller is expected to have
        // already tried full constant folding via TryEvaluateBinary.
        private bool TrySimplifyBinary(PartialEvaluationResult<TNumber> left,
                                       PartialEvaluationResult<TNumber> right,
                                       BinaryType type,
                                       [NotNullWhen(true)] out PartialEvaluationResult<TNumber>? simplified)
        {
            bool leftPure = IsPure<TNumber>(left.Operator, effectsByOperators);
            bool rightPure = IsPure<TNumber>(right.Operator, effectsByOperators);

            if (right is ConstantEvaluationResult<TNumber> constantRight && rightPure)
            {
                if (TNumber.IsZero(constantRight.ConstantValue))
                {
                    switch (type)
                    {
                        case BinaryType.Add:
                        case BinaryType.Sub:
                            simplified = left;
                            return true;
                        case BinaryType.Mult:
                            simplified = MaterializeConstant(left, leftPure, TNumber.Zero);
                            return true;
                    }
                }
                else if (constantRight.ConstantValue == TNumber.One)
                {
                    switch (type)
                    {
                        case BinaryType.Mult:
                        case BinaryType.Div:
                            simplified = left;
                            return true;
                        case BinaryType.Mod:
                            simplified = MaterializeConstant(left, leftPure, TNumber.Zero);
                            return true;
                    }
                }
            }

            if (left is ConstantEvaluationResult<TNumber> constantLeft && leftPure)
            {
                if (TNumber.IsZero(constantLeft.ConstantValue))
                {
                    switch (type)
                    {
                        case BinaryType.Add:
                            simplified = right;
                            return true;
                        case BinaryType.Mult:
                            simplified = MaterializeConstant(right, rightPure, TNumber.Zero);
                            return true;
                    }
                }
                else if (constantLeft.ConstantValue == TNumber.One)
                {
                    if (type == BinaryType.Mult)
                    {
                        simplified = right;
                        return true;
                    }
                }
            }

            // Self comparison: x OP x where x has no side effects.
            if (leftPure && rightPure && left.Operator.Equals(right.Operator)
                && TryEvaluateSelfBinary(type, out var selfValue))
            {
                simplified = Constant(new PreComputedOperator(selfValue), right.ExitState, selfValue);
                return true;
            }

            simplified = null;
            return false;
        }

        private PartialEvaluationResult<TNumber> MaterializeConstant(PartialEvaluationResult<TNumber> kept,
                                                                     bool keptIsPure,
                                                                     TNumber value)
        {
            IOperator op = keptIsPure
                ? new PreComputedOperator(value)
                : BuildSequence(kept.Operator, new PreComputedOperator(value));
            return Constant(op, kept.ExitState, value);
        }

        private static bool TryEvaluateSelfBinary(BinaryType type, out TNumber value)
        {
            switch (type)
            {
                case BinaryType.Sub:
                case BinaryType.NotEqual:
                case BinaryType.LessThan:
                case BinaryType.GreaterThan:
                    value = TNumber.Zero;
                    return true;
                case BinaryType.Equal:
                case BinaryType.LessThanOrEqual:
                case BinaryType.GreaterThanOrEqual:
                    value = TNumber.One;
                    return true;
                default:
                    value = TNumber.Zero;
                    return false;
            }
        }

        private PartialEvaluationResult<TNumber> VisitShortCircuit(BinaryOperator op,
                                                                   PreComputeFrame<TNumber> frame,
                                                                   PartialEvaluationResult<TNumber> left,
                                                                   bool isLogicalOr)
        {
            if (left is ConstantEvaluationResult<TNumber> constantLeft)
            {
                bool leftIsZero = TNumber.IsZero(constantLeft.ConstantValue);
                bool shortCircuits = isLogicalOr ? !leftIsZero : leftIsZero;

                if (shortCircuits)
                {
                    return WithConstantValue(constantLeft, isLogicalOr ? TNumber.One : TNumber.Zero);
                }

                var right = Evaluate(op.Right, frame with { State = left.ExitState });
                if (right is ConstantEvaluationResult<TNumber> constantRight)
                {
                    var normalized = TNumber.IsZero(constantRight.ConstantValue) ? TNumber.Zero : TNumber.One;
                    return PrependAndMaterialize(constantRight, normalized, left.Operator);
                }

                var rewritten = BuildSequence(left.Operator,
                                              new BinaryOperator(right.Operator,
                                                                 new PreComputedOperator(TNumber.Zero),
                                                                 BinaryType.NotEqual));
                return Unknown(rewritten, right.ExitState);
            }

            var rightResult = Evaluate(op.Right, frame with { State = left.ExitState });

            // With a constant, side-effect-free right side, the short-circuit collapses:
            //   x && 0 becomes (x, 0)       x && non-zero becomes x != 0
            //   x || non-zero becomes (x, 1)       x || 0 becomes x != 0
            // In all these cases the right side is never evaluated at runtime, so the
            // exit state is just left.ExitState (not the merged state).
            if (rightResult is ConstantEvaluationResult<TNumber> constantRightResult
                && IsPure<TNumber>(rightResult.Operator, effectsByOperators))
            {
                bool rightIsZero = TNumber.IsZero(constantRightResult.ConstantValue);
                bool shortCircuitValueHit = isLogicalOr ? !rightIsZero : rightIsZero;

                if (shortCircuitValueHit)
                {
                    var constant = isLogicalOr ? TNumber.One : TNumber.Zero;
                    var sequence = BuildSequence(left.Operator, new PreComputedOperator(constant));
                    return Constant(sequence, left.ExitState, constant);
                }

                var notEqual = new BinaryOperator(left.Operator,
                                                  new PreComputedOperator(TNumber.Zero),
                                                  BinaryType.NotEqual);
                return Unknown(notEqual, left.ExitState);
            }

            var mergedState = PreComputeState<TNumber>.Merge(left.ExitState, rightResult.ExitState);
            var rewrittenOperator = op with { Left = left.Operator, Right = rightResult.Operator };
            return Unknown(rewrittenOperator, mergedState);
        }

        private static PartialEvaluationResult<TNumber> Constant(IOperator op, PreComputeState<TNumber> state, TNumber value)
        {
            return new ConstantEvaluationResult<TNumber>(op, state, value);
        }

        private static PartialEvaluationResult<TNumber> Unknown(IOperator op, PreComputeState<TNumber> state)
        {
            return new UnknownEvaluationResult<TNumber>(op, state);
        }

        // Keeps the evaluated operator and exit state while assigning a known result value.
        // When the value changes, appends a literal so earlier effects still run.
        private PartialEvaluationResult<TNumber> WithConstantValue(ConstantEvaluationResult<TNumber> result, TNumber newValue)
        {
            if (result.ConstantValue == newValue)
            {
                return Constant(result.Operator, result.ExitState, newValue);
            }

            return Constant(BuildSequence(result.Operator, new PreComputedOperator(newValue)),
                            result.ExitState,
                            newValue);
        }

        // Runs a required prefix before the evaluated operator while assigning a known
        // result value. When the value changes, appends a literal after both operators.
        private PartialEvaluationResult<TNumber> PrependAndMaterialize(ConstantEvaluationResult<TNumber> result,
                                                                       TNumber newValue,
                                                                       IOperator prefix)
        {
            IOperator sequence = result.ConstantValue == newValue
                ? BuildSequence(prefix, result.Operator)
                : BuildSequence(prefix, result.Operator, new PreComputedOperator(newValue));
            return Constant(sequence, result.ExitState, newValue);
        }

        private IOperator BuildSequence(params IOperator[] operators)
        {
            return BuildSequence((IEnumerable<IOperator>)operators);
        }

        private IOperator BuildSequence(IEnumerable<IOperator> operators)
        {
            List<IOperator> flattened = [];

            foreach (var op in operators)
            {
                AppendOperator(flattened, op);
            }

            Debug.Assert(flattened.Count > 0);

            if (flattened.Count == 1)
            {
                return flattened[0];
            }

            var builder = ImmutableArray.CreateBuilder<IOperator>(flattened.Count);

            for (int i = 0; i < flattened.Count - 1; i++)
            {
                // Drop statements whose value is unused AND which have no observable side
                // effect. They cannot influence the final sequence value or program state.
                if (IsPure<TNumber>(flattened[i], effectsByOperators))
                {
                    continue;
                }

                builder.Add(flattened[i]);
            }

            builder.Add(flattened[^1]);
            return builder.Count == 1 ? builder[0] : new ParenthesisOperator(builder.DrainToImmutable());
        }

        private static void AppendOperator(List<IOperator> operators, IOperator op)
        {
            if (op is ParenthesisOperator parenthesis)
            {
                foreach (var inner in parenthesis.Operators)
                {
                    AppendOperator(operators, inner);
                }

                return;
            }

            operators.Add(op);
        }

        private static bool TryEvaluateBinary(BinaryType type, TNumber left, TNumber right, out TNumber value)
        {
            switch (type)
            {
                case BinaryType.Add:
                    value = left + right;
                    return true;
                case BinaryType.Sub:
                    value = left - right;
                    return true;
                case BinaryType.Mult:
                    value = left * right;
                    return true;
                case BinaryType.Div:
                    if (right == TNumber.Zero)
                    {
                        value = TNumber.Zero;
                        return false;
                    }
                    value = left / right;
                    return true;
                case BinaryType.Mod:
                    if (right == TNumber.Zero)
                    {
                        value = TNumber.Zero;
                        return false;
                    }
                    value = left % right;
                    return true;
                case BinaryType.Equal:
                    value = left == right ? TNumber.One : TNumber.Zero;
                    return true;
                case BinaryType.NotEqual:
                    value = left != right ? TNumber.One : TNumber.Zero;
                    return true;
                case BinaryType.LessThan:
                    value = left < right ? TNumber.One : TNumber.Zero;
                    return true;
                case BinaryType.LessThanOrEqual:
                    value = left <= right ? TNumber.One : TNumber.Zero;
                    return true;
                case BinaryType.GreaterThanOrEqual:
                    value = left >= right ? TNumber.One : TNumber.Zero;
                    return true;
                case BinaryType.GreaterThan:
                    value = left > right ? TNumber.One : TNumber.Zero;
                    return true;
                case BinaryType.LogicalAnd:
                case BinaryType.LogicalOr:
                default:
                    value = TNumber.Zero;
                    return false;
            }
        }
    }
}
