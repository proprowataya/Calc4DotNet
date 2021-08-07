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
    std::vector<int> ptrStack(PtrStackSize);
    int top = 0, bottom = 0;
    int ptrIndex = 0;
    const LowLevelOperation *op = operations;

    while (true)
    {
        switch (op->opcode)
        {
            case Opcode::Push:
                stack[top++] = 0;
                break;
            case Opcode::Pop:
                top--;
                break;
            case Opcode::LoadConst:
                stack[top++] = op->value;
                break;
            case Opcode::LoadConstTable:
                stack[top++] = constTable[op->value];
                break;
            case Opcode::LoadArg:
                stack[top++] = stack[bottom - op->value];
                break;
            case Opcode::StoreArg:
                stack[bottom - op->value] = stack[--top];
                break;
            case Opcode::Input:
                //std::cin >> stack[top++];
                // Not implemented
                break;
            case Opcode::Add:
                top--;
                stack[top - 1] = stack[top - 1] + stack[top];
                break;
            case Opcode::Sub:
                top--;
                stack[top - 1] = stack[top - 1] - stack[top];
                break;
            case Opcode::Mult:
                top--;
                stack[top - 1] = stack[top - 1] * stack[top];
                break;
            case Opcode::Div:
                top--;
                stack[top - 1] = stack[top - 1] / stack[top];
                break;
            case Opcode::Mod:
                top--;
                if constexpr (std::is_same_v<TNumber, double>)
                {
                    stack[top - 1] = std::fmod(stack[top - 1], stack[top]);
                }
                else
                {
                    stack[top - 1] = stack[top - 1] % stack[top];
                }
                break;
            case Opcode::Goto:
                op = operations + op->value;
                break;
            case Opcode::GotoIfTrue:
                if (stack[--top] != 0)
                {
                    op = operations + op->value;
                }
                break;
            case Opcode::GotoIfEqual:
                top -= 2;
                if (stack[top] == stack[top + 1])
                {
                    op = operations + op->value;
                }
                break;
            case Opcode::GotoIfLessThan:
                top -= 2;
                if (stack[top] < stack[top + 1])
                {
                    op = operations + op->value;
                }
                break;
            case Opcode::GotoIfLessThanOrEqual:
                top -= 2;
                if (stack[top] <= stack[top + 1])
                {
                    op = operations + op->value;
                }
                break;
            case Opcode::Call:
            {
                if (top + maxStackSizes[op->value] >= static_cast<int>(stack.size()))
                {
                    return std::make_pair(ExecutionState::StackOverflow, 0);
                }

                if (ptrIndex + 2 >= static_cast<int>(ptrStack.size()))
                {
                    return std::make_pair(ExecutionState::StackOverflow, 0);
                }

                ptrStack[ptrIndex++] = static_cast<int>(op - operations);
                ptrStack[ptrIndex++] = bottom;
                bottom = top;
                op = operations + op->value;
                break;
            }
            case Opcode::Return:
            {
                TNumber returnValue = stack[top - 1];
                top = bottom - op->value + 1;
                stack[top - 1] = returnValue;
                bottom = ptrStack[--ptrIndex];
                op = operations + ptrStack[--ptrIndex];
                break;
            }
            case Opcode::Halt:
                return std::make_pair(ExecutionState::Success, stack[top - 1]);
                break;
            case Opcode::Lavel:
            default:
                //throw std::runtime_error("Assertion failed");
                break;
        }

        op++;
    }
}

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
