using System.Numerics;
using System.Runtime.CompilerServices;

namespace Calc4DotNet.Core;

public interface IArraySource<TNumber>
    where TNumber : INumber<TNumber>
{
    TNumber this[TNumber index] { get; set; }
    IArraySource<TNumber> Clone();
}

public sealed class DefaultArraySource<TNumber> : IArraySource<TNumber>
    where TNumber : INumber<TNumber>
{
    private const int ArrayLength = 2048;
    private static TNumber BaseOffset => TNumber.CreateTruncating(-1024);

    private readonly TNumber[] array;
    private Dictionary<TNumber, TNumber>? dictionary;   // [index] = value

    public DefaultArraySource()
        : this(new TNumber[ArrayLength], null)
    { }

    private DefaultArraySource(TNumber[] array, Dictionary<TNumber, TNumber>? dictionary)
    {
        this.array = array;
        this.dictionary = dictionary;
    }

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
                dictionary ??= new();
                dictionary[index] = value;
            }
        }
    }

    public DefaultArraySource<TNumber> Clone()
    {
        return new DefaultArraySource<TNumber>((TNumber[])array.Clone(), dictionary is not null ? new(dictionary) : null);
    }

    IArraySource<TNumber> IArraySource<TNumber>.Clone()
    {
        return Clone();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryGetArrayIndex(TNumber index, out int arrayIndex)
    {
        TNumber offset = index - BaseOffset;

        if (offset >= TNumber.Zero && offset < TNumber.CreateTruncating(array.Length))
        {
            arrayIndex = NumberHelper.ConvertTruncating<TNumber, int>(offset);
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

    public IArraySource<TNumber> Clone() => this;
}
