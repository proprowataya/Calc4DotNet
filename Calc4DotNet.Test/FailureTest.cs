using System.Numerics;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Optimization;
using Xunit;
using static Calc4DotNet.Test.TestCommon;

namespace Calc4DotNet.Test;

public class FailureTest
{
    private static readonly FailureTestCase[] FailureTestCases = new[]
    {
        new FailureTestCase("D[x||{x} + 1] {x}",
                            typeof(Calc4DotNet.Core.Exceptions.StackOverflowException),
                            ExecutorTypes: new[]{ ExecutorType.LowLevel }),
        new FailureTestCase("1/0",
                            typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new FailureTestCase("1/(10 - 10)",
                            typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new FailureTestCase("1/L",
                            typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new FailureTestCase("1/(123@)",
                            typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new FailureTestCase("D[getzero||0] 1/{getzero}",
                            typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new FailureTestCase("1%0",
                            typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new FailureTestCase("1%(10 - 10)",
                            typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new FailureTestCase("1%L",
                            typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new FailureTestCase("1%(123@)",
                            typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
        new FailureTestCase("D[getzero||0] 1%{getzero}",
                            typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException)),
    };

    public static readonly object[][] Source =
        (from testCase in FailureTestCases
         from valueType in ValueTypes
         from executorType in testCase.ExecutorTypes ?? ExecutorTypes
         from target in OptimizeTargets
         select new object[] { testCase, valueType, executorType, target })
        .ToArray();

    [Theory, MemberData(nameof(Source))]
    private static void TestFailure(FailureTestCase testCase, Type valueType, ExecutorType executorType, OptimizeTarget? target)
    {
        TestFailureGeneric(testCase, executorType, target, (dynamic)Activator.CreateInstance(valueType)!);
    }

    private static void TestFailureGeneric<TNumber>(FailureTestCase testCase, ExecutorType executorType, OptimizeTarget? target, TNumber dummy)
        where TNumber : INumber<TNumber>
    {
        Assert.Throws(testCase.ExpectedException, () =>
        {
            var (op, context, module) = CompileGeneric<TNumber>(testCase.Source, target, default!);
            IEvaluationState<TNumber> state = CreateEvaluationState<TNumber>();

            switch (executorType)
            {
                case ExecutorType.Tree:
                    Evaluator.Evaluate<TNumber>(op, context, state);
                    break;
                case ExecutorType.LowLevel:
                    LowLevelExecutor.Execute<TNumber>(module, state);
                    break;
                case ExecutorType.Jit:
                    ILCompiler.Compile<TNumber>(module).Run(state);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        });
    }
}

internal sealed record FailureTestCase(string Source, Type ExpectedException, ExecutorType[]? ExecutorTypes = null);
