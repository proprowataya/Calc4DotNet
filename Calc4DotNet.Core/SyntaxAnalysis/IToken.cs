using System.Collections.Immutable;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.SyntaxAnalysis;

public interface IToken
{
    int NumOperands { get; }
    string? SupplementaryText { get; }
}

public sealed record ArgumentToken(string Name, int Index, string? SupplementaryText = null) : IToken
{
    public int NumOperands => 0;
}

public sealed record DefineToken(string Name, ImmutableArray<string> Arguments, ImmutableArray<IToken> Tokens, string? SupplementaryText = null) : IToken
{
    public int NumOperands => 0;
}

public sealed record LoadToken(string? SupplementaryText = null) : IToken
{
    public string? VariableName => SupplementaryText;
    public int NumOperands => 0;
}

public sealed record ParenthesisToken(ImmutableArray<IToken> Tokens, string? SupplementaryText = null) : IToken
{
    public int NumOperands => 0;
}

public sealed record DecimalToken(int Value, string? SupplementaryText = null) : IToken
{
    public int NumOperands => 1;
}

public sealed record StoreToken(string? SupplementaryText = null) : IToken
{
    public string? VariableName => SupplementaryText;
    public int NumOperands => 1;
}

public sealed record BinaryOperatorToken(BinaryType Type, string? SupplementaryText = null) : IToken
{
    public int NumOperands => 2;
}

public sealed record ConditionalOperatorToken(string? SupplementaryText = null) : IToken
{
    public int NumOperands => 3;
}

public sealed record UserDefinedOperatorToken(OperatorDefinition Definition, string? SupplementaryText = null) : IToken
{
    public int NumOperands => Definition.NumOperands;
}
