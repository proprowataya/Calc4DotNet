using Calc4DotNetCoreNativeExecutor;

namespace Calc4DotNet.Core.Execution
{
    public static class CppLowLevelExecutor
    {
        public unsafe static Int32 Execute(LowLevelModule<Int32> module)
        {
            try
            {
                var (operations, maxStackSizes) = module.FlattenOperations();
                var constTable = module.ConstTable.ToArray();

                fixed (LowLevelOperation* operationsPtr = operations)
                fixed (int* maxStackSizesPtr = maxStackSizes)
                fixed (Int32* constTablePtr = constTable)
                {
                    return NativeLowLevelExecutor.Execute(operationsPtr,
                                                          maxStackSizesPtr,
                                                          operations.Length,
                                                          constTablePtr,
                                                          constTable.Length);
                }
            }
            catch (NativeStackOverflowException)
            {
                throw new Calc4DotNet.Core.Exceptions.StackOverflowException();
            }
        }

        public unsafe static Int64 Execute(LowLevelModule<Int64> module)
        {
            try
            {
                var (operations, maxStackSizes) = module.FlattenOperations();
                var constTable = module.ConstTable.ToArray();

                fixed (LowLevelOperation* operationsPtr = operations)
                fixed (int* maxStackSizesPtr = maxStackSizes)
                fixed (Int64* constTablePtr = constTable)
                {
                    return NativeLowLevelExecutor.Execute(operationsPtr,
                                                          maxStackSizesPtr,
                                                          operations.Length,
                                                          constTablePtr,
                                                          constTable.Length);
                }
            }
            catch (NativeStackOverflowException)
            {
                throw new Calc4DotNet.Core.Exceptions.StackOverflowException();
            }
        }

        public unsafe static Double Execute(LowLevelModule<Double> module)
        {
            try
            {
                var (operations, maxStackSizes) = module.FlattenOperations();
                var constTable = module.ConstTable.ToArray();

                fixed (LowLevelOperation* operationsPtr = operations)
                fixed (int* maxStackSizesPtr = maxStackSizes)
                fixed (Double* constTablePtr = constTable)
                {
                    return NativeLowLevelExecutor.Execute(operationsPtr,
                                                          maxStackSizesPtr,
                                                          operations.Length,
                                                          constTablePtr,
                                                          constTable.Length);
                }
            }
            catch (NativeStackOverflowException)
            {
                throw new Calc4DotNet.Core.Exceptions.StackOverflowException();
            }
        }
    }
}
