using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        private const int MaxPreEvaluationStep = 100;

        public static void OptimizeUserDefinedOperators<TNumber>(ref CompilationContext context)
        {
            foreach (var implement in context.OperatorImplements)
            {
                OptimizeUserDefinedOperator<TNumber>(implement, ref context);
            }
        }

        public static void Optimize<TNumber>(ref IOperator op, ref CompilationContext context)
        {
            OptimizeUserDefinedOperators<TNumber>(ref context);
            op = OptimizeCore<TNumber>(op, context);
        }

        private static void OptimizeUserDefinedOperator<TNumber>(OperatorImplement implement, ref CompilationContext context)
        {
            var op = implement.Operator;
            var newRoot = OptimizeCore<TNumber>(op, context);
            context = context.WithAddOrUpdateOperatorImplement(implement with { Operator = newRoot });
        }

        private static IOperator OptimizeCore<TNumber>(IOperator op, CompilationContext context)
        {
            op = op.Accept(new PreComputeVisitor<TNumber>(context, MaxPreEvaluationStep));
            op = op.Accept(new TailCallVisitor(), /* isTailCallable */ true);
            return op;
        }
    }
}
