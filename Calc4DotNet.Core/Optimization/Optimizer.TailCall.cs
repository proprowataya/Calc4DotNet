using System.Collections.Immutable;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        private sealed class TailCallVisitor : IOperatorVisitor<IOperator, bool>
        {
            public IOperator Visit(ZeroOperator op, bool isTailCallable)
            {
                return op;
            }

            public IOperator Visit(PreComputedOperator op, bool isTailCallable)
            {
                return op;
            }

            public IOperator Visit(ArgumentOperator op, bool isTailCallable)
            {
                return op;
            }

            public IOperator Visit(DefineOperator op, bool isTailCallable)
            {
                return op;
            }

            public IOperator Visit(ParenthesisOperator op, bool isTailCallable)
            {
                ImmutableArray<IOperator> operators = op.Operators;
                var builder = ImmutableArray.CreateBuilder<IOperator>(operators.Length);

                for (int i = 0; i < operators.Length; i++)
                {
                    builder.Add(operators[i].Accept(this, i < operators.Length - 1 ? false : isTailCallable));
                }

                return new ParenthesisOperator(builder.MoveToImmutable(), op.SupplementaryText);
            }

            public IOperator Visit(DecimalOperator op, bool isTailCallable)
            {
                return new DecimalOperator(op.Operand.Accept(this, false), op.Value, op.SupplementaryText);
            }

            public IOperator Visit(BinaryOperator op, bool isTailCallable)
            {
                var left = op.Left.Accept(this, false);
                var right = op.Right.Accept(this, false);
                return new BinaryOperator(left, right, op.Type, op.SupplementaryText);
            }

            public IOperator Visit(ConditionalOperator op, bool isTailCallable)
            {
                var condition = op.Condition.Accept(this, false);
                var ifTrue = op.IfTrue.Accept(this, isTailCallable);
                var ifFalse = op.IfFalse.Accept(this, isTailCallable);
                return new ConditionalOperator(condition, ifTrue, ifFalse, op.SupplementaryText);
            }

            public IOperator Visit(UserDefinedOperator op, bool isTailCallable)
            {
                var operands = op.Operands;
                var builder = ImmutableArray.CreateBuilder<IOperator>(operands.Length);

                for (int i = 0; i < operands.Length; i++)
                {
                    builder.Add(operands[i].Accept(this, false));
                }

                return new UserDefinedOperator(op.Definition, builder.MoveToImmutable(), isTailCallable, op.SupplementaryText);
            }
        }
    }
}
