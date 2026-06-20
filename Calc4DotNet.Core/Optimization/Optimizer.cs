using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization;

[Flags]
public enum OptimizeTarget
{
    None = 0,
    MainOperator = 1 << 0,
    UserDefinedOperators = 1 << 1,
    All = MainOperator | UserDefinedOperators,
}

public static partial class Optimizer
{
    private const int MaxPreEvaluationUserDefinedCalls = 100;

    // Maximum node count for a partially specialized body that can be inlined at a call site.
    // Setting this value too high can make the generated code much larger.
    private const int MaxInlineSize = 32;

    public static void Optimize<TNumber>(ref IOperator op,
                                         ref CompilationContext context,
                                         OptimizeTarget target,
                                         IVariableSource<TNumber>? initalVariableValues = null,
                                         IArraySource<TNumber>? initialGlobalArray = null)
        where TNumber : INumber<TNumber>
    {
        if (target == OptimizeTarget.None)
        {
            // There is nothing to do
            return;
        }

        HashSet<string?> allVariableNames = GatherVariableNames(op, context);
        ImmutableDictionary<string, PotentialEffects> effects = ComputePotentialEffects<TNumber>(context);
        PreComputeSession<TNumber> session = new();
        int nextLetLocalIndex = Math.Max(FindNextLetLocalIndex(context), FindNextLetLocalIndex(op));

        if (target.HasFlag(OptimizeTarget.UserDefinedOperators))
        {
            // Optimize user defined operators
            foreach (var implement in context.OperatorImplements)
            {
                if (!implement.IsOptimized)
                {
                    OptimizeUserDefinedOperator<TNumber>(implement, ref context, allVariableNames, effects, session, ref nextLetLocalIndex);
                }
            }
        }

        if (target.HasFlag(OptimizeTarget.MainOperator))
        {
            // Optimize main operator
            op = OptimizeCore<TNumber>(op, context, allVariableNames, initalVariableValues, initialGlobalArray, effects, session, ref nextLetLocalIndex);
        }
    }

    private static void OptimizeUserDefinedOperator<TNumber>(OperatorImplement implement,
                                                             ref CompilationContext context,
                                                             HashSet<string?> allVariableNames,
                                                             ImmutableDictionary<string, PotentialEffects> effects,
                                                             PreComputeSession<TNumber> session,
                                                             ref int nextLetLocalIndex)
        where TNumber : INumber<TNumber>
    {
        Debug.Assert(!implement.IsOptimized);

        var op = implement.Operator;
        Debug.Assert(op is not null);
        // User-defined operators are invoked in the middle of a program.
        // Their caller's variable and array states are inherited and cannot be assumed.
        var newRoot = OptimizeCore<TNumber>(op,
                                            context,
                                            allVariableNames,
                                            initalVariableValues: null,
                                            initialGlobalArray: null,
                                            effects,
                                            session,
                                            ref nextLetLocalIndex);
        context = context.WithAddOrUpdateOperatorImplement(implement with { Operator = newRoot, IsOptimized = true });
    }

    private static IOperator OptimizeCore<TNumber>(IOperator op,
                                                   CompilationContext context,
                                                   HashSet<string?> allVariableNames,
                                                   IVariableSource<TNumber>? initalVariableValues,
                                                   IArraySource<TNumber>? initialGlobalArray,
                                                   ImmutableDictionary<string, PotentialEffects> effects,
                                                   PreComputeSession<TNumber> session,
                                                   ref int nextLetLocalIndex)
        where TNumber : INumber<TNumber>
    {
        var state = PreComputeState<TNumber>.Create(arraysZeroInitialized: initialGlobalArray is not null);

        if (initalVariableValues is not null)
        {
            foreach (var variableName in allVariableNames)
            {
                if (initalVariableValues.TryGet(variableName, out var value))
                {
                    state = state.SetVariable(variableName, value);
                }
            }
        }

        if (initialGlobalArray is not null)
        {
            foreach (var (index, value) in initialGlobalArray.ToImmutableDictionary())
            {
                state = state.SetArrayValue(index, value);
            }
        }

        // Evaluate expressions that can be resolved before execution.
        var budget = new UserDefinedCallBudget(MaxPreEvaluationUserDefinedCalls);
        var frame = new PreComputeFrame<TNumber>(state, budget, []);
        var visitor = new PreComputeVisitor<TNumber>(context, effects, session, nextLetLocalIndex);
        var result = visitor.Evaluate(op, frame);
        op = result.Operator;
        nextLetLocalIndex = visitor.NextLetLocalIndex;

        // Mark calls that are in tail position for later execution.
        op = op.Accept(new TailCallVisitor(), /* isTailCall */ true);

        return op;
    }

    private static HashSet<string?> GatherVariableNames(IOperator op, CompilationContext context)
    {
        static void Process(IOperator op, HashSet<string?> variables)
        {
            op.ForEachOperand(operand => Process(operand, variables));

            switch (op)
            {
                case LoadVariableOperator load:
                    variables.Add(load.VariableName);
                    break;
                case StoreVariableOperator store:
                    variables.Add(store.VariableName);
                    break;
                case ParenthesisOperator parenthesis:
                    foreach (var inner in parenthesis.Operators)
                    {
                        Process(inner, variables);
                    }
                    break;
                default:
                    break;
            }
        }

        HashSet<string?> variables = [];

        // Process main operator
        Process(op, variables);

        // Process user defined operators
        foreach (var implement in context.OperatorImplements)
        {
            Process(implement.Operator ?? throw new InvalidOperationException("Implement is null"), variables);
        }

        return variables;
    }
}
