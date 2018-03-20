using System;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;

namespace Calc4DotNet
{
    class Program
    {
        private const int Indent = 4;

        static void Main(string[] args)
        {
            var texts = new string[]
            {
                "1?2?3?4?5",
                "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 30{fib}",
                "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}]1+2*3/4-5+6*7-8?9?10?11{fib}?12{fib}",
                "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}]1+2*3/4-5+6*7-8?9?10?11{fib}?12{fib}"
            };

            foreach (var text in texts)
            {
                Console.WriteLine("====================");
                Console.WriteLine("Input:");
                Console.WriteLine(text);
                Console.WriteLine();
                Execute(text);
            }

            while (true)
            {
                Console.Write("> ");
                string text = Console.ReadLine();

                Console.WriteLine();
                Execute(text);
            }
        }

        private static void Execute(string text)
        {
            void PrintAll(IOperator op, Context context)
            {
                PrintTree(op);
                foreach (var def in context.OperatorDefinitions)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Operator {def.Name}");
                    PrintTree(def.Root, 1);
                }
            }

            try
            {
                Context context = new Context();
                var tokens = Lexer.Lex(text, context);
                var op = Parser.Parse(tokens, context);
                Console.WriteLine("----- Before optimized -----");
                PrintAll(op, context);
                Console.WriteLine($"Evaluated: {op.Evaluate(context, default)}");
                Console.WriteLine();

                op = Optimizer.Optimize(op, context);
                foreach (var item in context.OperatorDefinitions)
                {
                    context.AddOrUpdateOperatorDefinition(Optimizer.Optimize(item, context));
                }

                Console.WriteLine("----- After optimized -----");
                PrintAll(op, context);
                Console.WriteLine($"Evaluated: {op.Evaluate(context, default)}");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine();
            }
        }

        private static void PrintTree(IOperator op, int depth = 0)
        {
            Console.WriteLine(new string(' ', Indent * depth) + op.ToDetailString());
            foreach (var item in op.Operands)
            {
                PrintTree(item, depth + 1);
            }
        }
    }
}
