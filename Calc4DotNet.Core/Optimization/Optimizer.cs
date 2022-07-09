using System.Diagnostics;
using System.Numerics;
using Calc4DotNet.Core.Evaluation;
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
    private const int MaxPreEvaluationStep = 100;

    public static void Optimize<TNumber>(ref IOperator op,
                                         ref CompilationContext context,
                                         OptimizeTarget target,
                                         IVariableSource<TNumber>? initalVariableValues = null)
        where TNumber : INumber<TNumber>
    {
        if (target == OptimizeTarget.None)
        {
            // There is nothing to do
            return;
        }

        HashSet<string?> allVariableNames = GatherVariableNames(op, context);

        if (target.HasFlag(OptimizeTarget.UserDefinedOperators))
        {
            // Optimize user defined operators
            foreach (var implement in context.OperatorImplements)
            {
                if (!implement.IsOptimized)
                {
                    OptimizeUserDefinedOperator<TNumber>(implement, ref context, allVariableNames);
                }
            }
        }

        if (target.HasFlag(OptimizeTarget.MainOperator))
        {
            // Optimize main operator
            op = OptimizeCore<TNumber>(op, context, allVariableNames, initalVariableValues);
        }
    }

    private static void OptimizeUserDefinedOperator<TNumber>(OperatorImplement implement,
                                                             ref CompilationContext context,
                                                             HashSet<string?> allVariableNames)
        where TNumber : INumber<TNumber>
    {
        Debug.Assert(!implement.IsOptimized);

        var op = implement.Operator;
        Debug.Assert(op is not null);
        var newRoot = OptimizeCore<TNumber>(op, context, allVariableNames, initalVariableValues: null);
        context = context.WithAddOrUpdateOperatorImplement(implement with { Operator = newRoot, IsOptimized = true });
    }

    private static IOperator OptimizeCore<TNumber>(IOperator op,
                                                   CompilationContext context,
                                                   HashSet<string?> allVariableNames,
                                                   IVariableSource<TNumber>? initalVariableValues)
        where TNumber : INumber<TNumber>
    {
        // Create a dictionary given to OptimizeTimeEvaluationState
        Dictionary<ValueBox<string>, TNumber> dictionary = new();

        if (initalVariableValues is not null)
        {
            foreach (var variableName in allVariableNames)
            {
                if (initalVariableValues.TryGet(variableName, out var value))
                {
                    dictionary[ValueBox.Create(variableName)] = value;
                }
            }
        }

        op = op.Accept(new PreComputeVisitor<TNumber>(context, MaxPreEvaluationStep),
                       new OptimizeTimeEvaluationState<TNumber>(dictionary, allVariableNames));
        op = op.Accept(new TailCallVisitor(), /* isTailCall */ true);
        return op;
    }

    private static HashSet<string?> GatherVariableNames(IOperator op, CompilationContext context)
    {
        static void Process(IOperator op, HashSet<string?> variables)
        {
            foreach (var operand in op.GetOperands())
            {
                Process(operand, variables);
            }

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

        HashSet<string?> variables = new();

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
