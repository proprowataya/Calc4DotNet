namespace Calc4DotNet.Core.Execution;

public readonly struct LowLevelOperation
{
    private static readonly int MaxOpcodeNameLength = Enum.GetNames(typeof(Opcode)).Max(s => s.Length);

    public Opcode Opcode { get; }
    public short Value { get; }

    public LowLevelOperation(Opcode opcode, int value = default)
    {
        Opcode = opcode;
        Value = checked((short)value);
    }

    public override string ToString()
    {
        string str = Opcode.ToString();
        return $"{str.PadRight(MaxOpcodeNameLength)} [Value = {Value}]";
    }
}

public enum Opcode : byte
{
    Push, Pop, LoadConst, LoadConstTable, LoadArg, StoreArg, LoadVariable, StoreVariable, Input,
    Add, Sub, Mult, Div, Mod,
    Goto, GotoIfTrue, GotoIfEqual, GotoIfLessThan, GotoIfLessThanOrEqual,
    Call, Return, Halt,
    Lavel,
}
