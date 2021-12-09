using System.Reflection;
using System.Text;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;
using Calc4DotNet.Test;

var testCaseInputs = new (string Source, Type[]? SkipTypes)[]
{
    ("1<2", null),
    ("1<=2", null),
    ("1>=2", null),
    ("1>2", null),
    ("2<1", null),
    ("2<=1", null),
    ("2>=1", null),
    ("2>1", null),
    ("1<1", null),
    ("1<=1", null),
    ("1>=1", null),
    ("1>1", null),
    ("12345678", null),
    ("1+2*3-10", null),
    ("0?1?2?3?4", null),
    ("D[add|x,y|x+y] 12{add}23", null),
    ("D[get12345||12345] {get12345}+{get12345}", null),
    ("D[fact|x,y|x==0?y?(x-1){fact}(x*y)] 10{fact}1", null),

    // Fibonacci
    ("D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 10{fib}", null),
    ("D[fibImpl|x,a,b|x ? ((x-1) ? ((x-1){fibImpl}(a+b){fibImpl}a) ? a) ? b] D[fib|x|x{fibImpl}1{fibImpl}0] 10{fib}", null),
    ("D[f|a,b,p,q,c|c < 2 ? ((a*p) + (b*q)) ? (c % 2 ? ((a*p) + (b*q) {f} (a*q) + (b*q) + (b*p) {f} (p*p) + (q*q) {f} (2*p+q)*q {f} c/2) ? (a {f} b {f} (p*p) + (q*q) {f} (2*p+q)*q {f} c/2))] D[fib|n|0{f}1{f}0{f}1{f}n] 10{fib}", new[] { typeof(Double) }),

    // Tarai
    ("D[tarai|x,y,z|x <= y ? y ? (((x - 1){tarai}y{tarai}z){tarai}((y - 1){tarai}z{tarai}x){tarai}((z - 1){tarai}x{tarai}y))] 10{tarai}5{tarai}5", null),

    // User defined variables
    ("1S", null),
    ("L", null),
    ("1S[var]", null),
    ("L[var]", null),
    ("D[get||L[var]] D[set|x|xS[var]] 123{set} {get} * {get}", null),
    ("D[set|x|xS] 7{set}L", null),
    ("D[set|x|xS] 7{set}LS[var1] L[zero]3{set}LS[var2] L[var1]*L[var2]", null),
    ("(123S)L*L", null),
    ("(123S[var])L[var]*L[var]", null),
    ("((100+20+3)S)L*L", null),
    ("((100+20+3)S[var])L[var]*L[var]", null),
    ("D[op||(123S)L*L]{op}", null),
    ("D[op||L*L](123S){op}", null),
    ("D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] (20{fib}S)+L", null),
    ("D[get||L] D[set|x|xS] D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] (20{fib}>=1000?10?5)S {get}", null),
    ("D[get||L] D[set|x|xS] D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] (20{fib}>=1000?10S?5S) {get}", null),
    ("D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 3{set} {fib2}", null),
    ("D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 20{set} {fib2}", null),
    ("D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 3S {fib2}", null),
    ("D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 20S {fib2}", null),
    ("D[fib|n|10S(n<=1?n?((n-1){fib}+(n-2){fib}))S] 20{fib} L", null),

    // Array accesses
    ("0l", null),
    ("5s0", null),
    ("(10s20)L[zero]20l", null),
    ("((4+6)s(10+10))(20l)", null),
    ("D[func||(10s20)L[zero]20l] {func} (20l)", null),
    ("D[func||((4+6)s(10+10))(20l)] {func} (20l)", null),
    ("D[func||(10s20)L[zero]20l] D[get||20l] {func} (20l)", null),
    ("D[func||((4+6)s(10+10))(20l)] D[get||20l] {func} {get}", null),
};

string outputPath = Path.GetFullPath(Path.Join(new[] { Assembly.GetExecutingAssembly().Location, "..", "..", "..", "..", "..", "Calc4DotNet.Test", "TestCases.cs" }));
Console.WriteLine($"Output test cases to \"{outputPath}\"");

using var stream = new FileStream(outputPath, FileMode.Create);
using var writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

writer.Write(
@"#nullable disable

using System.Collections.Immutable;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Test;

internal static class TestCases
{
    public static readonly TestCase[] Values =
    {
");

var serializer = new CSharpSerializer(writer, 2);
foreach (var (source, skipTypes) in testCaseInputs)
{
    TestCase testCase = GenerateTestCase(source, skipTypes);
    serializer.Serialize(testCase);
    writer.Write(",\n");
}

writer.Write(
@"    };
}
");

static TestCase GenerateTestCase(string source, Type[]? skipTypes)
{
    CompilationContext context = CompilationContext.Empty;
    List<IToken> tokens = Lexer.Lex(source, ref context);
    IOperator op = Parser.Parse(tokens, ref context);
    DefaultVariableSource<int> variables = new(0);
    int expectedValue = Evaluator.Evaluate(op, context, new SimpleEvaluationState<int>(variables, new Calc4GlobalArraySource<Int32>()));

    CompilationResult<Int32> expectedWhenNotOptimized = new(op, context, LowLevelCodeGenerator.Generate<Int32>(op, context));

    Optimizer.Optimize<int>(ref op, ref context, OptimizeTarget.All, new DefaultVariableSource<Int32>(0));
    CompilationResult<Int32> expectedWhenOptimized = new(op, context, LowLevelCodeGenerator.Generate<Int32>(op, context));

    return new TestCase(source,
                        expectedValue,
                        variables.ToImmutableDictionary(),
                        expectedWhenOptimized,
                        expectedWhenNotOptimized,
                        skipTypes);
}
