namespace Calc4DotNet.Core;

public readonly record struct ValueBox<T>(T? Value);

public static class ValueBox
{
    public static ValueBox<T> Create<T>(T? value) => new ValueBox<T>(value);
}
