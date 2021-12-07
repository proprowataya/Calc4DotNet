using System.Collections;

namespace Calc4DotNet.Core.Operators;

internal static class OperatorHelper
{
    private static readonly HashSet<string> HiddenPropertyNames = new HashSet<string>() { "Operators" };

    public static string ToStringImplement(this IOperator op)
    {
        var type = op.GetType();
        var props = type.GetProperties()
                        .Where(p => !typeof(IOperator).IsAssignableFrom(p.PropertyType)
                                    && !HiddenPropertyNames.Contains(p.Name));
        var values = props.Select(p => (Property: p, Value: p.GetValue(op)))
                          .Where(t => t.Value != null);
        return $"{type.Name} [{string.Join(", ", values.Select(t => $"{t.Property.Name} = {ObjectToString(t.Value)}"))}]";
    }

    private static string ObjectToString(object? obj) => obj switch
    {
        string str => '"' + str + '"',
        IEnumerable enumerable => $"{{{string.Join(", ", enumerable.Cast<object>().Select(x => ObjectToString(x)))}}}",
        IOperator op => op.ToStringImplement(),
        _ => obj?.ToString() ?? "",
    };
}
