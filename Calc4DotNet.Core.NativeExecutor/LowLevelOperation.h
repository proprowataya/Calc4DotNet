#pragma once

#include <cstdint>

enum class Opcode : uint8_t
{
    Push, Pop, LoadConst, LoadConstTable, LoadArg, StoreArg, Input,
    Add, Sub, Mult, Div, Mod,
    Goto, GotoIfTrue, GotoIfEqual, GotoIfLessThan, GotoIfLessThanOrEqual,
    Call, Return, Halt,
    Lavel,
};

struct LowLevelOperation
{
    Opcode opcode;
    short value;
};
