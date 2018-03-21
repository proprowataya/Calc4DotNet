using System;
using System.Linq;

namespace Calc4DotNet.Core.Execution
{
    public readonly struct LowLevelOperation
    {
        private static readonly int MaxOpcodeNameLength = Enum.GetNames(typeof(Opcode)).Max(s => s.Length);

        public Opcode Opcode { get; }
        public int Value { get; }

        public LowLevelOperation(Opcode opcode, int value = default)
        {
            Opcode = opcode;
            Value = value;
        }

        public override string ToString()
        {
            string str = Opcode.ToString();
            return $"{str.PadRight(MaxOpcodeNameLength)} [Value = {Value}]";
        }
    }

    public enum Opcode : short
    {
        Push, Pop, LoadConst, LoadConstTable, Store, LoadArg, Input,
        Add, Sub, Mult, Div, Mod, LessThan, LessThanOrEqual, GreaterThanOrEqual, GreaterThan,
        Goto, GotoIfTrue, GotoIfFalse,
        Call, Return, Halt,
        Lavel,
    }
}
