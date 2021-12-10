using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Evaluation;

public sealed class EvaluationStepLimitExceedException : Exception
{ }

internal sealed class EvaluationArgumentNotSetException : Exception
{ }

public static class Evaluator
{
    public static TNumber Evaluate<TNumber>(IOperator op, CompilationContext compilationContext, IEvaluationState<TNumber> evaluationState, int maxStep = int.MaxValue)
        where TNumber : notnull
    {
        if (typeof(TNumber) == typeof(Int32))
        {
            var result =
                op.Accept(new Visitor<Int32, Int32Computer>(compilationContext,
                                                            (IEvaluationState<Int32>)(object)evaluationState,
                                                            maxStep), null);
            return Unsafe.As<Int32, TNumber>(ref result);
        }
        else if (typeof(TNumber) == typeof(Int64))
        {
            var result =
                op.Accept(new Visitor<Int64, Int64Computer>(compilationContext,
                                                            (IEvaluationState<Int64>)(object)evaluationState,
                                                            maxStep), null);
            return Unsafe.As<Int64, TNumber>(ref result);
        }
        else if (typeof(TNumber) == typeof(Double))
        {
            var result =
                op.Accept(new Visitor<Double, DoubleComputer>(compilationContext,
                                                              (IEvaluationState<Double>)(object)evaluationState,
                                                              maxStep), null);
            return Unsafe.As<Double, TNumber>(ref result);
        }
        else if (typeof(TNumber) == typeof(BigInteger))
        {
            var result =
                op.Accept(new Visitor<BigInteger, BigIntegerComputer>(compilationContext,
                                                                      (IEvaluationState<BigInteger>)(object)evaluationState,
                                                                      maxStep), null);
            return Unsafe.As<BigInteger, TNumber>(ref result);
        }
        else
        {
            throw new Calc4DotNet.Core.Exceptions.TypeNotSupportedException(typeof(TNumber));
        }
    }

    private sealed class Visitor<TNumber, TNumberComputer>
        : IOperatorVisitor<TNumber, TNumber[]? /* arguments */>
        where TNumber : notnull
        where TNumberComputer : struct, INumberComputer<TNumber>
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
            => default(TNumberComputer).Zero;

        public TNumber Visit(PreComputedOperator op, TNumber[]? arguments)
            => (TNumber)op.Value;

        public TNumber Visit(ArgumentOperator op, TNumber[]? arguments)
        {
            if (arguments is null)
                throw new EvaluationArgumentNotSetException();
            return arguments[op.Index];
        }

        public TNumber Visit(DefineOperator op, TNumber[]? arguments)
            => default(TNumberComputer).Zero;

        public TNumber Visit(LoadVariableOperator op, TNumber[]? arguments)
            => evaluationState.Variables[op.VariableName];

        public TNumber Visit(LoadArrayOperator op, TNumber[]? arguments)
        {
            TNumberComputer c = default;
            TNumber index = op.Index.Accept(this, arguments);
            return evaluationState.GlobalArray[c.ToInt(index)];
        }

        public TNumber Visit(PrintCharOperator op, TNumber[]? arguments)
        {
            TNumberComputer c = default;
            TNumber character = op.Character.Accept(this, arguments);
            evaluationState.IOService.PrintChar((char)c.ToInt(character));
            return c.Zero;
        }

        public TNumber Visit(ParenthesisOperator op, TNumber[]? arguments)
        {
            TNumber result = default(TNumberComputer).Zero;
            ImmutableArray<IOperator> operators = op.Operators;

            for (int i = 0; i < operators.Length; i++)
            {
                result = operators[i].Accept(this, arguments);
            }

            return result;
        }

        public TNumber Visit(DecimalOperator op, TNumber[]? arguments)
        {
            var c = default(TNumberComputer);
            TNumber operand = op.Operand.Accept(this, arguments);
            return c.Add(c.Multiply(operand, c.Ten), c.FromInt(op.Value));
        }

        public TNumber Visit(StoreVariableOperator op, TNumber[]? arguments)
        {
            TNumber value = op.Operand.Accept(this, arguments);
            evaluationState.Variables[op.VariableName] = value;
            return value;
        }

        public TNumber Visit(StoreArrayOperator op, TNumber[]? arguments)
        {
            TNumberComputer c = default;
            TNumber value = op.Value.Accept(this, arguments);
            TNumber index = op.Index.Accept(this, arguments);
            return evaluationState.GlobalArray[c.ToInt(index)] = value;
        }

        public TNumber Visit(BinaryOperator op, TNumber[]? arguments)
        {
            var c = default(TNumberComputer);
            TNumber left = op.Left.Accept(this, arguments);
            TNumber right = op.Right.Accept(this, arguments);

            return op.Type switch
            {
                BinaryType.Add => c.Add(left, right),
                BinaryType.Sub => c.Subtract(left, right),
                BinaryType.Mult => c.Multiply(left, right),
                BinaryType.Div => c.Divide(left, right),
                BinaryType.Mod => c.Modulo(left, right),
                BinaryType.Equal => c.Equals(left, right) ? c.One : c.Zero,
                BinaryType.NotEqual => c.NotEquals(left, right) ? c.One : c.Zero,
                BinaryType.LessThan => c.LessThan(left, right) ? c.One : c.Zero,
                BinaryType.LessThanOrEqual => c.LessThanOrEquals(left, right) ? c.One : c.Zero,
                BinaryType.GreaterThanOrEqual => c.GreaterThanOrEquals(left, right) ? c.One : c.Zero,
                BinaryType.GreaterThan => c.GreaterThan(left, right) ? c.One : c.Zero,
                _ => throw new InvalidOperationException(),
            };
        }

        public TNumber Visit(ConditionalOperator op, TNumber[]? arguments)
        {
            var c = default(TNumberComputer);
            TNumber condition = op.Condition.Accept(this, arguments);
            return c.NotEquals(condition, c.Zero) ? op.IfTrue.Accept(this, arguments) : op.IfFalse.Accept(this, arguments);
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
