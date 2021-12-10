using System.Collections.Immutable;
using System.Diagnostics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.SyntaxAnalysis;

public static class Lexer
{
    public static List<IToken> Lex(string text, ref CompilationContext context)
    {
        var boxedContext = new CompilationContext.Boxed(context);
        var implement = new Implement(boxedContext, text, new Dictionary<string, int>());
        var tokens = implement.Lex();
        if (implement.Index < text.Length)
        {
            throw new Calc4DotNet.Core.Exceptions.UnexpectedTokenException(text[implement.Index].ToString());
        }

        context = boxedContext.Value;
        return tokens;
    }

    private struct Implement
    {
        private readonly CompilationContext.Boxed context;
        private readonly string text;
        private readonly Dictionary<string, int> argumentDictionary;
        public int Index { get; private set; }

        public Implement(CompilationContext.Boxed context, string text, Dictionary<string, int> argumentDictionary)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.text = text ?? throw new ArgumentNullException(nameof(text));
            this.argumentDictionary = argumentDictionary ?? throw new ArgumentNullException(nameof(argumentDictionary));
            this.Index = 0;
        }

        public List<IToken> Lex()
        {
            var list = new List<IToken>();

            while (Index < text.Length && text[Index] != ')')
            {
                if (char.IsWhiteSpace(text[Index]))
                {
                    Index++;
                    continue;
                }

                list.Add(NextToken());
            }

            return list;
        }

        private IToken NextToken()
        {
            switch (text[Index])
            {
                case 'D':
                    return LexDefineToken();
                case 'L':
                    return LexLoadVariableToken();
                case 'S':
                    return LexStoreVariableToken();
                case 'P':
                    return LexPrintCharToken();
                case '@':
                    return LexLoadArrayToken();
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return LexDecimalToken();
                case '{':
                    return LexUserDefinedOperatorOrArgumentToken();
                case '(':
                    return LexParenthesisToken();
                default:
                    return LexSymbolOrArgumentToken();
            }
        }

        private DefineToken LexDefineToken()
        {
            Debug.Assert(text[Index] == 'D');
            Index++;

            string? supplementaryText = LexSupplementaryText();
            if (string.IsNullOrEmpty(supplementaryText))
            {
                throw new Calc4DotNet.Core.Exceptions.DefinitionTextIsEmptyException();
            }

            string[] elems = supplementaryText.Split("|");
            if (elems.Length != 3)
            {
                throw new Calc4DotNet.Core.Exceptions.DefinitionTextNotSplittedProperlyException(supplementaryText);
            }

            string name = elems[0];
            string[] arguments = elems[1].Length > 0 ? elems[1].Replace(" ", "").Split(",") : Array.Empty<string>();
            string content = elems[2];

            var definition = new OperatorDefinition(name, arguments.Length);
            context.Value = context.Value.WithAddOrUpdateOperatorImplement(new OperatorImplement(definition, IsOptimized: false));

            var dictionary = arguments.Select((argumentName, argumentIndex) => (argumentName, argumentIndex))
                                      .ToDictionary(t => t.argumentName, t => t.argumentIndex);
            var tokens = new Implement(context, content, dictionary).Lex();

            return new DefineToken(name, arguments.ToImmutableArray(), tokens.ToImmutableArray(), supplementaryText);
        }

        private LoadVariableToken LexLoadVariableToken()
        {
            Debug.Assert(text[Index] == 'L');
            Index++;
            return new LoadVariableToken(LexSupplementaryText());
        }

        private StoreVariableToken LexStoreVariableToken()
        {
            Debug.Assert(text[Index] == 'S');
            Index++;
            return new StoreVariableToken(LexSupplementaryText());
        }

        private PrintCharToken LexPrintCharToken()
        {
            Debug.Assert(text[Index] == 'P');
            Index++;
            return new PrintCharToken(LexSupplementaryText());
        }

        private LoadArrayToken LexLoadArrayToken()
        {
            Debug.Assert(text[Index] == '@');
            Index++;
            return new LoadArrayToken(LexSupplementaryText());
        }

        private DecimalToken LexDecimalToken()
        {
            int value = text[Index++] - '0';
            return new DecimalToken(value, LexSupplementaryText());
        }

