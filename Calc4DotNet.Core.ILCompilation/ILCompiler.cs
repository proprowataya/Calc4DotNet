﻿using System.Collections.Immutable;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;

namespace Calc4DotNet.Core.ILCompilation;

public static class ILCompiler
{
    private static readonly AssemblyName AsmName = new AssemblyName("Calc4Assembly");
    private const string ClassName = "<Calc4Implement>";
    private const string RunMethodName = nameof(ICompiledModule<int>.Run);

    public static ICompiledModule<TNumber> Compile<TNumber>(LowLevelModule<TNumber> module)
        where TNumber : INumber<TNumber>
    {
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(AsmName, AssemblyBuilderAccess.Run);
        var (_, typeBuilder) = CompileToAssembly(module, assemblyBuilder);
        Type type = typeBuilder.CreateType()!;
        return (ICompiledModule<TNumber>)Activator.CreateInstance(type)!;
    }

    public static (ModuleBuilder ModuleBuilder, TypeBuilder TypeBuilder) CompileToAssembly<TNumber>(LowLevelModule<TNumber> module, AssemblyBuilder assemblyBuilder)
        where TNumber : INumber<TNumber>
    {
        ModuleBuilder moduleBuilder
            = assemblyBuilder.DefineDynamicModule(AsmName.Name!);
        TypeBuilder typeBuilder
            = moduleBuilder.DefineType(ClassName,
                                       TypeAttributes.Class | TypeAttributes.Public,
                                       typeof(object),
                                       new[] { typeof(ICompiledModule<TNumber>) });
        // Define variables
        FieldBuilder[] fieldBuilders = new FieldBuilder[module.Variables.Length];
        for (int i = 0; i < fieldBuilders.Length; i++)
        {
            fieldBuilders[i] = typeBuilder.DefineField(GetVariableIlName(module.Variables[i], i), typeof(TNumber), FieldAttributes.Private | FieldAttributes.Static);
        }
        // Define Run Method
        MethodBuilder runMethod
            = typeBuilder.DefineMethod(nameof(ICompiledModule<TNumber>.Run),
                                       MethodAttributes.Public | MethodAttributes.Virtual,
                                       typeof(TNumber),
                                       new[] { typeof(IEvaluationState<TNumber>) });
        typeBuilder.DefineMethodOverride(runMethod,
                                         typeof(ICompiledModule<TNumber>).GetMethod(nameof(ICompiledModule<TNumber>.Run))!);

        // User defined methods
        var methods = new (MethodBuilder Method, int NumOperands)[module.UserDefinedOperators.Length];
        for (int i = 0; i < module.UserDefinedOperators.Length; i++)
        {
            var op = module.UserDefinedOperators[i];
            MethodBuilder methodBuilder
                = typeBuilder.DefineMethod(op.Definition.Name,
                                           MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig,
                                           typeof(TNumber),
                                           Enumerable.Repeat(typeof(TNumber), op.Definition.NumOperands).Append(typeof(IEvaluationState<TNumber>)).ToArray());
            methods[i] = (methodBuilder, op.Definition.NumOperands);
        }

        EmitIL(module, fieldBuilders, runMethod, methods);
        return (moduleBuilder, typeBuilder);
    }

    private static void EmitIL<TNumber>(LowLevelModule<TNumber> module, FieldBuilder[] fieldBuilders, MethodBuilder runMethod, (MethodBuilder Method, int NumOperands)[] methods)
        where TNumber : INumber<TNumber>
    {
        // Emit Main(Run) operator
        EmitILCore(module, module.EntryPoint, fieldBuilders, runMethod, 0, methods, isMain: true);

        // Emit user-defined operators
        for (int i = 0; i < module.UserDefinedOperators.Length; i++)
        {
            EmitILCore(module, module.UserDefinedOperators[i].Operations, fieldBuilders, methods[i].Method, methods[i].NumOperands, methods, isMain: false);
        }
    }

