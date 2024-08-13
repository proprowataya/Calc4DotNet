using System.Collections.Immutable;
using System.Numerics;
using System.Reflection;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Test;

internal struct CSharpSerializer
{
    private const int IndentWidth = 4;

    private readonly TextWriter writer;
    private int indent;

    public CSharpSerializer(TextWriter writer, int indent)
    {
        this.writer = writer;
        this.indent = indent;
    }

    public void Serialize(TestCase testCase, bool insertIndentFirst = true)
    {
        WriteLine($"new {nameof(TestCase)}(", insertIndentFirst);

        indent++;
        Write($"{nameof(TestCase.Source)}: ");
        Serialize(testCase.Source, insertIndentFirst: false);
        WriteLine(",", insertIndent: false);

        Write($"{nameof(TestCase.StandardInput)}: ");
        Serialize(testCase.StandardInput, insertIndentFirst: false);
        WriteLine(",", insertIndent: false);

        WriteLine($"{nameof(TestCase.ExpectedValue)}: {testCase.ExpectedValue},");

        WriteLine($"{nameof(TestCase.VariablesAfterExecution)}:");
        indent++;
        Serialize(testCase.VariablesAfterExecution);
        WriteLine(",", insertIndent: false);
        indent--;

        WriteLine($"{nameof(TestCase.ExpectedWhenNotOptimized)}:");
        indent++;
        Serialize(testCase.ExpectedWhenNotOptimized);
        WriteLine(",", insertIndent: false);
        indent--;

        WriteLine($"{nameof(TestCase.ExpectedWhenOptimized)}:");
        indent++;
        Serialize(testCase.ExpectedWhenOptimized);
        WriteLine(",", insertIndent: false);
        indent--;

        Write($"{nameof(TestCase.SkipTypes)}: {(testCase.SkipTypes is null ? "null" : $"new[] {{ {string.Join(", ", testCase.SkipTypes.Select(t => $"typeof({t.Name})"))} }}")}");

        if (testCase.ExpectedConsoleOutput is { } consoleOutput)
        {
            WriteLine(",", insertIndent: false);
            Write($"{nameof(TestCase.ExpectedConsoleOutput)}: ");
            Serialize(consoleOutput, insertIndentFirst: false);
        }

        WriteLine(null, insertIndent: false);
        indent--;
        Write(")");
    }

    public void Serialize<TNumber>(CompilationResult<TNumber> result, bool insertIndentFirst = true)
        where TNumber : INumber<TNumber>
    {
        WriteLine($"new {nameof(CompilationResult<TNumber>)}<{typeof(TNumber).Name}>(", insertIndentFirst);

        indent++;

        Write($"{nameof(CompilationResult<TNumber>.Operator)}: ");
        Serialize(result.Operator, insertIndentFirst: false);
        WriteLine(",", insertIndent: false);

        WriteLine($"{nameof(CompilationResult<TNumber>.Context)}:");
        indent++;
        Serialize(result.Context);
        WriteLine(",", insertIndent: false);
        indent--;

        WriteLine($"{nameof(CompilationResult<TNumber>.Module)}:");
        indent++;
        Serialize(result.Module);
        indent--;

        indent--;
        WriteLine(null, insertIndent: false);
        Write(")");
    }

    public void Serialize(IOperator op, bool insertIndentFirst = true)
    {
        Type type = op.GetType();
        Write($"new {type.Name}(", insertIndentFirst);
        ConstructorInfo constructor = type.GetConstructors().Single();
        ParameterInfo[] parameters = constructor.GetParameters();

        indent++;
        for (int i = 0; i < parameters.Length; i++)
        {
            object? value = type.GetProperty(parameters[i].Name ?? throw new InvalidOperationException())?.GetValue(op);

            if (i == 0)
            {
                WriteLine(null, insertIndent: false);
            }
            Write($"{parameters[i].Name}: ");

            if (value is null)
            {
                Write("null", insertIndent: false);
            }
            else
            {
                Serialize((dynamic)value, insertIndentFirst: false);
            }

            if (i < parameters.Length - 1)
            {
                WriteLine(",", insertIndent: false);
            }
        }

        indent--;
        if (parameters.Length == 0)
        {
            Write(")", insertIndent: false);
        }
        else
        {
            WriteLine(null, insertIndent: false);
            Write(")");
        }
    }

    public void Serialize(CompilationContext context, bool insertIndentFirst = true)
    {
        WriteLine($"{nameof(CompilationContext)}.{nameof(CompilationContext.Empty)}.{nameof(CompilationContext.WithAddOrUpdateOperatorImplements)}(", insertIndentFirst);
        indent++;
        WriteLine($"new {nameof(OperatorImplement)}[]");
        WriteLine("{");

        indent++;
        OperatorImplement[] implements = context.OperatorImplements.ToArray();
        for (int i = 0; i < implements.Length; i++)
        {
            OperatorImplement implement = implements[i];

            WriteLine($"new {nameof(OperatorImplement)}(");
            indent++;

            Write($"{nameof(OperatorImplement.Definition)}: ");
            Serialize(implement.Definition, insertIndentFirst: false);
            WriteLine(",", insertIndent: false);

            Write($"{nameof(OperatorImplement.IsOptimized)}: ");
            Serialize(implement.IsOptimized, insertIndentFirst: false);
            WriteLine(",", insertIndent: false);

            Write($"{nameof(OperatorImplement.Operator)}: ");
            Serialize(implement.Operator ?? throw new InvalidOperationException(), insertIndentFirst: false);
            WriteLine(null, insertIndent: false);

            indent--;
            WriteLine(i < implements.Length - 1 ? ")," : ")");
        }
        indent--;

        WriteLine("}");
        indent--;
        Write(")");
    }

