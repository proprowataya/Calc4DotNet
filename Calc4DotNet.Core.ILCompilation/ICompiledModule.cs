using System.Numerics;
using Calc4DotNet.Core.Evaluation;

namespace Calc4DotNet.Core.ILCompilation;

public interface ICompiledModule<TNumber>
    where TNumber : IBinaryInteger<TNumber>, ISignedNumber<TNumber>
{
    TNumber Run(IEvaluationState<TNumber> state);
}
