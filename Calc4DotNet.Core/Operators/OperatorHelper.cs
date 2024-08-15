using System.Collections;

namespace Calc4DotNet.Core.Operators;

internal static class OperatorHelper
{
    public static string ToStringImplement(this IOperator op)
    {
        var properties = op.GetProperties().Where(p => p.Value is not null);
        return $"{op.GetType().Name} [{string.Join(", ", properties.Select(p => $"{p.Name} = {ObjectToString(p.Value)}"))}]";
    }

    private static string ObjectToString(object? obj) => obj switch
    {
        string str => '"' + str + '"',
        IEnumerable enumerable => $"{{{string.Join(", ", enumerable.Cast<object>().Select(x => ObjectToString(x)))}}}",
        IOperator op => op.ToStringImplement(),
        _ => obj?.ToString() ?? "",
    };
}
