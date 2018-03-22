using System.Collections.Immutable;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        private sealed class TailCallVisitor<TNumber> : IOperatorVisitor<TNumber, IOperator<TNumber>, bool>
        {
            public IOperator<TNumber> Visit(ZeroOperator<TNumber> op, bool isTailCallable)
            {
                return op;
            }

            public IOperator<TNumber> Visit(PreComputedOperator<TNumber> op, bool isTailCallable)
            {
                return op;
            }

            public IOperator<TNumber> Visit(ArgumentOperator<TNumber> op, bool isTailCallable)
            {
                return op;
            }

            public IOperator<TNumber> Visit(DefineOperator<TNumber> op, bool isTailCallable)
            {
                return op;
            }

            public IOperator<TNumber> Visit(ParenthesisOperator<TNumber> op, bool isTailCallable)
            {
                ImmutableArray<IOperator<TNumber>> operators = op.Operators;
                var builder = ImmutableArray.CreateBuilder<IOperator<TNumber>>(operators.Length);

                for (int i = 0; i < operators.Length; i++)
                {
                    builder.Add(operators[i].Accept(this, i < operators.Length - 1 ? false : isTailCallable));
                }

                return new ParenthesisOperator<TNumber>(builder.MoveToImmutable(), op.SupplementaryText);
            }

            public IOperator<TNumber> Visit(DecimalOperator<TNumber> op, bool isTailCallable)
            {
                return new DecimalOperator<TNumber>(op.Operand.Accept(this, false), op.Value, op.SupplementaryText);
            }

            public IOperator<TNumber> Visit(BinaryOperator<TNumber> op, bool isTailCallable)
            {
                var left = op.Left.Accept(this, false);
                var right = op.Right.Accept(this, false);
                return new BinaryOperator<TNumber>(left, right, op.Type, op.SupplementaryText);
            }

            public IOperator<TNumber> Visit(ConditionalOperator<TNumber> op, bool isTailCallable)
            {
                var condition = op.Condition.Accept(this, false);
                var ifTrue = op.IfTrue.Accept(this, isTailCallable);
                var ifFalse = op.IfFalse.Accept(this, isTailCallable);
                return new ConditionalOperator<TNumber>(condition, ifTrue, ifFalse, op.SupplementaryText);
            }

            public IOperator<TNumber> Visit(UserDefinedOperator<TNumber> op, bool isTailCallable)
            {
                var operands = op.Operands;
                var builder = ImmutableArray.CreateBuilder<IOperator<TNumber>>(operands.Length);

                for (int i = 0; i < operands.Length; i++)
                {
                    builder.Add(operands[i].Accept(this, isTailCallable));
                }

                return new UserDefinedOperator<TNumber>(op.Definition, builder.MoveToImmutable(), isTailCallable, op.SupplementaryText);
            }
        }
    }
}
