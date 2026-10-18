using System.Collections.Immutable;
using System.Numerics;
using Calc4DotNet.Core;
using Xunit;

namespace Calc4DotNet.Test;

public sealed class DefaultArraySourceTest
{
    [Fact]
    public void Int32IndicesRemainDistinctAcrossDenseBoundaries()
    {
        // Find the first and last indices stored in the central array.
        const int CentralStart = -DefaultArraySource<Int32>.CentralArrayLength / 2;
        const int CentralEnd = CentralStart + DefaultArraySource<Int32>.CentralArrayLength - 1;
        var source = new DefaultArraySource<Int32>();

        // Assign distinct values just inside and outside both boundaries.
        source[CentralStart - 1] = 1;
        source[CentralStart] = 2;
        source[CentralEnd] = 3;
        source[CentralEnd + 1] = 4;

        // Verify that crossing either boundary does not mix values.
        Assert.Equal(1, source[CentralStart - 1]);
        Assert.Equal(2, source[CentralStart]);
        Assert.Equal(3, source[CentralEnd]);
        Assert.Equal(4, source[CentralEnd + 1]);
    }

    [Fact]
    public void ExtremeIndicesRemainDistinctForAllNumberTypes()
    {
        // Cover the minimum and maximum indices of each fixed size integer type.
        AssertDistinctIndices(Int32.MinValue, Int32.MaxValue);
        AssertDistinctIndices(Int64.MinValue, Int64.MaxValue);
        AssertDistinctIndices(Int128.MinValue, Int128.MaxValue);

        // Exercise BigInteger with indices beyond the Int64 range.
        BigInteger largeIndex = BigInteger.One << 100;
        AssertDistinctIndices(-largeIndex, largeIndex);

        static void AssertDistinctIndices<TNumber>(TNumber firstIndex, TNumber secondIndex)
            where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
        {
            var source = new DefaultArraySource<TNumber>();
            TNumber firstValue = TNumber.CreateChecked(1);
            TNumber secondValue = TNumber.CreateChecked(2);
            TNumber centerValue = TNumber.CreateChecked(3);

            // Store distinct values at both extremes and at zero.
            source[firstIndex] = firstValue;
            source[secondIndex] = secondValue;
            source[TNumber.Zero] = centerValue;

            // Verify that the distant indices remain independent.
            Assert.Equal(firstValue, source[firstIndex]);
            Assert.Equal(secondValue, source[secondIndex]);
            Assert.Equal(centerValue, source[TNumber.Zero]);

            // Verify that the snapshot preserves every index and value.
            ImmutableDictionary<TNumber, TNumber> snapshot = source.ToImmutableDictionary();
            Assert.Equal(3, snapshot.Count);
            Assert.Equal(firstValue, snapshot[firstIndex]);
            Assert.Equal(secondValue, snapshot[secondIndex]);
            Assert.Equal(centerValue, snapshot[TNumber.Zero]);
        }
    }

    [Fact]
    public void PagesPreserveValuesAcrossElementCountPromotionForAllNumberTypes()
    {
        AssertPagePromotion<Int32>();
        AssertPagePromotion<Int64>();
        AssertPagePromotion<Int128>();
        AssertPagePromotion<BigInteger>();

        static void AssertPagePromotion<TNumber>()
            where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
        {
            // Exercise complete pages outside the central array on both sides of zero.
            var (positivePageStart, negativePageStart) = GetPageStartsOutsideCentralArray<TNumber>();
            AssertPagePromotionAt<TNumber>(positivePageStart);
            AssertPagePromotionAt<TNumber>(negativePageStart);
        }

        static void AssertPagePromotionAt<TNumber>(int firstIndex)
            where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
        {
            // Ensure that the element threshold can be reached within one page.
            const int ValueCount = DefaultArraySource<TNumber>.DenseElementThreshold;
            Assert.InRange(ValueCount, 1, DefaultArraySource<TNumber>.PageLength);

            var source = new DefaultArraySource<TNumber>();

            // Fill one page until its element count triggers promotion.
            for (int i = 0; i < ValueCount; i++)
            {
                TNumber index = TNumber.CreateChecked(firstIndex + i);
                source[index] = -index;
            }

            // Verify that promotion preserves values in reads and snapshots.
            ImmutableDictionary<TNumber, TNumber> snapshot = source.ToImmutableDictionary();
            Assert.Equal(ValueCount, snapshot.Count);

            for (int i = 0; i < ValueCount; i++)
            {
                TNumber index = TNumber.CreateChecked(firstIndex + i);
                TNumber expected = -index;
                Assert.Equal(expected, source[index]);
                Assert.Equal(expected, snapshot[index]);
            }

            // Clear alternating values after promotion to exercise dense page updates.
            for (int i = 0; i < ValueCount; i += 2)
            {
                TNumber index = TNumber.CreateChecked(firstIndex + i);
                source[index] = TNumber.Zero;
            }

            // Verify that cleared values read as zero and are omitted from the snapshot.
            snapshot = source.ToImmutableDictionary();
            Assert.Equal(ValueCount / 2, snapshot.Count);

            for (int i = 0; i < ValueCount; i++)
            {
                TNumber index = TNumber.CreateChecked(firstIndex + i);
                TNumber expected = i % 2 == 0 ? TNumber.Zero : -index;
                Assert.Equal(expected, source[index]);
                Assert.Equal(expected, snapshot.GetValueOrDefault(index, TNumber.Zero));
            }
        }
    }

