using Calc4DotNet.Core;
using Calc4DotNet.Core.SyntaxAnalysis;
using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Calc4DotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Context context = new Context();
            string text = "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 30{fib}";
            var tokens = Lexer.Lex(text, context);
            var op = Parser.Parse(tokens, context);
            Console.WriteLine(op.Evaluate(context, default));
            Debugger.Break();
        }
    }
}
