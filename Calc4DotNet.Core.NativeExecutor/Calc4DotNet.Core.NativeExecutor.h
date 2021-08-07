#pragma once

using namespace System;

#include <cmath>
#include <utility>
#include <vector>
#include "LowLevelOperation.h"

namespace Calc4DotNetCoreNativeExecutor
{
public ref class NativeStackOverflowException : Exception
{};
}

#pragma unmanaged

namespace
{
constexpr int StackSize = 1 << 20;
constexpr int PtrStackSize = 1 << 20;

enum class ExecutionState
{
    Success, StackOverflow
};

template<typename TNumber>
std::pair<ExecutionState, TNumber> ExecuteCore(const LowLevelOperation *operations, const int32_t *maxStackSizes, int numOperations, const TNumber *constTable, int numConstants)
{
    std::vector<TNumber> stack(StackSize);
    std::vector<void *> ptrStack(PtrStackSize);
    TNumber *top = &*stack.begin(), *bottom = &*stack.begin();
    void **ptrTop = &*ptrStack.begin();
    TNumber *stackEnd = &*stack.begin() + stack.size();
    void **ptrStackEndMinusTwo = &*ptrStack.begin() + ptrStack.size() - 2;
    const LowLevelOperation *op = operations;

    while (true)
    {
        switch (op->opcode)
        {
            case Opcode::Push:
                *(top++) = 0;
                break;
            case Opcode::Pop:
                top--;
                break;
            case Opcode::LoadConst:
                *(top++) = op->value;
                break;
            case Opcode::LoadConstTable:
                *(top++) = constTable[op->value];
                break;
            case Opcode::LoadArg:
                *(top++) = bottom[-op->value];
                break;
            case Opcode::StoreArg:
                bottom[-op->value] = *(--top);
                break;
            case Opcode::Input:
                //std::cin >> *(top++);
                // Not implemented
                break;
            case Opcode::Add:
                top--;
                top[-1] = top[-1] + *top;
                break;
            case Opcode::Sub:
                top--;
                top[-1] = top[-1] - *top;
                break;
            case Opcode::Mult:
                top--;
                top[-1] = top[-1] * *top;
                break;
            case Opcode::Div:
                top--;
                top[-1] = top[-1] / *top;
                break;
            case Opcode::Mod:
                top--;
                if constexpr (std::is_same_v<TNumber, double>)
                {
                    top[-1] = std::fmod(top[-1], *top);
                }
                else
                {
                    top[-1] = top[-1] % *top;
                }
                break;
            case Opcode::Goto:
                op = operations + op->value;
                break;
            case Opcode::GotoIfTrue:
                if (*(--top) != 0)
                {
                    op = operations + op->value;
                }
                break;
            case Opcode::GotoIfEqual:
                top -= 2;
                if (*top == top[1])
                {
                    op = operations + op->value;
                }
                break;
            case Opcode::GotoIfLessThan:
                top -= 2;
                if (*top < top[1])
                {
                    op = operations + op->value;
                }
                break;
            case Opcode::GotoIfLessThanOrEqual:
                top -= 2;
                if (*top <= top[1])
                {
                    op = operations + op->value;
                }
                break;
            case Opcode::Call:
            {
                if (top + maxStackSizes[op->value] >= stackEnd)
                {
                    return std::make_pair(ExecutionState::StackOverflow, 0);
                }

                if (ptrTop >= ptrStackEndMinusTwo)
                {
                    return std::make_pair(ExecutionState::StackOverflow, 0);
                }

                *(ptrTop++) = const_cast<void *>(reinterpret_cast<const void *>(op));
                *(ptrTop++) = reinterpret_cast<void *>(bottom);
                bottom = top;
                op = operations + op->value;
                break;
            }
            case Opcode::Return:
            {
                TNumber returnValue = top[-1];
                top = bottom - op->value + 1;
                top[-1] = returnValue;
                bottom = reinterpret_cast<TNumber *>(*(--ptrTop));
                op = reinterpret_cast<const LowLevelOperation *>(*(--ptrTop));
                break;
            }
            case Opcode::Halt:
                return std::make_pair(ExecutionState::Success, top[-1]);
                break;
            case Opcode::Lavel:
            default:
                __assume(false);
                break;
        }

        op++;
    }
}

#pragma managed

template<typename TNumber>
static TNumber ExecuteInternal(const void *operations, const int32_t *maxStackSizes, int numOperations, const TNumber *constTable, int numConstants)
{
    auto result = ExecuteCore<TNumber>(reinterpret_cast<const LowLevelOperation *>(operations), maxStackSizes, numOperations, constTable, numConstants);

    switch (result.first)
    {
        case ExecutionState::Success:
            return result.second;
            break;
        case ExecutionState::StackOverflow:
            throw gcnew Calc4DotNetCoreNativeExecutor::NativeStackOverflowException();
        default:
            __assume(false);
            throw gcnew InvalidOperationException();
    }
}
}

namespace Calc4DotNetCoreNativeExecutor
{
public ref class NativeLowLevelExecutor
{
public:
    static int32_t Execute(const void *operations, const int32_t *maxStackSizes, int numOperations, const int32_t *constTable, int numConstants)
    {
        return ExecuteInternal<int32_t>(operations, maxStackSizes, numOperations, constTable, numConstants);
    }

    static int64_t Execute(const void *operations, const int32_t *maxStackSizes, int numOperations, const int64_t *constTable, int numConstants)
    {
        return ExecuteInternal<int64_t>(operations, maxStackSizes, numOperations, constTable, numConstants);
    }

    static double Execute(const void *operations, const int32_t *maxStackSizes, int numOperations, const double *constTable, int numConstants)
    {
        return ExecuteInternal<double>(operations, maxStackSizes, numOperations, constTable, numConstants);
    }
};
}
