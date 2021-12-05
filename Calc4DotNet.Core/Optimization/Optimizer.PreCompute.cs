using System.Collections.Immutable;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization;

public static partial class Optimizer
{
    private sealed class PreComputeVisitor<TNumber> : IOperatorVisitor<IOperator, OptimizeTimeEvaluationState<TNumber>>
        where TNumber : notnull
    {
        private readonly CompilationContext compilationContext;
        private readonly int maxStep;

        public PreComputeVisitor(CompilationContext context, int maxStep)
        {
            this.compilationContext = context ?? throw new ArgumentNullException(nameof(context));
            this.maxStep = maxStep;
        }

        private IOperator PreComputeIfPossible(IOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            try
            {
                return new PreComputedOperator(
                    Evaluator.Evaluate<TNumber>(op,
                                                compilationContext,
                                                new SimpleEvaluationState<TNumber>(state),
                                                maxStep));
            }
            catch (EvaluationStepLimitExceedException)
            {
                return op;
            }
            catch (VariableNotSetException)
            {
                return op;
            }
            catch (EvaluationArgumentNotSetException)
            {
                return op;
            }
        }

        public IOperator Visit(ZeroOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(PreComputedOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(ArgumentOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(DefineOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(LoadOperator op, OptimizeTimeEvaluationState<TNumber> state) => PreComputeIfPossible(op, state);

        public IOperator Visit(ParenthesisOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            ImmutableArray<IOperator> operators = op.Operators;
            var optimized = new List<IOperator>();

            // Optimize all operators in the ParenthesisOperator
            for (int i = 0; i < operators.Length; i++)
            {
                optimized.Add(operators[i].Accept(this, state));
            }

            // Eliminate unnecessary PreComputed operators
            var builder = ImmutableArray.CreateBuilder<IOperator>(operators.Length);

            for (int i = 0; i < optimized.Count - 1; i++)
            {
                if (optimized[i] is not PreComputedOperator)
                {
                    builder.Add(optimized[i]);
                }
            }

            // The last one is always necessary
            builder.Add(optimized[^1]);

            if (builder.Count == 1)
            {
                return builder[0];
            }
            else
            {
                return op with { Operators = builder.ToImmutable() };
            }
        }

        public IOperator Visit(DecimalOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var operand = op.Operand.Accept(this, state);
            var newOp = op with { Operand = operand };
            return PreComputeIfPossible(newOp, state);
        }

        public IOperator Visit(StoreOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var operand = op.Operand.Accept(this, state);
            var newOp = op with { Operand = operand };

            if (operand is PreComputedOperator preComputed)
            {
                // Tell the pre-computed value to another operator
                state[op.VariableName] = (TNumber)preComputed.Value;
            }
            else
            {
                // We failed to pre-compute the operand of this StoreOperator.
                // Therefore, the variable that this operator indicates is unknown.
                // We unset the variable so as not for another operator to load it.
                state.UnsetVariable(op.VariableName);
            }

            // Do NOT make PreComputedOperator for StoreOperator because it mistakenly eliminates store operation
            return newOp;
        }

        public IOperator Visit(BinaryOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var left = op.Left.Accept(this, state);
            var right = op.Right.Accept(this, state);
            var newOp = op with { Left = left, Right = right };
            return PreComputeIfPossible(newOp, state);
        }

        public IOperator Visit(ConditionalOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var condition = op.Condition.Accept(this, state);

            // From now on, we use cloned object of the evaluation state
            // because IfTrue and IfFalse will not always be executed.

            // Process IfTrue
            var ifTrueState = state.Clone();
            var ifTrue = op.IfTrue.Accept(this, ifTrueState);

            // Process IfFalse
            var ifFalseState = state.Clone();
            var ifFalse = op.IfFalse.Accept(this, ifFalseState);

            // We must unset variables that have different values after execution of ifTrue and ifFalse.
            // We calculate intersection of ifTrueState and ifFalseState
            var intersection = OptimizeTimeEvaluationState<TNumber>.Intersect(ifTrueState, ifFalseState);
            state.Assign(intersection);

            // Create new operator
            var newOp = op with { Condition = condition, IfTrue = ifTrue, IfFalse = ifFalse };

            if (PreComputeIfPossible(condition, state) is PreComputedOperator preComputed)
            {
                // TODO: More wise determination method of whether the value is zero or not
                return (dynamic)preComputed.Value != 0 ? ifTrue : ifFalse;
            }
            else
            {
                return newOp;
            }
        }

        public IOperator Visit(UserDefinedOperator op, OptimizeTimeEvaluationState<TNumber> state)
        {
            var operands = op.Operands.Select(x => x.Accept(this, state)).ToImmutableArray();
            var newOp = op with { Operands = operands };
            return PreComputeIfPossible(newOp, state);
        }
    }
}
