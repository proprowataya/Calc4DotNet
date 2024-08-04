using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using Calc4DotNet.Compiler;
using Xunit;
using static Calc4DotNet.Test.TestCommon;

namespace Calc4DotNet.Test;

public class AotCompilerTest
{
    public static readonly object[][] Source =
        (from testCase in TestCases.Values
         from valueType in ValueTypes
         where !testCase.SkipTypes?.Contains(valueType) ?? true
         from optimize in new bool[] { false, true }
         select new object[] { testCase, valueType, optimize })
        .ToArray();

    private static readonly string ExecutingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

    [Theory, MemberData(nameof(Source))]
    private static void TestAotCompilation(TestCase testCase, Type valueType, bool optimize)
    {
        /*
         * Compile
         */
        Setting setting = new Setting(valueType, optimize);
        string workingDirectory = Path.Join(ExecutingDirectory, Guid.NewGuid().ToString());
        Directory.CreateDirectory(workingDirectory);
        string sourcePath = Path.Join(workingDirectory, $"{Guid.NewGuid()}.txt");
        File.WriteAllText(sourcePath, testCase.Source);
        RunCompilationGeneric(sourcePath, setting, (dynamic)Activator.CreateInstance(valueType)!);

        /*
         * Validate result
         */
        string dllPath = Path.ChangeExtension(sourcePath, "dll");
        using var process = Process.Start(new ProcessStartInfo("dotnet", dllPath) { RedirectStandardOutput = true });
        if (process is null)
        {
            Assert.Fail("Could not start dotnet process.");
        }
        string result = process.StandardOutput.ReadToEnd();
        string expected = testCase.ExpectedConsoleOutput + testCase.ExpectedValue.ToString() + Environment.NewLine;
        Assert.Equal(expected, result);
    }

    private static void RunCompilationGeneric<TNumber>(string sourcePath, Setting setting, TNumber dummy)
        where TNumber : INumber<TNumber>
    {
        CompilerImplementation.Run<TNumber>(sourcePath, setting);
    }
}
