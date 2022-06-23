using System.Collections.Immutable;
using System.Numerics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Test;

internal sealed record TestCase(string Source,
                                int ExpectedValue,
                                ImmutableDictionary<ValueBox<string>, Int32> VariablesAfterExecution,
                                CompilationResult<Int32> ExpectedWhenOptimized,
                                CompilationResult<Int32> ExpectedWhenNotOptimized,
                                Type[]? SkipTypes,
                                string? ExpectedConsoleOutput = null);

internal sealed record CompilationResult<TNumber>(IOperator Operator, CompilationContext Context, LowLevelModule<TNumber> Module)
    where TNumber : INumber<TNumber>;
