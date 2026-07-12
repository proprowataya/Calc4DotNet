using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Evaluation;

public sealed class EvaluationStepLimitExceedException : Exception
{ }

internal sealed class EvaluationArgumentNotSetException : Exception
{ }

internal sealed class EvaluationVariableNotSetException : Exception
{ }

public static class Evaluator
{
    public static TNumber Evaluate<TNumber>(IOperator op, CompilationContext compilationContext, IEvaluationState<TNumber> evaluationState, int maxStep = int.MaxValue)
        where TNumber : INumber<TNumber>
    {
        var result = op.Accept(new Visitor<TNumber>(compilationContext, evaluationState, maxStep), null);
        return result.Succeeded ? result.Value : throw CreateException(result.Failure);
    }

    public static bool TryEvaluate<TNumber>(IOperator op,
                                            CompilationContext compilationContext,
                                            IEvaluationState<TNumber> evaluationState,
                                            int maxStep,
                                            out TNumber value)
        where TNumber : INumber<TNumber>
    {
        var result = op.Accept(new Visitor<TNumber>(compilationContext, evaluationState, maxStep), null);
        if (!result.Succeeded)
        {
            value = TNumber.Zero;
            return false;
        }

        value = result.Value;
        return true;
    }

    private enum EvaluationFailure
    {
        ArgumentNotSet,
        UnknownVariable,
        ArrayElementNotSet,
        InputNotSupported,
        StepLimitExceeded,
        ZeroDivision,
    }

    private readonly record struct EvaluationResult<TNumber>(bool Succeeded, TNumber Value, EvaluationFailure Failure)
        where TNumber : INumber<TNumber>
    {
        public static EvaluationResult<TNumber> Success(TNumber value) => new(true, value, default);
        public static EvaluationResult<TNumber> Fail(EvaluationFailure failure) => new(false, TNumber.Zero, failure);
        public static implicit operator EvaluationResult<TNumber>(TNumber value) => Success(value);
    }

    private static Exception CreateException(EvaluationFailure failure)
    {
        return failure switch
        {
            EvaluationFailure.ArgumentNotSet => new EvaluationArgumentNotSetException(),
            EvaluationFailure.UnknownVariable => new EvaluationVariableNotSetException(),
            EvaluationFailure.ArrayElementNotSet => new ArrayElementNotSetException(),
            EvaluationFailure.InputNotSupported => new InputIsNotSupportedException(),
            EvaluationFailure.StepLimitExceeded => new EvaluationStepLimitExceedException(),
            EvaluationFailure.ZeroDivision => new Calc4DotNet.Core.Exceptions.ZeroDivisionException(),
            _ => new InvalidOperationException(),
        };
    }

