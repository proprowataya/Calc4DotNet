using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Exceptions;
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
            void ExecuteCore(IOperator op, CompilationContext context)
            {
                // Generate low-level operations
                LowLevelModule<NumberType> module = LowLevelCodeGenerator.Generate<NumberType>(op, context);

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
                static void Execute(string type, Func<NumberType> executor)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    Console.WriteLine($"Evaluated ({type}): {executor()}");
                    sw.Stop();
                    Console.WriteLine($"Elapsed: {sw.Elapsed}");
                    Console.WriteLine();
                }

                Execute("Low Level Executor", () => LowLevelExecutor.Execute(module));
                Execute("JIT", () => ILCompiler.Compile(module).Run());
            }

            /* ******************** */

            try
            {
                // Compile
                var context = CompilationContext.Empty;
                var tokens = Lexer.Lex(text, ref context);
                var op = Parser.Parse(tokens, ref context);

                // Execute
                Console.WriteLine("----- Before optimized -----");
                ExecuteCore(op, context);

                // Optimize
                Optimizer.Optimize<NumberType>(ref op, ref context);

                // Execute
                Console.WriteLine("----- After optimized -----");
                ExecuteCore(op, context);
            }
            catch (Calc4Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
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

        private static void PrintLowLevelOperations(LowLevelModule<NumberType> module)
        {
            void Print(ImmutableArray<LowLevelOperation> operations, string name)
            {
                Console.WriteLine($"Operator \"{name}\"");
                for (int i = 0; i < operations.Length; i++)
                {
                    Console.WriteLine($"    {i.ToString().PadLeft(4)}: {operations[i].ToString()}");
                }
            }

            Print(module.EntryPoint, "Main");
            foreach (var (definition, operations) in module.UserDefinedOperators)
            {
                Print(operations, definition.Name);
            }
        }
    }
}
