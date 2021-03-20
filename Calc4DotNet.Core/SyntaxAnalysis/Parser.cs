using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.SyntaxAnalysis
{
    public static class Parser
    {
        public static IOperator Parse(IReadOnlyList<IToken> tokens, ref CompilationContext context)
        {
            var boxedContext = new CompilationContext.Boxed(context);
            var implement = new Implement(tokens, boxedContext);
            var op = implement.Parse();

            context = boxedContext.Value;
            return op;
        }

        private struct Implement
        {
            private readonly IReadOnlyList<IToken> tokens;
            private readonly CompilationContext.Boxed context;
            private readonly int maxNumOperands;
            private int index;

            public Implement(IReadOnlyList<IToken> tokens, CompilationContext.Boxed context)
            {
                this.tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.maxNumOperands = tokens.Count > 0 ? tokens.Max(t => t.NumOperands) : 0;
                this.index = 0;
            }

            public IOperator Parse()
            {
                GenerateUserDefinedCode();
                List<IOperator> results = new List<IOperator>();

                if (maxNumOperands == 0)
                {
                    while (index < tokens.Count)
                    {
                        results.Add(CreateOperator(tokens[index], Array.Empty<IOperator>()));
                        index++;
                    }
                }
                else
                {
                    List<IOperator> operands = new List<IOperator>();

                    var lower = ReadLower();
                    if (lower.Count == 0 && tokens.FirstOrDefault() is DecimalToken)
                    {
                        operands.Add(new ZeroOperator());
                    }
                    else
                    {
                        if (lower.Count == 0)
                        {
                            throw new Calc4DotNet.Core.Exceptions.SomeOperandsMissingException();
                        }

                        operands.Add(new Implement(lower, context).Parse());
                    }

                    while (index < tokens.Count)
                    {
                        IToken token = tokens[index];
                        index++;

                        while (operands.Count < maxNumOperands)
                        {
                            lower = ReadLower();
                            if (lower.Count == 0)
                            {
                                throw new Calc4DotNet.Core.Exceptions.SomeOperandsMissingException();
                            }

                            operands.Add(new Implement(lower, context).Parse());
                            if (operands.Count < maxNumOperands)
                                index++;
                        }

                        IOperator op = CreateOperator(token, operands);
                        operands.Clear();
                        operands.Add(op);
                    }

                    results.AddRange(operands);
                }

                return results.Count switch
                {
                    0 => throw new Calc4DotNet.Core.Exceptions.CodeIsEmptyException(),
                    1 => results[0],
                    _ => new ParenthesisOperator(results.ToImmutableArray()),
                };
            }

            private void GenerateUserDefinedCode()
            {
                foreach (var token in tokens.OfType<DefineToken>())
                {
                    IOperator op = new Implement(token.Tokens, context).Parse();
                    context.Value = context.Value.WithAddOrUpdateOperatorImplement(
                                                    context.Value.LookupOperatorImplement(token.Name) with { Operator = op });
                }
            }

            private IOperator CreateOperator(IToken token, IReadOnlyList<IOperator> operands)
            {
                return token switch
                {
                    ArgumentToken arg => new ArgumentOperator(arg.Index, arg.SupplementaryText),
                    DefineToken def => new DefineOperator(def.SupplementaryText),
                    ParenthesisToken parenthesis => new Implement(parenthesis.Tokens, context).Parse(),
                    DecimalToken dec => new DecimalOperator(operands[0], dec.Value, dec.SupplementaryText),
                    BinaryOperatorToken binary => new BinaryOperator(operands[0], operands[1], binary.Type, binary.SupplementaryText),
                    ConditionalOperatorToken conditional => new ConditionalOperator(operands[0], operands[1], operands[2], conditional.SupplementaryText),
                    UserDefinedOperatorToken userDefined => new UserDefinedOperator(userDefined.Definition, operands.ToImmutableArray(), null, userDefined.SupplementaryText),
                    _ => throw new InvalidOperationException(),
                };
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
