using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Exceptions;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization;

public static partial class Optimizer
{
    private sealed class PreComputeVisitor<TNumber> : IOperatorVisitor<IOperator, OptimizeTimeEvaluationState<TNumber>>
        where TNumber : INumber<TNumber>
    {
        private readonly CompilationContext compilationContext;
        private readonly int maxStep;

        private sealed class RecordingVariableSource : IVariableSource<TNumber>
        {
            private readonly IVariableSource<TNumber> inner;
            private readonly HashSet<string?> writtenVariables;

            public RecordingVariableSource(IVariableSource<TNumber> inner, HashSet<string?> writtenVariables)
            {
                this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
                this.writtenVariables = writtenVariables ?? throw new ArgumentNullException(nameof(writtenVariables));
            }

            public TNumber this[string? variableName]
            {
                get => inner[variableName];

                set
                {
                    inner[variableName] = value;
                    writtenVariables.Add(variableName);
                }
            }

            public bool TryGet(string? variableName, [MaybeNullWhen(false)] out TNumber value)
                => inner.TryGet(variableName, out value);

            public IVariableSource<TNumber> Clone()
                => new RecordingVariableSource(inner.Clone(), writtenVariables);
        }

        public PreComputeVisitor(CompilationContext context, int maxStep)
        {
            this.compilationContext = context ?? throw new ArgumentNullException(nameof(context));
            this.maxStep = maxStep;
        }

        private IOperator PreComputeIfPossible(IOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var variables = GetVariablesToBeWritten(op, compilationContext);

            void UnsetAllVariables()
            {
                foreach (var variableName in variables)
                {
                    state.UnsetVariable(variableName);
                }
            }

            // Otherwise, we try to execute
            TNumber preComputedValue;
            OptimizeTimeEvaluationState<TNumber> stateAfterPreComputation = state.Clone();
            HashSet<string?> writtenVariables = new();
            IVariableSource<TNumber> variableSource = new RecordingVariableSource(stateAfterPreComputation, writtenVariables);
            MemoryIOService ioService = new();
            try
            {
                preComputedValue = Evaluator.Evaluate<TNumber>(op,
                                                               compilationContext,
                                                               new SimpleEvaluationState<TNumber>(variableSource,
                                                                                                  AlwaysThrowGlobalArraySource<TNumber>.Instance,
                                                                                                  ioService),
                                                               maxStep);
            }
            catch (EvaluationStepLimitExceedException)
            {
                // If we failed to pre-compute, we must unset all variables to be written by this operator
                UnsetAllVariables();
                return op;
            }
            catch (VariableNotSetException)
            {
                UnsetAllVariables();
                return op;
            }
            catch (ArrayElementNotSetException)
            {
                UnsetAllVariables();
                return op;
            }
            catch (EvaluationArgumentNotSetException)
            {
                UnsetAllVariables();
                return op;
            }
            catch (InputIsNotSupportedException)
            {
                UnsetAllVariables();
                return op;
            }
            catch (ZeroDivisionException)
            {
                UnsetAllVariables();
                return op;
            }

            var operators = ImmutableArray.CreateBuilder<IOperator>();

            // If this operator writes variables, we keep StoreOperators.
            // Note that the optimization state can already contain initial values for variables.
            // Therefore, we must not rely on just the existence of a value in the post state.
            // We instead record variables that were actually written during evaluation.
            foreach (var variableName in writtenVariables.Order())
            {
                if (stateAfterPreComputation.TryGet(variableName, out var value))
                {
                    operators.Add(new StoreVariableOperator(new PreComputedOperator(value), variableName));
                }
                else
                {
                    // This should not happen because a write implies the variable is set.
                    // However, if it does, we conservatively do nothing.
                }
            }

            // Keep PrintCharOperators
            foreach (var c in ioService.GetHistory())
            {
                operators.Add(new PrintCharOperator(new PreComputedOperator(TNumber.CreateTruncating(c))));
            }

            operators.Add(new PreComputedOperator(preComputedValue));

            // Tell the variables after pre-computation
            state.Assign(stateAfterPreComputation);

            return operators.Count switch
            {
                1 => operators[0],
                _ => new ParenthesisOperator(operators.ToImmutable())
            };
        }

        public IOperator Visit(ZeroOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(PreComputedOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(ArgumentOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(DefineOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(LoadVariableOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(InputOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(LoadArrayOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var index = op.Index.Accept(this, state);
            var newOp = op with { Index = index };
            return PreComputeIfPossible(newOp, state);
        }

        public IOperator Visit(PrintCharOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var character = op.Character.Accept(this, state);
            var newOp = op with { Character = character };
            return PreComputeIfPossible(newOp, state);
        }

        public IOperator Visit(ParenthesisOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            ImmutableArray<IOperator> operators = op.Operators;
            var optimized = new List<IOperator>();

            // Optimize all operators in the ParenthesisOperator
            for (int i = 0; i < operators.Length; i++)
            {
                IOperator processed = operators[i].Accept(this, state);

                if (processed is ParenthesisOperator parenthesis)
                {
                    // Extract contents
                    foreach (var inner in parenthesis.Operators)
                    {
                        optimized.Add(inner);
                    }
                }
                else
                {
                    optimized.Add(processed);
                }
            }

            // Eliminate unnecessary PreComputed operators
            var builder = ImmutableArray.CreateBuilder<IOperator>(operators.Length);

            for (int i = 0; i < optimized.Count - 1; i++)
            {
                if (optimized[i] is not PreComputedOperator)
                {
                    builder.Add(optimized[i]);
                }
            }

            // The last one is always necessary
            builder.Add(optimized[^1]);

            if (builder.Count == 1)
            {
                return builder[0];
            }
            else
            {
                return op with { Operators = builder.ToImmutable() };
            }
        }

        public IOperator Visit(DecimalOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var operand = op.Operand.Accept(this, state);
            var newOp = op with { Operand = operand };
            return PreComputeIfPossible(newOp, state);
        }

        public IOperator Visit(StoreVariableOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var operand = op.Operand.Accept(this, state);
            var newOp = op with { Operand = operand };

            if (TryGetPrecomputedValue(operand, out var precomputedValue))
            {
                // Tell the pre-computed value to another operator
                state[op.VariableName] = precomputedValue;
            }
            else
            {
                // We failed to pre-compute the operand of this StoreOperator.
                // Therefore, the variable that this operator indicates is unknown.
                // We unset the variable so as not for another operator to load it.
                state.UnsetVariable(op.VariableName);
            }

            // Do NOT make PreComputedOperator for StoreOperator because it mistakenly eliminates store operation
            return newOp;
        }

        public IOperator Visit(StoreArrayOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var value = op.Value.Accept(this, state);
            var index = op.Index.Accept(this, state);

            // Do NOT make PreComputedOperator for StoreArrayOperator because it mistakenly eliminates store operation
            return op with { Value = value, Index = index };
        }

        public IOperator Visit(BinaryOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var left = op.Left.Accept(this, state);

            if (op.Type is BinaryType.LogicalAnd)
            {
                if (TryGetPrecomputedValue(left, out var leftValue))
                {
                    if (TNumber.IsZero(leftValue))
                    {
                        return new PreComputedOperator(TNumber.Zero);
                    }

                    var right = op.Right.Accept(this, state);
                    if (TryGetPrecomputedValue(right, out var rightValue))
                    {
                        return new PreComputedOperator(TNumber.IsZero(rightValue) ? TNumber.Zero : TNumber.One);
                    }

                    var newOp = new BinaryOperator(right, new PreComputedOperator(TNumber.Zero), BinaryType.NotEqual);
                    return PreComputeIfPossible(newOp, state);
                }
            }
            else if (op.Type is BinaryType.LogicalOr)
            {
                if (TryGetPrecomputedValue(left, out var leftValue))
                {
                    if (!TNumber.IsZero(leftValue))
                    {
                        return new PreComputedOperator(TNumber.One);
                    }

                    var right = op.Right.Accept(this, state);
                    if (TryGetPrecomputedValue(right, out var rightValue))
                    {
                        return new PreComputedOperator(TNumber.IsZero(rightValue) ? TNumber.Zero : TNumber.One);
                    }

                    var newOp = new BinaryOperator(right, new PreComputedOperator(TNumber.Zero), BinaryType.NotEqual);
                    return PreComputeIfPossible(newOp, state);
                }
            }

            if (op.Type is BinaryType.LogicalAnd or BinaryType.LogicalOr)
            {
                // The right operand is conditionally evaluated.
                // We must not commit its effects to the current state at this point.
                // If we did, PreComputeIfPossible might start evaluation from a state that already includes
                // the effects of the right operand even when the operator short-circuits.
                var rightState = state.Clone();
                var right = op.Right.Accept(this, rightState);
                var newOp = op with { Left = left, Right = right };
                return PreComputeIfPossible(newOp, state);
            }

            // Fallback path evaluates right and tries normal precompute with updated operands.
            // Use a block to keep right scoped and avoid name conflicts.
            {
                var right = op.Right.Accept(this, state);
                var newOp = op with { Left = left, Right = right };
                return PreComputeIfPossible(newOp, state);
            }
        }

        public IOperator Visit(ConditionalOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var condition = op.Condition.Accept(this, state);

            // From now on, we use cloned object of the evaluation state
            // because IfTrue and IfFalse will not always be executed.

            // Process IfTrue
            var ifTrueState = state.Clone();
            var ifTrue = op.IfTrue.Accept(this, ifTrueState);

            // Process IfFalse
            var ifFalseState = state.Clone();
            var ifFalse = op.IfFalse.Accept(this, ifFalseState);

            // We must unset variables that have different values after execution of ifTrue and ifFalse.
            // We calculate intersection of ifTrueState and ifFalseState
            var intersection = OptimizeTimeEvaluationState<TNumber>.Intersect(ifTrueState, ifFalseState);
            state.Assign(intersection);

            // Create new operator
            var newOp = op with { Condition = condition, IfTrue = ifTrue, IfFalse = ifFalse };

            if (TryGetPrecomputedValue(PreComputeIfPossible(condition, state), out var conditionValue))
            {
                return !TNumber.IsZero(conditionValue) ? ifTrue : ifFalse;
            }
            else
            {
                return newOp;
            }
        }

        public IOperator Visit(UserDefinedOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var operands = op.Operands.Select(x => x.Accept(this, state)).ToImmutableArray();
            var newOp = op with { Operands = operands };
            return PreComputeIfPossible(newOp, state);
        }

        private static bool TryGetPrecomputedValue(IOperator op, out TNumber precomputedValue)
        {
            if (op is PreComputedOperator preComputed)
            {
                precomputedValue = (TNumber)preComputed.Value;
                return true;
            }

            precomputedValue = TNumber.Zero;
            return false;
        }
    }

    private static HashSet<string?> GetVariablesToBeWritten(IOperator op, CompilationContext context)
    {
        HashSet<string?> variables = new();
        HashSet<string> visitedUserDefinedOperators = new();

        void Core(IOperator op)
        {
            switch (op)
            {
                case StoreVariableOperator store:
                    variables.Add(store.VariableName);
                    break;
                case UserDefinedOperator userDefined:
                    if (!visitedUserDefinedOperators.Contains(userDefined.Definition.Name))
                    {
                        visitedUserDefinedOperators.Add(userDefined.Definition.Name);
                        var implement = context.LookupOperatorImplement(userDefined.Definition.Name);
                        Debug.Assert(implement.Operator is not null);
                        Core(implement.Operator);
                    }
                    break;
                case ParenthesisOperator parenthesis:
                    foreach (var inner in parenthesis.Operators)
                    {
                        Core(inner);
                    }
                    break;
                default:
                    break;
            }

            foreach (var operand in op.GetOperands())
            {
                Core(operand);
            }
        }

        Core(op);
        return variables;
    }

    private static bool HasUserDefinedOperatorCalls(IOperator op, CompilationContext context)
    {
        switch (op)
        {
            case UserDefinedOperator:
                return true;
            case ParenthesisOperator parenthesis:
                foreach (var inner in parenthesis.Operators)
                {
                    if (HasUserDefinedOperatorCalls(inner, context))
                    {
                        return true;
                    }
                }
                break;
            default:
                break;
        }

        foreach (var operand in op.GetOperands())
        {
            if (HasUserDefinedOperatorCalls(operand, context))
            {
                return true;
            }
        }

        return false;
    }
}
