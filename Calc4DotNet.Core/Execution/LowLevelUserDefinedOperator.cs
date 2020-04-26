using System.Collections.Immutable;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution
{
    public readonly struct LowLevelUserDefinedOperator
    {
        public OperatorDefinition Definition { get; }
        public ImmutableArray<LowLevelOperation> Operations { get; }
        public int MaxStackSize { get; }

        public LowLevelUserDefinedOperator(OperatorDefinition definition, ImmutableArray<LowLevelOperation> operations, int maxStackSize)
        {
            Definition = definition;
            Operations = operations;
            MaxStackSize = maxStackSize;
        }
    }
}
