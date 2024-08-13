using System.Collections.Immutable;

namespace Calc4DotNet.Core.Operators;

public interface IOperator
{
    string? SupplementaryText { get; }
    IOperator[] GetOperands();

    void Accept(IOperatorVisitor visitor);
    TResult Accept<TResult>(IOperatorVisitor<TResult> visitor);
    TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param);
}

public sealed record ZeroOperator : IOperator
{
    public string? SupplementaryText => null;
    public IOperator[] GetOperands() => Array.Empty<IOperator>();

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record PreComputedOperator(object Value) : IOperator
{
    public string? SupplementaryText => null;
    public IOperator[] GetOperands() => Array.Empty<IOperator>();

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record ArgumentOperator(int Index, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => Array.Empty<IOperator>();

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record DefineOperator(string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => Array.Empty<IOperator>();

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record LoadVariableOperator(string? SupplementaryText = null) : IOperator
{
    public string? VariableName => SupplementaryText;
    public IOperator[] GetOperands() => Array.Empty<IOperator>();

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record InputOperator(string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => Array.Empty<IOperator>();

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record LoadArrayOperator(IOperator Index, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => new[] { Index };

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record PrintCharOperator(IOperator Character, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => new[] { Character };

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record ParenthesisOperator(ImmutableArray<IOperator> Operators, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => Array.Empty<IOperator>();

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

        foreach (var op in Operators)
        {
            hash.Add(op);
        }

        hash.Add(SupplementaryText);
        return hash.ToHashCode();
    }
}

public sealed record DecimalOperator(IOperator Operand, int Value, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => new[] { Operand };

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record StoreVariableOperator(IOperator Operand, string? SupplementaryText = null) : IOperator
{
    public string? VariableName => SupplementaryText;
    public IOperator[] GetOperands() => new[] { Operand };

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record StoreArrayOperator(IOperator Value, IOperator Index, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => new[] { Value, Index };

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public enum BinaryType { Add, Sub, Mult, Div, Mod, Equal, NotEqual, LessThan, LessThanOrEqual, GreaterThanOrEqual, GreaterThan }

public sealed record BinaryOperator(IOperator Left, IOperator Right, BinaryType Type, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => new[] { Left, Right };

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record ConditionalOperator(IOperator Condition, IOperator IfTrue, IOperator IfFalse, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => new[] { Condition, IfTrue, IfFalse };

    public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
    public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    public override string ToString() => this.ToStringImplement();
}

public sealed record UserDefinedOperator(OperatorDefinition Definition, ImmutableArray<IOperator> Operands, bool? IsTailCall, string? SupplementaryText = null) : IOperator
{
    public IOperator[] GetOperands() => Operands.ToArray();

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

        foreach (var op in Operands)
        {
            hash.Add(op);
        }

        hash.Add(IsTailCall);
        hash.Add(SupplementaryText);
        return hash.ToHashCode();
    }
}
