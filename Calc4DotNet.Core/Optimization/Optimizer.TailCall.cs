using System.Collections.Immutable;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization;

public static partial class Optimizer
{
    private sealed class TailCallVisitor : IOperatorVisitor<IOperator, bool>
    {
        public IOperator Visit(ZeroOperator op, bool isTailCall)
        {
            return op;
        }

        public IOperator Visit(PreComputedOperator op, bool isTailCall)
        {
            return op;
        }

        public IOperator Visit(ArgumentOperator op, bool isTailCall)
        {
            return op;
        }

        public IOperator Visit(DefineOperator op, bool isTailCall)
        {
            return op;
        }

        public IOperator Visit(LoadVariableOperator op, bool isTailCall)
        {
            return op;
        }

        public IOperator Visit(LoadArrayOperator op, bool isTailCall)
        {
            var index = op.Index.Accept(this, isTailCall);
            return op with { Index = index };
        }

        public IOperator Visit(ParenthesisOperator op, bool isTailCall)
        {
            ImmutableArray<IOperator> operators = op.Operators;
            var builder = ImmutableArray.CreateBuilder<IOperator>(operators.Length);

            for (int i = 0; i < operators.Length; i++)
            {
                builder.Add(operators[i].Accept(this, i < operators.Length - 1 ? false : isTailCall));
            }

            return op with { Operators = builder.MoveToImmutable() };
        }

        public IOperator Visit(DecimalOperator op, bool isTailCall)
        {
            return op with { Operand = op.Operand.Accept(this, false) };
        }

        public IOperator Visit(StoreVariableOperator op, bool isTailCall)
        {
            return op with { Operand = op.Operand.Accept(this, true) };
        }

        public IOperator Visit(StoreArrayOperator op, bool isTailCall)
        {
            var value = op.Value.Accept(this, isTailCall);
            var index = op.Index.Accept(this, isTailCall);
            return op with { Value = value, Index = index };
        }

        public IOperator Visit(BinaryOperator op, bool isTailCall)
        {
            var left = op.Left.Accept(this, false);
            var right = op.Right.Accept(this, false);
            return op with { Left = left, Right = right };
        }

        public IOperator Visit(ConditionalOperator op, bool isTailCall)
        {
            var condition = op.Condition.Accept(this, false);
            var ifTrue = op.IfTrue.Accept(this, isTailCall);
            var ifFalse = op.IfFalse.Accept(this, isTailCall);
            return op with { Condition = condition, IfTrue = ifTrue, IfFalse = ifFalse };
        }

        public IOperator Visit(UserDefinedOperator op, bool isTailCall)
        {
            var operands = op.Operands;
            var builder = ImmutableArray.CreateBuilder<IOperator>(operands.Length);

            for (int i = 0; i < operands.Length; i++)
            {
                builder.Add(operands[i].Accept(this, false));
            }

            return op with { Operands = builder.MoveToImmutable(), IsTailCall = isTailCall };
        }
    }
}
