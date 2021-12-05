namespace Calc4DotNet.Core;

internal readonly record struct ValueBox<T>(T? Value);

internal static class ValueBox
{
    public static ValueBox<T> Create<T>(T? value) => new ValueBox<T>(value);
}
