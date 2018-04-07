using System;
using System.Collections.Immutable;
using System.Linq;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        private sealed class PreComputeVisitor<TNumber> : IOperatorVisitor<TNumber, IOperator<TNumber>>
        {
            private readonly CompilationContext<TNumber> context;
            private readonly int maxStep;

            public PreComputeVisitor(CompilationContext<TNumber> context, int maxStep)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.maxStep = maxStep;
            }

            private IOperator<TNumber> PreComputeIfPossible(IOperator<TNumber> op)
            {
                try
                {
                    return new PreComputedOperator<TNumber>(Evaluator.EvaluateGeneric(op, context, maxStep));
                }
                catch (EvaluationStepLimitExceedException)
                {
                    return op;
                }
                catch (EvaluationArgumentNotSetException)
                {
                    return op;
                }
            }

            public IOperator<TNumber> Visit(ZeroOperator<TNumber> op) => PreComputeIfPossible(op);

            public IOperator<TNumber> Visit(PreComputedOperator<TNumber> op) => PreComputeIfPossible(op);

            public IOperator<TNumber> Visit(ArgumentOperator<TNumber> op) => PreComputeIfPossible(op);

            public IOperator<TNumber> Visit(DefineOperator<TNumber> op) => PreComputeIfPossible(op);

            public IOperator<TNumber> Visit(ParenthesisOperator<TNumber> op)
            {
                ImmutableArray<IOperator<TNumber>> operators = op.Operators;
                var builder = ImmutableArray.CreateBuilder<IOperator<TNumber>>(operators.Length);

                for (int i = 0; i < operators.Length; i++)
                {
                    builder.Add(operators[i].Accept(this));
                }

                var newOp = new ParenthesisOperator<TNumber>(builder.MoveToImmutable(), op.SupplementaryText);
                return PreComputeIfPossible(newOp);
            }

            public IOperator<TNumber> Visit(DecimalOperator<TNumber> op)
            {
                var operand = op.Operand.Accept(this);
                var newOp = new DecimalOperator<TNumber>(operand, op.Value, op.SupplementaryText);
                return PreComputeIfPossible(newOp);
            }

            public IOperator<TNumber> Visit(BinaryOperator<TNumber> op)
            {
                var left = op.Left.Accept(this);
                var right = op.Right.Accept(this);
                var newOp = new BinaryOperator<TNumber>(left, right, op.Type, op.SupplementaryText);
                return PreComputeIfPossible(newOp);
            }

            public IOperator<TNumber> Visit(ConditionalOperator<TNumber> op)
            {
                var condition = op.Condition.Accept(this);
                var ifTrue = op.IfTrue.Accept(this);
                var ifFalse = op.IfFalse.Accept(this);
                var newOp = new ConditionalOperator<TNumber>(condition, ifTrue, ifFalse, op.SupplementaryText);

                if (PreComputeIfPossible(condition) is PreComputedOperator<TNumber> preComputed)
                {
                    // TODO: More wise determination method of whether the value is zero or not
                    return (dynamic)preComputed.Value != 0 ? ifTrue : ifFalse;
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
                return PreComputeIfPossible(newOp);
            }
        }
    }
}
