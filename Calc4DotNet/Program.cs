using System;
using System.Diagnostics;
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
                "D[get12345||12345] {get12345}+{get12345}",
                "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}]1+2*3/4-5+6*7-8?9?10?11{fib}?12{fib}",
                "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 30{fib}",
                "D[add|x,y|x+y] 12{add}23",
                "D[Earth||12742] D[Moon||3474] D[RoundDiv|x,y|(x * 10 / y + 5) / 10] {Earth}{RoundDiv}{Moon}",
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
            void ExecuteCore(IOperator op, Context context)
            {
                // Print input and user-defined operators as trees
                Console.WriteLine("Main");
                Console.WriteLine("{");
                PrintTree(op, 1);
                Console.WriteLine("}");
                Console.WriteLine();
                foreach (var def in context.OperatorDefinitions)
                {
                    Console.WriteLine($"Operator \"{def.Name}\"");
                    Console.WriteLine("{");
                    PrintTree(context.LookUpOperatorImplement(def.Name), 1);
                    Console.WriteLine("}");
                    Console.WriteLine();
                }

                // Execute
                Stopwatch sw = Stopwatch.StartNew();
                Console.WriteLine($"Evaluated: {op.Evaluate(context, default)}");
                sw.Stop();
                Console.WriteLine($"Elapsed: {sw.Elapsed}");
                Console.WriteLine();
            }

            /* ******************** */

            try
            {
                // Compile
                Context context = new Context();
                var tokens = Lexer.Lex(text, context);
                var op = Parser.Parse(tokens, context);

                // Execute
                Console.WriteLine("----- Before optimized -----");
                ExecuteCore(op, context);

                // Optimize
                op = Optimizer.Optimize(op, context);

                // Execute
                Console.WriteLine("----- After optimized -----");
                ExecuteCore(op, context);
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
