namespace Calc4DotNet.Core.Execution;

public sealed record LowLevelCodeGenerationOption
{
    public static LowLevelCodeGenerationOption Default = new LowLevelCodeGenerationOption();

    public bool CheckZeroDivision { get; init; } = true;
}
