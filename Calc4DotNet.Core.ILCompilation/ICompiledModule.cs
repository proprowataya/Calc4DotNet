using System.Numerics;
using Calc4DotNet.Core.Evaluation;

namespace Calc4DotNet.Core.ILCompilation;

public interface ICompiledModule<TNumber>
    where TNumber : INumber<TNumber>
{
    TNumber Run(IEvaluationState<TNumber> state);
}
