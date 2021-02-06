# Operator Only Language "Calc4"

Calc4 is a programming language where everything in its code is an operator. This repository contains calc4's environment which is implemented by C#. C++ version is [here](https://github.com/proprowataya/calc4).

## Requirements

[.NET](https://dotnet.microsoft.com/) (>= 5.0)

## Getting started

```
git clone https://github.com/proprowataya/Calc4DotNet.git
cd Calc4DotNet
dotnet run --configuration Release --project Calc4DotNet
```

## Example

Calc4 works as REPL. Please input what you want to evaluate.

```
$ dotnet run --configuration Release --project Calc4DotNet
Calc4 Interpreter

> 1+2

3
Elapsed: 00:00:00.1166434

> D[fib|n|n <= 1? n ? (n-1){fib} + (n-2){fib}] 38{fib}

39088169
Elapsed: 00:00:00.3021941
```

## Sample codes and information about calc4

See [this repository](https://github.com/proprowataya/calc4#sample-codes).