    public void Serialize<TNumber>(LowLevelModule<TNumber> module, bool insertIndentFirst = true)
        where TNumber : INumber<TNumber>
    {
        WriteLine($"new {nameof(LowLevelModule<TNumber>)}<{typeof(TNumber).Name}>(", insertIndentFirst);
        indent++;

        Serialize(module.EntryPoint);
        WriteLine(",", insertIndent: false);

        Serialize(module.ConstTable);
        WriteLine(",", insertIndent: false);

        Serialize(module.UserDefinedOperators);
        WriteLine(",", insertIndent: false);

        Serialize(module.Variables);
        WriteLine(null, insertIndent: false);

        indent--;
        Write(")");
    }

    public void Serialize<T>(ImmutableArray<T> array, bool insertIndentFirst = true)
    {
        WriteLine($"new {typeof(T).Name}[]", insertIndentFirst);
        WriteLine("{");
        indent++;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] is LowLevelOperation)
            {
                Write($"/* {i:00} */ ");
            }
            else
            {
                Write(null);
            }

            if (array[i] is null)
            {
                Write("null", insertIndent: false);
            }
            else
            {
                Serialize((dynamic)array[i]!, insertIndentFirst: false);
            }

            WriteLine(i < array.Length - 1 ? "," : "", insertIndent: false);
        }
        indent--;
        Write($"}}.{nameof(ImmutableArray.ToImmutableArray)}()");
    }

    public void Serialize<TNumber>(ImmutableDictionary<ValueBox<string>, TNumber> dictionary, bool insertIndentFirst = true)
    {
        const string ValueBoxTypeName = $"{nameof(ValueBox)}<string>";

        WriteLine($"{nameof(ImmutableDictionary)}.{nameof(ImmutableDictionary.CreateRange)}(", insertIndentFirst);

        indent++;
        WriteLine($"new {nameof(KeyValuePair<ValueBox<string>, TNumber>)}<{ValueBoxTypeName}, {typeof(TNumber).Name}>[]");
        WriteLine("{");
        indent++;

        foreach (var (key, value) in dictionary.OrderBy(pair => pair.Key.Value))
        {
            Write($"new {nameof(KeyValuePair<ValueBox<string>, TNumber>)}<{ValueBoxTypeName}, {typeof(TNumber).Name}>(");
            Serialize((dynamic)key, insertIndentFirst: false);
            Write(", ", insertIndent: false);

            if (value is null)
            {
                Write("null", insertIndent: false);
            }
            else
            {
                Serialize((dynamic)value, insertIndentFirst: false);
            }

            WriteLine("),", insertIndent: false);
        }

        indent--;
        WriteLine("}");
        indent--;
        Write(")");
    }

    public void Serialize(LowLevelUserDefinedOperator op, bool insertIndentFirst = true)
    {
        WriteLine($"new {nameof(LowLevelUserDefinedOperator)}(", insertIndentFirst);
        indent++;
        Serialize(op.Definition);
        WriteLine(",", insertIndent: false);
        Serialize(op.Operations);
        WriteLine(",", insertIndent: false);
        Serialize(op.MaxStackSize);
        indent--;
        Write(")", insertIndent: false);
    }

    public void Serialize(LowLevelOperation operation, bool insertIndentFirst = true)
    {
        Write($"new {nameof(LowLevelOperation)}({nameof(Opcode)}.{operation.Opcode}, {operation.Value})", insertIndentFirst);
    }

    public void Serialize(BinaryType binaryType, bool insertIndentFirst = true)
    {
        Write($"{nameof(BinaryType)}.{binaryType}", insertIndentFirst);
    }

    public void Serialize(OperatorDefinition definition, bool insertIndentFirst = true)
    {
        Write($"new {nameof(OperatorDefinition)}({nameof(OperatorDefinition.Name)}: \"{definition.Name}\", {nameof(OperatorDefinition.NumOperands)}: {definition.NumOperands})", insertIndentFirst);
    }

    public void Serialize<T>(ValueBox<T> box, bool insertIndentFirst = true)
    {
        Write($"{nameof(ValueBox)}.{nameof(ValueBox.Create)}(", insertIndentFirst);

        if (box.Value is null)
        {
            Write($"({typeof(T).Name})null", insertIndent: false);
        }
        else
        {
            Serialize((dynamic)box.Value, insertIndentFirst: false);
        }

        Write(")", insertIndent: false);
    }

    public void Serialize(int value, bool insertIndentFirst = true)
    {
        Write(value.ToString(), insertIndentFirst);
    }

    public void Serialize(string str, bool insertIndentFirst = true)
    {
        Write($"\"{str.Replace("\n", "\\n")}\"", insertIndentFirst);
    }

    public void Serialize(bool b, bool insertIndentFirst = true)
    {
        Write(b ? "true" : "false", insertIndentFirst);
    }

    /*****/

    private void Write(string? text = null, bool insertIndent = true)
    {
        if (insertIndent)
        {
            Span<char> indentString = stackalloc char[indent * IndentWidth];
            indentString.Fill(' ');
            writer.Write(indentString);
        }

        writer.Write(text);
    }

    private void WriteLine(string? text = null, bool insertIndent = true)
    {
        Write(text, insertIndent);
        writer.Write('\n');
    }
}
