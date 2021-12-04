namespace Calc4DotNet.Core.Evaluation;

public sealed class EvaluationContext<TNumber>
{
    private readonly Dictionary<ValueBox<string>, TNumber> variables = new();

    public TNumber GetVariableValue(string? variableName, TNumber defaultValue) =>
        variables.TryGetValue(ValueBox.Create(variableName), out var value) ? value : defaultValue;

    public TNumber SetVariableValue(string? variableName, TNumber value) =>
        variables[ValueBox.Create(variableName)] = value;
}
