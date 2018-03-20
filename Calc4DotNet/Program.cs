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
            Context context = new Context();
            //string text = "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 30{fib}";
            //string text = "1?2?3?4?5";
            //string text = "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}]1+2*3/4-5+6*7-8?9?10?11{fib}?12{fib}";
            string text = "D[add|a,b|a+b] 123{add}200";
            Console.WriteLine("Input:");
            Console.WriteLine(text);
            Console.WriteLine();

            var tokens = Lexer.Lex(text, context);
            var op = Parser.Parse(tokens, context);
            Console.WriteLine("Before optimized:");
            PrintTree(op);
            Console.WriteLine();

            op = Optimizer.Optimize(op, context);
            Console.WriteLine("After optimized:");
            PrintTree(op);
            Console.WriteLine();

            Console.WriteLine(op.Evaluate(context, default));
            Debugger.Break();
        }

        static void PrintTree(IOperator op, int depth = 0)
        {
            Console.WriteLine(new string(' ', Indent * depth) + op.ToDetailString());
            foreach (var item in op.Operands)
            {
                PrintTree(item, depth + 1);
            }
        }
    }
}
