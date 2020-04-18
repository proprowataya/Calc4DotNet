using System;

namespace Calc4DotNet
{
    internal sealed class Setting
    {
        public Type NumberType { get; }
        public bool Optimize { get; }
        public bool PrintDetailInformation { get; }

        public Setting(Type numberType, bool optimize, bool printDetailInformation)
        {
            NumberType = numberType;
            Optimize = optimize;
            PrintDetailInformation = printDetailInformation;
        }
    }
}
