using System;

namespace Calc4DotNet
{
    internal sealed class Setting
    {
        public Type NumberType { get; }
        public ExecutorType ExecutorType { get; }
        public bool Optimize { get; }
        public bool PrintDetailInformation { get; }

        public Setting(Type numberType, ExecutorType executorType, bool optimize, bool printDetailInformation)
        {
            NumberType = numberType;
            ExecutorType = executorType;
            Optimize = optimize;
            PrintDetailInformation = printDetailInformation;
        }
    }

    internal enum ExecutorType
    {
        LowLevel, JIT,
    }
}
