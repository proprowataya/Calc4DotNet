using System.Diagnostics.CodeAnalysis;

namespace Calc4DotNet.Core.Execution;

public readonly struct LowLevelOperation : IEquatable<LowLevelOperation>
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

    public bool Equals(LowLevelOperation other)
    {
        return (Opcode, Value) == (other.Opcode, other.Value);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is LowLevelOperation other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Opcode, Value);
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
