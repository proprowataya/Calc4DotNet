using System.Collections.Immutable;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution
{
    public readonly struct LowLevelUserDefinedOperator
    {
        public OperatorDefinition Definition { get; }
        public ImmutableArray<LowLevelOperation> Operations { get; }

        public LowLevelUserDefinedOperator(OperatorDefinition definition, ImmutableArray<LowLevelOperation> operations)
        {
            Definition = definition;
            Operations = operations;
        }
    }
}
