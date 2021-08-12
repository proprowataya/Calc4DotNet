using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Calc4DotNet.Core.Execution
{
    public sealed class LowLevelModule<TNumber>
        where TNumber : INumber<TNumber>
    {
        public ImmutableArray<LowLevelOperation> EntryPoint { get; }
        public ImmutableArray<TNumber> ConstTable { get; }
        public ImmutableArray<LowLevelUserDefinedOperator> UserDefinedOperators { get; }

        public LowLevelModule(ImmutableArray<LowLevelOperation> entryPoint, ImmutableArray<TNumber> constTable, ImmutableArray<LowLevelUserDefinedOperator> userDefinedOperators)
        {
            EntryPoint = entryPoint;
            ConstTable = constTable;
            UserDefinedOperators = userDefinedOperators;
        }

        public (LowLevelOperation[] Operations, int[] MaxStackSizes) FlattenOperations()
        {
            int totalNumOperations = EntryPoint.Length + UserDefinedOperators.Sum(t => t.Operations.Length);
            LowLevelOperation[] result = new LowLevelOperation[totalNumOperations];
            int[] maxStackSizes = new int[totalNumOperations];
            int[] startAddresses = new int[UserDefinedOperators.Length];

            int index = 0;

            for (int i = 0; i < EntryPoint.Length; i++)
            {
                result[index++] = EntryPoint[i];
            }

            for (int i = 0; i < UserDefinedOperators.Length; i++)
            {
                int startAddress = index;
                startAddresses[i] = startAddress;

                var operations = UserDefinedOperators[i].Operations;
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
                        int operatorNo = result[i].Value;
                        result[i] = new LowLevelOperation(result[i].Opcode, startAddresses[operatorNo] - 1);
                        maxStackSizes[result[i].Value] = UserDefinedOperators[operatorNo].MaxStackSize;
                        break;
                    default:
                        break;
                }
            }

            return (result, maxStackSizes);
        }

        public void Serialize(Stream stream)
        {
            LowLevelOperation[] operations = FlattenOperations().Operations;
            stream.Write(MemoryMarshal.AsBytes(operations.AsSpan()));
        }
    }
}
