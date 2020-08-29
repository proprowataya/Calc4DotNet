﻿using System.Collections.Immutable;
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

                return op with { Operators = builder.MoveToImmutable() };
            }

            public IOperator Visit(DecimalOperator op, bool isTailCallable)
            {
                return op with { Operand = op.Operand.Accept(this, false) };
            }

            public IOperator Visit(BinaryOperator op, bool isTailCallable)
            {
                var left = op.Left.Accept(this, false);
                var right = op.Right.Accept(this, false);
                return op with { Left = left, Right = right };
            }

            public IOperator Visit(ConditionalOperator op, bool isTailCallable)
            {
                var condition = op.Condition.Accept(this, false);
                var ifTrue = op.IfTrue.Accept(this, isTailCallable);
                var ifFalse = op.IfFalse.Accept(this, isTailCallable);
                return op with { Condition = condition, IfTrue = ifTrue, IfFalse = ifFalse };
            }

            public IOperator Visit(UserDefinedOperator op, bool isTailCallable)
            {
                var operands = op.Operands;
                var builder = ImmutableArray.CreateBuilder<IOperator>(operands.Length);

                for (int i = 0; i < operands.Length; i++)
                {
                    builder.Add(operands[i].Accept(this, false));
                }

                return op with { Operands = builder.MoveToImmutable(), IsTailCallable = isTailCallable };
            }
        }
    }
}
