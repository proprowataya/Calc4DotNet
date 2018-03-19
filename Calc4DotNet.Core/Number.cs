#define PRIMITIVE
//#define BIG_INTEGER

using System;
using System.Runtime.CompilerServices;

namespace Calc4DotNet.Core
{
#if PRIMITIVE
    using NumberType = Int64;
#elif BIG_INTEGER
    using NumberType = BigInteger;
#else
    // Invalid
#endif

    public readonly struct Number : IEquatable<Number>
    {
        public static Number Zero => new Number(0);
        public static Number One => new Number(1);
        public static Number MinusOne => new Number(-1);
#if PRIMITIVE
        public static Number MaxValue => new Number(NumberType.MaxValue);
        public static Number MinValue => new Number(NumberType.MinValue);
#endif

        public NumberType Value { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(NumberType value) => Value = value;

        #region Comparison operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Number a, Number b) => a.Value == b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Number a, Number b) => a.Value != b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Number a, Number b) => a.Value < b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Number a, Number b) => a.Value <= b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Number a, Number b) => a.Value >= b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Number a, Number b) => a.Value > b.Value;

        #endregion

        #region Arithmetic operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number operator +(Number a, Number b) => new Number(a.Value + b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number operator -(Number a, Number b) => new Number(a.Value - b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number operator *(Number a, Number b) => new Number(a.Value * b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number operator /(Number a, Number b) => new Number(a.Value / b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number operator %(Number a, Number b) => new Number(a.Value % b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number operator +(Number i) => i;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number operator -(Number i) => new Number(-i.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number operator ++(Number i) => new Number(i.Value + 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number operator --(Number i) => new Number(i.Value - 1);

        #endregion

#if true
        #region Cast operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NumberType(Number i) => i.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(NumberType i) => new Number(i);

        #endregion
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if PRIMITIVE
        public Number Abs() => new Number(Math.Abs(Value));
#elif BIG_INTEGER
        public Number Abs() => new Number(NumberType.Abs(Value));
#else
        public Number Abs() => throw new NotImplementedException();
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => Value.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Number other) => Value == other.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Number i && Equals(i);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => Value.GetHashCode();
    }
}
