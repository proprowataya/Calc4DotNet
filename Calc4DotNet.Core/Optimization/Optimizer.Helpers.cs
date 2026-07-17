using System.Collections.Immutable;
using System.Numerics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization;

public static partial class Optimizer
{
    // Whether the operator can be safely dropped when its value is unused, that is, it
    // has no observable side effect at runtime, cannot throw, and is not known to be
    // able to recurse forever.
    // The effectsMap argument enables UserDefinedOperator to be considered pure when its
    // body has no writes, output, input, throwing operation, or recursive cycle.
    // Passing null falls back to the conservative answer (UserDefinedOperator is
    // treated as impure).
    private static bool IsPure<TNumber>(IOperator op, ImmutableDictionary<string, PotentialEffects>? effectsMap = null)
        where TNumber : INumber<TNumber>
    {
        switch (op)
        {
            case ZeroOperator:
            case PreComputedOperator:
            case ArgumentOperator:
            case LetVariableOperator:
            case DefineOperator:
            case LoadVariableOperator:
                return true;
            case LoadArrayOperator loadArray:
                // DefaultArraySource returns zero for unset indices, so reads never throw.
                return IsPure<TNumber>(loadArray.Index, effectsMap);
            case InputOperator:
            case PrintCharOperator:
            case StoreVariableOperator:
            case StoreArrayOperator:
                return false;
            case ParenthesisOperator parenthesis:
                foreach (var inner in parenthesis.Operators)
                {
                    if (!IsPure<TNumber>(inner, effectsMap))
                    {
                        return false;
                    }
                }
                return true;
            case DecimalOperator dec:
                return IsPure<TNumber>(dec.Operand, effectsMap);
            case BinaryOperator binary:
                // Division / modulo may throw ZeroDivisionException unless the right side
                // is a constant known to be non-zero.
                if (binary.Type is BinaryType.Div or BinaryType.Mod && !IsNonZeroConstant<TNumber>(binary.Right))
                {
                    return false;
                }
                return IsPure<TNumber>(binary.Left, effectsMap) && IsPure<TNumber>(binary.Right, effectsMap);
            case ConditionalOperator conditional:
                return IsPure<TNumber>(conditional.Condition, effectsMap)
                    && IsPure<TNumber>(conditional.IfTrue, effectsMap)
                    && IsPure<TNumber>(conditional.IfFalse, effectsMap);
            case LetOperator let:
                return IsPure<TNumber>(let.Value, effectsMap) && IsPure<TNumber>(let.Body, effectsMap);
            case UserDefinedOperator userDefined:
                if (effectsMap is null
                    || !effectsMap.TryGetValue(userDefined.Definition.Name, out var effects)
                    || effects.HasAnyEffect)
                {
                    return false;
                }
                foreach (var operand in userDefined.Operands)
                {
                    if (!IsPure<TNumber>(operand, effectsMap))
                    {
                        return false;
                    }
                }
                return true;
            default:
                throw new InvalidOperationException();
        }
    }

    private static bool IsNonZeroConstant<TNumber>(IOperator op)
        where TNumber : INumber<TNumber>
    {
        return op is PreComputedOperator preComputed && !TNumber.IsZero((TNumber)preComputed.Value);
    }

    private static bool IsDefinitelyNonZeroConstant<TNumber>(IOperator op)
        where TNumber : INumber<TNumber>
    {
        return TryEvaluateLiteral<TNumber>(op, out var value) && !TNumber.IsZero(value);
    }

    private static bool TryEvaluateLiteral<TNumber>(IOperator op, out TNumber value)
        where TNumber : INumber<TNumber>
    {
        switch (op)
        {
            case ZeroOperator:
                value = TNumber.Zero;
                return true;
            case PreComputedOperator preComputed when preComputed.Value is TNumber precomputed:
                value = precomputed;
                return true;
            case DecimalOperator dec when TryEvaluateLiteral<TNumber>(dec.Operand, out var operand):
                value = operand * TNumber.CreateTruncating(10) + TNumber.CreateTruncating(dec.Value);
                return true;
            case ParenthesisOperator { Operators.Length: 1 } parenthesis:
                return TryEvaluateLiteral<TNumber>(parenthesis.Operators[0], out value);
            default:
                value = TNumber.Zero;
                return false;
        }
    }

    private static int CountNodes(IOperator op)
    {
        return CountNodesUpTo(op, int.MaxValue);
    }

