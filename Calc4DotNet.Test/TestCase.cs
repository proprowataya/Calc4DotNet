﻿using Calc4DotNet.Core;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Test;

internal sealed record TestCase(string Source,
                                int ExpectedValue,
                                CompilationResult<Int32> ExpectedWhenOptimized,
                                CompilationResult<Int32> ExpectedWhenNotOptimized,
                                Type[]? SkipTypes);

internal sealed record CompilationResult<TNumber>(IOperator Operator, CompilationContext Context, LowLevelModule<TNumber> Module)
    where TNumber : notnull;