namespace Calc4DotNet;

internal enum ExecutorType
{
    LowLevel, JIT,
}

internal sealed record Setting(Type NumberType, ExecutorType ExecutorType, bool Optimize, bool Dump);
