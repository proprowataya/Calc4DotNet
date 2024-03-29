﻿using System.Numerics;
using Calc4DotNet;

Setting setting;
string[] sourcePaths;
bool printHelp;
try
{
    (setting, sourcePaths, printHelp) = CommandLineArgsParser.Parse(args);
}
catch (CommandLineArgsParseException e)
{
    Console.WriteLine($"Error: {e.Message}");
    Console.WriteLine();
    Console.WriteLine(CommandLineArgsParser.GetHelp());
    Environment.Exit(1);
    return;
}

if (printHelp)
{
    Console.WriteLine(CommandLineArgsParser.GetHelp());
    return;
}

try
{
    if (setting.NumberType == typeof(Int32))
    {
        Start<Int32>();
    }
    else if (setting.NumberType == typeof(Int64))
    {
        Start<Int64>();
    }
    else if (setting.NumberType == typeof(Int128))
    {
        Start<Int128>();
    }
    else if (setting.NumberType == typeof(Double))
    {
        Start<Double>();
    }
    else if (setting.NumberType == typeof(BigInteger))
    {
        Start<BigInteger>();
    }
    else
    {
        Console.WriteLine($"Error: Type {setting.NumberType} is not supported.");
        Console.WriteLine();
    }
}
catch (Exception e)
{
    Console.WriteLine($"Fatal error: {e.Message}");
}

void Start<TNumber>()
    where TNumber : INumber<TNumber>
{
    if (sourcePaths.Length > 0)
    {
        new Executor<TNumber>(setting, sourcePaths).Run();
    }
    else
    {
        new Repl<TNumber>(setting).Run();
    }
}
