using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Calc4DotNet.Core;

public interface IVariableSource<TNumber>
    where TNumber : INumber<TNumber>
{
    TNumber this[string? variableName] { get; set; }

    bool TryGet(string? variableName, [MaybeNullWhen(false)] out TNumber value);

    // Returns a snapshot of all observable non-zero variable values.
    // Zero-valued entries may be omitted and are interpreted as zero by optimizers.
    ImmutableDictionary<ValueBox<string>, TNumber> ToImmutableDictionary();
}

public sealed class DefaultVariableSource<TNumber> : IVariableSource<TNumber>
    where TNumber : INumber<TNumber>
{
    private readonly Dictionary<ValueBox<string>, TNumber> variables = [];

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
        // Calc4's variable space is conceptually pre-populated with zero for every
        // name, so "bound to zero" and "never bound" are externally indistinguishable.
        // Drop zero entries so the snapshot reflects observable state only.
        return ImmutableDictionary.CreateRange(variables.Where(p => !TNumber.IsZero(p.Value)));
    }
}
