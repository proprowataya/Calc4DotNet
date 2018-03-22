using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Calc4DotNet.Core.Execution;

namespace Calc4DotNet.Core.ILCompilation
{
    public static class ILCompiler
    {
        private static readonly AssemblyName AsmName = new AssemblyName("Calc4Assembly");
        private const string ClassName = "<Calc4Implement>";
        private const string RunMethodName = nameof(ICompiledModule<object>.Run);

        public static ICompiledModule<TNumber> Compile<TNumber>(Module<TNumber> module)
        {
            AssemblyBuilder assemblyBuilder
                = AssemblyBuilder.DefineDynamicAssembly(AsmName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder
                = assemblyBuilder.DefineDynamicModule(AsmName.Name);
            TypeBuilder typeBuilder
                = moduleBuilder.DefineType(ClassName,
                                           TypeAttributes.Class | TypeAttributes.Public,
                                           typeof(object),
                                           new[] { typeof(ICompiledModule<TNumber>) });
            // Define Run Method
            MethodBuilder runMethod
                = typeBuilder.DefineMethod(nameof(ICompiledModule<TNumber>.Run),
                                           MethodAttributes.Public | MethodAttributes.Virtual,
                                           typeof(TNumber),
                                           new[] { typeof(Context<TNumber>) });
            typeBuilder.DefineMethodOverride(runMethod,
                                             typeof(ICompiledModule<TNumber>).GetMethod(nameof(ICompiledModule<TNumber>.Run)));

            // User defined methods
            var methods = new Dictionary<int, (MethodBuilder Method, int NumOperands)>();
            foreach (var op in module.UserDefinedOperators)
            {
                MethodBuilder methodBuilder
                    = typeBuilder.DefineMethod(op.Definition.Name,
                                               MethodAttributes.Public | MethodAttributes.Static,
                                               typeof(TNumber),
                                               Enumerable.Repeat(typeof(TNumber), op.Definition.NumOperands).ToArray());
                methods[op.StartAddress] = (methodBuilder, op.Definition.NumOperands);
            }

            EmitIL(module, runMethod, methods);
            Type type = typeBuilder.CreateType();
            return (ICompiledModule<TNumber>)Activator.CreateInstance(type);
        }

        private static void EmitIL<TNumber>(Module<TNumber> module, MethodBuilder runMethod, Dictionary<int, (MethodBuilder Method, int NumOperands)> methods)
        {
            ReadOnlySpan<LowLevelOperation> operations = module.Operations.ToArray().AsReadOnlySpan();

            // Emit Main(Run) operator
            int mainOperatorLength = module.UserDefinedOperators.IsDefaultOrEmpty
                                     ? operations.Length : module.UserDefinedOperators[0].StartAddress;
            EmitILCore<TNumber>(module, operations.Slice(0, mainOperatorLength), 0, runMethod, 0, methods);

            // Emit user-defined operators
            foreach (var op in module.UserDefinedOperators)
            {
                EmitILCore<TNumber>(module, operations.Slice(op.StartAddress, op.Length),
                                    op.StartAddress, methods[op.StartAddress].Method, methods[op.StartAddress].NumOperands, methods);
            }
        }

        private static void EmitILCore<TNumber>(Module<TNumber> module, ReadOnlySpan<LowLevelOperation> operations, int firstOriginalAddress, MethodBuilder method, int numOperands, Dictionary<int, (MethodBuilder Method, int NumOperands)> methods)
        {
            /* Local method */
            int RestoreMethodParameterIndex(int value) => numOperands - value;

            ILGenerator il = method.GetILGenerator();
            Dictionary<int, Label> labels = new Dictionary<int, Label>();
            for (int i = 0; i < operations.Length; i++)
            {
                labels[firstOriginalAddress + i] = il.DefineLabel();
            }

            for (int i = 0; i < operations.Length; i++)
            {
                il.MarkLabel(labels[firstOriginalAddress + i]);
                LowLevelOperation op = operations[i];

                switch (op.Opcode)
                {
                    case Opcode.Push:
                        il.EmitLdc((TNumber)(dynamic)0);
                        break;
                    case Opcode.Pop:
                        il.Emit(OpCodes.Pop);
                        break;
                    case Opcode.LoadConst:
                        il.EmitLdc((TNumber)(dynamic)op.Value);
                        break;
                    case Opcode.LoadConstTable:
                        il.EmitLdc((TNumber)(dynamic)module.ConstTable[op.Value]);
                        break;
                    case Opcode.LoadArg:
                        il.EmitLdarg(RestoreMethodParameterIndex(op.Value));
                        break;
                    case Opcode.StoreArg:
                        il.EmitStarg(RestoreMethodParameterIndex(op.Value));
                        break;
                    case Opcode.Input:
                        throw new NotImplementedException();
                    case Opcode.Add:
                        il.Emit(OpCodes.Add);
                        break;
                    case Opcode.Sub:
                        il.Emit(OpCodes.Sub);
                        break;
                    case Opcode.Mult:
                        il.Emit(OpCodes.Mul);
                        break;
                    case Opcode.Div:
                        il.Emit(OpCodes.Div);
                        break;
                    case Opcode.Mod:
                        il.Emit(OpCodes.Rem);
                        break;
                    case Opcode.Equal:
                        il.EmitComparison<TNumber>(OpCodes.Beq);
                        break;
                    case Opcode.NotEqual:
                        il.EmitComparison<TNumber>(OpCodes.Bne_Un);
                        break;
                    case Opcode.LessThan:
                        il.EmitComparison<TNumber>(OpCodes.Blt);
                        break;
                    case Opcode.LessThanOrEqual:
                        il.EmitComparison<TNumber>(OpCodes.Ble);
                        break;
                    case Opcode.GreaterThanOrEqual:
                        il.EmitComparison<TNumber>(OpCodes.Bge);
                        break;
                    case Opcode.GreaterThan:
                        il.EmitComparison<TNumber>(OpCodes.Bgt);
                        break;
                    case Opcode.Goto:
                        il.Emit(OpCodes.Br, labels[op.Value + 1]);
                        break;
                    case Opcode.GotoIfTrue:
                        il.Emit(OpCodes.Brtrue, labels[op.Value + 1]);
                        break;
                    case Opcode.GotoIfFalse:
                        il.Emit(OpCodes.Brfalse, labels[op.Value + 1]);
                        break;
                    case Opcode.GotoIfEqual:
                        il.Emit(OpCodes.Beq, labels[op.Value + 1]);
                        break;
                    case Opcode.GotoIfLessThan:
                        il.Emit(OpCodes.Blt, labels[op.Value + 1]);
                        break;
                    case Opcode.GotoIfLessThanOrEqual:
                        il.Emit(OpCodes.Ble, labels[op.Value + 1]);
                        break;
                    case Opcode.Call:
                        il.Emit(OpCodes.Call, methods[op.Value + 1].Method);
                        break;
                    case Opcode.Return:
                    case Opcode.Halt:
                        il.Emit(OpCodes.Ret);
                        break;
                    case Opcode.Lavel:
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
