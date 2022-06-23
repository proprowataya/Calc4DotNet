using System.Numerics;

namespace Calc4DotNet;

internal class Repl<TNumber> : Calc4Base<TNumber>
    where TNumber : INumber<TNumber>
{
    public Repl(Setting setting) : base(setting)
    { }

    public override void Run()
    {
        Console.WriteLine("Calc4 Interpreter");
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
