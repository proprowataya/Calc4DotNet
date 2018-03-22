using System;
using System.Numerics;
using System.Reflection.Emit;

namespace Calc4DotNet.Core.ILCompilation
{
    internal static class ILEmitHelper
    {
        public static void EmitLdc<TNumber>(this ILGenerator il, TNumber value)
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
                        case int i when i < 256:
                            il.Emit(OpCodes.Ldc_I4_S, (byte)i);
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
                        il.Emit(OpCodes.Newobj, typeof(BigInteger).GetConstructor(new[] { typeof(long) }));
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

                        il.Emit(OpCodes.Newobj, typeof(BigInteger).GetConstructor(new[] { typeof(byte[]) }));
                    }
                    break;
                default:
                    throw new InvalidOperationException();
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
    }
}
