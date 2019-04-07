using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.SyntaxAnalysis
{
    public static class Lexer
    {
        public static List<IToken> Lex(string text, ref CompilationContext context)
        {
            var boxedContext = new CompilationContext.Boxed(context);
            var implement = new Implement(boxedContext, text, new Dictionary<string, int>());
            var tokens = implement.Lex();

            context = boxedContext.Value;
            return tokens;
        }

        private struct Implement
        {
            private readonly CompilationContext.Boxed context;
            private readonly string text;
            private readonly Dictionary<string, int> argumentDictionary;
            private int index;

            public Implement(CompilationContext.Boxed context, string text, Dictionary<string, int> argumentDictionary)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.text = text ?? throw new ArgumentNullException(nameof(text));
                this.argumentDictionary = argumentDictionary ?? throw new ArgumentNullException(nameof(argumentDictionary));
                this.index = 0;
            }

            public List<IToken> Lex()
            {
                var list = new List<IToken>();

                while (index < text.Length && text[index] != ')')
                {
                    if (char.IsWhiteSpace(text[index]))
                    {
                        index++;
                        continue;
                    }

                    list.Add(NextToken());
                }

                return list;
            }

            private IToken NextToken()
            {
                switch (text[index])
                {
                    case 'D':
                        return LexDefineToken();
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
                Debug.Assert(text[index] == 'D');
                index++;

                string supplementaryText = LexSupplementaryText();
                var elems = supplementaryText.Split("|");
                string name = elems[0];
                string[] arguments = elems[1].Length > 0 ? elems[1].Replace(" ", "").Split(",") : Array.Empty<string>();
                string content = elems[2];

                var definition = new OperatorDefinition(name, arguments.Length);
                context.Value = context.Value.WithAddOrUpdateOperatorImplement(new OperatorImplement(definition));

                var dictionary = arguments.Select((argumentName, argumentIndex) => (argumentName, argumentIndex))
                                          .ToDictionary(t => t.argumentName, t => t.argumentIndex);
                var tokens = new Implement(context, content, dictionary).Lex();

                return new DefineToken(name, arguments.ToImmutableArray(), tokens.ToImmutableArray(), supplementaryText);
            }

            private DecimalToken LexDecimalToken()
            {
                int value = text[index++] - '0';
                return new DecimalToken(value, LexSupplementaryText());
            }

            private IToken LexUserDefinedOperatorOrArgumentToken()
            {
                Debug.Assert(text[index] == '{');
                index++;

                (int begin, int end) = (index, text.IndexOf('}', index));
                string name = text.Substring(begin, end - begin);

                Debug.Assert(text[end] == '}');
                index = end + 1;

                if (context.Value.TryLookupOperatorImplement(name, out var implement))
                    return new UserDefinedOperatorToken(implement.Definition, LexSupplementaryText());
                else if (argumentDictionary.TryGetValue(name, out var argumentIndex))
                    return new ArgumentToken(name, argumentIndex, LexSupplementaryText());
                else
                    throw new InvalidOperationException();
            }

            private ParenthesisToken LexParenthesisToken()
            {
                Debug.Assert(text[index] == '(');
                index++;

                Implement implement = new Implement(context, text, argumentDictionary) { index = this.index };
                var tokens = implement.Lex();

                index = implement.index;
                Debug.Assert(text[index] == ')');
                index++;

                return new ParenthesisToken(tokens.ToImmutableArray(), LexSupplementaryText());
            }

            private IToken LexSymbolOrArgumentToken()
            {
                if (index + 1 < text.Length)
                {
                    switch (text.Substring(index, 2))
                    {
                        case "==":
                            index += 2;
                            return new BinaryOperatorToken(BinaryType.Equal, LexSupplementaryText());
                        case "!=":
                            index += 2;
                            return new BinaryOperatorToken(BinaryType.NotEqual, LexSupplementaryText());
                        case ">=":
                            index += 2;
                            return new BinaryOperatorToken(BinaryType.GreaterThanOrEqual, LexSupplementaryText());
                        case "<=":
                            index += 2;
                            return new BinaryOperatorToken(BinaryType.LessThanOrEqual, LexSupplementaryText());
                        default:
                            break;
                    }
                }

                switch (text[index])
                {
                    case '+':
                        index++;
                        return new BinaryOperatorToken(BinaryType.Add, LexSupplementaryText());
                    case '-':
                        index++;
                        return new BinaryOperatorToken(BinaryType.Sub, LexSupplementaryText());
                    case '*':
                        index++;
                        return new BinaryOperatorToken(BinaryType.Mult, LexSupplementaryText());
                    case '/':
                        index++;
                        return new BinaryOperatorToken(BinaryType.Div, LexSupplementaryText());
                    case '%':
                        index++;
                        return new BinaryOperatorToken(BinaryType.Mod, LexSupplementaryText());
                    case '<':
                        index++;
                        return new BinaryOperatorToken(BinaryType.LessThan, LexSupplementaryText());
                    case '>':
                        index++;
                        return new BinaryOperatorToken(BinaryType.GreaterThan, LexSupplementaryText());
                    case '?':
                        index++;
                        return new ConditionalOperatorToken(LexSupplementaryText());
                    default:
                        break;
                }

                if (context.Value.TryLookupOperatorImplement(text[index].ToString(), out var implement))
                {
                    index++;
                    return new UserDefinedOperatorToken(implement.Definition, LexSupplementaryText());
                }
                else if (argumentDictionary.TryGetValue(text[index].ToString(), out var argumentIndex))
                {
                    string name = text[index].ToString();
                    index++;
                    return new ArgumentToken(name, argumentIndex, LexSupplementaryText());
                }

                throw new InvalidOperationException();
            }

            private string LexSupplementaryText()
            {
                if (index >= text.Length || text[index] != '[')
                    return null;

                index++;
                (int begin, int end) = (index, text.IndexOf(']', index));
                index = end + 1;

                return text.Substring(begin, end - begin);
            }
        }
    }
}
