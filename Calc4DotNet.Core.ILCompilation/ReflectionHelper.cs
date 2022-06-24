using System.Reflection;

namespace Calc4DotNet.Core.ILCompilation;

internal static class ReflectionHelper
{
    public static MethodInfo GetInterfaceMethod<TNumber, TInterface>(string methodName)
    {
        return typeof(TNumber).GetInterfaceMap(typeof(TInterface)).InterfaceMethods.Single(method => method.Name == methodName);
    }
}