    private static void EmitILCore<TNumber>(LowLevelModule<TNumber> module, ImmutableArray<LowLevelOperation> operations, FieldBuilder[] fieldBuilders, MethodBuilder method, int numOperands, (MethodBuilder Method, int NumOperands)[] methods, bool isMain)
        where TNumber : INumber<TNumber>
    {
        /* Locals */
        ILGenerator il = method.GetILGenerator();
        Dictionary<int, Label> labels = new Dictionary<int, Label>();
        Label methodEnd = il.DefineLabel();
        LocalBuilder? value = null, index = null, character = null;
        int stateIndex = numOperands + (isMain ? 1 : 0);

        /* Local method */
        int RestoreMethodParameterIndex(int value) => numOperands - value;

        void EmitLoadArraySource()
        {
            il.Emit(OpCodes.Ldarg_S, stateIndex);
            il.Emit(OpCodes.Callvirt, typeof(IEvaluationState<TNumber>).GetProperty(nameof(IEvaluationState<TNumber>.GlobalArray))!.GetGetMethod()!);
        }

        void EmitLoadIOService()
        {
            il.Emit(OpCodes.Ldarg_S, stateIndex);
            il.Emit(OpCodes.Callvirt, typeof(IEvaluationState<TNumber>).GetProperty(nameof(IEvaluationState<TNumber>.IOService))!.GetGetMethod()!);
        }

        /* Start emit code */

        for (int i = 0; i < operations.Length; i++)
        {
            labels[i] = il.DefineLabel();
        }

        if (isMain && module.Variables.Length > 0)
        {
            // Restore all variables from state
            il.Emit(OpCodes.Ldarg_S, stateIndex);
            il.Emit(OpCodes.Callvirt, typeof(IEvaluationState<TNumber>).GetProperty(nameof(IEvaluationState<TNumber>.Variables))!.GetGetMethod()!);

            for (int i = 0; i < module.Variables.Length; i++)
            {
                if (i < module.Variables.Length - 1)
                {
                    il.Emit(OpCodes.Dup);
                }

                if (module.Variables[i] is string notnullName)
                {
                    il.Emit(OpCodes.Ldstr, notnullName);
                }
                else
                {
                    il.Emit(OpCodes.Ldnull);
                }

                il.Emit(OpCodes.Callvirt, typeof(IVariableSource<TNumber>).GetMethod("get_Item", new[] { typeof(string) })!);
                il.Emit(OpCodes.Stsfld, fieldBuilders[i]);
            }
        }

        for (int i = 0; i < operations.Length; i++)
        {
            il.MarkLabel(labels[i]);
            LowLevelOperation op = operations[i];

            switch (op.Opcode)
            {
                case Opcode.Push:
                    il.Emit(OpCodes.Constrained, typeof(TNumber));
                    il.Emit(OpCodes.Call, typeof(TNumber).GetProperty(nameof(TNumber.Zero))!.GetMethod!);
                    break;
                case Opcode.Pop:
                    il.Emit(OpCodes.Pop);
                    break;
                case Opcode.LoadConst:
                    il.EmitLdc(TNumber.CreateTruncating(op.Value));
                    break;
                case Opcode.LoadConstTable:
                    il.EmitLdc(module.ConstTable[op.Value]);
                    break;
                case Opcode.LoadArg:
                    il.EmitLdarg(RestoreMethodParameterIndex(op.Value));
                    break;
                case Opcode.StoreArg:
                    il.EmitStarg(RestoreMethodParameterIndex(op.Value));
                    break;
                case Opcode.LoadVariable:
                    il.Emit(OpCodes.Ldsfld, fieldBuilders[op.Value]);
                    break;
                case Opcode.StoreVariable:
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Stsfld, fieldBuilders[op.Value]);
                    break;
                case Opcode.LoadArrayElement:
                    index ??= il.DeclareLocal(typeof(TNumber));
                    il.Emit(OpCodes.Stloc_S, index.LocalIndex);

                    EmitLoadArraySource();
                    il.Emit(OpCodes.Ldloc_S, index.LocalIndex);
                    il.Emit(OpCodes.Callvirt, typeof(IArraySource<TNumber>).GetMethod("get_Item", new[] { typeof(TNumber) })!);
                    break;
                case Opcode.StoreArrayElement:
                    value ??= il.DeclareLocal(typeof(TNumber));
                    index ??= il.DeclareLocal(typeof(TNumber));

                    il.Emit(OpCodes.Stloc_S, index.LocalIndex);
                    il.Emit(OpCodes.Stloc_S, value.LocalIndex);

                    EmitLoadArraySource();
                    il.Emit(OpCodes.Ldloc_S, index.LocalIndex);
                    il.Emit(OpCodes.Ldloc_S, value.LocalIndex);
                    il.Emit(OpCodes.Callvirt, typeof(IArraySource<TNumber>).GetMethod("set_Item", new[] { typeof(TNumber), typeof(TNumber) })!);
                    il.Emit(OpCodes.Ldloc_S, value.LocalIndex);
                    break;
                case Opcode.Input:
                    EmitLoadIOService();
                    il.Emit(OpCodes.Callvirt, typeof(IIOService).GetMethod(nameof(IIOService.GetChar))!);
                    il.EmitConvToINumber<int, TNumber>();
                    break;
                case Opcode.PrintChar:
                    character ??= il.DeclareLocal(typeof(char));
                    il.EmitConvToINumber<TNumber, char>();
                    il.Emit(OpCodes.Stloc_S, character.LocalIndex);

                    EmitLoadIOService();
                    il.Emit(OpCodes.Ldloc_S, character.LocalIndex);
                    il.Emit(OpCodes.Callvirt, typeof(IIOService).GetMethod(nameof(IIOService.PrintChar))!);
                    il.EmitLdc(TNumber.Zero);
                    break;
                case Opcode.Add:
                    il.Emit(OpCodes.Constrained, typeof(TNumber));
                    il.Emit(OpCodes.Call, GetInterfaceMethod(typeof(IAdditionOperators<TNumber, TNumber, TNumber>), "op_Addition"));
                    break;
                case Opcode.Sub:
                    il.Emit(OpCodes.Constrained, typeof(TNumber));
                    il.Emit(OpCodes.Call, GetInterfaceMethod(typeof(ISubtractionOperators<TNumber, TNumber, TNumber>), "op_Subtraction"));
                    break;
                case Opcode.Mult:
                    il.Emit(OpCodes.Constrained, typeof(TNumber));
                    il.Emit(OpCodes.Call, GetInterfaceMethod(typeof(IMultiplyOperators<TNumber, TNumber, TNumber>), "op_Multiply"));
                    break;
                case Opcode.Div:
                case Opcode.Mod:
                    il.Emit(OpCodes.Constrained, typeof(TNumber));
                    il.Emit(OpCodes.Call,
                            op.Opcode switch
                            {
                                Opcode.Div => GetInterfaceMethod(typeof(IDivisionOperators<TNumber, TNumber, TNumber>), "op_Division"),
                                Opcode.Mod => GetInterfaceMethod(typeof(IModulusOperators<TNumber, TNumber, TNumber>), "op_Modulus"),
                                _ => throw new InvalidOperationException(),
                            });
                    break;
                case Opcode.DivChecked:
                case Opcode.ModChecked:
                    {
                        Label whenNotZero = il.DefineLabel();

                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Constrained, typeof(TNumber));
                        il.Emit(OpCodes.Call, GetInterfacePropertyGetter(typeof(INumberBase<TNumber>), nameof(TNumber.Zero)));
                        il.Emit(OpCodes.Constrained, typeof(TNumber));
                        il.Emit(OpCodes.Call, GetInterfaceMethod(typeof(IEqualityOperators<TNumber, TNumber, bool>), "op_Equality"));
                        il.Emit(OpCodes.Brfalse_S, whenNotZero);

                        // Throw ZeroDivisionException
                        il.Emit(OpCodes.Newobj, typeof(Calc4DotNet.Core.Exceptions.ZeroDivisionException).GetConstructor([])!);
                        il.Emit(OpCodes.Throw);

                        // Operate
                        il.MarkLabel(whenNotZero);
                        il.Emit(OpCodes.Constrained, typeof(TNumber));
                        il.Emit(OpCodes.Call,
                                op.Opcode switch
                                {
                                    Opcode.DivChecked => GetInterfaceMethod(typeof(IDivisionOperators<TNumber, TNumber, TNumber>), "op_Division"),
                                    Opcode.ModChecked => GetInterfaceMethod(typeof(IModulusOperators<TNumber, TNumber, TNumber>), "op_Modulus"),
                                    _ => throw new InvalidOperationException(),
                                });
                        break;
                    }
                case Opcode.Goto:
                    il.Emit(OpCodes.Br, labels[op.Value + 1]);
                    break;
                case Opcode.GotoIfTrue:
                    il.Emit(OpCodes.Constrained, typeof(TNumber));
                    il.Emit(OpCodes.Call, GetInterfaceMethod(typeof(INumberBase<TNumber>), nameof(INumberBase<TNumber>.IsZero)));
                    il.Emit(OpCodes.Brfalse, labels[op.Value + 1]);
                    break;
                case Opcode.GotoIfEqual:
                    il.Emit(OpCodes.Constrained, typeof(TNumber));
                    il.Emit(OpCodes.Call, GetInterfaceMethod(typeof(IEqualityOperators<TNumber, TNumber, bool>), "op_Equality"));
                    il.Emit(OpCodes.Brtrue, labels[op.Value + 1]);
                    break;
                case Opcode.GotoIfLessThan:
                    il.Emit(OpCodes.Constrained, typeof(TNumber));
                    il.Emit(OpCodes.Call, GetInterfaceMethod(typeof(IComparisonOperators<TNumber, TNumber, bool>), "op_LessThan"));
                    il.Emit(OpCodes.Brtrue, labels[op.Value + 1]);
                    break;
                case Opcode.GotoIfLessThanOrEqual:
                    il.Emit(OpCodes.Constrained, typeof(TNumber));
                    il.Emit(OpCodes.Call, GetInterfaceMethod(typeof(IComparisonOperators<TNumber, TNumber, bool>), "op_LessThanOrEqual"));
                    il.Emit(OpCodes.Brtrue, labels[op.Value + 1]);
                    break;
                case Opcode.Call:
                    il.Emit(OpCodes.Ldarg_S, stateIndex);
                    il.Emit(OpCodes.Call, methods[op.Value].Method);
                    break;
                case Opcode.Return:
                    il.Emit(OpCodes.Ret);
                    break;
                case Opcode.Halt:
                    if (!isMain)
                    {
                        throw new InvalidOperationException();
                    }
                    il.Emit(OpCodes.Br, methodEnd);
                    break;
                case Opcode.Lavel:
                default:
                    throw new InvalidOperationException();
            }
        }

        if (isMain)
        {
            il.MarkLabel(methodEnd);

            if (module.Variables.Length > 0)
            {
                // Save all variables to state
                il.Emit(OpCodes.Ldarg_S, stateIndex);
                il.Emit(OpCodes.Callvirt, typeof(IEvaluationState<TNumber>).GetProperty(nameof(IEvaluationState<TNumber>.Variables))!.GetGetMethod()!);

                for (int i = 0; i < module.Variables.Length; i++)
                {
                    if (i < module.Variables.Length - 1)
                    {
                        il.Emit(OpCodes.Dup);
                    }

                    if (module.Variables[i] is string notnullName)
                    {
                        il.Emit(OpCodes.Ldstr, notnullName);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldnull);
                    }

                    il.Emit(OpCodes.Ldsfld, fieldBuilders[i]);
                    il.Emit(OpCodes.Callvirt, typeof(IVariableSource<TNumber>).GetMethod("set_Item", new[] { typeof(string), typeof(TNumber) })!);
                }
            }

            il.Emit(OpCodes.Ret);
        }

        static MethodInfo GetInterfaceMethod(Type typeOfInterface, string methodName)
        {
            return ReflectionHelper.GetInterfaceMethod(typeof(TNumber), typeOfInterface, methodName);
        }

        static MethodInfo GetInterfacePropertyGetter(Type typeOfInterface, string propertyName)
        {
            return ReflectionHelper.GetInterfacePropertyGetter(typeof(TNumber), typeOfInterface, propertyName);
        }
    }

    private static string GetVariableIlName(string? name, int index)
    {
        return $"Field_{index}_{name ?? "default"}";
    }
}
