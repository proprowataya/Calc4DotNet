using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Calc4DotNet.Core.Execution
{
    public static class LowLevelExecutor
    {
        private const int StackSize = 1 << 20;
        private const int PtrStackSize = 1 << 20;

        public static Int32 Execute(LowLevelModule<Int32> module)
        {
            return ExecuteCore<Int32, Int32Computer>(module);
        }

        public static Int64 Execute(LowLevelModule<Int64> module)
        {
            return ExecuteCore<Int64, Int64Computer>(module);
        }

        public static Double Execute(LowLevelModule<Double> module)
        {
            return ExecuteCore<Double, DoubleComputer>(module);
        }

        public static BigInteger Execute(LowLevelModule<BigInteger> module)
        {
            return ExecuteCore<BigInteger, BigIntegerComputer>(module);
        }

        private static TNumber ExecuteCore<TNumber, TNumberComputer>(LowLevelModule<TNumber> module)
            where TNumberComputer : INumberComputer<TNumber>
        {
            TNumberComputer c = default;

            LowLevelOperation[] operationsArray = FlattenOperators(module);
            Span<LowLevelOperation> operations = stackalloc LowLevelOperation[operationsArray.Length];
            operationsArray.AsSpan().CopyTo(operations);
            ref LowLevelOperation firstOperation = ref operations[0];

            TNumber[] stack = new TNumber[StackSize];
            int[] ptrStack = new int[PtrStackSize];
            int top = 0, bottom = 0;
            int ptrIndex = 0;
            ref LowLevelOperation op = ref firstOperation;

            while (true)
            {
                Debug.Assert((uint)Unsafe.ByteOffset(ref firstOperation, ref op) / (uint)Unsafe.SizeOf<LowLevelOperation>() < (uint)operations.Length);

                switch (op.Opcode)
                {
                    case Opcode.Push:
                        stack[top++] = c.Zero;
                        break;
                    case Opcode.Pop:
                        top--;
                        break;
                    case Opcode.LoadConst:
                        stack[top++] = c.FromInt(op.Value);
                        break;
                    case Opcode.LoadConstTable:
                        stack[top++] = module.ConstTable[op.Value];
                        break;
                    case Opcode.LoadArg:
                        stack[top++] = stack[bottom - op.Value];
                        break;
                    case Opcode.StoreArg:
                        stack[bottom - op.Value] = stack[--top];
                        break;
                    case Opcode.Input:
                        throw new NotImplementedException();
                    case Opcode.Add:
                        top--;
                        stack[top - 1] = c.Add(stack[top - 1], stack[top]);
                        break;
                    case Opcode.Sub:
                        top--;
                        stack[top - 1] = c.Subtract(stack[top - 1], stack[top]);
                        break;
                    case Opcode.Mult:
                        top--;
                        stack[top - 1] = c.Multiply(stack[top - 1], stack[top]);
                        break;
                    case Opcode.Div:
                        top--;
                        stack[top - 1] = c.Divide(stack[top - 1], stack[top]);
                        break;
                    case Opcode.Mod:
                        top--;
                        stack[top - 1] = c.Modulo(stack[top - 1], stack[top]);
                        break;
                    case Opcode.Goto:
                        op = ref Unsafe.Add(ref firstOperation, op.Value);
                        break;
                    case Opcode.GotoIfTrue:
                        if (c.NotEquals(stack[--top], c.Zero))
                        {
                            op = ref Unsafe.Add(ref firstOperation, op.Value);
                        }
                        break;
                    case Opcode.GotoIfEqual:
                        top -= 2;
                        if (c.Equals(stack[top], stack[top + 1]))
                        {
                            op = ref Unsafe.Add(ref firstOperation, op.Value);
                        }
                        break;
                    case Opcode.GotoIfLessThan:
                        top -= 2;
                        if (c.LessThan(stack[top], stack[top + 1]))
                        {
                            op = ref Unsafe.Add(ref firstOperation, op.Value);
                        }
                        break;
                    case Opcode.GotoIfLessThanOrEqual:
                        top -= 2;
                        if (c.LessThanOrEquals(stack[top], stack[top + 1]))
                        {
                            op = ref Unsafe.Add(ref firstOperation, op.Value);
                        }
                        break;
                    case Opcode.Call:
                        ptrStack[ptrIndex++] = (int)Unsafe.ByteOffset(ref firstOperation, ref op);          // Push current program counter
                        ptrStack[ptrIndex++] = bottom;                                                      // Push current stack bottom
                        bottom = top;                                                                       // Create new stack frame
                        op = ref Unsafe.Add(ref firstOperation, op.Value);                                  // Branch
                        break;
                    case Opcode.Return:
                        TNumber returnValue = stack[top - 1];                                               // Restore previous stack top
                        top = bottom - op.Value + 1;                                                        // Restore previous stack top
                                                                                                            // while removing arguments from stack
                                                                                                            // (We ensure space of returning value)
                        stack[top - 1] = returnValue;                                                       // Store returning value on stack
                        bottom = ptrStack[--ptrIndex];                                                      // Pop previous stack bottom
                        op = ref Unsafe.AddByteOffset(ref firstOperation, (IntPtr)ptrStack[--ptrIndex]);    // Pop previous program counter
                        break;
                    case Opcode.Halt:
                        return stack[top - 1];
                    case Opcode.Lavel:
                    default:
                        throw new InvalidOperationException();
                }

                op = ref Unsafe.Add(ref op, 1);
            }
        }

        private static LowLevelOperation[] FlattenOperators<TNumber>(LowLevelModule<TNumber> module)
        {
            int totalNumOperations = module.EntryPoint.Length + module.UserDefinedOperators.Sum(t => t.Operations.Length);
            LowLevelOperation[] result = new LowLevelOperation[totalNumOperations];
            int[] startAddresses = new int[module.UserDefinedOperators.Length];

            int index = 0;

            for (int i = 0; i < module.EntryPoint.Length; i++)
            {
                result[index++] = module.EntryPoint[i];
            }

            for (int i = 0; i < module.UserDefinedOperators.Length; i++)
            {
                int startAddress = index;
                startAddresses[i] = startAddress;

                var operations = module.UserDefinedOperators[i].Operations;
                for (int j = 0; j < operations.Length; j++)
                {
                    result[index] = operations[j];

                    // Resolve labels
                    switch (result[index].Opcode)
                    {
                        case Opcode.Goto:
                        case Opcode.GotoIfTrue:
                        case Opcode.GotoIfEqual:
                        case Opcode.GotoIfLessThan:
                        case Opcode.GotoIfLessThanOrEqual:
                            result[index] = new LowLevelOperation(result[index].Opcode,
                                                                  result[index].Value + startAddress);
                            break;
                        default:
                            break;
                    }

                    index++;
                }
            }

            Debug.Assert(index == result.Length);

            // Resolve call operations
            for (int i = 0; i < result.Length; i++)
            {
                switch (result[i].Opcode)
                {
                    case Opcode.Call:
                        result[i] = new LowLevelOperation(result[i].Opcode, startAddresses[result[i].Value] - 1);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }
}