    private sealed class Visitor<TNumber>
        : IOperatorVisitor<EvaluationResult<TNumber>, TNumber[]? /* arguments */>
        where TNumber : INumber<TNumber>
    {
        private readonly CompilationContext compilationContext;
        private readonly IEvaluationState<TNumber> evaluationState;
        private readonly int maxStep = 0;
        private readonly Stack<Dictionary<int, TNumber>> letFrames = [];
        private int step;

        public Visitor(CompilationContext compilationContext,
                       IEvaluationState<TNumber> evaluationState,
                       int maxStep)
        {
            this.compilationContext = compilationContext;
            this.evaluationState = evaluationState;
            this.maxStep = maxStep;
            letFrames.Push([]);
        }

        /* ******************** */

        public EvaluationResult<TNumber> Visit(ZeroOperator op, TNumber[]? arguments)
            => TNumber.Zero;

        public EvaluationResult<TNumber> Visit(PreComputedOperator op, TNumber[]? arguments)
            => (TNumber)op.Value;

        public EvaluationResult<TNumber> Visit(ArgumentOperator op, TNumber[]? arguments)
        {
            if (arguments is null)
            {
                return Fail(EvaluationFailure.ArgumentNotSet);
            }

            if (op.Index < 0 || op.Index >= arguments.Length)
            {
                return Fail(EvaluationFailure.ArgumentNotSet);
            }

            return arguments[op.Index];
        }

        public EvaluationResult<TNumber> Visit(LetVariableOperator op, TNumber[]? arguments)
        {
            if (letFrames.Peek().TryGetValue(op.LocalIndex, out var value))
            {
                return value;
            }

            return Fail(EvaluationFailure.ArgumentNotSet);
        }

        public EvaluationResult<TNumber> Visit(DefineOperator op, TNumber[]? arguments)
            => TNumber.Zero;

        public EvaluationResult<TNumber> Visit(LoadVariableOperator op, TNumber[]? arguments)
        {
            return evaluationState.Variables.TryGet(op.VariableName, out var value)
                ? value
                : Fail(EvaluationFailure.UnknownVariable);
        }

        public EvaluationResult<TNumber> Visit(InputOperator op, TNumber[]? param)
        {
            return evaluationState.IOService.TryGetChar(out int c)
                ? TNumber.CreateTruncating(c)
                : Fail(EvaluationFailure.InputNotSupported);
        }

        public EvaluationResult<TNumber> Visit(LoadArrayOperator op, TNumber[]? arguments)
        {
            var indexResult = EvaluateChild(op.Index, arguments);
            if (!indexResult.Succeeded)
            {
                return indexResult;
            }

            return evaluationState.GlobalArray.TryGet(indexResult.Value, out var value)
                ? value
                : Fail(EvaluationFailure.ArrayElementNotSet);
        }

        public EvaluationResult<TNumber> Visit(PrintCharOperator op, TNumber[]? arguments)
        {
            var characterResult = EvaluateChild(op.Character, arguments);
            if (!characterResult.Succeeded)
            {
                return characterResult;
            }

            evaluationState.IOService.PrintChar((char)Int32.CreateTruncating(characterResult.Value));
            return TNumber.Zero;
        }

        public EvaluationResult<TNumber> Visit(ParenthesisOperator op, TNumber[]? arguments)
        {
            EvaluationResult<TNumber> result = TNumber.Zero;
            ImmutableArray<IOperator> operators = op.Operators;

            for (int i = 0; i < operators.Length; i++)
            {
                result = operators[i].Accept(this, arguments);
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            return result;
        }

        public EvaluationResult<TNumber> Visit(DecimalOperator op, TNumber[]? arguments)
        {
            var operandResult = EvaluateChild(op.Operand, arguments);
            if (!operandResult.Succeeded)
            {
                return operandResult;
            }

            return operandResult.Value * TNumber.CreateTruncating(10) + TNumber.CreateTruncating(op.Value);
        }

        public EvaluationResult<TNumber> Visit(StoreVariableOperator op, TNumber[]? arguments)
        {
            var valueResult = EvaluateChild(op.Operand, arguments);
            if (!valueResult.Succeeded)
            {
                return valueResult;
            }

            var value = valueResult.Value;
            evaluationState.Variables[op.VariableName] = value;
            return value;
        }

        public EvaluationResult<TNumber> Visit(StoreArrayOperator op, TNumber[]? arguments)
        {
            var valueResult = EvaluateChild(op.Value, arguments);
            if (!valueResult.Succeeded)
            {
                return valueResult;
            }

            var indexResult = EvaluateChild(op.Index, arguments);
            if (!indexResult.Succeeded)
            {
                return indexResult;
            }

            return evaluationState.GlobalArray.TrySet(indexResult.Value, valueResult.Value)
                ? valueResult.Value
                : Fail(EvaluationFailure.ArrayElementNotSet);
        }

        public EvaluationResult<TNumber> Visit(BinaryOperator op, TNumber[]? arguments)
        {
            if (op.Type is BinaryType.LogicalAnd)
            {
                var leftResult = EvaluateChild(op.Left, arguments);
                if (!leftResult.Succeeded)
                {
                    return leftResult;
                }

                var left = leftResult.Value;
                if (TNumber.IsZero(left))
                {
                    return TNumber.Zero;
                }

                var rightResult = EvaluateChild(op.Right, arguments);
                if (!rightResult.Succeeded)
                {
                    return rightResult;
                }

                var right = rightResult.Value;
                return !TNumber.IsZero(right) ? TNumber.One : TNumber.Zero;
            }
            else if (op.Type is BinaryType.LogicalOr)
            {
                var leftResult = EvaluateChild(op.Left, arguments);
                if (!leftResult.Succeeded)
                {
                    return leftResult;
                }

                var left = leftResult.Value;
                if (!TNumber.IsZero(left))
                {
                    return TNumber.One;
                }

                var rightResult = EvaluateChild(op.Right, arguments);
                if (!rightResult.Succeeded)
                {
                    return rightResult;
                }

                var right = rightResult.Value;
                return !TNumber.IsZero(right) ? TNumber.One : TNumber.Zero;
            }
            else
            {
                var leftResult = EvaluateChild(op.Left, arguments);
                if (!leftResult.Succeeded)
                {
                    return leftResult;
                }

                var rightResult = EvaluateChild(op.Right, arguments);
                if (!rightResult.Succeeded)
                {
                    return rightResult;
                }

                var left = leftResult.Value;
                var right = rightResult.Value;
                return op.Type switch
                {
                    BinaryType.Add => left + right,
                    BinaryType.Sub => left - right,
                    BinaryType.Mult => left * right,
                    BinaryType.Div => right == TNumber.Zero ? Fail(EvaluationFailure.ZeroDivision) : left / right,
                    BinaryType.Mod => right == TNumber.Zero ? Fail(EvaluationFailure.ZeroDivision) : left % right,
                    BinaryType.Equal => left == right ? TNumber.One : TNumber.Zero,
                    BinaryType.NotEqual => left != right ? TNumber.One : TNumber.Zero,
                    BinaryType.LessThan => left < right ? TNumber.One : TNumber.Zero,
                    BinaryType.LessThanOrEqual => left <= right ? TNumber.One : TNumber.Zero,
                    BinaryType.GreaterThanOrEqual => left >= right ? TNumber.One : TNumber.Zero,
                    BinaryType.GreaterThan => left > right ? TNumber.One : TNumber.Zero,
                    BinaryType.LogicalAnd => throw new InvalidOperationException(), // Treated above
                    BinaryType.LogicalOr => throw new InvalidOperationException(),  // Treated above
                    _ => throw new InvalidOperationException(),
                };
            }
        }

        public EvaluationResult<TNumber> Visit(ConditionalOperator op, TNumber[]? arguments)
        {
            var conditionResult = EvaluateChild(op.Condition, arguments);
            if (!conditionResult.Succeeded)
            {
                return conditionResult;
            }

            return !TNumber.IsZero(conditionResult.Value) ? op.IfTrue.Accept(this, arguments) : op.IfFalse.Accept(this, arguments);
        }

        public EvaluationResult<TNumber> Visit(LetOperator op, TNumber[]? arguments)
        {
            var valueResult = EvaluateChild(op.Value, arguments);
            if (!valueResult.Succeeded)
            {
                return valueResult;
            }

            Dictionary<int, TNumber> frame = letFrames.Peek();
            bool hadPrevious = frame.TryGetValue(op.LocalIndex, out var previous);
            frame[op.LocalIndex] = valueResult.Value;

            try
            {
                return op.Body.Accept(this, arguments);
            }
            finally
            {
                if (hadPrevious)
                {
                    Debug.Assert(previous is not null);
                    frame[op.LocalIndex] = previous;
                }
                else
                {
                    frame.Remove(op.LocalIndex);
                }
            }
        }

        public EvaluationResult<TNumber> Visit(UserDefinedOperator op, TNumber[]? arguments)
        {
            if (++step > maxStep)
            {
                return Fail(EvaluationFailure.StepLimitExceeded);
            }

            TNumber[] stack = new TNumber[op.Operands.Length];

            for (int i = 0; i < op.Operands.Length; i++)
            {
                var operandResult = EvaluateChild(op.Operands[i], arguments);
                if (!operandResult.Succeeded)
                {
                    return operandResult;
                }

                stack[i] = operandResult.Value;
            }

            var userDefinedOperatorBody = compilationContext.LookupOperatorImplement(op.Definition.Name).Operator;
            Debug.Assert(userDefinedOperatorBody is not null);
            letFrames.Push([]);
            try
            {
                return userDefinedOperatorBody.Accept(this, stack);
            }
            finally
            {
                letFrames.Pop();
            }
        }

        private EvaluationResult<TNumber> EvaluateChild(IOperator op, TNumber[]? arguments) => op.Accept(this, arguments);

        private static EvaluationResult<TNumber> Fail(EvaluationFailure failure) => EvaluationResult<TNumber>.Fail(failure);
    }
}
