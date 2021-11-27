using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Exceptions;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;

namespace Calc4DotNet;

class Program
{
    private const int Indent = 4;

    private sealed class CommandLineArgsParseException : Exception
    {
        public CommandLineArgsParseException(string message) : base(message)
        { }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Calc4 Interpreter");
        Console.WriteLine();

        Setting setting;
        try
        {
            setting = ParseCommandLineArgs(args);
        }
        catch (CommandLineArgsParseException e)
        {
            Console.WriteLine($"Error: {e.Message}");
            Environment.Exit(1);
            return;
        }

        while (true)
        {
            Console.Write("> ");
            string? text = Console.ReadLine();
            if (text is null)
            {
                break;
            }

            Console.WriteLine();

            if (setting.NumberType == typeof(Int32))
            {
                ReplCore<Int32>(text, setting);
            }
            else if (setting.NumberType == typeof(Int64))
            {
                ReplCore<Int64>(text, setting);
            }
            else if (setting.NumberType == typeof(Double))
            {
                ReplCore<Double>(text, setting);
            }
            else if (setting.NumberType == typeof(BigInteger))
            {
                ReplCore<BigInteger>(text, setting);
            }
            else
            {
                Console.WriteLine($"Error: Type {setting.NumberType} is not supported.");
                Console.WriteLine();
            }
        }
    }

    private static void ReplCore<TNumber>(string text, Setting setting)
        where TNumber : notnull
    {
        try
        {
            // Create a Stopwatch instance
            Stopwatch sw = Stopwatch.StartNew();

            // Compile
            CompilationContext context = CompilationContext.Empty;
            List<IToken> tokens = Lexer.Lex(text, ref context);
            IOperator op = Parser.Parse(tokens, ref context);

            // Optimize
            if (setting.Optimize)
            {
                Optimizer.Optimize<TNumber>(ref op, ref context);
            }

            // Generate low-level code and IL module
            LowLevelModule<TNumber> module = LowLevelCodeGenerator.Generate<TNumber>(op, context);

            // Print detail information of operators and low-level module
            if (setting.PrintDetailInformation)
            {
                PrintDetailInformation<TNumber>(op, context, module);
            }

            // Execute
            TNumber result = LowLevelExecutor.Execute((dynamic)module);

            TimeSpan elapsed = sw.Elapsed;

            // Print
            Console.WriteLine(result);
            Console.WriteLine($"Elapsed: {elapsed}");
            Console.WriteLine();
        }
        catch (Calc4Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            Console.WriteLine();
        }
    }

    private static Setting ParseCommandLineArgs(string[] args)
    {
        Type numberType = typeof(Int64);
        ExecutorType executorType = ExecutorType.JIT;
        bool optimize = true;
        bool printDetailInformation = false;

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--type":
                case "-t":
                    // Specify number type
                    if (i + 1 >= args.Length)
                    {
                        throw new CommandLineArgsParseException($"Option {args[i]} requires a parameter.");
                    }

                    numberType = (args[i + 1]) switch
                    {
                        "int32" => typeof(Int32),
                        "int64" => typeof(Int64),
                        "double" => typeof(Double),
                        "bigint" or "biginteger" => typeof(BigInteger),
                        var arg => throw new CommandLineArgsParseException($"Type {arg} is not supported."),
                    };

                    i++;
                    break;
                case "-o":
                    optimize = true;
                    break;
                case "-od":
                    optimize = false;
                    break;
                case "--detail":
                    printDetailInformation = true;
                    break;
                case "--low-level":
                    executorType = ExecutorType.LowLevel;
                    break;
                default:
                    throw new CommandLineArgsParseException($"Unknown option {args[i]}.");
            }
        }

        return new Setting(numberType, executorType, optimize, printDetailInformation);
    }

    private static void PrintDetailInformation<TNumber>(IOperator op, CompilationContext context, LowLevelModule<TNumber> module)
        where TNumber : notnull
    {
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
            Debug.Assert(implement.Operator is not null);
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
    }

    private static void PrintTree(IOperator op, int depth = 0)
    {
        Console.WriteLine(new string(' ', Indent * depth) + op.ToDetailString());
        foreach (var item in op.GetOperands())
        {
            PrintTree(item, depth + 1);
        }
    }

    private static void PrintLowLevelOperations<TNumber>(LowLevelModule<TNumber> module)
        where TNumber : notnull
    {
        static void Print(ImmutableArray<LowLevelOperation> operations, string name, int? maxStackSize)
        {
            Console.WriteLine($"Operator \"{name}\"");

            if (maxStackSize is not null)
            {
                Console.WriteLine($"MaxStack: {maxStackSize.GetValueOrDefault()}");
            }

            for (int i = 0; i < operations.Length; i++)
            {
                Console.WriteLine($"    {i,-4}: {operations[i]}");
            }
        }

        Print(module.EntryPoint, "Main", null);
        foreach (var userDefinedOperator in module.UserDefinedOperators)
        {
            Print(userDefinedOperator.Operations, userDefinedOperator.Definition.Name, userDefinedOperator.MaxStackSize);
        }
    }
}
