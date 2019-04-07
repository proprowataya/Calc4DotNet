using System;

namespace Calc4DotNet.Core.Operators
{
    public sealed class OperatorImplement
    {
        public OperatorDefinition Definition { get; }
        public IOperator Operator { get; }

        public OperatorImplement(OperatorDefinition definition, IOperator @operator = null)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Operator = @operator;
        }

        public OperatorImplement WithOperator(IOperator @operator) =>
            new OperatorImplement(this.Definition, @operator);

        public void Deconstruct(out OperatorDefinition definition, out IOperator @operator)
        {
            definition = Definition;
            @operator = Operator;
        }
    }
}
