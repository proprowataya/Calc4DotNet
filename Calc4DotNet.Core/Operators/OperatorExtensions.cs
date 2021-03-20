using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Calc4DotNet.Core.Operators
{
    public static class OperatorExtensions
    {
        private static readonly HashSet<string> HiddenPropertyNames = new HashSet<string>() { };

        public static string ToDetailString(this IOperator op)
        {
            var type = op.GetType();
            var props = type.GetProperties()
                            .Where(p => !typeof(IOperator).IsAssignableFrom(p.PropertyType)
                                        && !HiddenPropertyNames.Contains(p.Name));
            var values = props.Select(p => (Property: p, Value: p.GetValue(op)))
                              .Where(t => t.Value != null);
            return $"{type.Name} [{string.Join(", ", values.Select(t => $"{t.Property.Name} = {ObjectToString(t.Value)}"))}]";
        }

        private static string ObjectToString(object obj) => obj switch
        {
            string str => '"' + str + '"',
            IEnumerable enumerable => $"{{{string.Join(", ", enumerable.Cast<object>().Select(x => ObjectToString(x)))}}}",
            IOperator op => op.ToDetailString(),
            _ => obj.ToString(),
        };
    }
}
