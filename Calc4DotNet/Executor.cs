namespace Calc4DotNet;

internal sealed class Executor<TNumber> : Calc4Base<TNumber>
    where TNumber : notnull
{
    private readonly string[] sourcePaths;

    public Executor(Setting setting, string[] sourcePaths) : base(setting)
    {
        this.sourcePaths = sourcePaths;
    }

    public override void Run()
    {
        foreach (var path in sourcePaths)
        {
            string text = File.ReadAllText(path);
            Execute(text);
        }
    }
}
