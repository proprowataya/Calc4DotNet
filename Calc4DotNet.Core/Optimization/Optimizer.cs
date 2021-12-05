﻿using System.Diagnostics;
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

    public static void Optimize<TNumber>(ref IOperator op, ref CompilationContext context, OptimizeTarget target)
        where TNumber : notnull
    {
        if (target == OptimizeTarget.None)
        {
            // There is nothing to do
            return;
        }

        HashSet<string?> variables = GatherVariableNames(op, context);

        if (target.HasFlag(OptimizeTarget.UserDefinedOperators))
        {
            // Optimize user defined operators
            foreach (var implement in context.OperatorImplements)
            {
                if (!implement.IsOptimized)
                {
                    OptimizeUserDefinedOperator<TNumber>(implement, ref context, variables);
                }
            }
        }

        if (target.HasFlag(OptimizeTarget.MainOperator))
        {
            // Optimize main operator
            op = OptimizeCore<TNumber>(op, context, variables, inMain: true);
        }
    }

    private static void OptimizeUserDefinedOperator<TNumber>(OperatorImplement implement, ref CompilationContext context, HashSet<string?> variables)
        where TNumber : notnull
    {
        Debug.Assert(!implement.IsOptimized);

        var op = implement.Operator;
        Debug.Assert(op is not null);
        var newRoot = OptimizeCore<TNumber>(op, context, variables, inMain: false);
        context = context.WithAddOrUpdateOperatorImplement(implement with { Operator = newRoot, IsOptimized = true });
    }

    private static IOperator OptimizeCore<TNumber>(IOperator op, CompilationContext context, HashSet<string?> variables, bool inMain)
        where TNumber : notnull
    {
        op = op.Accept(new PreComputeVisitor<TNumber>(context, MaxPreEvaluationStep),
                       new OptimizeTimeEvaluationState<TNumber>(variables,
                                                                inMain ? variables : null,  // If we are in main operator, all variables have default value. Otherwise, their values are unknown.
                                                                (TNumber)(dynamic)0));
        op = op.Accept(new TailCallVisitor(), /* isTailCallable */ true);
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
                case LoadOperator load:
                    variables.Add(load.VariableName);
                    break;
                case StoreOperator store:
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
