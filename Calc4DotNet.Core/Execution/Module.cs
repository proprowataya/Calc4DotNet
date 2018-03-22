using System.Collections.Immutable;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution
{
    public sealed class Module<TNumber>
    {
        public ImmutableArray<LowLevelOperation> Operations { get; }
        public ImmutableArray<TNumber> ConstTable { get; }
        public ImmutableArray<(OperatorDefinition Definition, int StartAddress, int Length)> UserDefinedOperators { get; }

        public Module(ImmutableArray<LowLevelOperation> operations, ImmutableArray<TNumber> constTable, ImmutableArray<(OperatorDefinition Definition, int StartAddress, int Length)> userDefinedOperators)
        {
            Operations = operations;
            ConstTable = constTable;
            UserDefinedOperators = userDefinedOperators;
        }
    }
}
