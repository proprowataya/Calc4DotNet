using System.Diagnostics.CodeAnalysis;
using Calc4DotNet.Core;

namespace Calc4DotNet.Test;

internal sealed class CompilationContextEqualityComparer : IEqualityComparer<CompilationContext>
{
    public static readonly CompilationContextEqualityComparer Instance = new();

    private CompilationContextEqualityComparer()
    { }

    public bool Equals(CompilationContext? x, CompilationContext? y)
    {
        return (x, y) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            _ => x.OperatorImplements.SequenceEqual(y.OperatorImplements),
        };
    }

    public int GetHashCode([DisallowNull] CompilationContext obj)
    {
        throw new NotSupportedException();
    }
}
