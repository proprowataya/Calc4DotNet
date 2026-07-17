using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Calc4DotNet.Core;

public interface IArraySource<TNumber>
    where TNumber : INumber<TNumber>
{
    TNumber this[TNumber index] { get; set; }

    bool TryGet(TNumber index, out TNumber value);
    bool TrySet(TNumber index, TNumber value);

    // Returns a snapshot of all observable non-zero array values.
    // Zero-valued entries may be omitted and are interpreted as zero by optimizers.
    ImmutableDictionary<TNumber, TNumber> ToImmutableDictionary();
}

public sealed class DefaultArraySource<TNumber> : IArraySource<TNumber>
    where TNumber : INumber<TNumber>
{
    private const int ArrayLength = 2048;
    private static TNumber BaseOffset => TNumber.CreateTruncating(-1024);

    private readonly TNumber[] array = new TNumber[ArrayLength];
    private Dictionary<TNumber, TNumber>? dictionary = null;    // [index] = value

    public TNumber this[TNumber index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (TryGetArrayIndex(index, out var arrayIndex))
            {
                return array[arrayIndex];
            }
            else if (dictionary is not null && dictionary.TryGetValue(index, out var value))
            {
                return value;
            }
            else
            {
                return TNumber.Zero;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (TryGetArrayIndex(index, out var arrayIndex))
            {
                array[arrayIndex] = value;
            }
            else
            {
                dictionary ??= [];
                dictionary[index] = value;
            }
        }
    }

    public bool TryGet(TNumber index, out TNumber value)
    {
        value = this[index];
        return true;
    }

    public bool TrySet(TNumber index, TNumber value)
    {
        this[index] = value;
        return true;
    }

    public ImmutableDictionary<TNumber, TNumber> ToImmutableDictionary()
    {
        // Calc4's memory is conceptually pre-populated with zero for every
        // index, so "bound to zero" and "never bound" are externally indistinguishable.
        // Drop zero entries so the snapshot reflects observable state only.

        var builder = ImmutableDictionary.CreateBuilder<TNumber, TNumber>();

        for (int i = 0; i < array.Length; i++)
        {
            TNumber value = array[i];

            if (!TNumber.IsZero(value))
            {
                TNumber index = TNumber.CreateTruncating(i) + BaseOffset;
                builder[index] = value;
            }
        }

        if (dictionary is not null)
        {
            foreach (var (index, value) in dictionary)
            {
                if (!TNumber.IsZero(value))
                {
                    builder[index] = value;
                }
            }
        }

        return builder.ToImmutable();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryGetArrayIndex(TNumber index, out int arrayIndex)
    {
        TNumber offset = index - BaseOffset;

        if (offset >= TNumber.Zero && offset < TNumber.CreateTruncating(array.Length))
        {
            arrayIndex = Int32.CreateTruncating(offset);
            return true;
        }
        else
        {
            arrayIndex = default;
            return false;
        }
    }
}

internal sealed class ArrayElementNotSetException : Exception
{ }

internal sealed class AlwaysThrowGlobalArraySource<TNumber> : IArraySource<TNumber>
    where TNumber : INumber<TNumber>
{
    public static readonly AlwaysThrowGlobalArraySource<TNumber> Instance = new();

    private AlwaysThrowGlobalArraySource()
    { }

    public TNumber this[TNumber index]
    {
        get => throw new ArrayElementNotSetException();
        set => throw new ArrayElementNotSetException();
    }

    public bool TryGet(TNumber index, out TNumber value)
    {
        value = TNumber.Zero;
        return false;
    }

    public bool TrySet(TNumber index, TNumber value)
    {
        return false;
    }

    public ImmutableDictionary<TNumber, TNumber> ToImmutableDictionary()
    {
        throw new ArrayElementNotSetException();
    }
}
