using System.Diagnostics;
using System.Linq;

namespace Calc4DotNet.Core.Execution
{
    public static class LowLevelOperationFlattener
    {
        public static LowLevelOperation[] FlattenOperators<TNumber>(this LowLevelModule<TNumber> module)
        {
            int totalNumOperations = module.EntryPoint.Length + module.UserDefinedOperators.Sum(t => t.Operations.Length);
            LowLevelOperation[] result = new LowLevelOperation[totalNumOperations];
            int[] startAddresses = new int[module.UserDefinedOperators.Length];

            int index = 0;

            for (int i = 0; i < module.EntryPoint.Length; i++)
            {
                result[index++] = module.EntryPoint[i];
            }

            for (int i = 0; i < module.UserDefinedOperators.Length; i++)
            {
                int startAddress = index;
                startAddresses[i] = startAddress;

                var operations = module.UserDefinedOperators[i].Operations;
                for (int j = 0; j < operations.Length; j++)
                {
                    result[index] = operations[j];

                    // Resolve labels
                    switch (result[index].Opcode)
                    {
                        case Opcode.Goto:
                        case Opcode.GotoIfTrue:
                        case Opcode.GotoIfEqual:
                        case Opcode.GotoIfLessThan:
                        case Opcode.GotoIfLessThanOrEqual:
                            result[index] = new LowLevelOperation(result[index].Opcode,
                                                                  result[index].Value + startAddress);
                            break;
                        default:
                            break;
                    }

                    index++;
                }
            }

            Debug.Assert(index == result.Length);

            // Resolve call operations
            for (int i = 0; i < result.Length; i++)
            {
                switch (result[i].Opcode)
                {
                    case Opcode.Call:
                        result[i] = new LowLevelOperation(result[i].Opcode, startAddresses[result[i].Value] - 1);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }
}
