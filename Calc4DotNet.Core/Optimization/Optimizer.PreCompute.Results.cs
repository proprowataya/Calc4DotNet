using System.Collections.Immutable;
using System.Numerics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization;

public static partial class Optimizer
{
    /*
     * Partial evaluation results
     */

    private abstract record PartialEvaluationResult<TNumber>(IOperator Operator,
                                                             PreComputeState<TNumber> ExitState)
        where TNumber : INumber<TNumber>;

    private sealed record ConstantEvaluationResult<TNumber>(IOperator Operator,
                                                            PreComputeState<TNumber> ExitState,
                                                            TNumber ConstantValue)
        : PartialEvaluationResult<TNumber>(Operator, ExitState)
        where TNumber : INumber<TNumber>;

    private sealed record UnknownEvaluationResult<TNumber>(IOperator Operator,
                                                           PreComputeState<TNumber> ExitState)
        : PartialEvaluationResult<TNumber>(Operator, ExitState)
        where TNumber : INumber<TNumber>;

    /*
     * Specialization results
     */

    private sealed record SpecializationStateDelta<TNumber>(ImmutableDictionary<ValueBox<string>, TNumber> KnownVariables,
                                                            ImmutableHashSet<ValueBox<string>> UnknownVariables,
                                                            bool InvalidateAllArrayElements,
                                                            ImmutableDictionary<TNumber, TNumber> KnownArrayValues,
                                                            ImmutableHashSet<TNumber> UnknownArrayIndices)
        where TNumber : INumber<TNumber>;

    private abstract record SpecializationResult<TNumber>(IOperator Body,
                                                          SpecializationStateDelta<TNumber> ExitDelta)
        where TNumber : INumber<TNumber>;

    private sealed record ConstantSpecializationResult<TNumber>(IOperator Body,
                                                                SpecializationStateDelta<TNumber> ExitDelta,
                                                                TNumber ConstantValue)
        : SpecializationResult<TNumber>(Body, ExitDelta)
        where TNumber : INumber<TNumber>;

    private sealed record UnknownSpecializationResult<TNumber>(IOperator Body,
                                                               SpecializationStateDelta<TNumber> ExitDelta)
        : SpecializationResult<TNumber>(Body, ExitDelta)
        where TNumber : INumber<TNumber>;
}
