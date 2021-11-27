namespace Calc4DotNet.Core.ILCompilation;

public interface ICompiledModule<TNumber>
{
    TNumber Run();
}
