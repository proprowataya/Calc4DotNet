namespace Calc4DotNet.Core.ILCompilation;

public static class ThrowHelper
{
    public static void ThrowZeroDivisionException()
    {
        throw new Calc4DotNet.Core.Exceptions.ZeroDivisionException();
    }
}
