using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        public static void OptimizeUserDefinedOperators<TNumber>(Context<TNumber> context)
        {
            foreach (var definition in context.OperatorDefinitions)
            {
                OptimizeUserDefinedOperator(definition, context);
            }
        }

        public static IOperator<TNumber> Optimize<TNumber>(IOperator<TNumber> op, Context<TNumber> context)
        {
            OptimizeUserDefinedOperators(context);
            return OptimizeCore(op, context, isDefinition: false);
        }

        private static void OptimizeUserDefinedOperator<TNumber>(OperatorDefinition definition, Context<TNumber> context)
        {
            var op = context.LookUpOperatorImplement(definition.Name);
            var newRoot = OptimizeCore(op, context, isDefinition: true);
            context.AddOrUpdateOperatorImplement(definition.Name, newRoot);
        }

        private static IOperator<TNumber> OptimizeCore<TNumber>(IOperator<TNumber> op, Context<TNumber> context, bool isDefinition)
        {
            op = op.Accept(new PreEvaluateVisitor<TNumber>(context, isDefinition));
            op = op.Accept(new TailCallVisitor<TNumber>(), /* isTailCallable */ true);
            return op;
        }
    }
}
