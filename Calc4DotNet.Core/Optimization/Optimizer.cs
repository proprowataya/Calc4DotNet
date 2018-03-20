using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static class Optimizer
    {
        public static IOperator Optimize(IOperator op, Context context)
        {
            return op.Accept(new Visitor(context, false));
        }

        public static OperatorDefinition Optimize(OperatorDefinition definition, Context context)
        {
            return new OperatorDefinition(definition.Name, definition.NumOperands, definition.Root.Accept(new Visitor(context, true)));
        }

        private sealed class Visitor : IOperatorVisitor<IOperator>
        {
            private readonly Context context;
            private readonly bool isDefinition;
            private readonly Dictionary<IOperator, bool?> isPreComputable = new Dictionary<IOperator, bool?>();

            public Visitor(Context context, bool isDefinition)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.isDefinition = isDefinition;
            }

            private PreComputedOperator PerformPreCompute(IOperator op) => new PreComputedOperator(op.Evaluate(context, default));
            private IOperator PrecomputeIfPossible(IOperator op) => IsPreComputable(op) ? PerformPreCompute(op) : op;

            private bool IsPreComputable(IOperator op)
            {
                bool Core(IOperator current)
                {
                    if (!current.ThisTypeIsPreComputable)
                        return false;
                    if (isDefinition && current is ArgumentOperator)
                        return false;

                    ImmutableArray<IOperator> operands = current.Operands;
                    for (int i = 0; i < operands.Length; i++)
                    {
                        if (!IsPreComputable(operands[i]))
                            return false;
                    }

                    if (current is UserDefinedOperator userDefined)
                    {
                        if (!IsPreComputable(userDefined.Definition.Root))
                            return false;
                    }

                    return true;
                }

                if (isPreComputable.TryGetValue(op, out var result))
                {
                    if (result is bool b)
                    {
                        return b;
                    }
                    else
                    {
                        isPreComputable[op] = false;
                        return false;
                    }
                }

                isPreComputable[op] = null;
                return (isPreComputable[op] = Core(op)).Value;
            }

            public IOperator Visit(ZeroOperator op) => PerformPreCompute(op);

            public IOperator Visit(PreComputedOperator op) => PerformPreCompute(op);

            public IOperator Visit(ArgumentOperator op) => PrecomputeIfPossible(op);

            public IOperator Visit(DefineOperator op) => PerformPreCompute(op);

            public IOperator Visit(ParenthesisOperator op)
            {
                ImmutableArray<IOperator> operators = op.Operators;
                var builder = ImmutableArray.CreateBuilder<IOperator>(operators.Length);

                for (int i = 0; i < operators.Length; i++)
                {
                    builder.Add(operators[i].Accept(this));
                }

                var newOp = new ParenthesisOperator(builder.MoveToImmutable(), op.SupplementaryText);
                return PrecomputeIfPossible(newOp);
            }

            public IOperator Visit(DecimalOperator op)
            {
                var operand = op.Operand.Accept(this);
                var newOp = new DecimalOperator(operand, op.Value, op.SupplementaryText);
                return PrecomputeIfPossible(newOp);
            }

            public IOperator Visit(BinaryOperator op)
            {
                var left = op.Left.Accept(this);
                var right = op.Right.Accept(this);
                var newOp = new BinaryOperator(left, right, op.Type, op.SupplementaryText);
                return PrecomputeIfPossible(newOp);
            }

            public IOperator Visit(ConditionalOperator op)
            {
                var condition = op.Condition.Accept(this);
                var ifTrue = op.IfTrue.Accept(this);
                var ifFalse = op.IfFalse.Accept(this);
                var newOp = new ConditionalOperator(condition, ifTrue, ifFalse, op.SupplementaryText);

                if (IsPreComputable(condition))
                {
                    return PerformPreCompute(condition).Value != 0 ? ifTrue : ifFalse;
                }
                else
                {
                    return newOp;
                }
            }

            public IOperator Visit(UserDefinedOperator op)
            {
                var operands = op.Operands.Select(x => x.Accept(this)).ToImmutableArray();
                var newOp = new UserDefinedOperator(op.Definition, operands, op.SupplementaryText);
                return PrecomputeIfPossible(newOp);
            }
        }
    }
}
