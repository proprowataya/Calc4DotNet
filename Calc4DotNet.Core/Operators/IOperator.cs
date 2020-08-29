using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Calc4DotNet.Core.Operators
{
    public interface IOperator
    {
        string SupplementaryText { get; }
        IReadOnlyList<IOperator> Operands { get; }

        void Accept(IOperatorVisitor visitor);
        TResult Accept<TResult>(IOperatorVisitor<TResult> visitor);
        TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param);
    }

    public sealed record ZeroOperator : IOperator
    {
        public string SupplementaryText => null;
        public IReadOnlyList<IOperator> Operands => Array.Empty<IOperator>();

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed record PreComputedOperator(object Value) : IOperator
    {
        public string SupplementaryText => null;
        public IReadOnlyList<IOperator> Operands => Array.Empty<IOperator>();

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed record ArgumentOperator(int Index, string SupplementaryText = null) : IOperator
    {
        public IReadOnlyList<IOperator> Operands => Array.Empty<IOperator>();

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed record DefineOperator(string SupplementaryText = null) : IOperator
    {
        public IReadOnlyList<IOperator> Operands => Array.Empty<IOperator>();

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed record ParenthesisOperator(ImmutableArray<IOperator> Operators, string SupplementaryText = null) : IOperator
    {
        public IReadOnlyList<IOperator> Operands => Array.Empty<IOperator>();

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed record DecimalOperator(IOperator Operand, int Value, string SupplementaryText = null) : IOperator
    {
        private IOperator[] operands = null;
        public IReadOnlyList<IOperator> Operands => operands ??= new[] { Operand };

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public enum BinaryType { Add, Sub, Mult, Div, Mod, Equal, NotEqual, LessThan, LessThanOrEqual, GreaterThanOrEqual, GreaterThan }

    public sealed record BinaryOperator(IOperator Left, IOperator Right, BinaryType Type, string SupplementaryText = null) : IOperator
    {
        private IOperator[] operands = null;
        public IReadOnlyList<IOperator> Operands => operands ??= new[] { Left, Right };

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed record ConditionalOperator(IOperator Condition, IOperator IfTrue, IOperator IfFalse, string SupplementaryText = null) : IOperator
    {
        private IOperator[] operands = null;
        public IReadOnlyList<IOperator> Operands => operands ??= new[] { Condition, IfTrue, IfFalse };

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed record UserDefinedOperator(OperatorDefinition Definition, ImmutableArray<IOperator> Operands, bool? IsTailCallable, string SupplementaryText = null) : IOperator
    {
        IReadOnlyList<IOperator> IOperator.Operands => Operands;

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }
}
