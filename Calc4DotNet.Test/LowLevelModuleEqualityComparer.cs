﻿using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Calc4DotNet.Core.Execution;

namespace Calc4DotNet.Test;

internal sealed class LowLevelModuleEqualityComparer<TNumber> : IEqualityComparer<LowLevelModule<TNumber>>
    where TNumber : INumber<TNumber>
{
    public static readonly LowLevelModuleEqualityComparer<TNumber> Instance = new();

    private LowLevelModuleEqualityComparer()
    { }

    public bool Equals(LowLevelModule<TNumber>? x, LowLevelModule<TNumber>? y)
    {
        return (x, y) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            _ => x.EntryPoint.SequenceEqual(y.EntryPoint)
                 && x.ConstTable.SequenceEqual(y.ConstTable)
                 && x.UserDefinedOperators.SequenceEqual(y.UserDefinedOperators)
                 && x.Variables.SequenceEqual(y.Variables),
        };
    }

    public int GetHashCode([DisallowNull] LowLevelModule<TNumber> obj)
    {
        throw new NotSupportedException();
    }
}
