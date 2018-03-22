using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.SyntaxAnalysis
{
    public static class Parser
    {
        public static IOperator<TNumber> Parse<TNumber>(IReadOnlyList<IToken> tokens, Context<TNumber> context)
        {
            return new Implement<TNumber>(tokens, context).Parse();
        }

        private struct Implement<TNumber>
        {
            private readonly IReadOnlyList<IToken> tokens;
            private readonly Context<TNumber> context;
            private readonly int maxNumOperands;
            private int index;

            public Implement(IReadOnlyList<IToken> tokens, Context<TNumber> context)
            {
                this.tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.maxNumOperands = tokens.Max(t => t.NumOperands);
                this.index = 0;
            }

            public IOperator<TNumber> Parse()
            {
                GenerateUserDefinedCode();
                List<IOperator<TNumber>> results = new List<IOperator<TNumber>>();

                if (maxNumOperands == 0)
                {
                    while (index < tokens.Count)
                    {
                        results.Add(CreateOperator(tokens[index], Array.Empty<IOperator<TNumber>>()));
                        index++;
                    }
                }
                else
                {
                    List<IOperator<TNumber>> operands = new List<IOperator<TNumber>>();

                    var lower = ReadLower();
                    if (lower.Count == 0 && tokens.FirstOrDefault() is DecimalToken)
                    {
                        operands.Add(new ZeroOperator<TNumber>());
                    }
                    else
                    {
                        operands.Add(new Implement<TNumber>(lower, context).Parse());
                    }

                    while (index < tokens.Count)
                    {
                        IToken token = tokens[index];
                        index++;

                        while (operands.Count < maxNumOperands)
                        {
                            operands.Add(new Implement<TNumber>(ReadLower(), context).Parse());
                            if (operands.Count < maxNumOperands)
                                index++;
                        }

                        IOperator<TNumber> op = CreateOperator(token, operands);
                        operands.Clear();
                        operands.Add(op);
                    }

                    results.AddRange(operands);
                }

                switch (results.Count)
                {
                    case 0:
                        throw new InvalidOperationException();
                    case 1:
                        return results[0];
                    default:
                        return new ParenthesisOperator<TNumber>(results.ToImmutableArray());
                }
            }

            private void GenerateUserDefinedCode()
            {
                foreach (var token in tokens.OfType<DefineToken>())
                {
                    IOperator<TNumber> op = new Implement<TNumber>(token.Tokens, context).Parse();
                    context.AddOrUpdateOperatorImplement(token.Name, op);
                }
            }

            private IOperator<TNumber> CreateOperator(IToken token, IReadOnlyList<IOperator<TNumber>> operands)
            {
                switch (token)
                {
                    case ArgumentToken arg:
                        return new ArgumentOperator<TNumber>(arg.Index, arg.SupplementaryText);
                    case DefineToken def:
                        return new DefineOperator<TNumber>(def.SupplementaryText);
                    case ParenthesisToken parenthesis:
                        return new Implement<TNumber>(parenthesis.Tokens, context).Parse();
                    case DecimalToken dec:
                        return new DecimalOperator<TNumber>(operands[0], dec.Value, dec.SupplementaryText);
                    case BinaryOperatorToken binary:
                        return new BinaryOperator<TNumber>(operands[0], operands[1], binary.Type, binary.SupplementaryText);
                    case ConditionalOperatorToken conditional:
                        return new ConditionalOperator<TNumber>(operands[0], operands[1], operands[2], conditional.SupplementaryText);
                    case UserDefinedOperatorToken userDefined:
                        return new UserDefinedOperator<TNumber>(userDefined.Definition, operands.ToImmutableArray(), null, userDefined.SupplementaryText);
                    default:
                        throw new InvalidOperationException();
                }
            }

            private IReadOnlyList<IToken> ReadLower()
            {
                List<IToken> list = new List<IToken>();

                while (index < tokens.Count && tokens[index].NumOperands < maxNumOperands)
                {
                    list.Add(tokens[index]);
                    index++;
                }

                return list;
            }
        }
    }
}
