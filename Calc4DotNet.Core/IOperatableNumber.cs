using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Calc4DotNet.Core
{
    internal interface IOperatableNumber<T> where T : IOperatableNumber<T>
    {
        static abstract T Zero { get; }
        static abstract T One { get; }
        static abstract T MinusOne { get; }
        static abstract T Ten { get; }

        static abstract T FromInt(int value);

        static abstract T operator -(T value);
        static abstract T operator +(T a, T b);
        static abstract T operator -(T a, T b);
        static abstract T operator *(T a, T b);
        static abstract T operator /(T a, T b);
        static abstract T operator %(T a, T b);

        static abstract bool operator ==(T a, T b);
        static abstract bool operator !=(T a, T b);
        static abstract bool operator <(T a, T b);
        static abstract bool operator <=(T a, T b);
        static abstract bool operator >=(T a, T b);
        static abstract bool operator >(T a, T b);
    }

    internal readonly record struct Int32Operatable(Int32 Value) : IOperatableNumber<Int32Operatable>
    {
        /* Members from IOperatableNumber */

        public static Int32Operatable Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Int32Operatable((Int32)0);
        }

        public static Int32Operatable One
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Int32Operatable((Int32)1);
        }

        public static Int32Operatable MinusOne
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Int32Operatable((Int32)(-1));
        }

        public static Int32Operatable Ten
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Int32Operatable((Int32)10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Operatable FromInt(int value) => new Int32Operatable((Int32)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Operatable operator -(Int32Operatable value) => new Int32Operatable(-value.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Operatable operator +(Int32Operatable a, Int32Operatable b) => new Int32Operatable(a.Value + b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Operatable operator -(Int32Operatable a, Int32Operatable b) => new Int32Operatable(a.Value - b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Operatable operator *(Int32Operatable a, Int32Operatable b) => new Int32Operatable(a.Value * b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Operatable operator /(Int32Operatable a, Int32Operatable b) => new Int32Operatable(a.Value / b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Operatable operator %(Int32Operatable a, Int32Operatable b) => new Int32Operatable(a.Value % b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Int32Operatable(Int32 value) => new Int32Operatable(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Int32Operatable a, Int32Operatable b) => a.Value < b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Int32Operatable a, Int32Operatable b) => a.Value <= b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Int32Operatable a, Int32Operatable b) => a.Value >= b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Int32Operatable a, Int32Operatable b) => a.Value > b.Value;

        /**********/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Int32(Int32Operatable value) => value.Value;
    }

    internal readonly record struct Int64Operatable(Int64 Value) : IOperatableNumber<Int64Operatable>
    {
        /* Members from IOperatableNumber */

        public static Int64Operatable Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Int64Operatable((Int64)0);
        }

        public static Int64Operatable One
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Int64Operatable((Int64)1);
        }

        public static Int64Operatable MinusOne
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Int64Operatable((Int64)(-1));
        }

        public static Int64Operatable Ten
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Int64Operatable((Int64)10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64Operatable FromInt(int value) => new Int64Operatable((Int64)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64Operatable operator -(Int64Operatable value) => new Int64Operatable(-value.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64Operatable operator +(Int64Operatable a, Int64Operatable b) => new Int64Operatable(a.Value + b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64Operatable operator -(Int64Operatable a, Int64Operatable b) => new Int64Operatable(a.Value - b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64Operatable operator *(Int64Operatable a, Int64Operatable b) => new Int64Operatable(a.Value * b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64Operatable operator /(Int64Operatable a, Int64Operatable b) => new Int64Operatable(a.Value / b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64Operatable operator %(Int64Operatable a, Int64Operatable b) => new Int64Operatable(a.Value % b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Int64Operatable(Int64 value) => new Int64Operatable(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Int64Operatable a, Int64Operatable b) => a.Value < b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Int64Operatable a, Int64Operatable b) => a.Value <= b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Int64Operatable a, Int64Operatable b) => a.Value >= b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Int64Operatable a, Int64Operatable b) => a.Value > b.Value;

        /**********/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Int64(Int64Operatable value) => value.Value;
    }

    internal readonly record struct DoubleOperatable(Double Value) : IOperatableNumber<DoubleOperatable>
    {
        /* Members from IOperatableNumber */

        public static DoubleOperatable Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new DoubleOperatable((Double)0);
        }

        public static DoubleOperatable One
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new DoubleOperatable((Double)1);
        }

        public static DoubleOperatable MinusOne
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new DoubleOperatable((Double)(-1));
        }

        public static DoubleOperatable Ten
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new DoubleOperatable((Double)10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleOperatable FromInt(int value) => new DoubleOperatable((Double)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleOperatable operator -(DoubleOperatable value) => new DoubleOperatable(-value.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleOperatable operator +(DoubleOperatable a, DoubleOperatable b) => new DoubleOperatable(a.Value + b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleOperatable operator -(DoubleOperatable a, DoubleOperatable b) => new DoubleOperatable(a.Value - b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleOperatable operator *(DoubleOperatable a, DoubleOperatable b) => new DoubleOperatable(a.Value * b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleOperatable operator /(DoubleOperatable a, DoubleOperatable b) => new DoubleOperatable(a.Value / b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleOperatable operator %(DoubleOperatable a, DoubleOperatable b) => new DoubleOperatable(a.Value % b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator DoubleOperatable(Double value) => new DoubleOperatable(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(DoubleOperatable a, DoubleOperatable b) => a.Value < b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(DoubleOperatable a, DoubleOperatable b) => a.Value <= b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(DoubleOperatable a, DoubleOperatable b) => a.Value >= b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(DoubleOperatable a, DoubleOperatable b) => a.Value > b.Value;

        /**********/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Double(DoubleOperatable value) => value.Value;
    }

    internal readonly record struct BigIntegerOperatable(BigInteger Value) : IOperatableNumber<BigIntegerOperatable>
    {
        /* Members from IOperatableNumber */

        public static BigIntegerOperatable Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new BigIntegerOperatable((BigInteger)0);
        }

        public static BigIntegerOperatable One
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new BigIntegerOperatable((BigInteger)1);
        }

        public static BigIntegerOperatable MinusOne
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new BigIntegerOperatable((BigInteger)(-1));
        }

        public static BigIntegerOperatable Ten
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new BigIntegerOperatable((BigInteger)10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigIntegerOperatable FromInt(int value) => new BigIntegerOperatable((BigInteger)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigIntegerOperatable operator -(BigIntegerOperatable value) => new BigIntegerOperatable(-value.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigIntegerOperatable operator +(BigIntegerOperatable a, BigIntegerOperatable b) => new BigIntegerOperatable(a.Value + b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigIntegerOperatable operator -(BigIntegerOperatable a, BigIntegerOperatable b) => new BigIntegerOperatable(a.Value - b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigIntegerOperatable operator *(BigIntegerOperatable a, BigIntegerOperatable b) => new BigIntegerOperatable(a.Value * b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigIntegerOperatable operator /(BigIntegerOperatable a, BigIntegerOperatable b) => new BigIntegerOperatable(a.Value / b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigIntegerOperatable operator %(BigIntegerOperatable a, BigIntegerOperatable b) => new BigIntegerOperatable(a.Value % b.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BigIntegerOperatable(BigInteger value) => new BigIntegerOperatable(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BigIntegerOperatable a, BigIntegerOperatable b) => a.Value < b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BigIntegerOperatable a, BigIntegerOperatable b) => a.Value <= b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BigIntegerOperatable a, BigIntegerOperatable b) => a.Value >= b.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BigIntegerOperatable a, BigIntegerOperatable b) => a.Value > b.Value;

        /**********/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BigInteger(BigIntegerOperatable value) => value.Value;
    }
}
