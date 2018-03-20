using System;

namespace Calc4DotNet.Core.Operators
{
    public sealed class OperatorDefinition
    {
        public string Name { get; }
        public int NumOperands { get; }
        public IOperator Root { get; internal set; }

        public OperatorDefinition(string name, int numOperands, IOperator root)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            NumOperands = numOperands;
            Root = root;
        }

        public override string ToString() => $"Definition of operator \"{Name}\" ({NumOperands} operand{(NumOperands > 1 ? "s" : "")})";
    }
}
