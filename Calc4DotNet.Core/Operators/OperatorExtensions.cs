using System.Collections.Immutable;

namespace Calc4DotNet.Core.Operators
{
    public static class OperatorExtensions
    {
        public static bool IsPreComputable(this IOperator op)
        {
            if (!op.ThisTypeIsPreComputable)
                return false;

            ImmutableArray<IOperator> operands = op.Operands;
            for (int i = 0; i < operands.Length; i++)
            {
                if (!operands[i].ThisTypeIsPreComputable)
                    return false;
            }

            return true;
        }
    }
}
