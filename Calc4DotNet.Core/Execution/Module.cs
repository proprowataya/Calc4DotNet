using System.Collections.Immutable;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution
{
    public sealed class Module
    {
        public ImmutableArray<LowLevelOperation> Operations { get; }
        public ImmutableArray<Number> ConstTable { get; }
        public ImmutableArray<(OperatorDefinition Definition, int Address)> OperatorStartAddresses { get; }

        public Module(ImmutableArray<LowLevelOperation> operations, ImmutableArray<Number> constTable, ImmutableArray<(OperatorDefinition Definition, int Address)> operatorStartAddresses)
        {
            Operations = operations;
            ConstTable = constTable;
            OperatorStartAddresses = operatorStartAddresses;
        }
    }
}
