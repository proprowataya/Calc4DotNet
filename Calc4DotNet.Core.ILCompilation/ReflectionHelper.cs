using System.Reflection;

namespace Calc4DotNet.Core.ILCompilation;

internal static class ReflectionHelper
{
    public static MethodInfo GetInterfaceMethod(Type typeOfInterface, string methodName)
    {
        return typeOfInterface.GetMethod(methodName)!;
    }

    public static MethodInfo GetInterfacePropertyGetter(Type typeOfInterface, string propertyName)
    {
        return typeOfInterface.GetProperty(propertyName)!.GetMethod!;
    }
}
