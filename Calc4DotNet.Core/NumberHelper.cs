using System.Numerics;
using System.Runtime.CompilerServices;

namespace Calc4DotNet.Core;

internal static class NumberHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo ConvertTruncating<TFrom, TTo>(TFrom value)
        where TFrom : INumber<TFrom>
        where TTo : INumber<TTo>
    {
        return TTo.CreateTruncating(value);
    }
}