    [Fact]
    public void RepeatedWritesPreserveValuesForAllNumberTypes()
    {
        AssertRepeatedWrites<Int32>();
        AssertRepeatedWrites<Int64>();
        AssertRepeatedWrites<Int128>();
        AssertRepeatedWrites<BigInteger>();

        static void AssertRepeatedWrites<TNumber>()
            where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
        {
            // Store two distinct nonzero values in the same page outside central storage.
            var source = new DefaultArraySource<TNumber>();
            var (positivePageStart, _) = GetPageStartsOutsideCentralArray<TNumber>();
            TNumber firstIndex = TNumber.CreateChecked(positivePageStart);
            TNumber secondIndex = firstIndex + TNumber.One;
            TNumber secondValue = TNumber.CreateChecked(43);
            source[firstIndex] = TNumber.One;
            source[secondIndex] = secondValue;

            // Repeated writes trigger promotion without reaching the element count threshold.
            const int WriteCount = DefaultArraySource<TNumber>.DenseAccessThreshold;
            for (int i = 1; i <= WriteCount; i++)
            {
                source[firstIndex] = TNumber.CreateChecked(i);
            }

            TNumber expectedValue = TNumber.CreateChecked(WriteCount);

            // Read secondIndex before firstIndex because the last write cached firstIndex.
            // This makes both reads access page storage instead of the exact value cache.
            Assert.Equal(secondValue, source[secondIndex]);
            Assert.Equal(expectedValue, source[firstIndex]);

            // Verify that the snapshot preserves both values after promotion.
            ImmutableDictionary<TNumber, TNumber> snapshot = source.ToImmutableDictionary();
            Assert.Equal(2, snapshot.Count);
            Assert.Equal(secondValue, snapshot[secondIndex]);
            Assert.Equal(expectedValue, snapshot[firstIndex]);

            // Clear both values after promotion to exercise dense page updates.
            source[firstIndex] = TNumber.Zero;
            source[secondIndex] = TNumber.Zero;

            // Verify that cleared values read as zero and are omitted from the snapshot.
            Assert.Equal(TNumber.Zero, source[firstIndex]);
            Assert.Equal(TNumber.Zero, source[secondIndex]);
            Assert.Empty(source.ToImmutableDictionary());
        }
    }

    [Fact]
    public void RepeatedReadsPreserveValuesForAllNumberTypes()
    {
        AssertRepeatedReads<Int32>();
        AssertRepeatedReads<Int64>();
        AssertRepeatedReads<Int128>();
        AssertRepeatedReads<BigInteger>();

        static void AssertRepeatedReads<TNumber>()
            where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
        {
            // Store two distinct nonzero values in the same page outside central storage.
            var source = new DefaultArraySource<TNumber>();
            var (positivePageStart, _) = GetPageStartsOutsideCentralArray<TNumber>();
            TNumber firstIndex = TNumber.CreateChecked(positivePageStart);
            TNumber secondIndex = firstIndex + TNumber.One;
            TNumber firstValue = TNumber.CreateChecked(42);
            TNumber secondValue = TNumber.CreateChecked(43);
            source[firstIndex] = firstValue;
            source[secondIndex] = secondValue;

            // Alternate indices to bypass the exact value cache and trigger promotion through reads.
            // Each iteration records two page accesses, so this exceeds the promotion threshold.
            const int ReadIterationCount = DefaultArraySource<TNumber>.DenseAccessThreshold;
            for (int i = 0; i < ReadIterationCount; i++)
            {
                Assert.Equal(firstValue, source[firstIndex]);
                Assert.Equal(secondValue, source[secondIndex]);
            }

            // Verify that the snapshot preserves both values after promotion.
            ImmutableDictionary<TNumber, TNumber> snapshot = source.ToImmutableDictionary();
            Assert.Equal(2, snapshot.Count);
            Assert.Equal(firstValue, snapshot[firstIndex]);
            Assert.Equal(secondValue, snapshot[secondIndex]);
        }
    }

