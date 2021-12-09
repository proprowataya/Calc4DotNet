using System.Runtime.CompilerServices;

namespace Calc4DotNet.Core;

public interface IGlobalArraySource<TNumber>
{
    TNumber this[int index] { get; set; }
    IGlobalArraySource<TNumber> Clone();
}

public sealed class Calc4GlobalArraySource<TNumber> : IGlobalArraySource<TNumber>
{
    private const int ArrayLength = 2048;
    private const int BaseOffset = -1024;

    private readonly TNumber[] array;
    private Dictionary<int, TNumber>? dictionary;   // [index] = value

    public Calc4GlobalArraySource()
        : this(new TNumber[ArrayLength], null)
    { }

    public Calc4GlobalArraySource(TNumber[] array, Dictionary<int, TNumber>? dictionary)
    {
        this.array = array;
        this.dictionary = dictionary;
    }

    public TNumber this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index - BaseOffset is int arrayIndex && arrayIndex < array.Length)
            {
                return array[arrayIndex];
            }
            else
            {
                if (dictionary is not null && dictionary.TryGetValue(index, out var value))
                {
                    return value;
                }
                else
                {
                    return default!;    // TODO
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (index - BaseOffset is int arrayIndex && arrayIndex < array.Length)
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

    public Calc4GlobalArraySource<TNumber> Clone()
    {
        return new Calc4GlobalArraySource<TNumber>((TNumber[])array.Clone(), dictionary is not null ? new(dictionary) : null);
    }

    IGlobalArraySource<TNumber> IGlobalArraySource<TNumber>.Clone()
    {
        return Clone();
    }
}

internal sealed class ArrayElementNotSetException : Exception
{ }

internal sealed class AlwaysThrowGlobalArraySource<TNumber> : IGlobalArraySource<TNumber>
{
    public static readonly AlwaysThrowGlobalArraySource<TNumber> Instance = new();

    private AlwaysThrowGlobalArraySource()
    { }

    public TNumber this[int index]
    {
        get => throw new ArrayElementNotSetException();
        set => throw new ArrayElementNotSetException();
    }

    public IGlobalArraySource<TNumber> Clone() => this;
}
