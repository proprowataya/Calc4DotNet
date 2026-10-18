using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Calc4DotNet.Core.Exceptions;

namespace Calc4DotNet.Core;

public interface IArraySource<TNumber>
    where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
{
    TNumber this[TNumber index] { get; set; }

    bool TryGet(TNumber index, out TNumber value);
    bool TrySet(TNumber index, TNumber value);

    // Returns a snapshot of all observable non-zero array values.
    // Zero-valued entries may be omitted and are interpreted as zero by optimizers.
    ImmutableDictionary<TNumber, TNumber> ToImmutableDictionary();
}

/// <summary>
/// Stores indices near zero in a fixed array and all other indices in adaptive pages.
/// All indices default to zero.
/// </summary>
public sealed class DefaultArraySource<TNumber> : IArraySource<TNumber>
    where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
{
    // Central storage covers a range centered on zero.
    internal const int CentralArrayLength = 2048;
    private static TNumber CentralArrayStart => TNumber.CreateTruncating(-CentralArrayLength / 2);

    // Each page uses the low bits of an index as its offset.
    private const int PageOffsetBits = 10;
    internal const int PageLength = 1 << PageOffsetBits;

    // Must be a power of two.
    internal const int PageCacheLength = 64;

    // Nonzero element count that triggers promotion to dense storage.
    internal const int DenseElementThreshold = 64;

    // Recorded access count that triggers promotion to dense storage.
    internal const int DenseAccessThreshold = 1024;

    // The central range avoids all page lookup overhead.
    private readonly TNumber[] array = new TNumber[CentralArrayLength];

    // These pages contain indices outside the central range.
    private readonly Dictionary<TNumber, AdaptivePage> pages = [];

    // This direct mapped cache stores recently resolved pages.
    private readonly AdaptivePage?[] pageCache = new AdaptivePage?[PageCacheLength];

    // This cache stores the result of the most recent sparse array access.
    // The initial zero index is handled by central storage before this cache is checked.
    private TNumber cachedSparseIndex = TNumber.Zero;
    private TNumber cachedSparseValue = TNumber.Zero;

    public DefaultArraySource()
    {
        // SByte cannot represent the page offset mask.
        if (typeof(TNumber) == typeof(sbyte))
        {
            throw new TypeNotSupportedException(typeof(TNumber));
        }
    }

    public TNumber this[TNumber index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (TryGetArrayIndex(index, out var arrayIndex))
            {
                Debug.Assert((uint)arrayIndex < (uint)array.Length);
                return Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(array), arrayIndex);
            }

            // Return repeated sparse reads before entering the noninlined lookup path.
            if (index == cachedSparseIndex)
            {
                return cachedSparseValue;
            }

            return GetSparseValue(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (TryGetArrayIndex(index, out var arrayIndex))
            {
                Debug.Assert((uint)arrayIndex < (uint)array.Length);
                Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(array), arrayIndex) = value;
                return;
            }

            SetSparseValue(index, value);
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
        // Calc4's memory is conceptually pre-populated with zero for every index,
        // so "bound to zero" and "never bound" are externally indistinguishable.
        // Drop zero entries so the snapshot reflects observable state only.

        var builder = ImmutableDictionary.CreateBuilder<TNumber, TNumber>();

        for (int i = 0; i < array.Length; i++)
        {
            TNumber value = array[i];

            if (!TNumber.IsZero(value))
            {
                TNumber index = TNumber.CreateTruncating(i) + CentralArrayStart;
                builder[index] = value;
            }
        }

        foreach (var page in pages.Values)
        {
            page.CopyTo(builder);
        }

        return builder.ToImmutable();
    }

    // A successful result guarantees that arrayIndex is safe for unchecked central array access.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryGetArrayIndex(TNumber index, out int arrayIndex)
    {
        TNumber offset = index - CentralArrayStart;

        if (offset >= TNumber.Zero && offset < TNumber.CreateTruncating(CentralArrayLength))
        {
            arrayIndex = int.CreateTruncating(offset);
            return true;
        }
        else
        {
            arrayIndex = default;
            return false;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private TNumber GetSparseValue(TNumber index)
    {
        var (page, _, pageOffset, _) = ResolvePage(index);
        TNumber value = page is null ? TNumber.Zero : page.GetValue(pageOffset);
        CacheSparseValue(index, value);
        return value;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void SetSparseValue(TNumber index, TNumber value)
    {
        var (page, pageIndex, pageOffset, cacheIndex) = ResolvePage(index);

        if (page is null)
        {
            // Writing zero to an absent page does not allocate storage or evict a cached page.
            if (TNumber.IsZero(value))
            {
                CacheSparseValue(index, value);
                return;
            }

            page = new AdaptivePage(pageIndex);
            pages.Add(pageIndex, page);
        }

        page.SetValue(pageOffset, value);

        // Remove pages once all their values are zero.
        if (page.IsEmpty)
        {
            pages.Remove(pageIndex);
            page = null;
        }

        // Cache only the final state after possible page creation or removal.
        pageCache[cacheIndex] = page;

        CacheSparseValue(index, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private (AdaptivePage? Page, TNumber PageIndex, int PageOffset, int CacheIndex) ResolvePage(TNumber index)
    {
        // Bits at positions PageOffsetBits and above form the signed pageIndex.
        // Bits below PageOffsetBits form pageOffset, the index within the page.
        // The lower bits of pageIndex select a slot in the power-of-two page cache.
        TNumber pageIndex = index >> PageOffsetBits;
        TNumber pageOffsetMask = TNumber.CreateTruncating(PageLength - 1);
        int pageOffset = int.CreateTruncating(index & pageOffsetMask);
        TNumber cacheMask = TNumber.CreateTruncating(PageCacheLength - 1);
        int cacheIndex = int.CreateTruncating(pageIndex & cacheMask);

        AdaptivePage? page = pageCache[cacheIndex];

        if (page is not null && page.PageIndex == pageIndex)
        {
            return (page, pageIndex, pageOffset, cacheIndex);
        }

        if (pages.TryGetValue(pageIndex, out page))
        {
            pageCache[cacheIndex] = page;
        }

        return (page, pageIndex, pageOffset, cacheIndex);
    }

    // Keep all fields of the exact sparse value cache synchronized.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CacheSparseValue(TNumber index, TNumber value)
    {
        cachedSparseIndex = index;
        cachedSparseValue = value;
    }

    // A page starts with sparse dictionary storage and permanently promotes to dense array storage.
    private sealed class AdaptivePage
    {
        // The page index rejects collisions in the direct mapped cache.
        public TNumber PageIndex { get; }

        // Exactly one of sparseValues and denseValues is non-null.
        private Dictionary<int, TNumber>? sparseValues = [];
        private TNumber[]? denseValues = null;

        private int sparseAccessCount = 0;
        private int denseNonZeroCount = 0;

        public AdaptivePage(TNumber pageIndex)
        {
            PageIndex = pageIndex;
        }

        public bool IsEmpty => denseValues is null ? sparseValues is { Count: 0 } : denseNonZeroCount == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TNumber GetValue(int index)
        {
            if (denseValues is not null)
            {
                return denseValues[index];
            }

            Debug.Assert(sparseValues is not null);
            TNumber value = sparseValues.TryGetValue(index, out TNumber? storedValue) ? storedValue : TNumber.Zero;
            RecordSparseAccess();
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(int index, TNumber value)
        {
            if (denseValues is not null)
            {
                bool wasZero = TNumber.IsZero(denseValues[index]);
                bool isZero = TNumber.IsZero(value);

                if (wasZero != isZero)
                {
                    denseNonZeroCount += isZero ? -1 : 1;
                }

                denseValues[index] = value;
                return;
            }

            Debug.Assert(sparseValues is not null);

            // Zero is represented by the absence of a sparse dictionary entry.
            if (TNumber.IsZero(value))
            {
                sparseValues.Remove(index);
                return;
            }

            sparseValues[index] = value;
            RecordSparseAccess();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RecordSparseAccess()
        {
            Debug.Assert(sparseValues is not null);
            sparseAccessCount++;

            if (sparseValues.Count >= DenseElementThreshold || sparseAccessCount >= DenseAccessThreshold)
            {
                PromoteToDense();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void PromoteToDense()
        {
            // Dense pages are never demoted.
            TNumber[] values = new TNumber[PageLength];
            Debug.Assert(sparseValues is not null);

            foreach (var (index, value) in sparseValues)
            {
                values[index] = value;
            }

            denseValues = values;
            denseNonZeroCount = sparseValues.Count;
            sparseValues = null;
        }

        public void CopyTo(ImmutableDictionary<TNumber, TNumber>.Builder builder)
        {
            // Reconstruct the first original index represented by this page.
            TNumber firstIndex = PageIndex << PageOffsetBits;

            if (denseValues is not null)
            {
                // Snapshots contain only observable nonzero values.
                for (int i = 0; i < denseValues.Length; i++)
                {
                    TNumber value = denseValues[i];

                    if (!TNumber.IsZero(value))
                    {
                        builder[firstIndex + TNumber.CreateTruncating(i)] = value;
                    }
                }

                return;
            }

            Debug.Assert(sparseValues is not null);

            // Sparse storage never retains zero values.
            foreach (var (offset, value) in sparseValues)
            {
                builder[firstIndex + TNumber.CreateTruncating(offset)] = value;
            }
        }
    }
}

internal sealed class ArrayElementNotSetException : Exception
{ }

internal sealed class AlwaysThrowGlobalArraySource<TNumber> : IArraySource<TNumber>
    where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
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
