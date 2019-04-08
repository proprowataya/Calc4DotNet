using System;
using System.IO;

namespace Calc4DotNet.Core.Execution
{
    public static class LowLevelSerializer
    {
        public unsafe static void Serialize<TNumber>(this LowLevelModule<TNumber> module, Stream stream)
        {
            LowLevelOperation[] operators = module.FlattenOperators();
            fixed (LowLevelOperation* operation = operators)
            {
                stream.Write(new ReadOnlySpan<byte>(operation, operators.Length * sizeof(LowLevelOperation)));
            }
        }
    }
}
