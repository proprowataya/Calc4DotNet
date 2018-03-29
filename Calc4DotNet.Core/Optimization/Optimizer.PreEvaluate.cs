using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        private sealed class PreEvaluateVisitor<TNumber> : IOperatorVisitor<TNumber, IOperator<TNumber>>
        {
            private readonly CompilationContext<TNumber> context;
            private readonly bool isDefinition;
            private readonly Dictionary<IOperator<TNumber>, bool?> isPreComputable = new Dictionary<IOperator<TNumber>, bool?>();

            public PreEvaluateVisitor(CompilationContext<TNumber> context, bool isDefinition)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.isDefinition = isDefinition;
            }

            private PreComputedOperator<TNumber> PreCompute(IOperator<TNumber> op)
                => new PreComputedOperator<TNumber>(Evaluator.EvaluateGeneric(op, context));

            private IOperator<TNumber> PrecomputeIfPossible(IOperator<TNumber> op)
                => IsPreComputable(op) ? PreCompute(op) : op;

            private bool IsPreComputable(IOperator<TNumber> op)
            {
                bool Core(IOperator<TNumber> current)
                {
                    if (isDefinition && current is ArgumentOperator<TNumber>)
                        return false;

                    var operands = current.Operands;
                    for (int i = 0; i < operands.Count; i++)
                    {
                        if (!IsPreComputable(operands[i]))
                            return false;
                    }

                    if (current is UserDefinedOperator<TNumber> userDefined)
                    {
                        if (!IsPreComputable(context.LookupOperatorImplement(userDefined.Definition.Name).Operator))
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

            public IOperator<TNumber> Visit(ZeroOperator<TNumber> op) => PreCompute(op);

            public IOperator<TNumber> Visit(PreComputedOperator<TNumber> op) => PreCompute(op);

            public IOperator<TNumber> Visit(ArgumentOperator<TNumber> op) => PrecomputeIfPossible(op);

            public IOperator<TNumber> Visit(DefineOperator<TNumber> op) => PreCompute(op);

            public IOperator<TNumber> Visit(ParenthesisOperator<TNumber> op)
            {
                ImmutableArray<IOperator<TNumber>> operators = op.Operators;
                var builder = ImmutableArray.CreateBuilder<IOperator<TNumber>>(operators.Length);

                for (int i = 0; i < operators.Length; i++)
                {
                    builder.Add(operators[i].Accept(this));
                }

                var newOp = new ParenthesisOperator<TNumber>(builder.MoveToImmutable(), op.SupplementaryText);
                return PrecomputeIfPossible(newOp);
            }

            public IOperator<TNumber> Visit(DecimalOperator<TNumber> op)
            {
                var operand = op.Operand.Accept(this);
                var newOp = new DecimalOperator<TNumber>(operand, op.Value, op.SupplementaryText);
                return PrecomputeIfPossible(newOp);
            }

            public IOperator<TNumber> Visit(BinaryOperator<TNumber> op)
            {
                var left = op.Left.Accept(this);
                var right = op.Right.Accept(this);
                var newOp = new BinaryOperator<TNumber>(left, right, op.Type, op.SupplementaryText);
                return PrecomputeIfPossible(newOp);
            }

            public IOperator<TNumber> Visit(ConditionalOperator<TNumber> op)
            {
                var condition = op.Condition.Accept(this);
                var ifTrue = op.IfTrue.Accept(this);
                var ifFalse = op.IfFalse.Accept(this);
                var newOp = new ConditionalOperator<TNumber>(condition, ifTrue, ifFalse, op.SupplementaryText);

                if (IsPreComputable(condition))
                {
                    // TODO: More wise determination method of whether the value is zero or not
                    return (dynamic)PreCompute(condition).Value != 0 ? ifTrue : ifFalse;
                }
                else
                {
                    return newOp;
                }
            }

            public IOperator<TNumber> Visit(UserDefinedOperator<TNumber> op)
            {
                var operands = op.Operands.Select(x => x.Accept(this)).ToImmutableArray();
                var newOp = new UserDefinedOperator<TNumber>(op.Definition, operands, op.IsTailCallable, op.SupplementaryText);
                return PrecomputeIfPossible(newOp);
            }
        }
    }
}
