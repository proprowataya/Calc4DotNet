using System;
using System.Diagnostics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;

namespace Calc4DotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Context context = new Context();
            string text = "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 30{fib}";
            //string text = "1+2*3/4-5+6*7-8";
            var tokens = Lexer.Lex(text, context);
            var op = Parser.Parse(tokens, context);
            op = Optimizer.Optimize(op, context);
            Console.WriteLine(op.Evaluate(context, default));
            Debugger.Break();
        }
    }
}
