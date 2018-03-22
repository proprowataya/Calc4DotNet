using System;
using System.Linq;

namespace Calc4DotNet.Core.Execution
{
    public static class Executor
    {
        public const int StackSizeByBytes = 1 << 20;
        private const int NumPtrSave = 1 << 20;

        public unsafe static Int64 ExecuteInt64(Module<Int64> module)
        {
            Int64[] stack = new Int64[StackSizeByBytes / sizeof(Int64)];
            void*[] prtSave = new void*[NumPtrSave];

            fixed (LowLevelOperation* _operations = module.Operations.ToArray())
            fixed (Int64* _stack = stack)
            fixed (void** _prtSave = prtSave)
            {
                LowLevelOperation* op = _operations;
                Int64* top = _stack;
                Int64* bottom = _stack;
                void** ptrSaveTop = _prtSave;

                for (; ; op++)
                {
                    switch (op->Opcode)
                    {
                        case Opcode.Push:
                            top++;
                            break;
                        case Opcode.Pop:
                            top--;
                            break;
                        case Opcode.LoadConst:
                            *top = op->Value;
                            top++;
                            break;
                        case Opcode.LoadConstTable:
                            *top = module.ConstTable[op->Value];
                            top++;
                            break;
                        case Opcode.LoadArg:
                            *top = bottom[-op->Value];
                            top++;
                            break;
                        case Opcode.StoreArg:
                            bottom[-op->Value] = *--top;
                            break;
                        case Opcode.Input:
                            Console.Write("Input >> ");
                            *top = Int64.Parse(Console.ReadLine());
                            top++;
                            break;
                        case Opcode.Add:
                            top[-2] = top[-2] + top[-1];
                            top--;
                            break;
                        case Opcode.Sub:
                            top[-2] = top[-2] - top[-1];
                            top--;
                            break;
                        case Opcode.Mult:
                            top[-2] = top[-2] * top[-1];
                            top--;
                            break;
                        case Opcode.Div:
                            top[-2] = top[-2] / top[-1];
                            top--;
                            break;
                        case Opcode.Mod:
                            top[-2] = top[-2] % top[-1];
                            top--;
                            break;
                        case Opcode.Goto:
                            op = _operations + op->Value;
                            break;
                        case Opcode.GotoIfTrue:
                            if (*--top != 0)
                            {
                                op = _operations + op->Value;
                            }
                            break;
                        case Opcode.GotoIfEqual:
                            top -= 2;
                            if (top[0] == top[1])
                            {
                                op = _operations + op->Value;
                            }
                            break;
                        case Opcode.GotoIfLessThan:
                            top -= 2;
                            if (top[0] < top[1])
                            {
                                op = _operations + op->Value;
                            }
                            break;
                        case Opcode.GotoIfLessThanOrEqual:
                            top -= 2;
                            if (top[0] <= top[1])
                            {
                                op = _operations + op->Value;
                            }
                            break;
                        case Opcode.Call:
                            *ptrSaveTop++ = op;                     // Push current program counter
                            *ptrSaveTop++ = bottom;                 // Push current stack bottom
                            bottom = top;                           // Create new stack frame
                            op = _operations + op->Value;           // Branch
                            break;
                        case Opcode.Return:
                            Int64 returningValue = top[-1];
                            top = bottom - op->Value + 1;           // Restore previous stack top
                                                                    // while removing arguments from stack
                                                                    // (We ensure space of returning value)
                            top[-1] = returningValue;               // Store returning value on stack
                            bottom = (Int64*)*--ptrSaveTop;         // Pop previous stack bottom
                            op = (LowLevelOperation*)*--ptrSaveTop; // Pop previous program counter
                            break;
                        case Opcode.Halt:
                            return top[-1];
                        case Opcode.Lavel:
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }
        }
    }
}
