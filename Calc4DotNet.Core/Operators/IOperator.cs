using System.Collections.Immutable;

namespace Calc4DotNet.Core.Operators;

public interface IOperator
{
    string? SupplementaryText { get; }
    IOperator[] GetOperands();
    void ForEachOperand(Action<IOperator> action);
    (string Name, object? Value)[] GetProperties();

    void Accept(IOperatorVisitor visitor);
    TResult Accept<TResult>(IOperatorVisitor<TResult> visitor);
    TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param);
}

public sealed record ZeroOperator : IOperator
{
    public string? SupplementaryText => null;
    public IOperator[] GetOperands() => [];
    public void ForEachOperand(Action<IOperator> action) { }
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record PreComputedOperator(object Value) : IOperator
{
    public string? SupplementaryText => null;
    public IOperator[] GetOperands() => [];
    public void ForEachOperand(Action<IOperator> action) { }
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
        (nameof(Value), Value),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record ArgumentOperator(int Index, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [];
    public void ForEachOperand(Action<IOperator> action) { }
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
        (nameof(Index), Index),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record LetVariableOperator(int LocalIndex, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [];
    public void ForEachOperand(Action<IOperator> action) { }
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
        (nameof(LocalIndex), LocalIndex),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record DefineOperator(string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [];
    public void ForEachOperand(Action<IOperator> action) { }
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record LoadVariableOperator(string? SupplementaryText = null) : IOperator
{
    public string? VariableName => SupplementaryText;
    public IOperator[] GetOperands() => [];
    public void ForEachOperand(Action<IOperator> action) { }
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
        (nameof(VariableName), VariableName),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record InputOperator(string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [];
    public void ForEachOperand(Action<IOperator> action) { }
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record LoadArrayOperator(IOperator Index, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [Index];
    public void ForEachOperand(Action<IOperator> action) => action(Index);
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record PrintCharOperator(IOperator Character, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [Character];
    public void ForEachOperand(Action<IOperator> action) => action(Character);
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record ParenthesisOperator(ImmutableArray<IOperator> Operators, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [];
    public void ForEachOperand(Action<IOperator> action) { }
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
        // We don't show the 'Operators' property.
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();

    public bool Equals(ParenthesisOperator? other)
    {
        return ReferenceEquals(this, other)
                || (other is not null
                    && Operators.SequenceEqual(other.Operators)
                    && SupplementaryText == other.SupplementaryText);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        var operators = Operators;

        for (int i = 0; i < operators.Length; i++)
        {
            hash.Add(operators[i]);
        }

        hash.Add(SupplementaryText);
        return hash.ToHashCode();
    }
}

public sealed record DecimalOperator(IOperator Operand, int Value, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [Operand];
    public void ForEachOperand(Action<IOperator> action) => action(Operand);
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
        (nameof(Value), Value),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record StoreVariableOperator(IOperator Operand, string? SupplementaryText = null) : IOperator
{
    public string? VariableName => SupplementaryText;
    public IOperator[] GetOperands() => [Operand];
    public void ForEachOperand(Action<IOperator> action) => action(Operand);
    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
        (nameof(VariableName), VariableName),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record StoreArrayOperator(IOperator Value, IOperator Index, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [Value, Index];
    public void ForEachOperand(Action<IOperator> action)
    {
        action(Value);
        action(Index);
    }

    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public enum BinaryType { Add, Sub, Mult, Div, Mod, Equal, NotEqual, LessThan, LessThanOrEqual, GreaterThanOrEqual, GreaterThan, LogicalAnd, LogicalOr }

public sealed record BinaryOperator(IOperator Left, IOperator Right, BinaryType Type, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [Left, Right];
    public void ForEachOperand(Action<IOperator> action)
    {
        action(Left);
        action(Right);
    }

    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
        (nameof(Type), Type),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record ConditionalOperator(IOperator Condition, IOperator IfTrue, IOperator IfFalse, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [Condition, IfTrue, IfFalse];
    public void ForEachOperand(Action<IOperator> action)
    {
        action(Condition);
        action(IfTrue);
        action(IfFalse);
    }

    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record LetOperator(int LocalIndex, IOperator Value, IOperator Body, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => [Value, Body];
    public void ForEachOperand(Action<IOperator> action)
    {
        action(Value);
        action(Body);
    }

    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
        (nameof(LocalIndex), LocalIndex),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record UserDefinedOperator(OperatorDefinition Definition, ImmutableArray<IOperator> Operands, bool? IsTailCall, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => Operands.ToArray();
    public void ForEachOperand(Action<IOperator> action)
    {
        var operands = Operands;
        for (int i = 0; i < operands.Length; i++)
        {
            action(operands[i]);
        }
    }

    public (string Name, object? Value)[] GetProperties() =>
    [
        (nameof(SupplementaryText), SupplementaryText),
        (nameof(Definition), Definition),
        (nameof(IsTailCall), IsTailCall),
    ];

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();

    public bool Equals(UserDefinedOperator? other)
    {
        return ReferenceEquals(this, other)
                || (other is not null
                    && Definition.Equals(other.Definition)
                    && Operands.SequenceEqual(other.Operands)
                    && IsTailCall == other.IsTailCall
                    && SupplementaryText == other.SupplementaryText);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Definition);
        var operands = Operands;

        for (int i = 0; i < operands.Length; i++)
        {
            hash.Add(operands[i]);
        }

        hash.Add(IsTailCall);
        hash.Add(SupplementaryText);
        return hash.ToHashCode();
    }
}
