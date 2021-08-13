using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using Calc4DotNet.Core.Numbers;

namespace Calc4DotNet.Core.Execution
{
    public static class LowLevelExecutor
    {
        private const int StackSize = 1 << 20;
        private const int PtrStackSize = 1 << 20;

        public static TNumber Execute<TNumber>(LowLevelModule<TNumber> module)
            where TNumber : notnull
        {
            if (typeof(TNumber) == typeof(BigInteger))
            {
                LowLevelModule<WrappedBigInteger> castedModule =
                    new(module.EntryPoint,
                        module.ConstTable.Select(value => new WrappedBigInteger(Unsafe.As<TNumber, BigInteger>(ref value)))
                                         .ToImmutableArray(),
                        module.UserDefinedOperators);

                BigInteger result = ExecuteCore(castedModule).Value;
                return Unsafe.As<BigInteger, TNumber>(ref result);
            }

            try
            {
                return ExecuteCore((dynamic)module);
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                // TNumber does not implement INumber<TNumber>
                throw new Calc4DotNet.Core.Exceptions.TypeNotSupportedException(typeof(TNumber));
            }
        }

        private static TNumber ExecuteCore<TNumber>(LowLevelModule<TNumber> module)
            where TNumber : INumber<TNumber>
        {
            var (operationsArray, maxStackSizesArray) = module.FlattenOperations();
            Span<LowLevelOperation> operations = stackalloc LowLevelOperation[operationsArray.Length];
            Span<int> maxStackSizes = stackalloc int[maxStackSizesArray.Length];
            operationsArray.AsSpan().CopyTo(operations);
            maxStackSizesArray.AsSpan().CopyTo(maxStackSizes);
            ref LowLevelOperation firstOperation = ref operations[0];
            ref int firstStackSizes = ref maxStackSizes[0];

            TNumber[] stack = new TNumber[StackSize];
            int[] ptrStack = new int[PtrStackSize];
            ref TNumber top = ref stack[0];
            ref TNumber bottom = ref stack[0];
            ref int ptrTop = ref ptrStack[0];
            ref TNumber stackBegin = ref stack[0];
            ref TNumber stackEnd = ref stack[^1];
            ref int ptrStackEnd = ref ptrStack[^1];
            ref LowLevelOperation op = ref firstOperation;

            while (true)
            {
                VerifyRange(operations, ref op);

                switch (op.Opcode)
                {
                    case Opcode.Push:
                        VerifyRange(stack, ref top);
                        top = TNumber.Zero;
                        top = ref Unsafe.Add(ref top, 1);
                        break;
                    case Opcode.Pop:
                        top = ref Unsafe.Add(ref top, -1);
                        break;
                    case Opcode.LoadConst:
                        VerifyRange(stack, ref top);
                        top = TNumber.CreateTruncating(op.Value);
                        top = ref Unsafe.Add(ref top, 1);
                        break;
                    case Opcode.LoadConstTable:
                        VerifyRange(stack, ref top);
                        top = module.ConstTable[op.Value];
                        top = ref Unsafe.Add(ref top, 1);
                        break;
                    case Opcode.LoadArg:
                        VerifyRange(stack, ref top);
                        top = Unsafe.Add(ref bottom, -op.Value);
                        top = ref Unsafe.Add(ref top, 1);
                        break;
                    case Opcode.StoreArg:
                        top = ref Unsafe.Add(ref top, -1);
                        VerifyRange(stack, ref top);
                        VerifyRange(stack, ref Unsafe.Add(ref bottom, -op.Value));
                        Unsafe.Add(ref bottom, -op.Value) = top;
                        break;
                    case Opcode.Input:
                        throw new NotImplementedException();
                    case Opcode.Add:
                        top = ref Unsafe.Add(ref top, -1);
                        VerifyRange(stack, ref top);
                        VerifyRange(stack, ref Unsafe.Add(ref top, -1));
                        Unsafe.Add(ref top, -1) = Unsafe.Add(ref top, -1) + top;
                        break;
                    case Opcode.Sub:
                        top = ref Unsafe.Add(ref top, -1);
                        VerifyRange(stack, ref top);
                        VerifyRange(stack, ref Unsafe.Add(ref top, -1));
                        Unsafe.Add(ref top, -1) = Unsafe.Add(ref top, -1) - top;
                        break;
                    case Opcode.Mult:
                        top = ref Unsafe.Add(ref top, -1);
                        VerifyRange(stack, ref top);
                        VerifyRange(stack, ref Unsafe.Add(ref top, -1));
                        Unsafe.Add(ref top, -1) = Unsafe.Add(ref top, -1) * top;
                        break;
                    case Opcode.Div:
                        top = ref Unsafe.Add(ref top, -1);
                        VerifyRange(stack, ref top);
                        VerifyRange(stack, ref Unsafe.Add(ref top, -1));
                        Unsafe.Add(ref top, -1) = Unsafe.Add(ref top, -1) / top;
                        break;
                    case Opcode.Mod:
                        top = ref Unsafe.Add(ref top, -1);
                        VerifyRange(stack, ref top);
                        VerifyRange(stack, ref Unsafe.Add(ref top, -1));
                        Unsafe.Add(ref top, -1) = Unsafe.Add(ref top, -1) % top;
                        break;
                    case Opcode.Goto:
                        op = ref Unsafe.Add(ref firstOperation, op.Value);
                        break;
                    case Opcode.GotoIfTrue:
                        top = ref Unsafe.Add(ref top, -1);
                        VerifyRange(stack, ref top);
                        if (top != TNumber.Zero)
                        {
                            op = ref Unsafe.Add(ref firstOperation, op.Value);
                        }
                        break;
                    case Opcode.GotoIfEqual:
                        top = ref Unsafe.Add(ref top, -2);
                        VerifyRange(stack, ref top);
                        VerifyRange(stack, ref Unsafe.Add(ref top, 1));
                        if (top == Unsafe.Add(ref top, 1))
                        {
                            op = ref Unsafe.Add(ref firstOperation, op.Value);
                        }
                        break;
                    case Opcode.GotoIfLessThan:
                        top = ref Unsafe.Add(ref top, -2);
                        VerifyRange(stack, ref top);
                        VerifyRange(stack, ref Unsafe.Add(ref top, 1));
                        if (top < Unsafe.Add(ref top, 1))
                        {
                            op = ref Unsafe.Add(ref firstOperation, op.Value);
                        }
                        break;
                    case Opcode.GotoIfLessThanOrEqual:
                        top = ref Unsafe.Add(ref top, -2);
                        VerifyRange(stack, ref top);
                        VerifyRange(stack, ref Unsafe.Add(ref top, 1));
                        if (top <= Unsafe.Add(ref top, 1))
                        {
                            op = ref Unsafe.Add(ref firstOperation, op.Value);
                        }
                        break;
                    case Opcode.Call:
                        // Check stack overflow
                        VerifyRange(maxStackSizes, ref Unsafe.Add(ref firstStackSizes, op.Value));
                        if (Unsafe.IsAddressGreaterThan(ref Unsafe.Add(ref top, Unsafe.Add(ref firstStackSizes, op.Value)),
                                                        ref stackEnd))
                        {
                            ThrowStackOverflowException();
                        }

                        if (Unsafe.IsAddressGreaterThan(ref Unsafe.Add(ref ptrTop, 2), ref ptrStackEnd))
                        {
                            ThrowStackOverflowException();
                        }

                        // Push current program counter
                        VerifyRange(ptrStack, ref ptrTop);
                        ptrTop = (int)Unsafe.ByteOffset(ref firstOperation, ref op);
                        ptrTop = ref Unsafe.Add(ref ptrTop, 1);

                        // Push current stack bottom
                        VerifyRange(ptrStack, ref ptrTop);
                        ptrTop = (int)Unsafe.ByteOffset(ref stackBegin, ref bottom);
                        ptrTop = ref Unsafe.Add(ref ptrTop, 1);

                        // Create new stack frame
                        bottom = ref top;

                        // Branch
                        op = ref Unsafe.Add(ref firstOperation, op.Value);
                        break;
                    case Opcode.Return:
                        // Store returning value
                        VerifyRange(stack, ref Unsafe.Add(ref top, -1));
                        TNumber returnValue = Unsafe.Add(ref top, -1);

                        // Restore previous stack top while removing arguments from stack
                        // (We ensure space of returning value)
                        top = ref Unsafe.Add(ref bottom, -op.Value + 1);

                        // Store returning value on stack
                        VerifyRange(stack, ref Unsafe.Add(ref top, -1));
                        Unsafe.Add(ref top, -1) = returnValue;

                        // Pop previous stack bottom
                        ptrTop = ref Unsafe.Add(ref ptrTop, -1);
                        VerifyRange(ptrStack, ref ptrTop);
                        bottom = ref Unsafe.AddByteOffset(ref stackBegin, (IntPtr)ptrTop);

                        // Pop previous program counter
                        ptrTop = ref Unsafe.Add(ref ptrTop, -1);
                        VerifyRange(ptrStack, ref ptrTop);
                        op = ref Unsafe.AddByteOffset(ref firstOperation, (IntPtr)ptrTop);
                        break;
                    case Opcode.Halt:
                        VerifyRange(stack, ref Unsafe.Add(ref top, -1));
                        return Unsafe.Add(ref top, -1);
                    case Opcode.Lavel:
                    default:
                        throw new InvalidOperationException();
                }

                op = ref Unsafe.Add(ref op, 1);
            }
        }

        private static void ThrowStackOverflowException()
        {
            throw new Calc4DotNet.Core.Exceptions.StackOverflowException();
        }

        [Conditional("DEBUG")]
        private static void VerifyRange<T>(Span<T> array, ref T ptr)
        {
            nuint index = (nuint)(nint)Unsafe.ByteOffset(ref array[0], ref ptr) / (nuint)Unsafe.SizeOf<T>();

            if (index >= (nuint)array.Length)
            {
                throw new IndexOutOfRangeException();
            }
        }
    }
}
