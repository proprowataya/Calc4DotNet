# Operator Only Language "Calc4"

Calc4 is a programming language where everything in its code is an operator. This repository contains Calc4's environment implemented by C#. C++ version is [here](https://github.com/proprowataya/calc4).

## Requirements

[.NET](https://dotnet.microsoft.com/) (>= 8.0)

## Getting Started

```
git clone https://github.com/proprowataya/Calc4DotNet.git
cd Calc4DotNet
dotnet run --configuration Release --project Calc4DotNet
```

## Example

Calc4 works as REPL. Please input what you want to evaluate.

```
$ dotnet run --configuration Release --project Calc4DotNet
Calc4 REPL
    Integer type: System.Int64
    Executor: JIT
    Optimize: on

> 1+2

3
Elapsed: 00:00:00.0778310

> D[fib|n|n <= 1? n ? (n-1){fib} + (n-2){fib}] 38{fib}

39088169
Elapsed: 00:00:00.3669183
```

## Information about Calc4

Further information is available at [C++ implementation's repository](https://github.com/proprowataya/calc4). This repository contains a detailed description of the language and [execution environment on a web browser](https://proprowataya.github.io/calc4).
