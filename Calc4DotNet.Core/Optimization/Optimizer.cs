using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static class Optimizer
    {
        public static IOperator Optimize(IOperator op, Context context)
        {
            if (op.IsPreComputable())
            {
                return new PreComputedOperator(op.Evaluate(context, default));
            }

            return op;
        }
    }
}
