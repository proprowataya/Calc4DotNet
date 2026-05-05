using System.Collections.Immutable;
using System.Numerics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;
using Xunit;
using static Calc4DotNet.Test.TestCommon;

namespace Calc4DotNet.Test;

internal enum ExecutorType
{
    Tree, LowLevel, Jit,
}

public class ExecutionTest
{
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
            IEvaluationState<TNumber> state = CreateEvaluationState<TNumber>(testCase.StandardInput);

            object actual = executorType switch
            {
                ExecutorType.Tree => Evaluator.Evaluate<TNumber>(op, context, state),
                ExecutorType.LowLevel => LowLevelExecutor.Execute(module, state),
                ExecutorType.Jit => ILCompiler.Compile(module).Run(state),
                _ => throw new InvalidOperationException(),
            };

            // Test result
            Assert.Equal(expected, actual);

            // Test variables and array after execution. Both snapshots omit zero-valued
            // entries (see DefaultVariableSource / DefaultArraySource ToImmutableDictionary),
            // which matches Calc4's zero-init semantics where "bound to zero" and "never
            // bound" are externally indistinguishable.

            var expectedVariables = testCase.VariablesAfterExecution.ToImmutableDictionary(
                p => p.Key, p => TNumber.CreateTruncating(p.Value));
            var actualVariables = state.Variables.ToImmutableDictionary();
            Assert.Equal(expectedVariables, actualVariables);

            var expectedArray = testCase.ArrayAfterExecution.ToImmutableDictionary(
                p => TNumber.CreateTruncating(p.Key), p => TNumber.CreateTruncating(p.Value));
            var actualArray = state.GlobalArray.ToImmutableDictionary();
            Assert.Equal(expectedArray, actualArray);

            // Test console output
            string actualConsoleOutput = ((MemoryIOService)state.IOService).GetHistory();
            Assert.Equal(testCase.ExpectedConsoleOutput ?? "", actualConsoleOutput);
        }
    }

    [Fact]
    public static void OptimizerDoesNotFoldArrayReadWithoutInitialArray()
    {
        var context = CompilationContext.Empty;
        var tokens = Lexer.Lex("0@", ref context);
        var op = Parser.Parse(tokens, ref context);

        Optimizer.Optimize<Int32>(ref op, ref context, OptimizeTarget.All);
        var module = LowLevelCodeGenerator.Generate<Int32>(op, context, LowLevelCodeGenerationOption.Default);

        var array = new DefaultArraySource<Int32>();
        array[0] = 123;
        var state = new SimpleEvaluationState<Int32>(new DefaultVariableSource<Int32>(),
                                                     array,
                                                     new MemoryIOService());

        Assert.Equal(123, LowLevelExecutor.Execute(module, state));
    }

    [Fact]
    public static void OptimizerFoldsArrayReadWithInitialArray()
    {
        var context = CompilationContext.Empty;
        var tokens = Lexer.Lex("0@", ref context);
        var op = Parser.Parse(tokens, ref context);

        var array = new DefaultArraySource<Int32>();
        array[0] = 123;
        Optimizer.Optimize<Int32>(ref op,
                                  ref context,
                                  OptimizeTarget.All,
                                  new DefaultVariableSource<Int32>(),
                                  array);
        var module = LowLevelCodeGenerator.Generate<Int32>(op, context, LowLevelCodeGenerationOption.Default);

        // Use a fresh runtime array to prove the optimized operator captured the initial array snapshot.
        var state = new SimpleEvaluationState<Int32>(new DefaultVariableSource<Int32>(),
                                                     new DefaultArraySource<Int32>(),
                                                     new MemoryIOService());

        Assert.Equal(123, LowLevelExecutor.Execute(module, state));
    }
}
