using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution;

public readonly struct LowLevelUserDefinedOperator : IEquatable<LowLevelUserDefinedOperator>
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

    public bool Equals(LowLevelUserDefinedOperator other)
    {
        return Definition.Equals(other.Definition) && Operations.SequenceEqual(other.Operations) && MaxStackSize == other.MaxStackSize;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is LowLevelUserDefinedOperator other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Definition);

        foreach (var op in Operations)
        {
            hash.Add(op);
        }

        hash.Add(MaxStackSize);
        return hash.ToHashCode();
    }
}
