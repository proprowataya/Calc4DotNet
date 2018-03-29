using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        public static void OptimizeUserDefinedOperators<TNumber>(ref CompilationContext<TNumber> context)
        {
            foreach (var implement in context.OperatorImplements)
            {
                OptimizeUserDefinedOperator(implement, ref context);
            }
        }

        public static IOperator<TNumber> Optimize<TNumber>(IOperator<TNumber> op, ref CompilationContext<TNumber> context)
        {
            OptimizeUserDefinedOperators(ref context);
            return OptimizeCore(op, context, isDefinition: false);
        }

        private static void OptimizeUserDefinedOperator<TNumber>(OperatorImplement<TNumber> implement, ref CompilationContext<TNumber> context)
        {
            var op = implement.Operator;
            var newRoot = OptimizeCore(op, context, isDefinition: true);
            context = context.WithAddOrUpdateOperatorImplement(implement.WithOperator(newRoot));
        }

        private static IOperator<TNumber> OptimizeCore<TNumber>(IOperator<TNumber> op, CompilationContext<TNumber> context, bool isDefinition)
        {
            op = op.Accept(new PreEvaluateVisitor<TNumber>(context, isDefinition));
            op = op.Accept(new TailCallVisitor<TNumber>(), /* isTailCallable */ true);
            return op;
        }
    }
}
