using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Calc4DotNet.Core.Operators
{
    public interface IOperator
    {
        IReadOnlyList<IOperator> Operands { get; }

        void Accept(IOperatorVisitor visitor);
        TResult Accept<TResult>(IOperatorVisitor<TResult> visitor);
        TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param);
    }

    public sealed class ZeroOperator : IOperator
    {
        public string SupplementaryText => null;
        public IReadOnlyList<IOperator> Operands => Array.Empty<IOperator>();

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class PreComputedOperator : IOperator
    {
        public object Value { get; }

        public PreComputedOperator(object value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string SupplementaryText => null;
        public IReadOnlyList<IOperator> Operands => Array.Empty<IOperator>();

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class ArgumentOperator : IOperator
    {
        public int Index { get; }
        public string SupplementaryText { get; }

        public ArgumentOperator(int index, string supplementaryText = null)
        {
            Index = index;
            SupplementaryText = supplementaryText;
        }

        public IReadOnlyList<IOperator> Operands => Array.Empty<IOperator>();

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class DefineOperator : IOperator
    {
        public string SupplementaryText { get; }

        public DefineOperator(string supplementaryText = null)
        {
            SupplementaryText = supplementaryText;
        }

        public IReadOnlyList<IOperator> Operands => Array.Empty<IOperator>();

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class ParenthesisOperator : IOperator
    {
        public ImmutableArray<IOperator> Operators { get; }
        public string SupplementaryText { get; }

        public ParenthesisOperator(ImmutableArray<IOperator> operators, string supplementaryText = null)
        {
            Operators = operators;
            SupplementaryText = supplementaryText;
        }

        public IReadOnlyList<IOperator> Operands => Array.Empty<IOperator>();

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class DecimalOperator : IOperator
    {
        public IOperator Operand { get; }
        public int Value { get; }
        public string SupplementaryText { get; }

        public DecimalOperator(IOperator operand, int value, string supplementaryText = null)
        {
            Operand = operand ?? throw new ArgumentNullException(nameof(operand));
            Value = value;
            SupplementaryText = supplementaryText;
            if (value < 0 || value > 9)
                throw new ArgumentException(nameof(value));
        }

        public IReadOnlyList<IOperator> Operands => new[] { Operand };

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public enum BinaryType { Add, Sub, Mult, Div, Mod, Equal, NotEqual, LessThan, LessThanOrEqual, GreaterThanOrEqual, GreaterThan }

    public sealed class BinaryOperator : IOperator
    {
        public IOperator Left { get; }
        public IOperator Right { get; }
        public BinaryType Type { get; }
        public string SupplementaryText { get; }

        public BinaryOperator(IOperator left, IOperator right, BinaryType type, string supplementaryText = null)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
            Type = type;
            SupplementaryText = supplementaryText;
        }

        public IReadOnlyList<IOperator> Operands => new[] { Left, Right };

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class ConditionalOperator : IOperator
    {
        public IOperator Condition { get; }
        public IOperator IfTrue { get; }
        public IOperator IfFalse { get; }
        public string SupplementaryText { get; }

        public ConditionalOperator(IOperator condition, IOperator ifTrue, IOperator ifFalse, string supplementaryText = null)
        {
            Condition = condition ?? throw new ArgumentNullException(nameof(condition));
            IfTrue = ifTrue ?? throw new ArgumentNullException(nameof(ifTrue));
            IfFalse = ifFalse ?? throw new ArgumentNullException(nameof(ifFalse));
            SupplementaryText = supplementaryText;
        }

        public IReadOnlyList<IOperator> Operands => new[] { Condition, IfTrue, IfFalse };

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class UserDefinedOperator : IOperator
    {
        public OperatorDefinition Definition { get; }
        public ImmutableArray<IOperator> Operands { get; }
        public bool? IsTailCallable { get; }
        public string SupplementaryText { get; }

        public UserDefinedOperator(OperatorDefinition definition, ImmutableArray<IOperator> operands, bool? isTailCallable = null, string supplementaryText = null)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Operands = operands;
            IsTailCallable = isTailCallable;
            SupplementaryText = supplementaryText;

            if (operands.Length != definition.NumOperands)
                throw new ArgumentException($"Number of operands does not match between {nameof(definition)} and {nameof(operands)}");
        }

        IReadOnlyList<IOperator> IOperator.Operands => Operands;

        public void Accept(IOperatorVisitor visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }
}
