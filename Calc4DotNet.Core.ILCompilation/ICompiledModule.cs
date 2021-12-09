namespace Calc4DotNet.Core.ILCompilation;

public interface ICompiledModule<TNumber>
{
    TNumber Run(Calc4GlobalArraySource<TNumber> array);
}
