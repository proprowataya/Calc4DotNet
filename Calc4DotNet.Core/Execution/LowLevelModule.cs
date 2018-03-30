using System.Collections.Immutable;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution
{
    public sealed class LowLevelModule<TNumber>
    {
        public ImmutableArray<LowLevelOperation> EntryPoint { get; }
        public ImmutableArray<TNumber> ConstTable { get; }
        public ImmutableArray<(OperatorDefinition Definition, ImmutableArray<LowLevelOperation> Operations)> UserDefinedOperators { get; }

        public LowLevelModule(ImmutableArray<LowLevelOperation> entryPoint, ImmutableArray<TNumber> constTable, ImmutableArray<(OperatorDefinition Definition, ImmutableArray<LowLevelOperation> Operations)> userDefinedOperators)
        {
            EntryPoint = entryPoint;
            ConstTable = constTable;
            UserDefinedOperators = userDefinedOperators;
        }
    }
}
