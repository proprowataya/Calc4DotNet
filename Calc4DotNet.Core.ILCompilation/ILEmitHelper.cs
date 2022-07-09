using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection.Emit;
using static Calc4DotNet.Core.ILCompilation.ReflectionHelper;

namespace Calc4DotNet.Core.ILCompilation;

internal static class ILEmitHelper
{
    public static void EmitLdc<TNumber>(this ILGenerator il, TNumber value)
        where TNumber : INumber<TNumber>
    {
        switch (value)
        {
            case Int32 i32:
                switch (i32)
                {
                    case -1:
                        il.Emit(OpCodes.Ldc_I4_M1);
                        break;
                    case 0:
                        il.Emit(OpCodes.Ldc_I4_0);
                        break;
                    case 1:
                        il.Emit(OpCodes.Ldc_I4_1);
                        break;
                    case 2:
                        il.Emit(OpCodes.Ldc_I4_2);
                        break;
                    case 3:
                        il.Emit(OpCodes.Ldc_I4_3);
                        break;
                    case 4:
                        il.Emit(OpCodes.Ldc_I4_4);
                        break;
                    case 5:
                        il.Emit(OpCodes.Ldc_I4_5);
                        break;
                    case 6:
                        il.Emit(OpCodes.Ldc_I4_6);
                        break;
                    case 7:
                        il.Emit(OpCodes.Ldc_I4_7);
                        break;
                    case 8:
                        il.Emit(OpCodes.Ldc_I4_8);
                        break;
                    case int i when (sbyte)i == i:
                        il.Emit(OpCodes.Ldc_I4_S, (sbyte)i);
                        break;
                    default:
                        il.Emit(OpCodes.Ldc_I4, i32);
                        break;
                }
                break;
            case Int64 i64:
                il.Emit(OpCodes.Ldc_I8, i64);
                break;
            case Double d:
                il.Emit(OpCodes.Ldc_R8, d);
                break;
            case BigInteger bigInteger:
                try
                {
                    long i64 = (long)bigInteger;
                    il.EmitLdc(i64);
                    il.Emit(OpCodes.Newobj, typeof(BigInteger).GetConstructor(new[] { typeof(long) })!);
                }
                catch (OverflowException)
                {
                    byte[] array = bigInteger.ToByteArray();
                    il.EmitLdc(array.Length);
                    il.Emit(OpCodes.Newarr, typeof(byte));

                    for (int i = 0; i < array.Length; i++)
                    {
                        il.Emit(OpCodes.Dup);
                        il.EmitLdc(i);
                        il.EmitLdc((int)array[i]);
                        il.Emit(OpCodes.Stelem_I1);
                    }

                    il.Emit(OpCodes.Newobj, typeof(BigInteger).GetConstructor(new[] { typeof(byte[]) })!);
                }
                break;
            default:
                {
                    bool TryCastTo<TTo>([NotNullWhen(true)] out TTo? result) where TTo : INumber<TTo>
                    {
                        try
                        {
                            result = TTo.CreateChecked(value);
                            return true;
                        }
                        catch
                        {
                            result = default;
                            return false;
                        }
                    }

                    // If the value is representable as Int32 or Int64, we use INumberBase<TNumber>.CreateTruncating.
                    // Otherwise, we will parse its string representation.
                    if (TryCastTo<Int32>(out var i32))
                    {
                        il.EmitLdc(i32);
                        il.Emit(OpCodes.Constrained, typeof(TNumber));
                        il.Emit(OpCodes.Call, GetInterfaceMethod<TNumber, INumberBase<TNumber>>(nameof(INumberBase<TNumber>.CreateTruncating)).MakeGenericMethod(typeof(Int32)));
                    }
                    else if (TryCastTo<Int64>(out var i64))
                    {
                        il.EmitLdc(i64);
                        il.Emit(OpCodes.Constrained, typeof(TNumber));
                        il.Emit(OpCodes.Call, GetInterfaceMethod<TNumber, INumberBase<TNumber>>(nameof(INumberBase<TNumber>.CreateTruncating)).MakeGenericMethod(typeof(Int64)));
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldstr, value.ToString() ?? throw new InvalidOperationException("ToString() returned null"));
                        il.Emit(OpCodes.Ldnull);
                        il.Emit(OpCodes.Constrained, typeof(TNumber));
                        il.Emit(OpCodes.Call, GetInterfaceMethod<TNumber, IParsable<TNumber>>(nameof(IParsable<TNumber>.Parse)));
                    }

                    break;
                }
        }
    }

    public static void EmitLdarg(this ILGenerator il, int index)
    {
        switch (index)
        {
            case 0:
                il.Emit(OpCodes.Ldarg_0);
                break;
            case 1:
                il.Emit(OpCodes.Ldarg_1);
                break;
            case 2:
                il.Emit(OpCodes.Ldarg_2);
                break;
            case 3:
                il.Emit(OpCodes.Ldarg_3);
                break;
            case int i when i < 256:
                il.Emit(OpCodes.Ldarg_S, (byte)i);
                break;
            default:
                il.Emit(OpCodes.Ldarg, index);
                break;
        }
    }

    public static void EmitStarg(this ILGenerator il, int index)
    {
        switch (index)
        {
            case int i when i < 256:
                il.Emit(OpCodes.Starg_S, (byte)i);
                break;
            default:
                il.Emit(OpCodes.Starg, index);
                break;
        }
    }

    public static void EmitConvToINumber<TFrom, TTo>(this ILGenerator il)
        where TFrom : INumber<TFrom>
        where TTo : INumber<TTo>
    {
        il.Emit(OpCodes.Constrained, typeof(TTo));
        il.Emit(OpCodes.Call, GetInterfaceMethod<TTo, INumberBase<TTo>>(nameof(INumberBase<TTo>.CreateTruncating)).MakeGenericMethod(typeof(TFrom)));
    }
}