    private static bool HasMoreThanNodes(IOperator op, int limit)
    {
        return CountNodesUpTo(op, limit + 1) > limit;
    }

    private static int CountNodesUpTo(IOperator op, int limit)
    {
        if (limit <= 0)
        {
            return 0;
        }

        int count = 1;

        if (op is ParenthesisOperator parenthesis)
        {
            foreach (var inner in parenthesis.Operators)
            {
                if (count >= limit)
                {
                    break;
                }

                count += CountNodesUpTo(inner, limit - count);
            }
        }

        op.ForEachOperand(operand =>
        {
            if (count < limit)
            {
                count += CountNodesUpTo(operand, limit - count);
            }
        });

        return count;
    }

    private static int FindNextLetLocalIndex(CompilationContext context)
    {
        int nextIndex = 0;

        foreach (var implement in context.OperatorImplements)
        {
            if (implement.Operator is not null)
            {
                nextIndex = Math.Max(nextIndex, FindNextLetLocalIndex(implement.Operator));
            }
        }

        return nextIndex;
    }

    private static int FindNextLetLocalIndex(IOperator op)
    {
        int nextIndex = op switch
        {
            LetOperator let => let.LocalIndex + 1,
            LetVariableOperator letVariable => letVariable.LocalIndex + 1,
            _ => 0,
        };

        if (op is ParenthesisOperator parenthesis)
        {
            foreach (var inner in parenthesis.Operators)
            {
                nextIndex = Math.Max(nextIndex, FindNextLetLocalIndex(inner));
            }
        }

        op.ForEachOperand(operand => nextIndex = Math.Max(nextIndex, FindNextLetLocalIndex(operand)));

        return nextIndex;
    }

    private static bool CanInlineAtUseSite<TNumber>(IOperator op)
        where TNumber : INumber<TNumber>
    {
        switch (op)
        {
            case ZeroOperator:
            case PreComputedOperator:
            case ArgumentOperator:
            case LetVariableOperator:
            case DefineOperator:
                return true;
            case ParenthesisOperator parenthesis:
                foreach (var inner in parenthesis.Operators)
                {
                    if (!CanInlineAtUseSite<TNumber>(inner))
                    {
                        return false;
                    }
                }
                return true;
            case DecimalOperator dec:
                return CanInlineAtUseSite<TNumber>(dec.Operand);
            case BinaryOperator binary:
                if (binary.Type is BinaryType.Div or BinaryType.Mod
                    && !IsDefinitelyNonZeroConstant<TNumber>(binary.Right))
                {
                    return false;
                }
                return CanInlineAtUseSite<TNumber>(binary.Left) && CanInlineAtUseSite<TNumber>(binary.Right);
            case ConditionalOperator conditional:
                return CanInlineAtUseSite<TNumber>(conditional.Condition)
                    && CanInlineAtUseSite<TNumber>(conditional.IfTrue)
                    && CanInlineAtUseSite<TNumber>(conditional.IfFalse);
            case LetOperator let:
                return CanInlineAtUseSite<TNumber>(let.Value) && CanInlineAtUseSite<TNumber>(let.Body);
            default:
                return false;
        }
    }

    private static ImmutableDictionary<int, int> CountLetVariableReferences(IOperator op, ImmutableHashSet<int> localIndices)
    {
        var counts = ImmutableDictionary.CreateBuilder<int, int>();
        CountLetVariableReferences(op, localIndices, counts);
        return counts.ToImmutable();
    }

    private static void CountLetVariableReferences(IOperator op,
                                                   ImmutableHashSet<int> localIndices,
                                                   ImmutableDictionary<int, int>.Builder counts)
    {
        if (localIndices.IsEmpty)
        {
            return;
        }

        if (op is LetVariableOperator letVariable && localIndices.Contains(letVariable.LocalIndex))
        {
            counts.TryGetValue(letVariable.LocalIndex, out int count);
            counts[letVariable.LocalIndex] = count + 1;
            return;
        }

        if (op is LetOperator let)
        {
            CountLetVariableReferences(let.Value, localIndices, counts);
            CountLetVariableReferences(let.Body, localIndices.Remove(let.LocalIndex), counts);
            return;
        }

        if (op is ParenthesisOperator parenthesis)
        {
            foreach (var inner in parenthesis.Operators)
            {
                CountLetVariableReferences(inner, localIndices, counts);
            }
        }

        op.ForEachOperand(operand => CountLetVariableReferences(operand, localIndices, counts));
    }

