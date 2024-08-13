using System.Collections.Immutable;
using System.Diagnostics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.SyntaxAnalysis;

public static class Parser
{
    public static IOperator Parse(IReadOnlyList<IToken> tokens, ref CompilationContext context)
    {
        var boxedContext = new CompilationContext.Boxed(context);
        GenerateUserDefinedCode(tokens, boxedContext);
        var op = ParseCore(tokens, boxedContext);
        context = boxedContext.Value;
        return op;
    }

    private static IOperator ParseCore(IReadOnlyList<IToken> tokens, CompilationContext.Boxed context)
    {
        var operators = ImmutableArray.CreateBuilder<IOperator>();

        int index = 0;
        while (index < tokens.Count)
        {
            (var op, index) = new Implement(tokens, context, index).ParseOne();
            operators.Add(op);
        }

        return operators.Count switch
        {
            0 => throw new Calc4DotNet.Core.Exceptions.CodeIsEmptyException(),
            1 => operators[0],
            _ => new ParenthesisOperator(operators.ToImmutable()),
        };
    }

    private static void GenerateUserDefinedCode(IReadOnlyList<IToken> tokens, CompilationContext.Boxed context)
    {
        foreach (var token in tokens.OfType<DefineToken>())
        {
            IOperator op = ParseCore(token.Tokens, context);
            context.Value = context.Value.WithAddOrUpdateOperatorImplement(
                context.Value.LookupOperatorImplement(token.Name) with { Operator = op });
        }
    }

    private struct Implement
    {
        private readonly IReadOnlyList<IToken> tokens;
        private readonly CompilationContext.Boxed context;
        private readonly int maxNumOperands;
        private int index;

        public Implement(IReadOnlyList<IToken> tokens, CompilationContext.Boxed context, int startIndex)
        {
            this.tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.maxNumOperands = tokens.Skip(startIndex).DefaultIfEmpty(null).Max(t => t?.NumOperands ?? 0);
            this.index = startIndex;
        }

        public (IOperator Operator, int NextIndex) ParseOne()
        {
            if (maxNumOperands == 0)
            {
                IOperator result = CreateOperator(tokens[index], Array.Empty<IOperator>());
                index++;
                return (result, index);
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

                    operands.Add(ParseCore(lower, context));
                }

                IOperator? result = null;
                while (index < tokens.Count)
                {
                    IToken token = tokens[index];
                    if (token.NumOperands < maxNumOperands)
                    {
                        break;
                    }

                    index++;

                    while (operands.Count < maxNumOperands)
                    {
                        lower = ReadLower();
                        if (lower.Count == 0)
                        {
                            throw new Calc4DotNet.Core.Exceptions.SomeOperandsMissingException();
                        }

                        operands.Add(ParseCore(lower, context));
                        if (operands.Count < maxNumOperands)
                            index++;
                    }

                    IOperator op = CreateOperator(token, operands);
                    operands.Clear();
                    operands.Add(op);
                    result = op;
                }

                Debug.Assert(result is not null);
                return (result, index);
            }
        }

        private IOperator CreateOperator(IToken token, IReadOnlyList<IOperator> operands)
        {
            return token switch
            {
                ArgumentToken arg => new ArgumentOperator(arg.Index, arg.SupplementaryText),
                DefineToken def => new DefineOperator(def.SupplementaryText),
                LoadVariableToken loadVariable => new LoadVariableOperator(loadVariable.SupplementaryText),
                InputToken input => new InputOperator(input.SupplementaryText),
                LoadArrayToken loadArray => new LoadArrayOperator(operands[0], loadArray.SupplementaryText),
                PrintCharToken printChar => new PrintCharOperator(operands[0], printChar.SupplementaryText),
                ParenthesisToken parenthesis => ParseCore(parenthesis.Tokens, context),
                DecimalToken dec => new DecimalOperator(operands[0], dec.Value, dec.SupplementaryText),
                StoreVariableToken storeVariable => new StoreVariableOperator(operands[0], storeVariable.SupplementaryText),
                StoreArrayToken storeArray => new StoreArrayOperator(operands[0], operands[1], storeArray.SupplementaryText),
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
