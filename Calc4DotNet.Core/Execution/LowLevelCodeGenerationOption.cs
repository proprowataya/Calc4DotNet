namespace Calc4DotNet.Core.Execution;

public sealed record LowLevelCodeGenerationOption
{
    public static readonly LowLevelCodeGenerationOption Default = new LowLevelCodeGenerationOption();

    public bool CheckZeroDivision { get; init; } = true;
}
