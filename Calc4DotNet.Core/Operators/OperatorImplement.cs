using System;

namespace Calc4DotNet.Core.Operators
{
    public sealed class OperatorImplement<TNumber>
    {
        public OperatorDefinition Definition { get; }
        public IOperator<TNumber> Operator { get; }

        public OperatorImplement(OperatorDefinition definition, IOperator<TNumber> @operator = null)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Operator = @operator;
        }

        public OperatorImplement<TNumber> WithOperator(IOperator<TNumber> @operator) =>
            new OperatorImplement<TNumber>(this.Definition, @operator);

        public void Deconstruct(out OperatorDefinition definition, out IOperator<TNumber> @operator)
        {
            definition = Definition;
            @operator = Operator;
        }
    }
}
