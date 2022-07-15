using System.Reflection;

namespace Calc4DotNet.Core.ILCompilation;

internal static class ReflectionHelper
{
    public static MethodInfo GetInterfaceMethod(Type typeOfTNumber, Type typeOfInterface, string methodName)
    {
        return typeOfTNumber.GetInterfaceMap(typeOfInterface).InterfaceMethods.Single(method => method.Name == methodName);
    }
}
