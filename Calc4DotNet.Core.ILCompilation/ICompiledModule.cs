using Calc4DotNet.Core.Evaluation;

namespace Calc4DotNet.Core.ILCompilation;

public interface ICompiledModule<TNumber>
{
    TNumber Run(IEvaluationState<TNumber> state);
}
