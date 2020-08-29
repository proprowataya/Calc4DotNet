namespace Calc4DotNet.Core.Operators
{
    public sealed record OperatorDefinition(string Name, int NumOperands)
    {
        public override string ToString() => $"Definition of operator \"{Name}\" ({NumOperands} operand{(NumOperands > 1 ? "s" : "")})";
    }
}
