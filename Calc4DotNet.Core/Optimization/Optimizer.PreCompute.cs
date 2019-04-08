﻿using System;
using System.Collections.Immutable;
using System.Linq;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization
{
    public static partial class Optimizer
    {
        private sealed class PreComputeVisitor<TNumber> : IOperatorVisitor<IOperator>
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

                var newOp = new ParenthesisOperator(builder.MoveToImmutable(), op.SupplementaryText);
                return PreComputeIfPossible(newOp);
            }

            public IOperator Visit(DecimalOperator op)
            {
                var operand = op.Operand.Accept(this);
                var newOp = new DecimalOperator(operand, op.Value, op.SupplementaryText);
                return PreComputeIfPossible(newOp);
            }

            public IOperator Visit(BinaryOperator op)
            {
                var left = op.Left.Accept(this);
                var right = op.Right.Accept(this);
                var newOp = new BinaryOperator(left, right, op.Type, op.SupplementaryText);
                return PreComputeIfPossible(newOp);
            }

            public IOperator Visit(ConditionalOperator op)
            {
                var condition = op.Condition.Accept(this);
                var ifTrue = op.IfTrue.Accept(this);
                var ifFalse = op.IfFalse.Accept(this);
                var newOp = new ConditionalOperator(condition, ifTrue, ifFalse, op.SupplementaryText);

                if (PreComputeIfPossible(condition) is PreComputedOperator preComputed)
                {
                    // TODO: More wise determination method of whether the value is zero or not
                    return (dynamic)preComputed.Value != 0 ? ifTrue : ifFalse;
                }
                else
                {
                    return newOp;
                }
            }

            public IOperator Visit(UserDefinedOperator op)
            {
                var operands = op.Operands.Select(x => x.Accept(this)).ToImmutableArray();
                var newOp = new UserDefinedOperator(op.Definition, operands, op.IsTailCallable, op.SupplementaryText);
                return PreComputeIfPossible(newOp);
            }
        }
    }
}