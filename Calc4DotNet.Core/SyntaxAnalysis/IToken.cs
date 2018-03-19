using System;
using System.Collections.Immutable;

namespace Calc4DotNet.Core.SyntaxAnalysis
{
    public interface IToken
    {
        int NumOperands { get; }
        string SupplementaryText { get; }
    }

    public sealed class ArgumentToken : IToken
    {
        public string Name { get; }
        public int Index { get; }
        public string SupplementaryText { get; }
        public int NumOperands => 0;

        public ArgumentToken(string name, int index, string supplementaryText = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Index = index;
            SupplementaryText = supplementaryText;
        }
    }

    public sealed class DefineToken : IToken
    {
        public string Name { get; }
        public ImmutableArray<string> Arguments { get; }
        public ImmutableArray<IToken> Tokens { get; }
        public string SupplementaryText { get; }
        public int NumOperands => 0;

        public DefineToken(string name, ImmutableArray<string> arguments, ImmutableArray<IToken> tokens, string supplementaryText = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Arguments = arguments;
            Tokens = tokens;
            SupplementaryText = supplementaryText;
        }
    }

    public sealed class ParenthesisToken : IToken
    {
        public ImmutableArray<IToken> Tokens { get; }
        public string SupplementaryText { get; }
        public int NumOperands => 0;

        public ParenthesisToken(ImmutableArray<IToken> tokens, string supplementaryText = null)
        {
            Tokens = tokens;
            SupplementaryText = supplementaryText;
        }
    }

    public sealed class DecimalToken : IToken
    {
        public int Value { get; }
        public string SupplementaryText { get; }
        public int NumOperands => 1;

        public DecimalToken(int value, string supplementaryText = null)
        {
            Value = value;
            SupplementaryText = supplementaryText;
        }
    }

    public sealed class BinaryOperatorToken : IToken
    {
        public BinaryOperator.ArithmeticType Type { get; }
        public string SupplementaryText { get; }
        public int NumOperands => 2;

        public BinaryOperatorToken(BinaryOperator.ArithmeticType type, string supplementaryText = null)
        {
            Type = type;
            SupplementaryText = supplementaryText;
        }
    }

    public sealed class ConditionalOperatorToken : IToken
    {
        public string SupplementaryText { get; }
        public int NumOperands => 3;

        public ConditionalOperatorToken(string supplementaryText = null)
        {
            SupplementaryText = supplementaryText;
        }
    }

    public sealed class UserDefinedOperatorToken : IToken
    {
        public OperatorDefinition Definition { get; }
        public string SupplementaryText { get; }
        public int NumOperands => Definition.NumOperands;

        public UserDefinedOperatorToken(OperatorDefinition definition, string supplementaryText = null)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            SupplementaryText = supplementaryText;
        }
    }
}
