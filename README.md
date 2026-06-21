# Operator Only Language "Calc4" (C# Implementation)

Calc4 is a programming language where everything in its code is an operator. This repository contains the C# implementation of Calc4, with a REPL, an ahead-of-time compiler that emits standalone .NET DLLs, and a Native AOT-publishable build. The C++ version is [here](https://github.com/proprowataya/calc4), and an in-browser playground is available at [Try Calc4](https://proprowataya.github.io/calc4).

## Language at a Glance

Calc4's defining feature is that every token in the code is an operator. Look at the following expression.

```
46
```

Here `46` is two operators, `4` and `6`, not a single literal. Each digit is a unary operator that multiplies its operand by ten and adds its own value. Reading left to right, the operator `4` is first applied to an implicit zero, giving `0 * 10 + 4 = 4`. Then `6` is applied to that result, giving `4 * 10 + 6 = 46`.

Arithmetic, comparison, and the conditional `?` are written infix in the usual way. Precedence in Calc4 is decided by operand count, where operators that take fewer operands bind tighter than those that take more, and operators with the same operand count have equal precedence. Within the same precedence level, all operators are left-associative. So `1 + 2 * 3` evaluates to `9`, not `7`, because both `+` and `*` take two operands, share precedence, and are evaluated left to right.

You can define your own operators with `D[name|operands|body]` and call them by writing `{name}`. Here is a recursive Fibonacci operator applied to the 38th term.

```
D[fib|n|n <= 1 ? n ? (n - 1){fib} + (n - 2){fib}] 38{fib}
```

Calc4 has no loop construct, so iteration is expressed through recursion. The optimizer turns tail calls into in-place jumps, so tail-recursive code can run without growing the stack.

For the full grammar and rationale, see the [C++ repository](https://github.com/proprowataya/calc4).

## Requirements

[.NET](https://dotnet.microsoft.com/) (>= 10.0)

## Getting Started

```bash
git clone https://github.com/proprowataya/Calc4DotNet.git
cd Calc4DotNet
dotnet run --configuration Release --project Calc4DotNet
```

## Example

Calc4 works as a REPL. Please input what you want to evaluate.

```
$ dotnet run --configuration Release --project Calc4DotNet
Calc4 REPL
    Integer type: System.Int64
    Executor: JIT
    Optimize: on

> 1+2

3
Elapsed: 00:00:00.0203923

> D[fib|n|n <= 1 ? n ? (n - 1){fib} + (n - 2){fib}] 38{fib}

39088169
Elapsed: 00:00:00.2263669
```

## Running Calc4 Source Files

If you pass file paths instead of starting the REPL, they are executed as scripts.

```bash
dotnet run --configuration Release --project Calc4DotNet -- path/to/source.txt
```

Run with `--help` to see the full list of CLI options. Inside the REPL, the following commands toggle settings without restarting.

| Command | Effect |
| --- | --- |
| `#dump on`, `#dump off` | Toggle dumping the AST and bytecode for each input |
| `#optimize on`, `#optimize off` | Toggle the optimizer |
| `#reset` | Discard all REPL state, including user-defined operators, variables, and arrays |

## Numeric Types

The REPL exposes the runtime numeric type via `--size` or `-s`.

| `--size` value | .NET type |
| --- | --- |
| `32` | `System.Int32` |
| `64` (default) | `System.Int64` |
| `128` | `System.Int128` |
| `inf` | `System.Numerics.BigInteger` (arbitrary precision) |

## Native AOT REPL

`Calc4DotNet.Native` is the same REPL compiled with Native AOT, producing a self-contained executable with no .NET runtime dependency. The JIT backend is not available in Native AOT builds, so an interpreter is used instead.

```bash
dotnet publish Calc4DotNet.Native --configuration Release -r <RID>
```

Replace `<RID>` with your runtime identifier such as `linux-x64`, `win-x64`, or `osx-arm64`.

## Ahead-of-Time Compiler

`Calc4DotNet.Compiler` turns a Calc4 source file into a .NET DLL that runs without re-compiling.

```bash
dotnet run --configuration Release --project Calc4DotNet.Compiler -- path/to/source.txt
dotnet path/to/source.dll
```

The compiler writes `source.dll` and `source.runtimeconfig.json` next to the input, alongside copies of the assemblies it depends on.

## Tests and Benchmarks

```bash
dotnet test --configuration Release Calc4DotNet.Test
dotnet run --configuration Release --project Calc4DotNet.Benchmark
```

The test suite cross-checks every execution backend across all supported numeric types and optimization settings. The AOT path spawns `dotnet <generated.dll>` to validate the produced DLL, so a working `dotnet` CLI must be on `PATH`.

The test cases in `Calc4DotNet.Test/TestCases.cs` are generated. After editing inputs in `Calc4DotNet.TestCaseGenerator/Program.cs`, regenerate them.

```bash
dotnet run --configuration Release --project Calc4DotNet.TestCaseGenerator
```

## Information about Calc4

Further information is available at the [C++ implementation's repository](https://github.com/proprowataya/calc4), which contains a detailed description of the language and an [execution environment in your web browser](https://proprowataya.github.io/calc4).

## Copyright

Copyright (C) 2018-2026 Yuya Watari

## License

MIT License
