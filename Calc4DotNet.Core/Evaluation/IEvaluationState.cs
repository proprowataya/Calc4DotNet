﻿using System.Diagnostics;

namespace Calc4DotNet.Core.Evaluation;

public interface IEvaluationState<TNumber>
{
    IVariableSource<TNumber> Variables { get; }
    IEvaluationState<TNumber> Clone();
}

public interface IVariableSource<TNumber>
{
    TNumber this[string? variableName] { get; set; }
    IVariableSource<TNumber> Clone();
}

public sealed class SimpleEvaluationState<TNumber> : IEvaluationState<TNumber>
{
    public IVariableSource<TNumber> Variables { get; }

    public SimpleEvaluationState(IVariableSource<TNumber> variables)
    {
        Variables = variables;
    }

    public SimpleEvaluationState<TNumber> Clone()
    {
        return new SimpleEvaluationState<TNumber>(Variables.Clone());
    }

    IEvaluationState<TNumber> IEvaluationState<TNumber>.Clone()
    {
        return Clone();
    }
}

public sealed class DefaultVariableSource<TNumber> : IVariableSource<TNumber>
{
    private readonly TNumber defaultValue;
    private readonly Dictionary<ValueBox<string>, TNumber> variables;

    public DefaultVariableSource(TNumber defaultValue)
    {
        this.defaultValue = defaultValue;
        this.variables = new Dictionary<ValueBox<string>, TNumber>();
    }

    private DefaultVariableSource(TNumber defaultValue, Dictionary<ValueBox<string>, TNumber> variables)
    {
        this.defaultValue = defaultValue;
        this.variables = variables;
    }

    public TNumber this[string? variableName]
    {
        get => variables.TryGetValue(ValueBox.Create(variableName), out var value) ? value : defaultValue;
        set => variables[ValueBox.Create(variableName)] = value;
    }

    public DefaultVariableSource<TNumber> Clone()
    {
        return new DefaultVariableSource<TNumber>(defaultValue, new(variables));
    }

    IVariableSource<TNumber> IVariableSource<TNumber>.Clone()
    {
        return Clone();
    }
}

internal sealed class VariableNotSetException : Exception
{ }

internal sealed class OptimizeTimeEvaluationState<TNumber> : IVariableSource<TNumber>
{
    private readonly Dictionary<ValueBox<string>, TNumber> variables;
    private readonly HashSet<string?> knownVariables;

    // Do NOT moify the given 'knownVariables' after construction
    public OptimizeTimeEvaluationState(HashSet<string?> knownVariables, IEnumerable<string?>? variablesWithDefaultValue, TNumber defaultValue)
    {
        // Assert that all variables in 'variablesWithDefaultValue' exist in 'knownVariables'
        Debug.Assert(variablesWithDefaultValue is null || !variablesWithDefaultValue.Except(knownVariables).Any());

        variablesWithDefaultValue ??= Enumerable.Empty<string>();
        this.variables = variablesWithDefaultValue.Select(variableName => ValueBox.Create(variableName))
                                                  .ToDictionary(variableName => variableName, _ => defaultValue);
        this.knownVariables = knownVariables;
    }

    private OptimizeTimeEvaluationState(Dictionary<ValueBox<string>, TNumber> variables, HashSet<string?> knownVariables)
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