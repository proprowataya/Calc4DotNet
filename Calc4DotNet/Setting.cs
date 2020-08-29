using System;

namespace Calc4DotNet
{
    internal sealed record Setting(Type NumberType, ExecutorType ExecutorType, bool Optimize, bool PrintDetailInformation);

    internal enum ExecutorType
    {
        LowLevel, JIT,
    }
}
