using System.Diagnostics;
using System.Numerics;
using Calc4DotNet.Compiler;
using Calc4DotNet.Core.Exceptions;

try
{
    var (setting, sourcePath, printHelp) = CommandLineArgsParser.Parse(args);

    if (printHelp)
    {
        Console.WriteLine(CommandLineArgsParser.GetHelp());
        return 0;
    }

    Debug.Assert(sourcePath is not null);

    if (setting.NumberType == typeof(Int32))
    {
        CompilerImplementation.Run<Int32>(sourcePath, setting);
    }
    else if (setting.NumberType == typeof(Int64))
    {
        CompilerImplementation.Run<Int64>(sourcePath, setting);
    }
    else if (setting.NumberType == typeof(Int128))
    {
        CompilerImplementation.Run<Int128>(sourcePath, setting);
    }
    else if (setting.NumberType == typeof(BigInteger))
    {
        CompilerImplementation.Run<BigInteger>(sourcePath, setting);
    }
    else
    {
        throw new InvalidOperationException($"Type {setting.NumberType} is not supported.");
    }

    return 0;
}
catch (Exception e) when (e is CommandLineArgsParseException or Calc4Exception)
{
    Console.Error.WriteLine($"Error: {e.Message}");
    return 1;
}
catch (Exception e)
{
    Console.Error.WriteLine($"Fatal error: {e.Message}");
    return 1;
}
