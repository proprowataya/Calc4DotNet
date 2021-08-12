using System.Collections.Immutable;
using System.Diagnostics;
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
            where TNumber : INumber<TNumber>
        {
            return op.Accept(new Visitor<TNumber>(maxStep), (context, null));
        }

        private sealed class Visitor<TNumber>
            : IOperatorVisitor<TNumber, (CompilationContext context, TNumber[]? arguments)>
            where TNumber : INumber<TNumber>
        {
            private readonly int maxStep = 0;
            private int step;

            public Visitor(int maxStep)
            {
                this.maxStep = maxStep;
            }

            /* ******************** */

            public TNumber Visit(ZeroOperator op, (CompilationContext context, TNumber[]? arguments) param)
                => TNumber.Zero;

            public TNumber Visit(PreComputedOperator op, (CompilationContext context, TNumber[]? arguments) param)
                => (TNumber)op.Value;

            public TNumber Visit(ArgumentOperator op, (CompilationContext context, TNumber[]? arguments) param)
            {
                if (param.arguments is null)
                    throw new EvaluationArgumentNotSetException();
                return param.arguments[op.Index];
            }

            public TNumber Visit(DefineOperator op, (CompilationContext context, TNumber[]? arguments) param)
                => TNumber.Zero;

            public TNumber Visit(ParenthesisOperator op, (CompilationContext context, TNumber[]? arguments) param)
            {
                TNumber result = TNumber.Zero;
                ImmutableArray<IOperator> operators = op.Operators;

                for (int i = 0; i < operators.Length; i++)
                {
                    result = operators[i].Accept(this, param);
                }

                return result;
            }

            public TNumber Visit(DecimalOperator op, (CompilationContext context, TNumber[]? arguments) param)
            {
                TNumber operand = op.Operand.Accept(this, param);
                return operand * TNumber.CreateTruncating(10) + TNumber.CreateTruncating(op.Value);
            }

            public TNumber Visit(BinaryOperator op, (CompilationContext context, TNumber[]? arguments) param)
            {
                TNumber left = op.Left.Accept(this, param);
                TNumber right = op.Right.Accept(this, param);

                return op.Type switch
                {
                    BinaryType.Add => left + right,
                    BinaryType.Sub => left - right,
                    BinaryType.Mult => left * right,
                    BinaryType.Div => left / right,
                    BinaryType.Mod => left % right,
                    BinaryType.Equal => left == right ? TNumber.One : TNumber.Zero,
                    BinaryType.NotEqual => left != right ? TNumber.One : TNumber.Zero,
                    BinaryType.LessThan => left < right ? TNumber.One : TNumber.Zero,
                    BinaryType.LessThanOrEqual => left <= right ? TNumber.One : TNumber.Zero,
                    BinaryType.GreaterThanOrEqual => left <= right ? TNumber.One : TNumber.Zero,
                    BinaryType.GreaterThan => left < right ? TNumber.One : TNumber.Zero,
                    _ => throw new InvalidOperationException(),
                };
            }

            public TNumber Visit(ConditionalOperator op, (CompilationContext context, TNumber[]? arguments) param)
            {
                TNumber condition = op.Condition.Accept(this, param);
                return condition != TNumber.Zero ? op.IfTrue.Accept(this, param) : op.IfFalse.Accept(this, param);
            }

            public TNumber Visit(UserDefinedOperator op, (CompilationContext context, TNumber[]? arguments) param)
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

                var userDefinedOperatorBody = param.context.LookupOperatorImplement(op.Definition.Name).Operator;
                Debug.Assert(userDefinedOperatorBody is not null);
                return userDefinedOperatorBody.Accept(this, (param.context, stack));
            }
        }
    }
}