        private IToken LexUserDefinedOperatorOrArgumentToken()
        {
            Debug.Assert(text[Index] == '{');
            Index++;

            (int begin, int end) = (Index, text.IndexOf('}', Index));
            if (end < 0)
            {
                throw new Calc4DotNet.Core.Exceptions.TokenExpectedException("}");
            }

            string name = text[begin..end];
            Index = end + 1;

            if (context.Value.TryLookupOperatorImplement(name, out var implement))
                return new UserDefinedOperatorToken(implement.Definition, LexSupplementaryText());
            else if (argumentDictionary.TryGetValue(name, out var argumentIndex))
                return new ArgumentToken(name, argumentIndex, LexSupplementaryText());
            else
                throw new Calc4DotNet.Core.Exceptions.OperatorOrOperandNotDefinedException(name);
        }

        private ParenthesisToken LexParenthesisToken()
        {
            Debug.Assert(text[Index] == '(');
            Index++;

            Implement implement = new Implement(context, text, argumentDictionary) { Index = this.Index };
            var tokens = implement.Lex();

            Index = implement.Index;
            if (Index >= text.Length || text[Index] != ')')
            {
                throw new Calc4DotNet.Core.Exceptions.TokenExpectedException(")");
            }
            Index++;

            return new ParenthesisToken(tokens.ToImmutableArray(), LexSupplementaryText());
        }

        private IToken LexSymbolOrArgumentToken()
        {
            if (Index + 1 < text.Length)
            {
                switch (text.Substring(Index, 2))
                {
                    case "==":
                        Index += 2;
                        return new BinaryOperatorToken(BinaryType.Equal, LexSupplementaryText());
                    case "!=":
                        Index += 2;
                        return new BinaryOperatorToken(BinaryType.NotEqual, LexSupplementaryText());
                    case ">=":
                        Index += 2;
                        return new BinaryOperatorToken(BinaryType.GreaterThanOrEqual, LexSupplementaryText());
                    case "<=":
                        Index += 2;
                        return new BinaryOperatorToken(BinaryType.LessThanOrEqual, LexSupplementaryText());
                    case "->":
                        Index += 2;
                        return new StoreArrayToken(LexSupplementaryText());
                    default:
                        break;
                }
            }

            switch (text[Index])
            {
                case '+':
                    Index++;
                    return new BinaryOperatorToken(BinaryType.Add, LexSupplementaryText());
                case '-':
                    Index++;
                    return new BinaryOperatorToken(BinaryType.Sub, LexSupplementaryText());
                case '*':
                    Index++;
                    return new BinaryOperatorToken(BinaryType.Mult, LexSupplementaryText());
                case '/':
                    Index++;
                    return new BinaryOperatorToken(BinaryType.Div, LexSupplementaryText());
                case '%':
                    Index++;
                    return new BinaryOperatorToken(BinaryType.Mod, LexSupplementaryText());
                case '<':
                    Index++;
                    return new BinaryOperatorToken(BinaryType.LessThan, LexSupplementaryText());
                case '>':
                    Index++;
                    return new BinaryOperatorToken(BinaryType.GreaterThan, LexSupplementaryText());
                case '?':
                    Index++;
                    return new ConditionalOperatorToken(LexSupplementaryText());
                default:
                    break;
            }

            if (context.Value.TryLookupOperatorImplement(text[Index].ToString(), out var implement))
            {
                Index++;
                return new UserDefinedOperatorToken(implement.Definition, LexSupplementaryText());
            }
            else if (argumentDictionary.TryGetValue(text[Index].ToString(), out var argumentIndex))
            {
                string name = text[Index].ToString();
                Index++;
                return new ArgumentToken(name, argumentIndex, LexSupplementaryText());
            }

            throw new Calc4DotNet.Core.Exceptions.OperatorOrOperandNotDefinedException(text[Index].ToString());
        }

        private string? LexSupplementaryText()
        {
            if (Index >= text.Length || text[Index] != '[')
                return null;

            Index++;
            int begin = Index;
            int depth = 1;

            while (Index < text.Length && depth > 0)
            {
                char c = text[Index];

                if (c == '[')
                {
                    depth++;
                }
                else if (c == ']')
                {
                    depth--;
                }

                Index++;
            }

            if (depth != 0)
            {
                throw new Calc4DotNet.Core.Exceptions.TokenExpectedException("]");
            }

            int end = Index - 1;
            return text[begin..end];
        }
    }
}
