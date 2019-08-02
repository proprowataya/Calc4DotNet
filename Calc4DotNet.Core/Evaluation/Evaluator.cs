using System;
using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Evaluation
{
    public sealed class EvaluationStepLimitExceedException : Exception
    { }

    internal sealed class EvaluationArgumentNotSetException : Exception
    { }

    public static class Evaluator
    {
        internal static TNumber Evaluate<TNumber>(IOperator op, CompilationContext context, int maxStep, TNumber dummy)
        {
            return Evaluate<TNumber>(op, context, maxStep);
        }

        public static TNumber Evaluate<TNumber>(IOperator op, CompilationContext context, int maxStep = int.MaxValue)
        {
            if (typeof(TNumber) == typeof(Int32))
            {
                var result = op.Accept(new Visitor<Int32, Int32Computer>(maxStep), (context, null));
                return Unsafe.As<Int32, TNumber>(ref result);
            }
            else if (typeof(TNumber) == typeof(Int64))
            {
                var result = op.Accept(new Visitor<Int64, Int64Computer>(maxStep), (context, null));
                return Unsafe.As<Int64, TNumber>(ref result);
            }
            else if (typeof(TNumber) == typeof(Double))
            {
                var result = op.Accept(new Visitor<Double, DoubleComputer>(maxStep), (context, null));
                return Unsafe.As<Double, TNumber>(ref result);
            }
            else if (typeof(TNumber) == typeof(BigInteger))
            {
                var result = op.Accept(new Visitor<BigInteger, BigIntegerComputer>(maxStep), (context, null));
                return Unsafe.As<BigInteger, TNumber>(ref result);
            }
            else
            {
                throw new Calc4DotNet.Core.Exceptions.TypeNotSupportedException(typeof(TNumber));
            }
        }

        private sealed class Visitor<TNumber, TNumberComputer>
            : IOperatorVisitor<TNumber, (CompilationContext context, TNumber[] arguments)>
            where TNumberComputer : INumberComputer<TNumber>
        {
            private readonly int maxStep = 0;
            private int step;

            public Visitor(int maxStep)
            {
                this.maxStep = maxStep;
            }

            /* ******************** */

            public TNumber Visit(ZeroOperator op, (CompilationContext context, TNumber[] arguments) param)
                => default(TNumberComputer).Zero;

            public TNumber Visit(PreComputedOperator op, (CompilationContext context, TNumber[] arguments) param)
                => (TNumber)op.Value;

            public TNumber Visit(ArgumentOperator op, (CompilationContext context, TNumber[] arguments) param)
            {
                if (param.arguments is null)
                    throw new EvaluationArgumentNotSetException();
                return param.arguments[op.Index];
            }

            public TNumber Visit(DefineOperator op, (CompilationContext context, TNumber[] arguments) param)
                => default(TNumberComputer).Zero;

            public TNumber Visit(ParenthesisOperator op, (CompilationContext context, TNumber[] arguments) param)
            {
                TNumber result = default(TNumberComputer).Zero;
                ImmutableArray<IOperator> operators = op.Operators;

                for (int i = 0; i < operators.Length; i++)
                {
                    result = operators[i].Accept(this, param);
                }

                return result;
            }

            public TNumber Visit(DecimalOperator op, (CompilationContext context, TNumber[] arguments) param)
            {
                var c = default(TNumberComputer);
                TNumber operand = op.Operand.Accept(this, param);
                return c.Add(c.Multiply(operand, c.Ten), c.FromInt(op.Value));
            }

            public TNumber Visit(BinaryOperator op, (CompilationContext context, TNumber[] arguments) param)
            {
                var c = default(TNumberComputer);
                TNumber left = op.Left.Accept(this, param);
                TNumber right = op.Right.Accept(this, param);

                switch (op.Type)
                {
                    case BinaryType.Add:
                        return c.Add(left, right);
                    case BinaryType.Sub:
                        return c.Subtract(left, right);
                    case BinaryType.Mult:
                        return c.Multiply(left, right);
                    case BinaryType.Div:
                        return c.Divide(left, right);
                    case BinaryType.Mod:
                        return c.Modulo(left, right);
                    case BinaryType.Equal:
                        return c.Equals(left, right) ? c.One : c.Zero;
                    case BinaryType.NotEqual:
                        return c.NotEquals(left, right) ? c.One : c.Zero;
                    case BinaryType.LessThan:
                        return c.LessThan(left, right) ? c.One : c.Zero;
                    case BinaryType.LessThanOrEqual:
                        return c.LessThanOrEquals(left, right) ? c.One : c.Zero;
                    case BinaryType.GreaterThanOrEqual:
                        return c.GreaterThanOrEquals(left, right) ? c.One : c.Zero;
                    case BinaryType.GreaterThan:
                        return c.GreaterThan(left, right) ? c.One : c.Zero;
                    default:
                        throw new InvalidOperationException();
                }
            }

            public TNumber Visit(ConditionalOperator op, (CompilationContext context, TNumber[] arguments) param)
            {
                var c = default(TNumberComputer);
                TNumber condition = op.Condition.Accept(this, param);
                return c.NotEquals(condition, c.Zero) ? op.IfTrue.Accept(this, param) : op.IfFalse.Accept(this, param);
            }

            public TNumber Visit(UserDefinedOperator op, (CompilationContext context, TNumber[] arguments) param)
            {
                if (++step > maxStep)
                {
                    throw new EvaluationStepLimitExceedException();
                }

                TNumber[] stack = new TNumber[op.Operands.Length];

                for (int i = 0; i < op.Operands.Length; i++)
                {
                    stack[i] = op.Operands[i].Accept(this, param);
                }

                return param.context.LookupOperatorImplement(op.Definition.Name).Operator.Accept(this, (param.context, stack));
            }
        }
    }
}
