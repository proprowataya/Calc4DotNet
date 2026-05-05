using System.Collections.Immutable;
using System.Numerics;

namespace Calc4DotNet.Core.Optimization;

public static partial class Optimizer
{
    private sealed class UnknownVariableValueException : Exception
    { }

    private sealed class RecordingKnownVariableSource<TNumber> : IVariableSource<TNumber>
        where TNumber : INumber<TNumber>
    {
        private readonly PreComputeState<TNumber> initialState;
        private readonly Dictionary<ValueBox<string>, TNumber> values = [];

        public RecordingKnownVariableSource(PreComputeState<TNumber> initialState)
        {
            this.initialState = initialState;
        }

        public TNumber this[string? variableName]
        {
            get
            {
                if (values.TryGetValue(ValueBox.Create(variableName), out var value))
                {
                    return value;
                }

                if (initialState.TryGetVariable(variableName, out value))
                {
                    return value;
                }

                throw new UnknownVariableValueException();
            }

            set
            {
                values[ValueBox.Create(variableName)] = value;
            }
        }

        public bool TryGet(string? variableName, out TNumber value)
        {
            if (values.TryGetValue(ValueBox.Create(variableName), out var foundValue))
            {
                value = foundValue;
                return true;
            }

            return initialState.TryGetVariable(variableName, out value);
        }

        public ImmutableDictionary<ValueBox<string>, TNumber> ToImmutableDictionary()
        {
            throw new NotSupportedException();
        }

        public IEnumerable<KeyValuePair<string?, TNumber>> EnumerateWrittenVariables()
        {
            return values.OrderBy(p => p.Key.Value)
                         .Select(p => KeyValuePair.Create(p.Key.Value, p.Value));
        }
    }

    private sealed class RecordingKnownArraySource<TNumber> : IArraySource<TNumber>
        where TNumber : INumber<TNumber>
    {
        private readonly PreComputeState<TNumber> initialState;
        private readonly Dictionary<TNumber, TNumber> values = [];

        public RecordingKnownArraySource(PreComputeState<TNumber> initialState)
        {
            this.initialState = initialState;
        }

        public TNumber this[TNumber index]
        {
            get
            {
                if (values.TryGetValue(index, out var value))
                {
                    return value;
                }

                if (initialState.TryGetArrayValue(index, out value))
                {
                    return value;
                }

                throw new ArrayElementNotSetException();
            }

            set
            {
                values[index] = value;
            }
        }

        public ImmutableDictionary<TNumber, TNumber> ToImmutableDictionary()
        {
            throw new NotSupportedException();
        }

        public IEnumerable<KeyValuePair<TNumber, TNumber>> EnumerateWrittenArrayValues()
        {
            return values.OrderBy(p => p.Key);
        }
    }
}
