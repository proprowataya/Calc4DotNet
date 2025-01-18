using System.Numerics;

namespace Calc4DotNet.Compiler;

internal static class CommandLineArgs
{
    public const string Help = "--help";
    public const string IntegerSize = "--size";
    public const string IntegerSizeShort = "-s";
    public const string EnableOptimization = "-O1";
    public const string DisableOptimization = "-O0";
    public const string DoubleFloatingPoint = "double";
    public const string InfinitePrecisionInteger = "inf";
}

internal static class CommandLineArgsParser
{
    private const string HelpText =
$@"Calc4 Compiler

Calc4DotNet.Compiler [options] [files]

Options:
{CommandLineArgs.IntegerSize}|{CommandLineArgs.IntegerSizeShort} <size>
    Specify the size of integer
    size: 32, 64 (default), 128, inf (meaning infinite-precision or arbitrary-precision)
{CommandLineArgs.DisableOptimization}
    Disable optimization
{CommandLineArgs.EnableOptimization}
    Enable optimization (default)";

    public static string GetHelp() => HelpText;

    public static (Setting Setting, string? SourcePath, bool PrintHelp) Parse(string[] args)
    {
        Type numberType = typeof(Int64);
        bool optimize = true;
        string? sourcePath = null;
        bool printHelp = false;

        for (var i = 0; i < args.Length; i++)
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
                default:
                    if (sourcePath is not null)
                    {
                        throw new CommandLineArgsParseException("Multiple source files are not allowed.");
                    }
                    sourcePath = args[i];
                    break;
            }
        }

        if (sourcePath is null && !printHelp)
        {
            throw new CommandLineArgsParseException("Specify a source code file to be compiled.");
        }

        return (new Setting(numberType, optimize), sourcePath, printHelp);
    }
}

internal sealed class CommandLineArgsParseException : Exception
{
    public CommandLineArgsParseException(string message) : base(message)
    { }
}
