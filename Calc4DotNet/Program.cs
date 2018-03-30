using System;
using System.Diagnostics;
using System.Linq;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;

namespace Calc4DotNet
{
    using NumberType = Int64;

    class Program
    {
        private const int Indent = 4;

        static void Main(string[] args)
        {
            Console.WriteLine("Calc4 Interpreter");
            Console.WriteLine();

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
            void ExecuteCore(IOperator<NumberType> op, CompilationContext<NumberType> context)
            {
                // Generate low-level operations
                Module<NumberType> module = LowLevelCodeGenerator.Generate(op, context);

                // Print input and user-defined operators as trees
                Console.WriteLine("Main");
                Console.WriteLine("{");
                PrintTree(op, 1);
                Console.WriteLine("}");
                Console.WriteLine();
                foreach (var implement in context.OperatorImplements)
                {
                    Console.WriteLine($"Operator \"{implement.Definition.Name}\"");
                    Console.WriteLine("{");
                    PrintTree(implement.Operator, 1);
                    Console.WriteLine("}");
                    Console.WriteLine();
                }

                // Print low-level operations
                Console.WriteLine("Low-level operation codes");
                Console.WriteLine("{");
                PrintLowLevelOperations(module);
                if (!module.ConstTable.IsDefaultOrEmpty)
                {
                    Console.WriteLine("Constants: " + string.Join(", ", module.ConstTable.Select((num, i) => $"[{i}] = {num}")));
                }
                Console.WriteLine("}");
                Console.WriteLine();

                // Execute
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    Console.WriteLine($"Evaluated (tree): {Evaluator.Evaluate(op, context)}");
                    sw.Stop();
                    Console.WriteLine($"Elapsed: {sw.Elapsed}");
                    Console.WriteLine();
                }
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    Console.WriteLine($"Evaluated (low-level): {Executor.ExecuteInt64(module)}");
                    sw.Stop();
                    Console.WriteLine($"Elapsed: {sw.Elapsed}");
                    Console.WriteLine();
                }
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    ICompiledModule<NumberType> compiledModule = ILCompiler.Compile(module);
                    Console.WriteLine($"Evaluated (JIT): {compiledModule.Run()}");
                    sw.Stop();
                    Console.WriteLine($"Elapsed: {sw.Elapsed}");
                    Console.WriteLine();
                }
            }

            /* ******************** */

#if !DEBUG
            try
#endif
            {
                // Compile
                var context = CompilationContext<NumberType>.Empty;
                var tokens = Lexer.Lex(text, ref context);
                var op = Parser.Parse(tokens, ref context);

                // Execute
                Console.WriteLine("----- Before optimized -----");
                ExecuteCore(op, context);

                // Optimize
                Optimizer.Optimize(ref op, ref context);

                // Execute
                Console.WriteLine("----- After optimized -----");
                ExecuteCore(op, context);
            }
#if !DEBUG
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine();
            }
#endif
        }

        private static void PrintTree(IOperator<NumberType> op, int depth = 0)
        {
            Console.WriteLine(new string(' ', Indent * depth) + op.ToDetailString());
            foreach (var item in op.Operands)
            {
                PrintTree(item, depth + 1);
            }
        }

        private static void PrintLowLevelOperations(Module<NumberType> module)
        {
            var operations = module.Operations;
            var dictionary = module.UserDefinedOperators.ToDictionary(p => p.StartAddress, p => p.Definition);

            Console.WriteLine("Main");
            for (int i = 0; i < operations.Length; i++)
            {
                if (dictionary.TryGetValue(i, out var definition))
                {
                    Console.WriteLine($"Operator \"{definition.Name}\"");
                }
                Console.WriteLine($"    {i.ToString().PadLeft(4)}: {operations[i].ToString()}");
            }
        }
    }
}
