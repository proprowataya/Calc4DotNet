using System.Collections.Immutable;
using System.Numerics;

namespace Calc4DotNet.Core.Optimization;

public static partial class Optimizer
{
    private sealed class PreComputeState<TNumber>
        where TNumber : INumber<TNumber>
    {
        private readonly ImmutableDictionary<ValueBox<string>, TNumber> variables;
        private readonly ImmutableDictionary<TNumber, TNumber> knownArrayValues;
        private readonly ImmutableHashSet<TNumber> unknownArrayIndices;
        private readonly ImmutableDictionary<int, TNumber> arguments;

        // When true, any array index not explicitly tracked is known to hold zero.
        // - Main operator optimization starts with true only when the caller supplies
        //   an initial global array snapshot.
        // - Without such a snapshot, array contents are unknown.
        // - User-defined operator optimization starts with false because the caller's
        //   array state is inherited and cannot be assumed.
        // Once any unknown write invalidates the array (InvalidateAllArrayElements), this
        // flag drops to false and cannot recover.
        private readonly bool arraysZeroInitialized;

        // True when this state was reached through an unknown-index array write or another
        // operation whose array writes cannot be localized to known constant indices. This
        // does not affect reads directly. It lets specialization results describe the caller
        // state transformation as "invalidate all, then apply the tracked writes that remain
        // known afterwards".
        private readonly bool arrayElementsInvalidated;

        public static PreComputeState<TNumber> Create(bool arraysZeroInitialized)
        {
            return new([], [], [], [], arraysZeroInitialized, arrayElementsInvalidated: false);
        }

        private PreComputeState(ImmutableDictionary<ValueBox<string>, TNumber> variables,
                                ImmutableDictionary<TNumber, TNumber> knownArrayValues,
                                ImmutableHashSet<TNumber> unknownArrayIndices,
                                ImmutableDictionary<int, TNumber> arguments,
                                bool arraysZeroInitialized,
                                bool arrayElementsInvalidated)
        {
            this.variables = variables;
            this.knownArrayValues = knownArrayValues;
            this.unknownArrayIndices = unknownArrayIndices;
            this.arguments = arguments;
            this.arraysZeroInitialized = arraysZeroInitialized;
            this.arrayElementsInvalidated = arrayElementsInvalidated;
        }

        public bool HasInvalidatedArrayElements => arrayElementsInvalidated;

        public bool TryGetVariable(string? variableName, out TNumber value)
        {
            if (variables.TryGetValue(ValueBox.Create(variableName), out var foundValue))
            {
                value = foundValue;
                return true;
            }

            value = TNumber.Zero;
            return false;
        }

        public PreComputeState<TNumber> SetVariable(string? variableName, TNumber value)
        {
            return new PreComputeState<TNumber>(variables.SetItem(ValueBox.Create(variableName), value),
                                                knownArrayValues,
                                                unknownArrayIndices,
                                                arguments,
                                                arraysZeroInitialized,
                                                arrayElementsInvalidated);
        }

        public PreComputeState<TNumber> UnsetVariable(string? variableName)
        {
            return new PreComputeState<TNumber>(variables.Remove(ValueBox.Create(variableName)),
                                                knownArrayValues,
                                                unknownArrayIndices,
                                                arguments,
                                                arraysZeroInitialized,
                                                arrayElementsInvalidated);
        }

        public bool TryGetArrayValue(TNumber index, out TNumber value)
        {
            if (knownArrayValues.TryGetValue(index, out var knownValue))
            {
                value = knownValue;
                return true;
            }

            if (unknownArrayIndices.Contains(index) || !arraysZeroInitialized)
            {
                value = TNumber.Zero;
                return false;
            }

            value = TNumber.Zero;
            return true;
        }

        public IEnumerable<KeyValuePair<TNumber, TNumber>> EnumerateKnownArrayValues()
        {
            return knownArrayValues.OrderBy(p => p.Key);
        }

        public IEnumerable<TNumber> EnumerateUnknownArrayIndices()
        {
            return unknownArrayIndices.Order();
        }

        public PreComputeState<TNumber> SetArrayValue(TNumber index, TNumber value)
        {
            ImmutableDictionary<TNumber, TNumber> nextKnownArrayValues;
            if (arraysZeroInitialized && value == TNumber.Zero)
            {
                nextKnownArrayValues = knownArrayValues.Remove(index);
            }
            else
            {
                nextKnownArrayValues = knownArrayValues.SetItem(index, value);
            }

            return new PreComputeState<TNumber>(variables,
                                                nextKnownArrayValues,
                                                unknownArrayIndices.Remove(index),
                                                arguments,
                                                arraysZeroInitialized,
                                                arrayElementsInvalidated);
        }

        public PreComputeState<TNumber> UnsetArrayValue(TNumber index)
        {
            return new PreComputeState<TNumber>(variables,
                                                knownArrayValues.Remove(index),
                                                unknownArrayIndices.Add(index),
                                                arguments,
                                                arraysZeroInitialized,
                                                arrayElementsInvalidated);
        }

        public PreComputeState<TNumber> InvalidateAllArrayElements()
        {
            return new PreComputeState<TNumber>(variables,
                                                [],
                                                [],
                                                arguments,
                                                arraysZeroInitialized: false,
                                                arrayElementsInvalidated: true);
        }

        public bool TryGetArgument(int index, out TNumber value)
        {
            if (arguments.TryGetValue(index, out var argumentValue))
            {
                value = argumentValue;
                return true;
            }

            value = TNumber.Zero;
            return false;
        }

        public PreComputeState<TNumber> WithArguments(ImmutableDictionary<int, TNumber> arguments)
        {
            return new PreComputeState<TNumber>(variables,
                                                knownArrayValues,
                                                unknownArrayIndices,
                                                arguments,
                                                arraysZeroInitialized,
                                                arrayElementsInvalidated);
        }

        public PreComputeState<TNumber> Apply(PotentialEffects effects)
        {
            var state = this;

            foreach (var variableName in effects.WrittenVariables)
            {
                state = state.UnsetVariable(variableName);
            }

            if (effects.MayWriteArray)
            {
                state = state.InvalidateAllArrayElements();
            }

            return state;
        }

        public PreComputeState<TNumber> Apply(SpecializationStateDelta<TNumber> delta)
        {
            var state = this;

            foreach (var variableName in delta.UnknownVariables)
            {
                state = state.UnsetVariable(variableName.Value);
            }

            foreach (var (variableName, value) in delta.KnownVariables)
            {
                state = state.SetVariable(variableName.Value, value);
            }

            if (delta.InvalidateAllArrayElements)
            {
                state = state.InvalidateAllArrayElements();
            }

            foreach (var index in delta.UnknownArrayIndices)
            {
                state = state.UnsetArrayValue(index);
            }

            foreach (var (index, value) in delta.KnownArrayValues)
            {
                state = state.SetArrayValue(index, value);
            }

            return state;
        }

        public static PreComputeState<TNumber> Merge(PreComputeState<TNumber> left, PreComputeState<TNumber> right)
        {
            var mergedVariables = ImmutableDictionary.CreateBuilder<ValueBox<string>, TNumber>();

            foreach (var (key, value) in left.variables)
            {
                if (right.variables.TryGetValue(key, out var rightValue) && value == rightValue)
                {
                    mergedVariables[key] = value;
                }
            }

            var mergedKnownArrayValues = ImmutableDictionary.CreateBuilder<TNumber, TNumber>();
            var mergedUnknownArrayIndices = ImmutableHashSet.CreateBuilder<TNumber>();

            foreach (var index in EnumerateTrackedArrayIndices(left, right))
            {
                bool leftKnown = left.TryGetArrayValue(index, out var leftValue);
                bool rightKnown = right.TryGetArrayValue(index, out var rightValue);

                if (leftKnown && rightKnown && leftValue == rightValue)
                {
                    mergedKnownArrayValues[index] = leftValue;
                }
                else
                {
                    mergedUnknownArrayIndices.Add(index);
                }
            }

            return new PreComputeState<TNumber>(mergedVariables.ToImmutable(),
                                                mergedKnownArrayValues.ToImmutable(),
                                                mergedUnknownArrayIndices.ToImmutable(),
                                                left.arguments,
                                                left.arraysZeroInitialized && right.arraysZeroInitialized,
                                                left.arrayElementsInvalidated || right.arrayElementsInvalidated);
        }

        private static IEnumerable<TNumber> EnumerateTrackedArrayIndices(PreComputeState<TNumber> left, PreComputeState<TNumber> right)
        {
            HashSet<TNumber> indices = [];
            indices.UnionWith(left.knownArrayValues.Keys);
            indices.UnionWith(left.unknownArrayIndices);
            indices.UnionWith(right.knownArrayValues.Keys);
            indices.UnionWith(right.unknownArrayIndices);
            return indices;
        }
    }
}
