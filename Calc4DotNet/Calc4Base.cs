using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Exceptions;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;

namespace Calc4DotNet;

internal abstract class Calc4Base<TNumber>
    where TNumber : INumber<TNumber>
{
    protected const int Indent = 4;

    protected readonly Setting setting;
    protected CompilationContext context = CompilationContext.Empty;
    protected IEvaluationState<TNumber> state =
        new SimpleEvaluationState<TNumber>(new DefaultVariableSource<TNumber>(TNumber.Zero),
                                           new Calc4GlobalArraySource<TNumber>(),
                                           new TextWriterIOService(Console.Out));

    protected Calc4Base(Setting setting)
    {
        this.setting = setting;
    }

    public abstract void Run();

    protected void Execute(string text)
    {
        try
        {
            // Create a Stopwatch instance
            Stopwatch sw = Stopwatch.StartNew();

            // Compile
            List<IToken> tokens = Lexer.Lex(text, ref context);
            IOperator op = Parser.Parse(tokens, ref context);

            // Optimize
            if (setting.Optimize)
            {
                Optimizer.Optimize<TNumber>(ref op, ref context, OptimizeTarget.All, state.Variables);
            }

            // Generate low-level code and IL module
            LowLevelModule<TNumber> module = LowLevelCodeGenerator.Generate<TNumber>(op, context);

            // Print detail information of operators and low-level module
            if (setting.PrintDetailInformation)
            {
                PrintDetailInformation(op, context, module);
            }

            // Execute
            TNumber result;
            if (setting.ExecutorType == ExecutorType.LowLevel)
            {
                result = LowLevelExecutor.Execute(module, state);
            }
            else if (setting.ExecutorType == ExecutorType.JIT)
            {
                ICompiledModule<TNumber> ilModule = ILCompiler.Compile(module);
                result = ilModule.Run(state);
            }
            else
            {
                throw new InvalidOperationException();
            }

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

    private static void PrintDetailInformation(IOperator op, CompilationContext context, LowLevelModule<TNumber> module)
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
        Console.WriteLine(new string(' ', Indent * depth) + op.ToString());
        foreach (var item in op is ParenthesisOperator parenthesis ? (IEnumerable<IOperator>)parenthesis.Operators : op.GetOperands())
        {
            PrintTree(item, depth + 1);
        }
    }

    private static void PrintLowLevelOperations(LowLevelModule<TNumber> module)
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
