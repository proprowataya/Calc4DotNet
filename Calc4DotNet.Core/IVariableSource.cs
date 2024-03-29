﻿using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Calc4DotNet.Core;

public interface IVariableSource<TNumber>
    where TNumber : INumber<TNumber>
{
    TNumber this[string? variableName] { get; set; }
    bool TryGet(string? variableName, [MaybeNullWhen(false)] out TNumber value);
    IVariableSource<TNumber> Clone();
}

public sealed class DefaultVariableSource<TNumber> : IVariableSource<TNumber>
    where TNumber : INumber<TNumber>
{
    private readonly Dictionary<ValueBox<string>, TNumber> variables;

    public DefaultVariableSource()
    {
        this.variables = new Dictionary<ValueBox<string>, TNumber>();
    }

    private DefaultVariableSource(Dictionary<ValueBox<string>, TNumber> variables)
    {
        this.variables = variables;
    }

    public TNumber this[string? variableName]
    {
        get => variables.TryGetValue(ValueBox.Create(variableName), out var value) ? value : TNumber.Zero;
        set => variables[ValueBox.Create(variableName)] = value;
    }

    public bool TryGet(string? variableName, [MaybeNullWhen(false)] out TNumber value)
    {
        value = this[variableName];
        return true;
    }

    public ImmutableDictionary<ValueBox<string>, TNumber> ToImmutableDictionary()
    {
        return variables.ToImmutableDictionary();
    }

    public DefaultVariableSource<TNumber> Clone()
    {
        return new DefaultVariableSource<TNumber>(new(variables));
    }

    IVariableSource<TNumber> IVariableSource<TNumber>.Clone()
    {
        return Clone();
    }
}

internal sealed class VariableNotSetException : Exception
{ }

internal sealed class OptimizeTimeEvaluationState<TNumber> : IVariableSource<TNumber>
    where TNumber : INumber<TNumber>
{
    private readonly Dictionary<ValueBox<string>, TNumber> variables;
    private readonly HashSet<string?> knownVariables;

    // Do NOT moify the given parameters after construction
    public OptimizeTimeEvaluationState(Dictionary<ValueBox<string>, TNumber> variables, HashSet<string?> knownVariables)
    {
        this.variables = variables;
        this.knownVariables = knownVariables;
    }

    public TNumber this[string? variableName]
    {
        get
        {
            if (!knownVariables.Contains(variableName))
            {
                throw new InvalidOperationException($"Variable \"{variableName}\" does not exist.");
            }

            if (variables.TryGetValue(ValueBox.Create(variableName), out var value))
            {
                return value;
            }
            else
            {
                throw new VariableNotSetException();
            }
        }

        set
        {
            variables[ValueBox.Create(variableName)] = value;
        }
    }

    public bool TryGet(string? variableName, [MaybeNullWhen(false)] out TNumber value)
    {
        return variables.TryGetValue(ValueBox.Create(variableName), out value);
    }

    public void UnsetVariable(string? variableName)
    {
        variables.Remove(ValueBox.Create(variableName));
    }

    public void Assign(OptimizeTimeEvaluationState<TNumber> other)
    {
        Debug.Assert(knownVariables.SequenceEqual(other.knownVariables));

        variables.Clear();
        foreach (var (key, value) in other.variables)
        {
            variables.Add(key, value);
        }
    }

    public static OptimizeTimeEvaluationState<TNumber> Intersect(OptimizeTimeEvaluationState<TNumber> left, OptimizeTimeEvaluationState<TNumber> right)
    {
        Debug.Assert(left.knownVariables.SequenceEqual(right.knownVariables));

        Dictionary<ValueBox<string>, TNumber> variables = new();
        foreach (var (key, value) in left.variables)
        {
            Debug.Assert(value is not null);

            if (right.variables.TryGetValue(key, out var rightValue) && value.Equals(rightValue))
            {
                // This variable have the same value in both state
                variables.Add(key, value);
            }
            else
            {
                // Otherwise, the variables will be unknown.
                // We do not do anything
            }
        }

        return new OptimizeTimeEvaluationState<TNumber>(variables, left.knownVariables);
    }

    public OptimizeTimeEvaluationState<TNumber> Clone()
    {
        return new OptimizeTimeEvaluationState<TNumber>(new(variables),   // Make a copy of variables, which will be modified
                                                        knownVariables);  // We do not make a copy of this, because it is constant
    }

    IVariableSource<TNumber> IVariableSource<TNumber>.Clone()
    {
        return Clone();
    }
}
