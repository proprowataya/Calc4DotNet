using System.Numerics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;
using Xunit;

namespace Calc4DotNet.Test;

internal enum ExecutorType
{
    Tree, LowLevel, Jit,
}

public class ExecutionTest
{
    private static readonly Type[] ValueTypes = new[] { typeof(Int32), typeof(Int64), typeof(Int128), typeof(Double), typeof(BigInteger) };

    private static readonly ExecutorType[] ExecutorTypes = Enum.GetValues<ExecutorType>();

    private static readonly OptimizeTarget?[] OptimizeTargets = Enum.GetValues<OptimizeTarget>()
                                                                    .Select(target => (OptimizeTarget?)target)
                                                                    .Append(null)
                                                                    .ToArray();

    public static readonly object[][] Source =
        (from testCase in TestCases.Values
         from valueType in ValueTypes
         where !testCase.SkipTypes?.Contains(valueType) ?? true
         from executorType in ExecutorTypes
         from target in OptimizeTargets
         select new object[] { testCase, valueType, executorType, target })
        .ToArray();

    [Theory, MemberData(nameof(Source))]
    private static void TestCompilationAndExecution(TestCase testCase,
                                                    Type valueType,
                                                    ExecutorType executorType,
                                                    OptimizeTarget? target)
    {
        TestCoreGeneric(executorType, target, testCase, (dynamic)Activator.CreateInstance(valueType)!);
    }

    private static void TestCoreGeneric<TNumber>(ExecutorType executorType,
                                                 OptimizeTarget? target,
                                                 TestCase testCase,
                                                 TNumber dummy)
        where TNumber : INumber<TNumber>
    {
        /*****
         * Compile
         *****/

        var (op, context, module) = CompileGeneric<TNumber>(testCase.Source, target, default!);

        /*****
         * Check compilation result
         *****/

        if (typeof(TNumber) == typeof(Int32) && target is (null or OptimizeTarget.None or OptimizeTarget.All))
        {
            // TODO:
            // Checking compilation result is currenly supported only for Int32 type.
            // Also, we cannot handle partial optimizations, that is the ones optimizing only for main operator or user defined operator.

            // Main operator
            {
                CompilationResult<Int32> expected =
                    (target?.HasFlag(OptimizeTarget.MainOperator) ?? false)
                    ? testCase.ExpectedWhenOptimized
                    : testCase.ExpectedWhenNotOptimized;

                Assert.Equal(expected.Operator, op);
            }

            // CompilationContext
            {
                CompilationResult<Int32> expected =
                    (target?.HasFlag(OptimizeTarget.UserDefinedOperators) ?? false)
                    ? testCase.ExpectedWhenOptimized
                    : testCase.ExpectedWhenNotOptimized;

                Assert.Equal(expected.Context, context, CompilationContextEqualityComparer.Instance);
            }

            // LowLevelModule
            {
                CompilationResult<Int32>? expected = target switch
                {
                    null or OptimizeTarget.None => testCase.ExpectedWhenNotOptimized,

                    // TODO: The following two cases are not currently supported
                    OptimizeTarget.MainOperator => null,
                    OptimizeTarget.UserDefinedOperators => null,

                    OptimizeTarget.All => testCase.ExpectedWhenOptimized,
                    _ => throw new InvalidOperationException()
                };

                if (expected is not null)
                {
                    Assert.Equal(expected.Module, (LowLevelModule<int>)(object)module, LowLevelModuleEqualityComparer<int>.Instance);
                }
            }
        }

        /*****
         * Check execution result
         *****/

        {
            TNumber expected = TNumber.CreateTruncating(testCase.ExpectedValue);
            IEvaluationState<TNumber> state = CreateEvaluationState<TNumber>();

            object actual = executorType switch
            {
                ExecutorType.Tree => Evaluator.Evaluate<TNumber>(op, context, state),
                ExecutorType.LowLevel => LowLevelExecutor.Execute(module, state),
                ExecutorType.Jit => ILCompiler.Compile(module).Run(state),
                _ => throw new InvalidOperationException(),
            };

            // Test result
            Assert.Equal(expected, actual);

            // Test variables after execution
            foreach (var (name, value) in testCase.VariablesAfterExecution)
            {
                Assert.Equal(TNumber.CreateTruncating(value), state.Variables[name.Value]);
            }

            // Test console output
            string actualConsoleOutput = ((MemoryIOService)state.IOService).GetHistory();
            Assert.Equal(testCase.ExpectedConsoleOutput ?? "", actualConsoleOutput);
        }
    }

    [Fact]
    public static void TestStackOverflow()
    {
        const string Source = "D[x||{x} + 1] {x}";

        foreach (var valueType in ValueTypes)
        {
            foreach (var target in OptimizeTargets)
            {
                Assert.Throws<Calc4DotNet.Core.Exceptions.StackOverflowException>(() =>
                {
                    dynamic dummy = Activator.CreateInstance(valueType)!;
                    dynamic module = CompileGeneric(Source, target, dummy).Module;
                    LowLevelExecutor.Execute(module, CreateEvaluationState(dummy));
                });
            }
        }
    }

    private static CompilationResult<TNumber> CompileGeneric<TNumber>(string source, OptimizeTarget? target, TNumber dummy)
        where TNumber : INumber<TNumber>
    {
        CompilationContext context = CompilationContext.Empty;
        List<IToken> tokens = Lexer.Lex(source, ref context);
        IOperator op = Parser.Parse(tokens, ref context);
        if (target is not null)
        {
            Optimizer.Optimize<TNumber>(ref op, ref context, target.GetValueOrDefault(), new DefaultVariableSource<TNumber>(TNumber.Zero));
        }
        LowLevelModule<TNumber> module = LowLevelCodeGenerator.Generate<TNumber>(op, context);

        return new CompilationResult<TNumber>(op, context, module);
    }

    private static IEvaluationState<TNumber> CreateEvaluationState<TNumber>(TNumber? dummy = default)
        where TNumber : INumber<TNumber>
    {
        return new SimpleEvaluationState<TNumber>(new DefaultVariableSource<TNumber>(TNumber.Zero),
                                                  new DefaultArraySource<TNumber>(),
                                                  new MemoryIOService());
    }
}
