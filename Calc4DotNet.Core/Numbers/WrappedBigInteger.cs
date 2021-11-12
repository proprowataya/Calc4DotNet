using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Calc4DotNet.Core.Numbers;

internal readonly struct WrappedBigInteger : INumber<WrappedBigInteger>
{
    public BigInteger Value { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public WrappedBigInteger(BigInteger value)
    {
        Value = value;
    }

    public static WrappedBigInteger Zero
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(BigInteger.Zero);
    }

    public static WrappedBigInteger One
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(BigInteger.One);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WrappedBigInteger Create<TOther>(TOther value) where TOther : INumber<TOther>
    {
        if (typeof(TOther) == typeof(Int16))
        {
            return new WrappedBigInteger(new BigInteger((Int16)(object)value));
        }
        else if (typeof(TOther) == typeof(Int32))
        {
            return new WrappedBigInteger(new BigInteger((Int32)(object)value));
        }
        else if (typeof(TOther) == typeof(Int64))
        {
            return new WrappedBigInteger(new BigInteger((Int64)(object)value));
        }
        else if (typeof(TOther) == typeof(Double))
        {
            return new WrappedBigInteger(new BigInteger((Double)(object)value));
        }
        else if (typeof(TOther) == typeof(BigInteger))
        {
            return new WrappedBigInteger((BigInteger)(object)value);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WrappedBigInteger CreateTruncating<TOther>(TOther value) where TOther : INumber<TOther>
    {
        return Create(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(WrappedBigInteger left, WrappedBigInteger right) => left.Value < right.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(WrappedBigInteger left, WrappedBigInteger right) => left.Value <= right.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(WrappedBigInteger left, WrappedBigInteger right) => left.Value > right.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(WrappedBigInteger left, WrappedBigInteger right) => left.Value >= right.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(WrappedBigInteger left, WrappedBigInteger right) => left.Value == right.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(WrappedBigInteger left, WrappedBigInteger right) => left.Value != right.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WrappedBigInteger operator +(WrappedBigInteger left, WrappedBigInteger right) => new(left.Value + right.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WrappedBigInteger operator -(WrappedBigInteger left, WrappedBigInteger right) => new(left.Value - right.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WrappedBigInteger operator *(WrappedBigInteger left, WrappedBigInteger right) => new(left.Value * right.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WrappedBigInteger operator /(WrappedBigInteger left, WrappedBigInteger right) => new(left.Value / right.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WrappedBigInteger operator %(WrappedBigInteger left, WrappedBigInteger right) => new(left.Value % right.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator WrappedBigInteger(BigInteger param) => new(param);

    public override bool Equals(object? obj) => obj is WrappedBigInteger other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();

    // The followings will never be used in this application, so we don't have their implementations.

    static WrappedBigInteger IAdditiveIdentity<WrappedBigInteger, WrappedBigInteger>.AdditiveIdentity => throw new NotImplementedException();
    static WrappedBigInteger IMultiplicativeIdentity<WrappedBigInteger, WrappedBigInteger>.MultiplicativeIdentity => throw new NotImplementedException();
    static WrappedBigInteger IDecrementOperators<WrappedBigInteger>.operator --(WrappedBigInteger value) => throw new NotImplementedException();
    static WrappedBigInteger IIncrementOperators<WrappedBigInteger>.operator ++(WrappedBigInteger value) => throw new NotImplementedException();
    static WrappedBigInteger IUnaryNegationOperators<WrappedBigInteger, WrappedBigInteger>.operator -(WrappedBigInteger value) => throw new NotImplementedException();
    static WrappedBigInteger IUnaryPlusOperators<WrappedBigInteger, WrappedBigInteger>.operator +(WrappedBigInteger value) => throw new NotImplementedException();
    static WrappedBigInteger INumber<WrappedBigInteger>.Abs(WrappedBigInteger value) => throw new NotImplementedException();
    static WrappedBigInteger INumber<WrappedBigInteger>.Clamp(WrappedBigInteger value, WrappedBigInteger min, WrappedBigInteger max) => throw new NotImplementedException();
    static WrappedBigInteger INumber<WrappedBigInteger>.CreateSaturating<TOther>(TOther value) => throw new NotImplementedException();
    static (WrappedBigInteger Quotient, WrappedBigInteger Remainder) INumber<WrappedBigInteger>.DivRem(WrappedBigInteger left, WrappedBigInteger right) => throw new NotImplementedException();
    static WrappedBigInteger INumber<WrappedBigInteger>.Max(WrappedBigInteger x, WrappedBigInteger y) => throw new NotImplementedException();
    static WrappedBigInteger INumber<WrappedBigInteger>.Min(WrappedBigInteger x, WrappedBigInteger y) => throw new NotImplementedException();
    static WrappedBigInteger INumber<WrappedBigInteger>.Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
    static WrappedBigInteger INumber<WrappedBigInteger>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
    static WrappedBigInteger INumber<WrappedBigInteger>.Sign(WrappedBigInteger value) => throw new NotImplementedException();
    static bool INumber<WrappedBigInteger>.TryCreate<TOther>(TOther value, out WrappedBigInteger result) => throw new NotImplementedException();
    static bool INumber<WrappedBigInteger>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out WrappedBigInteger result) => throw new NotImplementedException();
    static bool INumber<WrappedBigInteger>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out WrappedBigInteger result) => throw new NotImplementedException();
    int IComparable.CompareTo(object? obj) => throw new NotImplementedException();
    int IComparable<WrappedBigInteger>.CompareTo(WrappedBigInteger other) => throw new NotImplementedException();
    bool IEquatable<WrappedBigInteger>.Equals(WrappedBigInteger other) => throw new NotImplementedException();
    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => throw new NotImplementedException();
    static WrappedBigInteger ISpanParseable<WrappedBigInteger>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();
    static bool ISpanParseable<WrappedBigInteger>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out WrappedBigInteger result) => throw new NotImplementedException();
    static WrappedBigInteger IParseable<WrappedBigInteger>.Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
    static bool IParseable<WrappedBigInteger>.TryParse(string? s, IFormatProvider? provider, out WrappedBigInteger result) => throw new NotImplementedException();
}
