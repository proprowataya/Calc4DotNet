using System.Numerics;

namespace Calc4DotNet;

internal enum ExecutorType
{
    LowLevel, JIT,
}

internal sealed class CommandLineArgsParseException : Exception
{
    public CommandLineArgsParseException(string message) : base(message)
    { }
}

internal sealed record Setting(Type NumberType, ExecutorType ExecutorType, bool Optimize, bool PrintDetailInformation)
{
    public static (Setting Setting, string[] SourcePaths) ParseCommandLineArgs(string[] args)
    {
        Type numberType = typeof(Int64);
        ExecutorType executorType = ExecutorType.JIT;
        bool optimize = true;
        bool printDetailInformation = false;
        List<string> sourcePaths = new();

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--type":
                case "-t":
                    // Specify number type
                    if (i + 1 >= args.Length)
                    {
                        throw new CommandLineArgsParseException($"Option {args[i]} requires a parameter.");
                    }

                    numberType = (args[i + 1]) switch
                    {
                        "int32" => typeof(Int32),
                        "int64" => typeof(Int64),
                        "double" => typeof(Double),
                        "bigint" or "biginteger" => typeof(BigInteger),
                        var arg => throw new CommandLineArgsParseException($"Type {arg} is not supported."),
                    };

                    i++;
                    break;
                case "-o":
                    optimize = true;
                    break;
                case "-od":
                    optimize = false;
                    break;
                case "--detail":
                    printDetailInformation = true;
                    break;
                case "--low-level":
                    executorType = ExecutorType.LowLevel;
                    break;
                default:
                    sourcePaths.Add(args[i]);
                    break;
            }
        }

        return (new Setting(numberType, executorType, optimize, printDetailInformation), sourcePaths.ToArray());
    }
}
