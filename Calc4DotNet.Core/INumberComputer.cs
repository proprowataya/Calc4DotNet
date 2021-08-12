using System.Numerics;
using System.Runtime.CompilerServices;

namespace Calc4DotNet.Core
{
    internal interface INumberComputer<T>
    {
        T Zero { get; }
        T One { get; }
        T MinusOne { get; }
        T Ten { get; }

        T FromInt(int value);

        T Negate(T value);
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);
        T Modulo(T a, T b);

        bool Equals(T a, T b);
        bool NotEquals(T a, T b);
        bool LessThan(T a, T b);
        bool LessThanOrEquals(T a, T b);
        bool GreaterThanOrEquals(T a, T b);
        bool GreaterThan(T a, T b);
    }

    internal readonly struct Int32Computer : INumberComputer<Int32>
    {
        public Int32 Zero => 0;
        public Int32 One => 1;
        public Int32 MinusOne => -1;
        public Int32 Ten => 10;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int32 FromInt(int value) => (Int32)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int32 Negate(Int32 value) => -value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int32 Add(Int32 a, Int32 b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int32 Subtract(Int32 a, Int32 b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int32 Multiply(Int32 a, Int32 b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int32 Divide(Int32 a, Int32 b) => a / b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int32 Modulo(Int32 a, Int32 b) => a % b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Int32 a, Int32 b) => a == b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NotEquals(Int32 a, Int32 b) => a != b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(Int32 a, Int32 b) => a < b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThanOrEquals(Int32 a, Int32 b) => a <= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThanOrEquals(Int32 a, Int32 b) => a >= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(Int32 a, Int32 b) => a > b;
    }

    internal readonly struct Int64Computer : INumberComputer<Int64>
    {
        public Int64 Zero => 0;
        public Int64 One => 1;
        public Int64 MinusOne => -1;
        public Int64 Ten => 10;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64 FromInt(int value) => (Int64)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64 Negate(Int64 value) => -value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64 Add(Int64 a, Int64 b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64 Subtract(Int64 a, Int64 b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64 Multiply(Int64 a, Int64 b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64 Divide(Int64 a, Int64 b) => a / b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64 Modulo(Int64 a, Int64 b) => a % b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Int64 a, Int64 b) => a == b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NotEquals(Int64 a, Int64 b) => a != b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(Int64 a, Int64 b) => a < b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThanOrEquals(Int64 a, Int64 b) => a <= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThanOrEquals(Int64 a, Int64 b) => a >= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(Int64 a, Int64 b) => a > b;
    }

    internal readonly struct DoubleComputer : INumberComputer<Double>
    {
        public Double Zero => 0;
        public Double One => 1;
        public Double MinusOne => -1;
        public Double Ten => 10;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Double FromInt(int value) => (Double)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Double Negate(Double value) => -value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Double Add(Double a, Double b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Double Subtract(Double a, Double b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Double Multiply(Double a, Double b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Double Divide(Double a, Double b) => a / b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Double Modulo(Double a, Double b) => a % b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Double a, Double b) => a == b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NotEquals(Double a, Double b) => a != b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(Double a, Double b) => a < b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThanOrEquals(Double a, Double b) => a <= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThanOrEquals(Double a, Double b) => a >= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(Double a, Double b) => a > b;
    }

    internal readonly struct BigIntegerComputer : INumberComputer<BigInteger>
    {
        public BigInteger Zero => 0;
        public BigInteger One => 1;
        public BigInteger MinusOne => -1;
        public BigInteger Ten => 10;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigInteger FromInt(int value) => (BigInteger)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigInteger Negate(BigInteger value) => -value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigInteger Add(BigInteger a, BigInteger b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigInteger Subtract(BigInteger a, BigInteger b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigInteger Multiply(BigInteger a, BigInteger b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigInteger Divide(BigInteger a, BigInteger b) => a / b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigInteger Modulo(BigInteger a, BigInteger b) => a % b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BigInteger a, BigInteger b) => a == b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NotEquals(BigInteger a, BigInteger b) => a != b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(BigInteger a, BigInteger b) => a < b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThanOrEquals(BigInteger a, BigInteger b) => a <= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThanOrEquals(BigInteger a, BigInteger b) => a >= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(BigInteger a, BigInteger b) => a > b;
    }
}
