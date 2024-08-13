using System.Runtime.InteropServices;

namespace Calc4DotNet.Core;

public interface IIOService
{
    void PrintChar(char c);
    char GetChar();
    IIOService Clone();
}

public sealed class TextReaderWriterIOService : IIOService
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public TextReaderWriterIOService(TextReader reader, TextWriter writer)
    {
        this.reader = reader;
        this.writer = writer;
    }

    public void PrintChar(char c)
    {
        writer.Write(c);
    }

    public char GetChar()
    {
        return (char)reader.Read();
    }

    public TextReaderWriterIOService Clone()
    {
        return this;
    }

    IIOService IIOService.Clone()
    {
        return Clone();
    }
}

internal sealed class InputIsNotSupportedException : Exception
{ }

public sealed class MemoryIOService : IIOService
{
    private readonly string? input;
    private readonly List<char> history;
    private int nextInputIndex = 0;

    public MemoryIOService()
        : this(null, new())
    { }

    public MemoryIOService(string? input)
        : this(input, new())
    { }

    private MemoryIOService(List<char> history)
        : this(null, history)
    { }

    public MemoryIOService(string? input, List<char> history)
    {
        this.input = input;
        this.history = history;
    }

    public void PrintChar(char c)
    {
        history.Add(c);
    }

    public char GetChar()
    {
        if (input is null)
        {
            throw new InputIsNotSupportedException();
        }

        int index = nextInputIndex;
        if (index < input.Length)
        {
            nextInputIndex++;
            return input[index];
        }

        return (char)0;
    }

    public string GetHistory()
    {
        Span<char> span = CollectionsMarshal.AsSpan(history);
        return span.ToString();
    }

    public MemoryIOService Clone()
    {
        return new MemoryIOService(input, new(history));
    }

    IIOService IIOService.Clone()
    {
        return Clone();
    }
}