    [Fact]
    public void ValuesCanBeClearedAndWrittenAgainForAllNumberTypes()
    {
        AssertValueCanBeClearedAndWrittenAgain<Int32>();
        AssertValueCanBeClearedAndWrittenAgain<Int64>();
        AssertValueCanBeClearedAndWrittenAgain<Int128>();
        AssertValueCanBeClearedAndWrittenAgain<BigInteger>();

        static void AssertValueCanBeClearedAndWrittenAgain<TNumber>()
            where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
        {
            // Choose an index outside the central array.
            var source = new DefaultArraySource<TNumber>();
            var (positivePageStart, _) = GetPageStartsOutsideCentralArray<TNumber>();
            TNumber index = TNumber.CreateChecked(positivePageStart);
            TNumber firstValue = TNumber.CreateChecked(42);
            TNumber secondValue = TNumber.CreateChecked(43);

            // Store a value before clearing it.
            source[index] = firstValue;
            Assert.Equal(firstValue, source[index]);

            // Clear the value and verify that it disappears from the snapshot.
            source[index] = TNumber.Zero;

            Assert.Equal(TNumber.Zero, source[index]);
            Assert.Empty(source.ToImmutableDictionary());

            // Write a different value at the same index and verify that it is retained.
            source[index] = secondValue;

            Assert.Equal(secondValue, source[index]);
            ImmutableDictionary<TNumber, TNumber> snapshot = source.ToImmutableDictionary();
            Assert.Single(snapshot);
            Assert.Equal(secondValue, snapshot[index]);
        }
    }

    [Fact]
    public void PageCacheCollisionsPreserveValuesForAllNumberTypes()
    {
        AssertPageCacheCollisions<Int32>();
        AssertPageCacheCollisions<Int64>();
        AssertPageCacheCollisions<Int128>();
        AssertPageCacheCollisions<BigInteger>();

        static void AssertPageCacheCollisions<TNumber>()
            where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
        {
            var source = new DefaultArraySource<TNumber>();

            // Use more pages than cache slots so at least two pages must share a slot.
            const int PageCount = DefaultArraySource<TNumber>.PageCacheLength + 1;
            const int PageLength = DefaultArraySource<TNumber>.PageLength;
            var (positivePageStart, _) = GetPageStartsOutsideCentralArray<TNumber>();

            // Write distinct nonzero values at page starts outside central storage.
            for (int i = 0; i < PageCount; i++)
            {
                TNumber index = TNumber.CreateChecked(positivePageStart + i * PageLength);
                source[index] = -index;
            }

            // Verify that every value survives cache collisions.
            for (int i = 0; i < PageCount; i++)
            {
                TNumber index = TNumber.CreateChecked(positivePageStart + i * PageLength);
                Assert.Equal(-index, source[index]);
            }

            // Update the first page after reading all pages.
            source[TNumber.CreateChecked(positivePageStart)] = TNumber.One;

            // Verify the updated value and all unchanged values.
            for (int i = 0; i < PageCount; i++)
            {
                TNumber index = TNumber.CreateChecked(positivePageStart + i * PageLength);
                TNumber expected = i == 0 ? TNumber.One : -index;
                Assert.Equal(expected, source[index]);
            }
        }
    }

    private static (int Positive, int Negative) GetPageStartsOutsideCentralArray<TNumber>()
        where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
    {
        const int PageLength = DefaultArraySource<TNumber>.PageLength;

        // Choose a positive page boundary beyond the central array length.
        int positivePageStart = (DefaultArraySource<TNumber>.CentralArrayLength / PageLength + 1) * PageLength;

        // Treat the negated boundary as the exclusive end of the negative page.
        int negativePageStart = -positivePageStart - PageLength;

        return (positivePageStart, negativePageStart);
    }
}
