using System.Diagnostics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        private const int MaxPreEvaluationStep = 100;

        public static void OptimizeUserDefinedOperators<TNumber>(ref CompilationContext context)
            where TNumber : INumber<TNumber>
        {
            foreach (var implement in context.OperatorImplements)
            {
                OptimizeUserDefinedOperator<TNumber>(implement, ref context);
            }
        }

        public static void Optimize<TNumber>(ref IOperator op, ref CompilationContext context)
            where TNumber : INumber<TNumber>
        {
            OptimizeUserDefinedOperators<TNumber>(ref context);
            op = OptimizeCore<TNumber>(op, context);
        }

        private static void OptimizeUserDefinedOperator<TNumber>(OperatorImplement implement, ref CompilationContext context)
            where TNumber : INumber<TNumber>
        {
            var op = implement.Operator;
            Debug.Assert(op is not null);
            var newRoot = OptimizeCore<TNumber>(op, context);
            context = context.WithAddOrUpdateOperatorImplement(implement with { Operator = newRoot });
        }

        private static IOperator OptimizeCore<TNumber>(IOperator op, CompilationContext context)
            where TNumber : INumber<TNumber>
        {
            op = op.Accept(new PreComputeVisitor<TNumber>(context, MaxPreEvaluationStep));
            op = op.Accept(new TailCallVisitor(), /* isTailCallable */ true);
            return op;
        }
    }
}