    private static IOperator RewriteChildren<TState>(IOperator op, TState state, Func<IOperator, TState, IOperator> rewrite)
    {
        switch (op)
        {
            case ZeroOperator:
            case PreComputedOperator:
            case ArgumentOperator:
            case LetVariableOperator:
            case DefineOperator:
            case LoadVariableOperator:
            case InputOperator:
                return op;
            case LoadArrayOperator loadArray:
                return loadArray with { Index = rewrite(loadArray.Index, state) };
            case PrintCharOperator printChar:
                return printChar with { Character = rewrite(printChar.Character, state) };
            case ParenthesisOperator parenthesis:
                return parenthesis with { Operators = RewriteOperators(parenthesis.Operators, state, rewrite) };
            case DecimalOperator dec:
                return dec with { Operand = rewrite(dec.Operand, state) };
            case StoreVariableOperator storeVariable:
                return storeVariable with { Operand = rewrite(storeVariable.Operand, state) };
            case StoreArrayOperator storeArray:
                return storeArray with
                {
                    Value = rewrite(storeArray.Value, state),
                    Index = rewrite(storeArray.Index, state),
                };
            case BinaryOperator binary:
                return binary with
                {
                    Left = rewrite(binary.Left, state),
                    Right = rewrite(binary.Right, state),
                };
            case ConditionalOperator conditional:
                return conditional with
                {
                    Condition = rewrite(conditional.Condition, state),
                    IfTrue = rewrite(conditional.IfTrue, state),
                    IfFalse = rewrite(conditional.IfFalse, state),
                };
            case LetOperator let:
                return let with
                {
                    Value = rewrite(let.Value, state),
                    Body = rewrite(let.Body, state),
                };
            case UserDefinedOperator userDefined:
                return userDefined with { Operands = RewriteOperators(userDefined.Operands, state, rewrite) };
            default:
                throw new InvalidOperationException();
        }
    }

    private static ImmutableArray<IOperator> RewriteOperators<TState>(ImmutableArray<IOperator> operators,
                                                                      TState state,
                                                                      Func<IOperator, TState, IOperator> rewrite)
    {
        var builder = ImmutableArray.CreateBuilder<IOperator>(operators.Length);

        foreach (var op in operators)
        {
            builder.Add(rewrite(op, state));
        }

        return builder.DrainToImmutable();
    }

    private static IOperator ReplaceLetVariables(IOperator op, ImmutableDictionary<int, IOperator> replacements)
    {
        if (replacements.IsEmpty)
        {
            return op;
        }

        switch (op)
        {
            case LetVariableOperator letVariable:
                return replacements.TryGetValue(letVariable.LocalIndex, out var replacement) ? replacement : letVariable;
            case LetOperator let:
                return let with
                {
                    Value = ReplaceLetVariables(let.Value, replacements),
                    Body = ReplaceLetVariables(let.Body, replacements.Remove(let.LocalIndex)),
                };
            default:
                return RewriteChildren(op, replacements, ReplaceLetVariables);
        }
    }

    private static IOperator ReplaceLetLocalIndices(IOperator op, ImmutableDictionary<int, int> replacements)
    {
        if (replacements.IsEmpty)
        {
            return op;
        }

        switch (op)
        {
            case LetVariableOperator letVariable:
                return replacements.TryGetValue(letVariable.LocalIndex, out int replacement)
                    ? letVariable with { LocalIndex = replacement }
                    : letVariable;
            case LetOperator let:
                int localIndex = replacements.TryGetValue(let.LocalIndex, out int localReplacement)
                    ? localReplacement
                    : let.LocalIndex;
                return let with
                {
                    LocalIndex = localIndex,
                    Value = ReplaceLetLocalIndices(let.Value, replacements),
                    Body = ReplaceLetLocalIndices(let.Body, replacements),
                };
            default:
                return RewriteChildren(op, replacements, ReplaceLetLocalIndices);
        }
    }

    // Replaces ArgumentOperator(i) with bindings[i] throughout op. UserDefinedOperator
    // bodies are NOT entered. Only the call's own operand expressions are traversed,
    // since the callee has its own argument scope.
    private static IOperator SubstituteArguments(IOperator op, ImmutableDictionary<int, IOperator> bindings)
    {
        if (bindings.IsEmpty)
        {
            return op;
        }

        switch (op)
        {
            case ArgumentOperator argument:
                return bindings.TryGetValue(argument.Index, out var replacement) ? replacement : argument;
            default:
                return RewriteChildren(op, bindings, SubstituteArguments);
        }
    }
}
