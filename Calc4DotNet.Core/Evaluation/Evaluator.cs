using System;
using System.Collections.Immutable;
using System.Diagnostics;
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
        public static TNumber Evaluate<TNumber>(IOperator op, CompilationContext context, int maxStep = int.MaxValue)
            where TNumber : notnull
        {
            // TODO:
            // The following casts by Unsafe.As assume that the TNumber and TNumberOperatable have
            // the same memory layout.

            if (typeof(TNumber) == typeof(Int32))
            {
                var result = op.Accept(new Visitor<Int32, Int32Operatable>(maxStep), (context, null));
                return Unsafe.As<Int32Operatable, TNumber>(ref result);
            }
            else if (typeof(TNumber) == typeof(Int64))
            {
                var result = op.Accept(new Visitor<Int64, Int64Operatable>(maxStep), (context, null));
                return Unsafe.As<Int64Operatable, TNumber>(ref result);
            }
            else if (typeof(TNumber) == typeof(Double))
            {
                var result = op.Accept(new Visitor<Double, DoubleOperatable>(maxStep), (context, null));
                return Unsafe.As<DoubleOperatable, TNumber>(ref result);
            }
            else if (typeof(TNumber) == typeof(BigInteger))
            {
                var result = op.Accept(new Visitor<BigInteger, BigIntegerOperatable>(maxStep), (context, null));
                return Unsafe.As<BigIntegerOperatable, TNumber>(ref result);
            }
            else
            {
                throw new Calc4DotNet.Core.Exceptions.TypeNotSupportedException(typeof(TNumber));
            }
        }

        private sealed class Visitor<TNumberOriginal, TNumberOperatable>
            : IOperatorVisitor<TNumberOperatable, (CompilationContext context, TNumberOperatable[]? arguments)>
            where TNumberOriginal : struct
            where TNumberOperatable : struct, IOperatableNumber<TNumberOperatable>
        {
            private readonly int maxStep = 0;
            private int step;

            public Visitor(int maxStep)
            {
                this.maxStep = maxStep;
            }

            /* ******************** */

            public TNumberOperatable Visit(ZeroOperator op, (CompilationContext context, TNumberOperatable[]? arguments) param)
                => TNumberOperatable.Zero;

            public TNumberOperatable Visit(PreComputedOperator op, (CompilationContext context, TNumberOperatable[]? arguments) param)
                => Unsafe.As<TNumberOriginal, TNumberOperatable>(ref Unsafe.Unbox<TNumberOriginal>(op.Value));

            public TNumberOperatable Visit(ArgumentOperator op, (CompilationContext context, TNumberOperatable[]? arguments) param)
            {
                if (param.arguments is null)
                    throw new EvaluationArgumentNotSetException();
                return param.arguments[op.Index];
            }

            public TNumberOperatable Visit(DefineOperator op, (CompilationContext context, TNumberOperatable[]? arguments) param)
                => TNumberOperatable.Zero;

            public TNumberOperatable Visit(ParenthesisOperator op, (CompilationContext context, TNumberOperatable[]? arguments) param)
            {
                TNumberOperatable result = TNumberOperatable.Zero;
                ImmutableArray<IOperator> operators = op.Operators;

                for (int i = 0; i < operators.Length; i++)
                {
                    result = operators[i].Accept(this, param);
                }

                return result;
            }

            public TNumberOperatable Visit(DecimalOperator op, (CompilationContext context, TNumberOperatable[]? arguments) param)
            {
                TNumberOperatable operand = op.Operand.Accept(this, param);
                return operand * TNumberOperatable.Ten + TNumberOperatable.FromInt(op.Value);
            }

            public TNumberOperatable Visit(BinaryOperator op, (CompilationContext context, TNumberOperatable[]? arguments) param)
            {
                TNumberOperatable left = op.Left.Accept(this, param);
                TNumberOperatable right = op.Right.Accept(this, param);

                return op.Type switch
                {
                    BinaryType.Add => left + right,
                    BinaryType.Sub => left - right,
                    BinaryType.Mult => left * right,
                    BinaryType.Div => left / right,
                    BinaryType.Mod => left % right,
                    BinaryType.Equal => left == right ? TNumberOperatable.One : TNumberOperatable.Zero,
                    BinaryType.NotEqual => left != right ? TNumberOperatable.One : TNumberOperatable.Zero,
                    BinaryType.LessThan => left < right ? TNumberOperatable.One : TNumberOperatable.Zero,
                    BinaryType.LessThanOrEqual => left <= right ? TNumberOperatable.One : TNumberOperatable.Zero,
                    BinaryType.GreaterThanOrEqual => left <= right ? TNumberOperatable.One : TNumberOperatable.Zero,
                    BinaryType.GreaterThan => left < right ? TNumberOperatable.One : TNumberOperatable.Zero,
                    _ => throw new InvalidOperationException(),
                };
            }

            public TNumberOperatable Visit(ConditionalOperator op, (CompilationContext context, TNumberOperatable[]? arguments) param)
            {
                TNumberOperatable condition = op.Condition.Accept(this, param);
                return condition != TNumberOperatable.Zero ? op.IfTrue.Accept(this, param) : op.IfFalse.Accept(this, param);
            }

            public TNumberOperatable Visit(UserDefinedOperator op, (CompilationContext context, TNumberOperatable[]? arguments) param)
            {
                if (++step > maxStep)
                {
                    throw new EvaluationStepLimitExceedException();
                }

                TNumberOperatable[] stack = new TNumberOperatable[op.Operands.Length];

                for (int i = 0; i < op.Operands.Length; i++)
                {
                    stack[i] = op.Operands[i].Accept(this, param);
                }

                var userDefinedOperatorBody = param.context.LookupOperatorImplement(op.Definition.Name).Operator;
                Debug.Assert(userDefinedOperatorBody is not null);
                return userDefinedOperatorBody.Accept(this, (param.context, stack));
            }
        }
    }
}
