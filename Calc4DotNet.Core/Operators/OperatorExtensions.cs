using System.Collections.Generic;
using System.Collections.Immutable;

namespace Calc4DotNet.Core.Operators
{
    public static class OperatorExtensions
    {
        public static bool IsPreComputable(this IOperator op)
        {
            HashSet<IOperator> scanned = new HashSet<IOperator>();

            bool Core(IOperator current)
            {
                if (scanned.Contains(current))
                    return false;
                if (!current.ThisTypeIsPreComputable)
                    return false;

                scanned.Add(current);

                ImmutableArray<IOperator> operands = current.Operands;
                for (int i = 0; i < operands.Length; i++)
                {
                    if (!Core(operands[i]))
                        return false;
                }

                if (current is UserDefinedOperator userDefined)
                {
                    if (!Core(userDefined.Definition.Root))
                        return false;
                }

                return true;
            }

            return Core(op);
        }
    }
}
