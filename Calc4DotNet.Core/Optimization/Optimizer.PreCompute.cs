using System.Collections.Immutable;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        private sealed class PreComputeVisitor<TNumber> : IOperatorVisitor<IOperator>
            where TNumber : notnull
        {
            private readonly CompilationContext context;
            private readonly int maxStep;

            public PreComputeVisitor(CompilationContext context, int maxStep)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.maxStep = maxStep;
            }

            private IOperator PreComputeIfPossible(IOperator op)
            {
                try
                {
                    return new PreComputedOperator(Evaluator.Evaluate<TNumber>(op, context, maxStep));
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

            public IOperator Visit(ZeroOperator op) => PreComputeIfPossible(op);

            public IOperator Visit(PreComputedOperator op) => PreComputeIfPossible(op);

            public IOperator Visit(ArgumentOperator op) => PreComputeIfPossible(op);

            public IOperator Visit(DefineOperator op) => PreComputeIfPossible(op);

            public IOperator Visit(ParenthesisOperator op)
            {
                ImmutableArray<IOperator> operators = op.Operators;
                var builder = ImmutableArray.CreateBuilder<IOperator>(operators.Length);

                for (int i = 0; i < operators.Length; i++)
                {
                    builder.Add(operators[i].Accept(this));
                }

                var newOp = op with { Operators = builder.MoveToImmutable() };
                return PreComputeIfPossible(newOp);
            }

            public IOperator Visit(DecimalOperator op)
            {
                var operand = op.Operand.Accept(this);
                var newOp = op with { Operand = operand };
                return PreComputeIfPossible(newOp);
            }

            public IOperator Visit(BinaryOperator op)
            {
                var left = op.Left.Accept(this);
                var right = op.Right.Accept(this);
                var newOp = op with { Left = left, Right = right };
                return PreComputeIfPossible(newOp);
            }

            public IOperator Visit(ConditionalOperator op)
            {
                var condition = op.Condition.Accept(this);
                var ifTrue = op.IfTrue.Accept(this);
                var ifFalse = op.IfFalse.Accept(this);
                var newOp = op with { Condition = condition, IfTrue = ifTrue, IfFalse = ifFalse };

                if (PreComputeIfPossible(condition) is PreComputedOperator preComputed)
                {
                    return (dynamic)(TNumber)preComputed.Value != 0 ? ifTrue : ifFalse;
                }
                else
                {
                    return newOp;
                }
            }

            public IOperator Visit(UserDefinedOperator op)
            {
                var operands = op.Operands.Select(x => x.Accept(this)).ToImmutableArray();
                var newOp = op with { Operands = operands };
                return PreComputeIfPossible(newOp);
            }
        }
    }
}
