using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;

namespace Calc4DotNet.OriginalBenchmark
{
    internal static class Program
    {
        private const int NumRepetations = 4;

        private static readonly Type[] Types = new[]
        {
            typeof(Int32), typeof(Int64), typeof(Double), typeof(BigInteger)
        };

        private static readonly string[] Sources = new[]
        {
            "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 38{fib}",
            "D[tarai|x,y,z|x <= y ? y ? (((x - 1){tarai}y{tarai}z){tarai}((y - 1){tarai}z{tarai}x){tarai}((z - 1){tarai}x{tarai}y))] 18{tarai}12{tarai}5"
        };

        private static readonly HashSet<(Type Type, ExecutorType ExecutorType)> SkipCombinations = new()
        {
            (typeof(BigInteger), ExecutorType.LowLevelCpp)
        };

        static void Main(string[] args)
        {
            // Execute sources

            List<BenchmarkResult> benchmarkResults = new();

            foreach (var source in Sources)
            {
                Console.WriteLine($"Starting benchmark: Code = \"{source}\"");
                Console.WriteLine();

                foreach (var type in Types)
                {
                    foreach (var executorType in Enum.GetValues<ExecutorType>())
                    {
                        if (SkipCombinations.Contains((type, executorType)))
                        {
                            continue;
                        }

                        Console.WriteLine($"Type = {type}, Executor = {executorType}");
                        TimeSpan totalElapsed = TimeSpan.Zero;

                        for (int i = 0; i < NumRepetations; i++)
                        {
                            ExecutionResult result = ExecuteSource(source, executorType, (dynamic)Activator.CreateInstance(type)!);
                            Console.WriteLine($"--> Result = {result.Result}, Elapsed = {result.Elapsed}");
                            totalElapsed += result.Elapsed;
                        }

                        TimeSpan averageElapsed = totalElapsed / NumRepetations;
                        Console.WriteLine($"--> Average: {averageElapsed}");
                        benchmarkResults.Add(new BenchmarkResult(source, type, executorType, averageElapsed));
                        Console.WriteLine();
                    }
                }
            }

            // Summarize results

            int maxLengthOfExecutorTypeNames = Enum.GetValues<ExecutorType>().Max(name => name.ToString().Length);

            foreach (var item in from result in benchmarkResults
                                 group result by (result.Source, result.Type) into g
                                 select (g.Key.Source,
                                         g.Key.Type,
                                         ElapsedTimes: g.Select(item => (item.ExecutorType, item.AverageElapsed))))
            {
                Console.WriteLine($"Source = \"{item.Source}\", Type = {item.Type}");
                TimeSpan jitElapsedTime = item.ElapsedTimes.Single(result => result.ExecutorType == ExecutorType.Jit).AverageElapsed;

                foreach (var elapsed in item.ElapsedTimes)
                {
                    Console.WriteLine($" {elapsed.ExecutorType.ToString().PadRight(maxLengthOfExecutorTypeNames)} {elapsed.AverageElapsed} ({elapsed.AverageElapsed / jitElapsedTime:0.00})");
                }
                Console.WriteLine();
            }
        }

        private static ExecutionResult ExecuteSource<TNumber>(string source, ExecutorType executorType, TNumber dummy)
            where TNumber : notnull
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Compile
            CompilationContext context = CompilationContext.Empty;
            List<IToken> tokens = Lexer.Lex(source, ref context);
            IOperator op = Parser.Parse(tokens, ref context);
            Optimizer.Optimize<TNumber>(ref op, ref context);
            LowLevelModule<TNumber> module = LowLevelCodeGenerator.Generate<TNumber>(op, context);

            TNumber result = executorType switch
            {
                ExecutorType.Jit => ILCompiler.Compile<TNumber>(module).Run(),
                ExecutorType.LowLevel => LowLevelExecutor.Execute((dynamic)module),
                ExecutorType.LowLevelCpp => CppLowLevelExecutor.Execute((dynamic)module),
                _ => throw new InvalidOperationException()
            };

            stopwatch.Stop();
            return new ExecutionResult(result, stopwatch.Elapsed);
        }
    }

    internal enum ExecutorType
    {
        Jit, LowLevel, LowLevelCpp
    }

    internal sealed record ExecutionResult(object Result, TimeSpan Elapsed);

    internal sealed record BenchmarkResult(string Source, Type Type, ExecutorType ExecutorType, TimeSpan AverageElapsed);
}
