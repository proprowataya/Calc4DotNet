using System.Numerics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Optimization;
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
}
