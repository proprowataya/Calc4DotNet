using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        private const int MaxPreEvaluationStep = 100;

        public static void OptimizeUserDefinedOperators<TNumber>(ref CompilationContext<TNumber> context)
        {
            foreach (var implement in context.OperatorImplements)
            {
                OptimizeUserDefinedOperator(implement, ref context);
            }
        }

        public static void Optimize<TNumber>(ref IOperator<TNumber> op, ref CompilationContext<TNumber> context)
        {
            OptimizeUserDefinedOperators(ref context);
            op = OptimizeCore(op, context);
        }

        private static void OptimizeUserDefinedOperator<TNumber>(OperatorImplement<TNumber> implement, ref CompilationContext<TNumber> context)
        {
            var op = implement.Operator;
            var newRoot = OptimizeCore(op, context);
            context = context.WithAddOrUpdateOperatorImplement(implement.WithOperator(newRoot));
        }

        private static IOperator<TNumber> OptimizeCore<TNumber>(IOperator<TNumber> op, CompilationContext<TNumber> context)
        {
            op = op.Accept(new PreComputeVisitor<TNumber>(context, MaxPreEvaluationStep));
            op = op.Accept(new TailCallVisitor<TNumber>(), /* isTailCallable */ true);
            return op;
        }
    }
}
