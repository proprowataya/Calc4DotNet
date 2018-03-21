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
        public static IReadOnlyList<IToken> Lex(string text, Context context)
        {
            return new Implement(context, text, new Dictionary<string, int>()).Lex();
        }

        private struct Implement
        {
            private readonly Context context;
            private readonly string text;
            private readonly Dictionary<string, int> argumentDictionary;
            private int index;

            public Implement(Context context, string text, Dictionary<string, int> argumentDictionary)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.text = text ?? throw new ArgumentNullException(nameof(text));
                this.argumentDictionary = argumentDictionary ?? throw new ArgumentNullException(nameof(argumentDictionary));
                this.index = 0;
            }

            public IReadOnlyList<IToken> Lex()
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
                context.AddOperatorDefinition(definition);

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

                if (context.TryLookUpOperatorDefinition(name, out var definition))
                    return new UserDefinedOperatorToken(definition, LexSupplementaryText());
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
                        case ">=":
                            index += 2;
                            return new BinaryOperatorToken(BinaryOperator.ArithmeticType.GreaterThanOrEqual, LexSupplementaryText());
                        case "<=":
                            index += 2;
                            return new BinaryOperatorToken(BinaryOperator.ArithmeticType.LessThanOrEqual, LexSupplementaryText());
                        default:
                            break;
                    }
                }

                switch (text[index])
                {
                    case '+':
                        index++;
                        return new BinaryOperatorToken(BinaryOperator.ArithmeticType.Add, LexSupplementaryText());
                    case '-':
                        index++;
                        return new BinaryOperatorToken(BinaryOperator.ArithmeticType.Sub, LexSupplementaryText());
                    case '*':
                        index++;
                        return new BinaryOperatorToken(BinaryOperator.ArithmeticType.Mult, LexSupplementaryText());
                    case '/':
                        index++;
                        return new BinaryOperatorToken(BinaryOperator.ArithmeticType.Div, LexSupplementaryText());
                    case '<':
                        index++;
                        return new BinaryOperatorToken(BinaryOperator.ArithmeticType.LessThan, LexSupplementaryText());
                    case '>':
                        index++;
                        return new BinaryOperatorToken(BinaryOperator.ArithmeticType.GreaterThan, LexSupplementaryText());
                    case '?':
                        index++;
                        return new ConditionalOperatorToken(LexSupplementaryText());
                    default:
                        break;
                }

                if (context.TryLookUpOperatorDefinition(text[index].ToString(), out var definition))
                {
                    index++;
                    return new UserDefinedOperatorToken(definition, LexSupplementaryText());
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
