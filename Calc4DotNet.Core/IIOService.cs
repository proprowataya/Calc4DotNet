using System.Runtime.InteropServices;

namespace Calc4DotNet.Core;

public interface IIOService
{
    void PrintChar(char c);
    IIOService Clone();
}

public sealed class TextWriterIOService : IIOService
{
    private readonly TextWriter writer;

    public TextWriterIOService(TextWriter writer)
    {
        this.writer = writer;
    }

    public void PrintChar(char c)
    {
        writer.Write(c);
    }

    public TextWriterIOService Clone()
    {
        return this;
    }

    IIOService IIOService.Clone()
    {
        return Clone();
    }
}

public sealed class MemoryIOService : IIOService
{
    private readonly List<char> history;

    public MemoryIOService()
        : this(new())
    { }

    private MemoryIOService(List<char> history)
    {
        this.history = history;
    }

    public void PrintChar(char c)
    {
        history.Add(c);
    }

    public string GetHistory()
    {
        Span<char> span = CollectionsMarshal.AsSpan(history);
        return span.ToString();
    }

    public MemoryIOService Clone()
    {
        return new MemoryIOService(new(history));
    }

    IIOService IIOService.Clone()
    {
        return Clone();
    }
}
