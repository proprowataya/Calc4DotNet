using System.Numerics;

namespace Calc4DotNet;

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
            Execute(text);
        }
    }
}
