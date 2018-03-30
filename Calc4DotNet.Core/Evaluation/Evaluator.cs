using System;
using System.Collections.Immutable;
using System.Numerics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Evaluation
{
    public static class Evaluator
    {
        internal static TNumber EvaluateGeneric<TNumber>(IOperator<TNumber> op, CompilationContext<TNumber> context)
        {
            return Evaluate((dynamic)op, (dynamic)context);
        }

        public static Int32 Evaluate(IOperator<Int32> op, CompilationContext<Int32> context)
        {
            return op.Accept(new Visitor<Int32, Int32Computer>(), (context, null));
        }

        public static Int64 Evaluate(IOperator<Int64> op, CompilationContext<Int64> context)
        {
            return op.Accept(new Visitor<Int64, Int64Computer>(), (context, null));
        }

        public static Double Evaluate(IOperator<Double> op, CompilationContext<Double> context)
        {
            return op.Accept(new Visitor<Double, DoubleComputer>(), (context, null));
        }

        public static BigInteger Evaluate(IOperator<BigInteger> op, CompilationContext<BigInteger> context)
        {
            return op.Accept(new Visitor<BigInteger, BigIntegerComputer>(), (context, null));
        }

        private static TNumber EvaluateCore<TNumber, TNumberComputer>(IOperator<TNumber> op, CompilationContext<TNumber> context)
            where TNumberComputer : INumberComputer<TNumber>
        {
            return op.Accept(new Visitor<TNumber, TNumberComputer>(), (context, null));
        }

        private sealed class Visitor<TNumber, TNumberComputer>
            : IOperatorVisitor<TNumber, TNumber, (CompilationContext<TNumber> context, TNumber[] arguments)>
            where TNumberComputer : INumberComputer<TNumber>
        {
            public TNumber Visit(ZeroOperator<TNumber> op, (CompilationContext<TNumber> context, TNumber[] arguments) param)
                => default(TNumberComputer).Zero;

            public TNumber Visit(PreComputedOperator<TNumber> op, (CompilationContext<TNumber> context, TNumber[] arguments) param)
                => op.Value;

            public TNumber Visit(ArgumentOperator<TNumber> op, (CompilationContext<TNumber> context, TNumber[] arguments) param)
                => param.arguments[op.Index];

            public TNumber Visit(DefineOperator<TNumber> op, (CompilationContext<TNumber> context, TNumber[] arguments) param)
                => default(TNumberComputer).Zero;

            public TNumber Visit(ParenthesisOperator<TNumber> op, (CompilationContext<TNumber> context, TNumber[] arguments) param)
            {
                TNumber result = default(TNumberComputer).Zero;
                ImmutableArray<IOperator<TNumber>> operators = op.Operators;

                for (int i = 0; i < operators.Length; i++)
                {
                    result = operators[i].Accept(this, param);
                }

                return result;
            }

            public TNumber Visit(DecimalOperator<TNumber> op, (CompilationContext<TNumber> context, TNumber[] arguments) param)
            {
                var c = default(TNumberComputer);
                TNumber operand = op.Operand.Accept(this, param);
                return c.Add(c.Multiply(operand, c.Ten), c.FromInt(op.Value));
            }

            public TNumber Visit(BinaryOperator<TNumber> op, (CompilationContext<TNumber> context, TNumber[] arguments) param)
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

            public TNumber Visit(ConditionalOperator<TNumber> op, (CompilationContext<TNumber> context, TNumber[] arguments) param)
            {
                var c = default(TNumberComputer);
                TNumber condition = op.Condition.Accept(this, param);
                return c.NotEquals(condition, c.Zero) ? op.IfTrue.Accept(this, param) : op.IfFalse.Accept(this, param);
            }

            public TNumber Visit(UserDefinedOperator<TNumber> op, (CompilationContext<TNumber> context, TNumber[] arguments) param)
            {
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
