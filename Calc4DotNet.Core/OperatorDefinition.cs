using Calc4DotNet.Core.Operators;
using System;

namespace Calc4DotNet.Core
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
    }
}
