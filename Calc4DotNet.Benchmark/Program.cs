using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;

namespace Calc4DotNet.Benchmark;

using NumberType = Int32;

public class Program
{
    public static string[] Sources => new string[]
    {
        "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 30{fib}",
        "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 34{fib}",
        "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 38{fib}",
        "D[fib|n|10S(n<=1?n?((n-1){fib}+(n-2){fib}))S] 30{fib} L",
        "D[count|n,acc|n==0?{acc}?(n-1){count}({acc}+1)] 50000000{count}0",
        "D[tarai|x,y,z|x <= y ? y ? (((x - 1){tarai}y{tarai}z){tarai}((y - 1){tarai}z{tarai}x){tarai}((z - 1){tarai}x{tarai}y))] 18{tarai}12{tarai}5",
    };

    [ParamsSource(nameof(Sources))]
    public string? Source { get; set; }

    static void Main(string[] args)
    {
        BenchmarkRunner.Run<Program>();
    }

    [Benchmark]
    public void RunByLowLevelExecutor()
    {
        // Compile
        CompilationContext context = CompilationContext.Empty;
        List<IToken> tokens = Lexer.Lex(Source!, ref context);
        IOperator op = Parser.Parse(tokens, ref context);
        Optimizer.Optimize<NumberType>(ref op,
                                       ref context,
                                       OptimizeTarget.All,
                                       new DefaultVariableSource<NumberType>(),
                                       new DefaultArraySource<NumberType>());
        LowLevelModule<NumberType> module = LowLevelCodeGenerator.Generate<NumberType>(op, context, LowLevelCodeGenerationOption.Default);

        // Run
        var state = new SimpleEvaluationState<NumberType>(new DefaultVariableSource<NumberType>(),
                                                          new DefaultArraySource<NumberType>(),
                                                          new MemoryIOService());
        LowLevelExecutor.Execute(module, state);
    }
}
