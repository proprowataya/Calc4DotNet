using System.Numerics;
using System.Runtime.CompilerServices;

namespace Calc4DotNet;

internal static class CommandLineArgs
{
    public const string Help = "--help";
    public const string EnableJit = "--enable-jit";
    public const string DisableJit = "--disable-jit";
    public const string IntegerSize = "--size";
    public const string IntegerSizeShort = "-s";
    public const string EnableOptimization = "-O1";
    public const string DisableOptimization = "-O0";
    public const string DumpProgram = "--dump";
    public const string DoubleFloatingPoint = "double";
    public const string InfinitePrecisionInteger = "inf";
}

internal static class CommandLineArgsParser
{
    private const string HelpText =
$@"Calc4 REPL

Calc4DotNet [options] [files]

Options:
{CommandLineArgs.IntegerSize}|{CommandLineArgs.IntegerSizeShort} <size>
    Specify the size of integer
    size: 32, 64 (default), 128, inf (meaning infinite-precision or arbitrary-precision)
{CommandLineArgs.DisableJit}
    Disable JIT compilation
{CommandLineArgs.EnableJit}
    Enable JIT compilation (default)
{CommandLineArgs.DisableOptimization}
    Disable optimization
{CommandLineArgs.EnableOptimization}
    Enable optimization (default)
{CommandLineArgs.DumpProgram}
    Dump the given program's structures such as an abstract syntax tree";

    public static string GetHelp() => HelpText;

    public static (Setting Setting, string[] SourcePaths, bool PrintHelp) Parse(string[] args)
    {
        Type numberType = typeof(Int64);
        ExecutorType executorType = RuntimeFeature.IsDynamicCodeCompiled ? ExecutorType.JIT : ExecutorType.LowLevel;
        bool optimize = true;
        bool dump = false;
        List<string> sourcePaths = new();
        bool printHelp = false;

        for (int i = 0; i < args.Length; i++)
        {
            string GetNextArgument()
            {
                if (i + 1 >= args.Length)
                {
                    throw new CommandLineArgsParseException($"Option \"{args[i]}\" requires an argument.");
                }

                return args[++i];
            }

            switch (args[i])
            {
                case CommandLineArgs.Help:
                    printHelp = true;
                    break;
                case CommandLineArgs.EnableJit:
                    if (!RuntimeFeature.IsDynamicCodeCompiled)
                    {
                        throw new CommandLineArgsParseException("JIT compiler is not supported on this platform.");
                    }
                    executorType = ExecutorType.JIT;
                    break;
                case CommandLineArgs.DisableJit:
                    executorType = ExecutorType.LowLevel;
                    break;
                case CommandLineArgs.IntegerSize:
                case CommandLineArgs.IntegerSizeShort:
                    numberType = GetNextArgument() switch
                    {
                        "32" => typeof(Int32),
                        "64" => typeof(Int64),
                        "128" => typeof(Int128),
                        CommandLineArgs.DoubleFloatingPoint => typeof(Double),
                        CommandLineArgs.InfinitePrecisionInteger => typeof(BigInteger),
                        var arg => throw new CommandLineArgsParseException($"Type \"{arg}\" is not supported."),
                    };
                    break;
                case CommandLineArgs.EnableOptimization:
                    optimize = true;
                    break;
                case CommandLineArgs.DisableOptimization:
                    optimize = false;
                    break;
                case CommandLineArgs.DumpProgram:
                    dump = true;
                    break;
                default:
                    sourcePaths.Add(args[i]);
                    break;
            }
        }

        return (new Setting(numberType, executorType, optimize, dump), sourcePaths.ToArray(), printHelp);
    }
}

internal sealed class CommandLineArgsParseException : Exception
{
    public CommandLineArgsParseException(string message) : base(message)
    { }
}
