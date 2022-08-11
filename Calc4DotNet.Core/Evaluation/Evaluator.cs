using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Evaluation;

public sealed class EvaluationStepLimitExceedException : Exception
{ }

internal sealed class EvaluationArgumentNotSetException : Exception
{ }

public static class Evaluator
{
    public static TNumber Evaluate<TNumber>(IOperator op, CompilationContext compilationContext, IEvaluationState<TNumber> evaluationState, int maxStep = int.MaxValue)
        where TNumber : INumber<TNumber>
    {
        return op.Accept(new Visitor<TNumber>(compilationContext, evaluationState, maxStep), null);
    }

    private sealed class Visitor<TNumber>
        : IOperatorVisitor<TNumber, TNumber[]? /* arguments */>
        where TNumber : INumber<TNumber>
    {
        private readonly CompilationContext compilationContext;
        private readonly IEvaluationState<TNumber> evaluationState;
        private readonly int maxStep = 0;
        private int step;

        public Visitor(CompilationContext compilationContext, IEvaluationState<TNumber> evaluationState, int maxStep)
        {
            this.compilationContext = compilationContext;
            this.evaluationState = evaluationState;
            this.maxStep = maxStep;
        }

        /* ******************** */

        public TNumber Visit(ZeroOperator op, TNumber[]? arguments)
            => TNumber.Zero;

        public TNumber Visit(PreComputedOperator op, TNumber[]? arguments)
            => (TNumber)op.Value;

        public TNumber Visit(ArgumentOperator op, TNumber[]? arguments)
        {
            if (arguments is null)
                throw new EvaluationArgumentNotSetException();
            return arguments[op.Index];
        }

        public TNumber Visit(DefineOperator op, TNumber[]? arguments)
            => TNumber.Zero;

        public TNumber Visit(LoadVariableOperator op, TNumber[]? arguments)
            => evaluationState.Variables[op.VariableName];

        public TNumber Visit(LoadArrayOperator op, TNumber[]? arguments)
        {
            TNumber index = op.Index.Accept(this, arguments);
            return evaluationState.GlobalArray[index];
        }

        public TNumber Visit(PrintCharOperator op, TNumber[]? arguments)
        {
            TNumber character = op.Character.Accept(this, arguments);
            evaluationState.IOService.PrintChar((char)Int32.CreateTruncating(character));
            return TNumber.Zero;
        }

        public TNumber Visit(ParenthesisOperator op, TNumber[]? arguments)
        {
            TNumber result = TNumber.Zero;
            ImmutableArray<IOperator> operators = op.Operators;

            for (int i = 0; i < operators.Length; i++)
            {
                result = operators[i].Accept(this, arguments);
            }

            return result;
        }

        public TNumber Visit(DecimalOperator op, TNumber[]? arguments)
        {
            TNumber operand = op.Operand.Accept(this, arguments);
            return operand * TNumber.CreateTruncating(10) + TNumber.CreateTruncating(op.Value);
        }

        public TNumber Visit(StoreVariableOperator op, TNumber[]? arguments)
        {
            TNumber value = op.Operand.Accept(this, arguments);
            evaluationState.Variables[op.VariableName] = value;
            return value;
        }

        public TNumber Visit(StoreArrayOperator op, TNumber[]? arguments)
        {
            TNumber value = op.Value.Accept(this, arguments);
            TNumber index = op.Index.Accept(this, arguments);
            return evaluationState.GlobalArray[index] = value;
        }

        public TNumber Visit(BinaryOperator op, TNumber[]? arguments)
        {
            TNumber left = op.Left.Accept(this, arguments);
            TNumber right = op.Right.Accept(this, arguments);

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
                BinaryType.GreaterThanOrEqual => left >= right ? TNumber.One : TNumber.Zero,
                BinaryType.GreaterThan => left > right ? TNumber.One : TNumber.Zero,
                _ => throw new InvalidOperationException(),
            };
        }

        public TNumber Visit(ConditionalOperator op, TNumber[]? arguments)
        {
            TNumber condition = op.Condition.Accept(this, arguments);
            return !TNumber.IsZero(condition) ? op.IfTrue.Accept(this, arguments) : op.IfFalse.Accept(this, arguments);
        }

        public TNumber Visit(UserDefinedOperator op, TNumber[]? arguments)
        {
            if (++step > maxStep)
            {
                throw new EvaluationStepLimitExceedException();
            }

            TNumber[] stack = new TNumber[op.Operands.Length];

            for (int i = 0; i < op.Operands.Length; i++)
            {
                stack[i] = op.Operands[i].Accept(this, arguments);
            }

            var userDefinedOperatorBody = compilationContext.LookupOperatorImplement(op.Definition.Name).Operator;
            Debug.Assert(userDefinedOperatorBody is not null);
            return userDefinedOperatorBody.Accept(this, stack);
        }
    }
}
