using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Exceptions;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;

namespace Calc4DotNet.Compiler;

internal static class Program
{
    private static readonly string[] ReferencingDlls = ["Calc4DotNet.Core.dll", "Calc4DotNet.Core.ILCompilation.dll"];
    private const string RuntimeConfigFileExtension = "runtimeconfig.json";
    private static readonly string RuntimeConfigFileContext =
        $$"""
        {
          "runtimeOptions": {
            "tfm": "{{GetRuntimeVersion()}}",
            "framework": {
              "name": "Microsoft.NETCore.App",
              "version": "{{GetFrameworkVersion()}}"
            }
          }
        }

        """;

    public static void Main(string[] args)
    {
        try
        {
            var (setting, sourcePath, printHelp) = CommandLineArgsParser.Parse(args);

            if (printHelp)
            {
                Console.WriteLine(CommandLineArgsParser.GetHelp());
                return;
            }

            Debug.Assert(sourcePath is not null);

            if (setting.NumberType == typeof(Int32))
            {
                Run<Int32>(sourcePath, setting);
            }
            else if (setting.NumberType == typeof(Int64))
            {
                Run<Int64>(sourcePath, setting);
            }
            else if (setting.NumberType == typeof(Int128))
            {
                Run<Int128>(sourcePath, setting);
            }
            else if (setting.NumberType == typeof(BigInteger))
            {
                Run<BigInteger>(sourcePath, setting);
            }
            else
            {
                throw new InvalidOperationException($"Type {setting.NumberType} is not supported.");
            }
        }
        catch (CommandLineArgsParseException e)
        {
            Console.Out.WriteLine($"Error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.Out.WriteLine($"Fatal error: {e.Message}");
        }
    }

    private static void Run<TNumber>(string sourcePath, Setting setting)
        where TNumber : INumber<TNumber>
    {
        string source = File.ReadAllText(sourcePath);
        string outputDirectory = Path.GetDirectoryName(sourcePath) ?? throw new InvalidOperationException();
        string outputDllPath = Path.ChangeExtension(sourcePath, "dll");

        /*
         * Compile the given code.
         */
        LowLevelModule<TNumber> module;
        try
        {
            module = Compile<TNumber>(source);
        }
        catch (Calc4Exception e)
        {
            Console.Error.WriteLine($"Error: {e.Message}");
            return;
        }

        /*
         * Generate an executable.
         */
        GenerateDll(module, outputDllPath);

        /*
         * Copy referenced assemblies.
         */
        string executableDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException();
        foreach (var dllFileName in ReferencingDlls)
        {
            File.Copy(Path.Join(executableDirectory, dllFileName), Path.Combine(outputDirectory, dllFileName), overwrite: true);
        }

        /*
         * Write .runtimeconfig.json file.
         */
        File.WriteAllText(Path.ChangeExtension(sourcePath, RuntimeConfigFileExtension), RuntimeConfigFileContext);
    }

    private static LowLevelModule<TNumber> Compile<TNumber>(string source, bool optimize = true)
        where TNumber : INumber<TNumber>
    {
        /*
         * Parse and compile the given code.
         */
        var context = CompilationContext.Empty;
        var tokens = Lexer.Lex(source, ref context);
        var op = Parser.Parse(tokens, ref context);
        if (optimize)
        {
            Optimizer.Optimize<TNumber>(ref op, ref context, OptimizeTarget.All);
        }
        var module = LowLevelCodeGenerator.Generate<TNumber>(op, context, LowLevelCodeGenerationOption.Default);

        return module;
    }

    private static void GenerateDll<TNumber>(LowLevelModule<TNumber> module, string outputDllPath, bool optimize = true)
        where TNumber : INumber<TNumber>
    {
        /*
         * Make PersistedAssemblyBuilder and compile the given module to the builder.
         */
        PersistedAssemblyBuilder assemblyBuilder
            = new(new AssemblyName("Calc4CompiledAssembly"), typeof(object).Assembly);
        CustomAttributeBuilder attributeBuilder
            = new(typeof(AssemblyConfigurationAttribute).GetConstructor([typeof(string)])!, [optimize ? "Release" : "Debug"]);
        assemblyBuilder.SetCustomAttribute(attributeBuilder);
        var (moduleBuidler, compiledModuleTypeBuilder) = ILCompiler.CompileToAssembly(module, assemblyBuilder);
        var compiledModule = compiledModuleTypeBuilder.CreateType();

        /*
         * Generate an entry point code.
         */
        var entryPointTypeBuilder = moduleBuidler.DefineType("Program");
        var entryPoint = entryPointTypeBuilder.DefineMethod("Main", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static);
        var il = entryPoint.GetILGenerator();
        il.Emit(OpCodes.Newobj, compiledModule.GetConstructor([])!);
        il.Emit(OpCodes.Newobj, typeof(DefaultVariableSource<TNumber>).GetConstructor([])!);
        il.Emit(OpCodes.Newobj, typeof(DefaultArraySource<TNumber>).GetConstructor([])!);
        il.Emit(OpCodes.Call, typeof(Console).GetProperty(nameof(Console.Out))!.GetMethod!);
        il.Emit(OpCodes.Newobj, typeof(TextWriterIOService).GetConstructor([typeof(TextWriter)])!);
        il.Emit(OpCodes.Newobj, typeof(SimpleEvaluationState<TNumber>).GetConstructor([typeof(IVariableSource<TNumber>), typeof(IArraySource<TNumber>), typeof(IIOService)])!);
        il.Emit(OpCodes.Callvirt, compiledModuleTypeBuilder.GetMethod(nameof(ICompiledModule<TNumber>.Run)!)!);
        il.Emit(OpCodes.Box, typeof(TNumber));
        il.Emit(OpCodes.Callvirt, typeof(object).GetMethod("ToString")!);
        il.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.WriteLine), [typeof(string)])!);
        il.Emit(OpCodes.Ret);
        _ = entryPointTypeBuilder.CreateType();

        /*
         * Write an executable.
         */
        var metadataBuilder
            = assemblyBuilder.GenerateMetadata(out var ilStream, out var fieldData);
        var peHeaderBuilder
            = new PEHeaderBuilder(imageCharacteristics: Characteristics.ExecutableImage);
        var peBuilder
            = new ManagedPEBuilder(header: peHeaderBuilder,
                                   metadataRootBuilder: new MetadataRootBuilder(metadataBuilder),
                                   ilStream: ilStream,
                                   mappedFieldData: fieldData,
                                   entryPoint: MetadataTokens.MethodDefinitionHandle(entryPoint.MetadataToken));
        var blobBuilder = new BlobBuilder();
        peBuilder.Serialize(blobBuilder);

        using var stream = File.OpenWrite(outputDllPath);
        blobBuilder.WriteContentTo(stream);
    }

    private static string GetRuntimeVersion()
    {
        Version version = Environment.Version;
        return $"net{version.Major}.{version.Minor}";
    }

    private static string GetFrameworkVersion()
    {
        string description = RuntimeInformation.FrameworkDescription;

        Debug.Assert(Regex.IsMatch(description, @"\.NET .*"));
        int index = description.IndexOf(' ');
        return description.Substring(index + 1);
    }
}
