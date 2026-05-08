using System.Reflection;
using System.Text;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;
using Calc4DotNet.Test;

var testCaseInputs = new (string Source, string StandardInput, Type[]? SkipTypes)[]
{
    ("200", "", null),
    ("1<2", "", null),
    ("1<=2", "", null),
    ("1>=2", "", null),
    ("1>2", "", null),
    ("2<1", "", null),
    ("2<=1", "", null),
    ("2>=1", "", null),
    ("2>1", "", null),
    ("1<1", "", null),
    ("1<=1", "", null),
    ("1>=1", "", null),
    ("1>1", "", null),
    ("12345678", "", null),
    ("1+2*3-10", "", null),
    ("0?1?2?3?4", "", null),
    ("1==0?2?3", "", null),
    ("0==1?2?3", "", null),
    ("0==0?2?3", "", null),
    ("I==0?2?3", "A", null),
    ("0==I?2?3", "A", null),
    ("1!=0?2?3", "", null),
    ("0!=1?2?3", "", null),
    ("0!=0?2?3", "", null),
    ("I!=0?2?3", "A", null),
    ("0!=I?2?3", "A", null),
    ("72P101P108P108P111P10P", "", null),
    ("1+// C++ style comment\n2", "", null),
    ("1+/* C style comment*/2", "", null),
    ("D[print||72P101P108P108P111P10P] {print}", "", null),
    ("D[add|x,y|x+y] 12{add}23", "", null),
    ("D[get12345||12345] {get12345}+{get12345}", "", null),
    ("D[fact|x,y|x==0?y?(x-1){fact}(x*y)] 10{fact}1", "", null),

    // Division by zero in an unused branch.
    // The checked division path needs a clean evaluation stack.
    // Without that, the compiler can emit invalid IL.
    ("0?1/0?2", "", null),

    // Logical operators
    ("1&&2?3?4", "", null),
    ("1&&0?3?4", "", null),
    ("0&&2?3?4", "", null),
    ("0&&0?3?4", "", null),
    ("1||2?3?4", "", null),
    ("1||0?3?4", "", null),
    ("0||2?3?4", "", null),
    ("0||0?3?4", "", null),
    ("1&&(65P)", "", null),
    ("0&&(65P)", "", null),
    ("1||(65P)", "", null),
    ("0||(65P)", "", null),
    ("0&&(1/0)?1?2", "", null),
    ("1||(1/0)?1?2", "", null),
    ("(1&&2)+5", "", null),
    ("(2&&3)+5", "", null),
    ("(0||2)+5", "", null),
    ("(2||0)+5", "", null),
    ("0&&1&&(65P)", "", null),
    ("1||0||(65P)", "", null),
    ("1&&1&&(65P)", "", null),
    ("0||0||(65P)", "", null),
    ("(1&&0)||1", "", null),
    ("1&&(0||1)", "", null),
    ("(1<2)&&(2<1)", "", null),
    ("(1<2)||(2<1)", "", null),
    ("(0-1)&&1", "", null),
    ("(0-1)||0", "", null),
    ("(I&&(1S[x]))L[x]", "A", null),
    ("D[true||1||2]{true}", "", null),
    ("D[select|a,b|a?a?b] (0{select}5) + (3{select}4)", "", null),
    ("D[pick|a,b,c|a?b?c] (0{pick}5{pick}9) + (1{pick}2{pick}3)", "", null),
    ("D[sum|n,acc|n==0?{acc}?(n-1){sum}({acc}+1)] (5{sum}0) + 7", "", null),
    ("((0S[y])?0?(1S[y]))(L[y])", "", null),
    ("(((0S[y])?0?(1S[y]))&&(L[y]S[x]))", "", null),
    ("(((1S[y])?0?(0S[y]))||(L[y]S[x]))", "", null),
    ("((0S[x])&&(1S[y]))(L[y])", "", null),
    ("(10S[x])(20S[y])((0S[x])&&(1S[y]))(L[y])", "", null),
    ("((0S[x])&&((5->2)(3S[y])))((2)@)+(L[y])", "", null),
    ("((1S[x])||((5->2)(3S[y])))((2)@)+(L[y])", "", null),
    ("D[write||(5->2)(3S[y])] (0&&{write})((2@)+(L[y]))", "", null),

    // Fibonacci
    ("D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 10{fib}", "", null),
    ("D[fibImpl|x,a,b|x ? ((x-1) ? ((x-1){fibImpl}(a+b){fibImpl}a) ? a) ? b] D[fib|x|x{fibImpl}1{fibImpl}0] 10{fib}", "", null),
    ("D[f|a,b,p,q,c|c < 2 ? ((a*p) + (b*q)) ? (c % 2 ? ((a*p) + (b*q) {f} (a*q) + (b*q) + (b*p) {f} (p*p) + (q*q) {f} (2*p+q)*q {f} c/2) ? (a {f} b {f} (p*p) + (q*q) {f} (2*p+q)*q {f} c/2))] D[fib|n|0{f}1{f}0{f}1{f}n] 10{fib}", "", null),

    // Tarai
    ("D[tarai|x,y,z|x <= y ? y ? (((x - 1){tarai}y{tarai}z){tarai}((y - 1){tarai}z{tarai}x){tarai}((z - 1){tarai}x{tarai}y))] 10{tarai}5{tarai}5", "", null),

    // User defined variables
    ("1S", "", null),
    ("L", "", null),
    ("1S[var]", "", null),
    ("L[var]", "", null),
    ("D[get||L[var]] D[set|x|xS[var]] 123{set} {get} * {get}", "", null),
    ("D[set|x|xS] 7{set}L", "", null),
    ("D[set|x|xS] 7{set}LS[var1] L[zero]3{set}LS[var2] L[var1]*L[var2]", "", null),
    ("(123S)L*L", "", null),
    ("(123S[var])L[var]*L[var]", "", null),
    ("((100+20+3)S)L*L", "", null),
    ("((100+20+3)S[var])L[var]*L[var]", "", null),
    ("(I?(1S[x])?(2S[x]))L[x]", "A", null),
    ("D[op||(123S)L*L]{op}", "", null),
    ("D[op||L*L](123S){op}", "", null),
    ("D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] (20{fib}S)+L", "", null),
    ("D[get||L] D[set|x|xS] D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] (20{fib}>=1000?10?5)S {get}", "", null),
    ("D[get||L] D[set|x|xS] D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] (20{fib}>=1000?10S?5S) {get}", "", null),
    ("D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 3{set} {fib2}", "", null),
    ("D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 20{set} {fib2}", "", null),
    ("D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 3S {fib2}", "", null),
    ("D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 20S {fib2}", "", null),
    ("D[fib|n|10S(n<=1?n?((n-1){fib}+(n-2){fib}))S] 20{fib} L", "", null),

    // Array accesses
    ("0@", "", null),
    ("5->0", "", null),
    ("(10->20)L[zero]20@", "", null),
    ("((4+6)->(10+10))(20@)", "", null),
    ("(10->20)(0->20)(20@)", "", null),
    ("(I?(1->5)?(2->5))(5@)", "A", null),
    ("(5->(0-1))((0-1)@)", "", null),
    ("(7->131072)((131072)@)", "", null),
    ("D[func||(10->20)L[zero]20@] {func} (20@)", "", null),
    ("D[func||((4+6)->(10+10))(20@)] {func} (20@)", "", null),
    ("D[func||(10->20)L[zero]20@] D[get||20@] {func} (20@)", "", null),
    ("D[func||((4+6)->(10+10))(20@)] D[get||20@] {func} {get}", "", null),

    // Input operator
    ("I", "A", null),
    ("I+I", "AB", null),
    ("1+2+I", "A", null),
    ("D[Input||I]{Input}", "A", null),
    ("(I?5->2?7->2)(2@)", "A", null),
    ("(I?5->2?7->2)(2@)", "\0", null),
    ("I", "", null),

    // Byte IO tests
    ("128P255P0", "", null),
    ("I", "\xFF", null),

    // Identifier sanitization regression tests
    ("(1S)(2S[empty])(L+L[empty])", "", null),
    ("(1S)(2S[default])(L+L[default])", "", null),
    ("(1S[/])(2S[_2F])(L[/]+L[_2F])", "", null),
    ("(1S[1])(2S[_1])(L[1]+L[_1])", "", null),
    ("D[/||1] D[_2F||2] ({/}+{_2F})", "", null),
    ("D[1||1] D[_1||2] ({1}+{_1})", "", null),
    ("1S[a-b]L[a-b]", "", null),
    ("D[a-b||1]{a-b}", "", null),

    // Tail-call marking must not cross operations that still need to run
    // after evaluating their operands.
    ("D[f|n|n==0?0?((n-1){f}P)] I{f}", "\u0001", null),
    ("D[f|n|n==0?65?((n-1){f}S[x])] I{f} L[x]", "\u0001", null),
    ("D[f|n|n==0?5?((n-1){f}@)] (99->5) I{f}", "\u0001", null),
    ("D[f|n|n==0?5?((n-1){f}->2)] I{f} 2@", "\u0001", null),
    ("D[f|n|n==0?2?(5->(n-1){f})] I{f} 2@", "\u0001", null),

    // A nested tail-call jump in the false branch must not let other false-branch paths
    // fall through into the true branch. The first case takes the non-recursive false path
    // directly, while the second reaches that path after taking tail-call jumps.
    ("D[f|n|n<0?111?(n>0?(n-1){f}?222)] I{f}", "\0", null),
    ("D[f|n|n<0?111?(n>0?(n-1){f}?222)] I{f}", "\u0002", null),

    // Algebraic simplification, single-constant rewrites.
    ("I+0", "A", null),
    ("0+I", "A", null),
    ("I-0", "A", null),
    ("I*1", "A", null),
    ("1*I", "A", null),
    ("I*0", "A", null),
    ("0*I", "A", null),
    ("I/1", "A", null),
    ("I%1", "A", null),
    // Multiply-by-zero must preserve side effects of the non-constant side.
    ("(65P)*0", "", null),
    ("0*(65P)", "", null),
    // Self operations on repeated input operands must not fold.
    // Each I reads independently, so the operands are not the same pure value.
    ("I-I", "A", null),
    ("I==I", "A", null),
    ("I!=I", "A", null),
    // Conditional with two identical branches keeps the condition effects,
    // then uses the branch value.
    ("I?5?5", "A", null),
    ("I?(1+2)?(1+2)", "A", null),

    // Short-circuit with a constant right side that has no observable effect.
    ("I&&0", "A", null),        // Result becomes sequence I then 0.
    ("I&&5", "A", null),        // Result becomes I != 0.
    ("I||0", "A", null),        // Result becomes I != 0.
    ("I||5", "A", null),        // Result becomes sequence I then 1.
    // Right has side effects, short-circuit simplification does NOT apply.
    ("I&&(65P)", "A", null),

    // Pure statements in a sequence whose value is discarded should be dropped.
    // In user-defined operator bodies variables start unknown, so the inner LoadVariable
    // is not constant-folded away. It is removed only because its value is unused
    // and it has no observable effects.
    ("D[op||(L[x])(I)] {op}", "A", null),       // L[x] pure unused, drop it and keep I.
    ("D[op||(L[x])(L[y])(I)] {op}", "A", null), // Multiple pure reads dropped
    ("D[op||(L[x])(65P)] {op}", "", null),      // Pure read dropped, PrintChar kept
    ("D[op||(L[x]/(1+1))(I)] {op}", "A", null),
    ("D[op||(L[x]%(1+1))(I)] {op}", "A", null),

    // Partial specialization with constant operands.
    // A body with input cannot be fully precomputed, but constants can still be
    // substituted so the smaller body can be inlined.
    ("D[f|n|n*2+I] 5{f}", "A", null),           // Body becomes 10+I.
    ("D[f|x,y|x+y] 10{f}I", "A", null),         // Body becomes 10 plus the second operand value.
    ("D[g|a|a*a+I] 7{g}", "A", null),           // Body becomes 49+I.
    ("D[h|a,b|(aP)(bP)] 65{h}I", "A", null),    // Body prints 65 then the second operand value.
    // Constant operands with side effects keep their effects before the inlined body.
    ("D[f|x|x+I] (65P){f}", "A", null),
    ("D[f|x|x+I] (1S[a]){f}", "A", null),
    // Calls with non-constant operands preserve operand evaluation order and single evaluation.
    ("D[double|x|x+x] I{double}", "A", null),
    ("D[add1|x|x+1] D[caller|y|(y+1){add1}] I{caller}", "A", null),
    ("D[ignore|x|1] D[caller|y|(y+1){ignore}] I{caller}", "A", null),
    ("D[double|x|x+x] D[caller|y|(y+1){double}] I{caller}", "A", null),
    ("D[ignore|x|(66P)] (IP){ignore}", "A", null),
    // Zero-operand calls can be inlined even when their body is not constant.
    ("D[get||L] D[op||{get}] {get}", "", null),

    // Specialized calls with input cannot be fully precomputed, but their exit-state
    // deltas should still keep known variable and array writes visible to following
    // operations.
    ("D[setx|flag|{flag}?((5S[x])I)?((7S[y])I)] 1{setx} L[x]", "A", null),
    ("D[setx|flag|{flag}?((5S[x])I)?((7S[y])I)] 0{setx} L[x]", "A", null),
    ("D[setarr|flag|{flag}?((5->2)I)?((7->3)I)] 1{setarr} (2@)", "A", null),
    ("D[setarr|flag|{flag}?((5->2)I)?((7->3)I)] 0{setarr} (2@)", "A", null),
    ("D[setarr|i|((9->i)(5->2)I)] I{setarr} (2@)", "AB", null),
    // Existing caller facts that are unrelated to the specialization delta should
    // survive the delta application.
    ("D[setx|flag|{flag}?((5S[x])I)?((7S[y])I)] (9S[z]) (1{setx}) L[z]", "A", null),
    ("D[setarr|flag|{flag}?((5->2)I)?((7->3)I)] (9->4) (1{setarr}) (4)@", "A", null),
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
foreach (var (source, standardInput, skipTypes) in testCaseInputs)
{
    TestCase testCase = GenerateTestCase(source, standardInput, skipTypes);
    serializer.Serialize(testCase);
    writer.Write(",\n");
}

writer.Write(
@"    };
}
");

static TestCase GenerateTestCase(string source, string standardInput, Type[]? skipTypes)
{
    CompilationContext context = CompilationContext.Empty;
    List<IToken> tokens = Lexer.Lex(source, ref context);
    IOperator op = Parser.Parse(tokens, ref context);
    DefaultVariableSource<int> variables = new();
    DefaultArraySource<Int32> arraySource = new();
    MemoryIOService ioService = new(standardInput);
    int expectedValue = Evaluator.Evaluate(op, context, new SimpleEvaluationState<int>(variables,
                                                                                       arraySource,
                                                                                       ioService));
    string? expectedConsoleOutput = ioService.GetHistory() is string output && output.Length > 0 ? output : null;

    CompilationResult<Int32> expectedWhenNotOptimized = new(op, context, LowLevelCodeGenerator.Generate<Int32>(op, context, LowLevelCodeGenerationOption.Default));

    Optimizer.Optimize<int>(ref op,
                            ref context,
                            OptimizeTarget.All,
                            new DefaultVariableSource<Int32>(),
                            new DefaultArraySource<Int32>());
    CompilationResult<Int32> expectedWhenOptimized = new(op, context, LowLevelCodeGenerator.Generate<Int32>(op, context, LowLevelCodeGenerationOption.Default));

    return new TestCase(source,
                        standardInput,
                        expectedValue,
                        variables.ToImmutableDictionary(),
                        arraySource.ToImmutableDictionary(),
                        expectedWhenOptimized,
                        expectedWhenNotOptimized,
                        skipTypes,
                        expectedConsoleOutput);
}
