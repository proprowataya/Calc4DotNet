using System.Numerics;
using Calc4DotNet.Core;

namespace Calc4DotNet;

internal static class ReplCommand
{
    public const string DumpOff = "#dump off";
    public const string DumpOn = "#dump on";
    public const string OptimizeOff = "#optimize off";
    public const string OptimizeOn = "#optimize on";
    public const string Reset = "#reset";
}

internal class Repl<TNumber> : Calc4Base<TNumber>
    where TNumber : INumber<TNumber>
{
    public Repl(Setting setting) : base(setting)
    { }

    public override void Run()
    {
        Console.WriteLine("Calc4 REPL");
        Console.WriteLine($"    Integer type: {setting.NumberType}");
        Console.WriteLine($"    Executor: {setting.ExecutorType}");
        Console.WriteLine($"    Optimize: {(setting.Optimize ? "on" : "off")}");
        Console.WriteLine();

        while (true)
        {
            Console.Write("> ");
            string? text = Console.ReadLine();
            if (text is null)
            {
                break;
            }
            Console.WriteLine();

            switch (text)
            {
                case ReplCommand.DumpOff:
                    setting = setting with { Dump = false };
                    break;
                case ReplCommand.DumpOn:
                    setting = setting with { Dump = true };
                    break;
                case ReplCommand.OptimizeOff:
                    setting = setting with { Optimize = false };
                    break;
                case ReplCommand.OptimizeOn:
                    setting = setting with { Optimize = true };
                    break;
                case ReplCommand.Reset:
                    context = CompilationContext.Empty;
                    state = CreateEvaluationState();
                    break;
                default:
                    Execute(text);
                    break;
            }
        }
    }
}
