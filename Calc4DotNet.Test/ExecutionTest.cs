using System;
using System.Linq;
using System.Numerics;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Evaluation;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.ILCompilation;
using Calc4DotNet.Core.Optimization;
using Calc4DotNet.Core.SyntaxAnalysis;
using Xunit;

namespace Calc4DotNet.Test
{
    public class ExecutionTest
    {
        #region Sources

        private static readonly (string text, int expected, Type[] skipTypes)[] TestCases = new(string text, int expected, Type[] skipTypes)[]
        {
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

        public static readonly object[][] Source =
            (from test in TestCases
             from type in new[] { typeof(Int32), typeof(Int64), typeof(Double), typeof(BigInteger) }
             where !test.skipTypes?.Contains(type) ?? true
             from optimize in new[] { false, true }
             select new object[] { test.text, test.expected, type, optimize })
            .ToArray();

        public static object[][] SourceForLowLevelExecutor =>
            Source.Where(s => (Type)s[2] == typeof(Int64)).ToArray();

        #endregion

        #region Helpers

        private static object NewContext(Type type) => Activator.CreateInstance(typeof(Context<>).MakeGenericType(type));

        private static void TestCore(string text, object expected, Type type, bool optimize, object context, Func<object, object> executor)
        {
            var tokens = Lexer.Lex(text, (dynamic)context);
            var op = Parser.Parse(tokens, (dynamic)context);
            if (optimize)
            {
                op = Optimizer.Optimize(op, (dynamic)context);
            }

            // TODO
            Assert.Equal(expected.ToString(), executor(op).ToString());
        }

        #endregion

        [Theory, MemberData(nameof(Source))]
        public static void TestByTreeEvaluator(string text, object expected, Type type, bool optimize)
        {
            var context = NewContext(type);
            TestCore(text, expected, type, optimize, context, op =>
            {
                return Evaluator.Evaluate((dynamic)op, (dynamic)context);
            });
        }

        [Theory, MemberData(nameof(SourceForLowLevelExecutor))]
        public static void TestByLowLevelExecutor(string text, object expected, Type type, bool optimize)
        {
            var context = NewContext(type);
            TestCore(text, expected, type, optimize, context, op =>
            {
                var module = LowLevelCodeGenerator.Generate((dynamic)op, (dynamic)context);
                return Executor.ExecuteInt64(module);
            });
        }

        [Theory, MemberData(nameof(Source))]
        public static void TestByJIT(string text, object expected, Type type, bool optimize)
        {
            var context = NewContext(type);
            TestCore(text, expected, type, optimize, context, op =>
            {
                var module = LowLevelCodeGenerator.Generate((dynamic)op, (dynamic)context);
                var compiled = ILCompiler.Compile(module);
                return compiled.Run((dynamic)context);
            });
        }
    }
}
