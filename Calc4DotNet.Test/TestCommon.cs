using System.Numerics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;

namespace Calc4DotNet.Test;

internal static class TestCommon
{
    public static readonly Type[] ValueTypes = new[] { typeof(Int32), typeof(Int64), typeof(Int128), typeof(Double), typeof(BigInteger) };

    public static readonly ExecutorType[] ExecutorTypes = Enum.GetValues<ExecutorType>();

    public static readonly OptimizeTarget?[] OptimizeTargets = Enum.GetValues<OptimizeTarget>()
                                                                   .Select(target => (OptimizeTarget?)target)
                                                                   .Append(null)
                                                                   .ToArray();

    public static CompilationResult<TNumber> CompileGeneric<TNumber>(string source, OptimizeTarget? target, TNumber dummy)
        where TNumber : INumber<TNumber>
    {
        CompilationContext context = CompilationContext.Empty;
        List<IToken> tokens = Lexer.Lex(source, ref context);
        IOperator op = Parser.Parse(tokens, ref context);
        if (target is not null)
        {
            Optimizer.Optimize<TNumber>(ref op, ref context, target.GetValueOrDefault(), new DefaultVariableSource<TNumber>());
        }
        LowLevelModule<TNumber> module = LowLevelCodeGenerator.Generate<TNumber>(op, context, LowLevelCodeGenerationOption.Default);

        return new CompilationResult<TNumber>(op, context, module);
    }

    public static IEvaluationState<TNumber> CreateEvaluationState<TNumber>(string standardInput, TNumber? dummy = default)
        where TNumber : INumber<TNumber>
    {
        return new SimpleEvaluationState<TNumber>(new DefaultVariableSource<TNumber>(),
                                                  new DefaultArraySource<TNumber>(),
                                                  new MemoryIOService(standardInput));
    }
}
