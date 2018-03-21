using System;

namespace Calc4DotNet.Core.Operators
{
    public sealed class OperatorDefinition
    {
        public string Name { get; }
        public int NumOperands { get; }

        public OperatorDefinition(string name, int numOperands)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            NumOperands = numOperands;
        }

        public override string ToString() => $"Definition of operator \"{Name}\" ({NumOperands} operand{(NumOperands > 1 ? "s" : "")})";
    }
}
