using System;
using System.Linq;
using System.Numerics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;
using Xunit;

namespace Calc4DotNet.Test
{
    public class ExecutionTest
    {
        #region Sources

        private static readonly (string text, int expected, Type[]? skipTypes)[] TestCases = new (string text, int expected, Type[]? skipTypes)[]
        {
            ("1<2", 1, null),
            ("12345678", 12345678, null),
            ("1+2*3", (1 + 2) * 3, null),
            ("0?1?2?3?4", 3, null),
            ("D[add|x,y|x+y] 12{add}23", 12 + 23, null),
            ("D[get12345||12345] {get12345}+{get12345}", 12345 + 12345, null),
            ("D[fact|x,y|x==0?y?(x-1){fact}(x*y)] 10{fact}1", 3628800, null),

            // Fibonacci
            ("D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 10{fib}", 55, null),
            ("D[fibImpl|x,a,b|x ? ((x-1) ? ((x-1){fibImpl}(a+b){fibImpl}a) ? a) ? b] D[fib|x|x{fibImpl}1{fibImpl}0] 10{fib}", 55, null),
            ("D[f|a,b,p,q,c|c < 2 ? ((a*p) + (b*q)) ? (c % 2 ? ((a*p) + (b*q) {f} (a*q) + (b*q) + (b*p) {f} (p*p) + (q*q) {f} (2*p+q)*q {f} c/2) ? (a {f} b {f} (p*p) + (q*q) {f} (2*p+q)*q {f} c/2))] D[fib|n|0{f}1{f}0{f}1{f}n] 10{fib}", 55, new[] { typeof(Double) }),

            // Tarai
            ("D[tarai|x,y,z|x <= y ? y ? (((x - 1){tarai}y{tarai}z){tarai}((y - 1){tarai}z{tarai}x){tarai}((z - 1){tarai}x{tarai}y))] 10{tarai}5{tarai}5", 5, null),
        };

        private static readonly Type[] TestTypes = new[] { typeof(Int32), typeof(Int64), typeof(Double), typeof(BigInteger) };

        public static readonly object[][] Source =
            (from test in TestCases
             from type in TestTypes
             where !test.skipTypes?.Contains(type) ?? true
             from optimize in new[] { false, true }
             select new object[] { test.text, test.expected, type, optimize })
            .ToArray();

        #endregion

        #region Helpers

        private static TNumber EvaluateDynamic<TNumber>(IOperator op, CompilationContext context, int maxStep, TNumber dummy)
            where TNumber : notnull
        {
            return Evaluator.Evaluate<TNumber>(op, context, maxStep);
        }

        private static LowLevelModule<TNumber> GenerateLowLevelModuleDynamic<TNumber>(IOperator op, CompilationContext context, TNumber dummy)
            where TNumber : notnull
        {
            return LowLevelCodeGenerator.Generate<TNumber>(op, context);
        }

        private static void TestCore(string text, object expected, Type type, bool optimize,
                                     Func<object, object, object> executor)
        {
            TestCoreGeneric(text, (dynamic)expected, type, optimize,
                            executor, (dynamic)Activator.CreateInstance(type)!);
        }

        private static void TestCoreGeneric<TNumber>(string text, TNumber expected, Type type, bool optimize,
                                                     Func<object, object, object> executor, TNumber dummy)
            where TNumber : notnull
        {
            var context = CompilationContext.Empty;
            var tokens = Lexer.Lex(text, ref context);
            var op = Parser.Parse(tokens, ref context);
            if (optimize)
            {
                Optimizer.Optimize<TNumber>(ref op, ref context);
            }

            // TODO
            Assert.Equal(expected, executor(op, context));
        }

        #endregion

        [Theory, MemberData(nameof(Source))]
        public static void TestByTreeEvaluator(string text, object expected, Type type, bool optimize)
        {
            TestCore(text, expected, type, optimize, (op, context) =>
            {
                return EvaluateDynamic((dynamic)op,
                                       (dynamic)context,
                                       int.MaxValue,
                                       (dynamic)Activator.CreateInstance(type)!);
            });
        }

        [Theory, MemberData(nameof(Source))]
        public static void TestByLowLevelExecutor(string text, object expected, Type type, bool optimize)
        {
            TestCore(text, expected, type, optimize, (op, context) =>
            {
                var module = GenerateLowLevelModuleDynamic((dynamic)op,
                                                           (dynamic)context,
                                                           (dynamic)Activator.CreateInstance(type)!);
                return LowLevelExecutor.Execute(module);
            });
        }

        [Theory, MemberData(nameof(Source))]
        public static void TestByJIT(string text, object expected, Type type, bool optimize)
        {
            TestCore(text, expected, type, optimize, (op, context) =>
            {
                var module = GenerateLowLevelModuleDynamic((dynamic)op,
                                                           (dynamic)context,
                                                           (dynamic)Activator.CreateInstance(type)!);
                var compiled = ILCompiler.Compile(module);
                return compiled.Run();
            });
        }

        [Fact]
        public static void TestStackOverflow()
        {
            const string text = "D[x||{x} + 1] {x}";

            foreach (var type in TestTypes)
            {
                foreach (var optimize in new[] { true, false })
                {
                    Assert.Throws<Calc4DotNet.Core.Exceptions.StackOverflowException>(() =>
                    {
                        TestByLowLevelExecutor(text, Activator.CreateInstance(type)! /* dummy */, type, optimize);
                    });
                }
            }
        }
    }
}
