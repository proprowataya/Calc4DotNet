using System.Numerics;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Optimization;
using Xunit;
using static Calc4DotNet.Test.TestCommon;

namespace Calc4DotNet.Test;

public class ExecutionFailureTest
{
    private static readonly ExecutionFailureTestCase[] ExecutionFailureTestCases = new[]
    {
        new ExecutionFailureTestCase("D[x||{x} + 1] {x}",
                                     typeof(Calc4DotNet.Core.Exceptions.StackOverflowException),
                                     ExecutorTypes: new[]{ ExecutorType.LowLevel }),
        new ExecutionFailureTestCase("1/0",
                                     typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new ExecutionFailureTestCase("1/(10 - 10)",
                                     typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new ExecutionFailureTestCase("1/L",
                                     typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new ExecutionFailureTestCase("1/(123@)",
                                     typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new ExecutionFailureTestCase("D[getzero||0] 1/{getzero}",
                                     typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new ExecutionFailureTestCase("1%0",
                                     typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new ExecutionFailureTestCase("1%(10 - 10)",
                                     typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new ExecutionFailureTestCase("1%L",
                                     typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new ExecutionFailureTestCase("1%(123@)",
                                     typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new ExecutionFailureTestCase("D[getzero||0] 1%{getzero}",
                                     typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
    };

    public static readonly object[][] Source =
        (from testCase in ExecutionFailureTestCases
         from valueType in ValueTypes
         from executorType in testCase.ExecutorTypes ?? ExecutorTypes
         from target in OptimizeTargets
         select new object[] { testCase, valueType, executorType, target })
        .ToArray();

    [Theory, MemberData(nameof(Source))]
    private static void TestFailure(ExecutionFailureTestCase testCase, Type valueType, ExecutorType executorType, OptimizeTarget? target)
    {
        TestFailureGeneric(testCase, executorType, target, (dynamic)Activator.CreateInstance(valueType)!);
    }

    private static void TestFailureGeneric<TNumber>(ExecutionFailureTestCase testCase, ExecutorType executorType, OptimizeTarget? target, TNumber dummy)
        where TNumber : INumber<TNumber>
    {
        // This test intends to check whether the expected exception is thrown in execution time.
        // So, we compile the given code outside Assert.Throws().
        var (op, context, module) = CompileGeneric<TNumber>(testCase.Source, target, default!);
        ICompiledModule<TNumber> ilModule = ILCompiler.Compile<TNumber>(module);
        IEvaluationState<TNumber> state = CreateEvaluationState<TNumber>("");

        Assert.Throws(testCase.ExpectedException, () =>
        {
            switch (executorType)
            {
                case ExecutorType.Tree:
                    Evaluator.Evaluate<TNumber>(op, context, state);
                    break;
                case ExecutorType.LowLevel:
                    LowLevelExecutor.Execute<TNumber>(module, state);
                    break;
                case ExecutorType.Jit:
                    ilModule.Run(state);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        });
    }
}

internal sealed record ExecutionFailureTestCase(string Source, Type ExpectedException, ExecutorType[]? ExecutorTypes = null);
