using System.Runtime.InteropServices;

namespace Calc4DotNet.Core;

public interface IIOService
{
    void PrintChar(char c);
    int GetChar();
    bool TryGetChar(out int c);
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

    public int GetChar()
    {
        return reader.Read();
    }

    public bool TryGetChar(out int c)
    {
        c = reader.Read();
        return true;
    }
}

internal sealed class InputIsNotSupportedException : Exception
{ }

public sealed class MemoryIOService : IIOService
{
    private readonly string? input;
    private readonly List<char> history = [];
    private int nextInputIndex = 0;

    public MemoryIOService()
        : this(null)
    { }

    public MemoryIOService(string? input)
    {
        this.input = input;
    }

    public void PrintChar(char c)
    {
        history.Add(c);
    }

    public int GetChar()
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

        return -1;
    }

    public bool TryGetChar(out int c)
    {
        if (input is null)
        {
            c = 0;
            return false;
        }

        c = GetChar();
        return true;
    }

    public string GetHistory()
    {
        Span<char> span = CollectionsMarshal.AsSpan(history);
        return span.ToString();
    }
}
