using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Calc4DotNet.Core.Operators
{
    public interface IMinimalOperator
    {
        string SupplementaryText { get; }
        IReadOnlyList<IMinimalOperator> Operands { get; }
    }

    public interface IOperator<TNumber> : IMinimalOperator
    {
        new IReadOnlyList<IOperator<TNumber>> Operands { get; }

        void Accept(IOperatorVisitor<TNumber> visitor);
        TResult Accept<TResult>(IOperatorVisitor<TNumber, TResult> visitor);
        TResult Accept<TResult, TParam>(IOperatorVisitor<TNumber, TResult, TParam> visitor, TParam param);
    }

    public sealed class ZeroOperator<TNumber> : IOperator<TNumber>
    {
        public string SupplementaryText => null;
        public IReadOnlyList<IOperator<TNumber>> Operands => Array.Empty<IOperator<TNumber>>();
        IReadOnlyList<IMinimalOperator> IMinimalOperator.Operands => Operands;

        public void Accept(IOperatorVisitor<TNumber> visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TNumber, TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TNumber, TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class PreComputedOperator<TNumber> : IOperator<TNumber>
    {
        public TNumber Value { get; }

        public PreComputedOperator(TNumber value)
        {
            Value = value;
        }

        public string SupplementaryText => null;
        public IReadOnlyList<IOperator<TNumber>> Operands => Array.Empty<IOperator<TNumber>>();
        IReadOnlyList<IMinimalOperator> IMinimalOperator.Operands => Operands;

        public void Accept(IOperatorVisitor<TNumber> visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TNumber, TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TNumber, TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class ArgumentOperator<TNumber> : IOperator<TNumber>
    {
        public int Index { get; }
        public string SupplementaryText { get; }

        public ArgumentOperator(int index, string supplementaryText = null)
        {
            Index = index;
            SupplementaryText = supplementaryText;
        }

        public IReadOnlyList<IOperator<TNumber>> Operands => Array.Empty<IOperator<TNumber>>();
        IReadOnlyList<IMinimalOperator> IMinimalOperator.Operands => Operands;

        public void Accept(IOperatorVisitor<TNumber> visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TNumber, TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TNumber, TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class DefineOperator<TNumber> : IOperator<TNumber>
    {
        public string SupplementaryText { get; }

        public DefineOperator(string supplementaryText = null)
        {
            SupplementaryText = supplementaryText;
        }

        public IReadOnlyList<IOperator<TNumber>> Operands => Array.Empty<IOperator<TNumber>>();
        IReadOnlyList<IMinimalOperator> IMinimalOperator.Operands => Operands;

        public void Accept(IOperatorVisitor<TNumber> visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TNumber, TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TNumber, TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class ParenthesisOperator<TNumber> : IOperator<TNumber>
    {
        public ImmutableArray<IOperator<TNumber>> Operators { get; }
        public string SupplementaryText { get; }

        public ParenthesisOperator(ImmutableArray<IOperator<TNumber>> operators, string supplementaryText = null)
        {
            Operators = operators;
            SupplementaryText = supplementaryText;
        }

        public IReadOnlyList<IOperator<TNumber>> Operands => Array.Empty<IOperator<TNumber>>();
        IReadOnlyList<IMinimalOperator> IMinimalOperator.Operands => Operands;

        public void Accept(IOperatorVisitor<TNumber> visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TNumber, TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TNumber, TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class DecimalOperator<TNumber> : IOperator<TNumber>
    {
        public IOperator<TNumber> Operand { get; }
        public int Value { get; }
        public string SupplementaryText { get; }

        public DecimalOperator(IOperator<TNumber> operand, int value, string supplementaryText = null)
        {
            Operand = operand ?? throw new ArgumentNullException(nameof(operand));
            Value = value;
            SupplementaryText = supplementaryText;
            if (value < 0 || value > 9)
                throw new ArgumentException(nameof(value));
        }

        public IReadOnlyList<IOperator<TNumber>> Operands => new[] { Operand };
        IReadOnlyList<IMinimalOperator> IMinimalOperator.Operands => Operands;

        public void Accept(IOperatorVisitor<TNumber> visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TNumber, TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TNumber, TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public enum BinaryType { Add, Sub, Mult, Div, Mod, Equal, NotEqual, LessThan, LessThanOrEqual, GreaterThanOrEqual, GreaterThan }

    public sealed class BinaryOperator<TNumber> : IOperator<TNumber>
    {
        public IOperator<TNumber> Left { get; }
        public IOperator<TNumber> Right { get; }
        public BinaryType Type { get; }
        public string SupplementaryText { get; }

        public BinaryOperator(IOperator<TNumber> left, IOperator<TNumber> right, BinaryType type, string supplementaryText = null)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
            Type = type;
            SupplementaryText = supplementaryText;
        }

        public IReadOnlyList<IOperator<TNumber>> Operands => new[] { Left, Right };
        IReadOnlyList<IMinimalOperator> IMinimalOperator.Operands => Operands;

        public void Accept(IOperatorVisitor<TNumber> visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TNumber, TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TNumber, TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class ConditionalOperator<TNumber> : IOperator<TNumber>
    {
        public IOperator<TNumber> Condition { get; }
        public IOperator<TNumber> IfTrue { get; }
        public IOperator<TNumber> IfFalse { get; }
        public string SupplementaryText { get; }

        public ConditionalOperator(IOperator<TNumber> condition, IOperator<TNumber> ifTrue, IOperator<TNumber> ifFalse, string supplementaryText = null)
        {
            Condition = condition ?? throw new ArgumentNullException(nameof(condition));
            IfTrue = ifTrue ?? throw new ArgumentNullException(nameof(ifTrue));
            IfFalse = ifFalse ?? throw new ArgumentNullException(nameof(ifFalse));
            SupplementaryText = supplementaryText;
        }

        public IReadOnlyList<IOperator<TNumber>> Operands => new[] { Condition, IfTrue, IfFalse };
        IReadOnlyList<IMinimalOperator> IMinimalOperator.Operands => Operands;

        public void Accept(IOperatorVisitor<TNumber> visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TNumber, TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TNumber, TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }

    public sealed class UserDefinedOperator<TNumber> : IOperator<TNumber>
    {
        public OperatorDefinition Definition { get; }
        public ImmutableArray<IOperator<TNumber>> Operands { get; }
        public string SupplementaryText { get; }

        public UserDefinedOperator(OperatorDefinition definition, ImmutableArray<IOperator<TNumber>> operands, string supplementaryText = null)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Operands = operands;
            SupplementaryText = supplementaryText;

            if (operands.Length != definition.NumOperands)
                throw new ArgumentException($"Number of operands does not match between {nameof(definition)} and {nameof(operands)}");
        }

        IReadOnlyList<IOperator<TNumber>> IOperator<TNumber>.Operands => Operands;
        IReadOnlyList<IMinimalOperator> IMinimalOperator.Operands => Operands;

        public void Accept(IOperatorVisitor<TNumber> visitor) => visitor.Visit(this);
        public TResult Accept<TResult>(IOperatorVisitor<TNumber, TResult> visitor) => visitor.Visit(this);
        public TResult Accept<TResult, TParam>(IOperatorVisitor<TNumber, TResult, TParam> visitor, TParam param) => visitor.Visit(this, param);
    }
}
