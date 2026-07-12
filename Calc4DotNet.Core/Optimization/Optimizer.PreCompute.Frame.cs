using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;

namespace Calc4DotNet.Core.Optimization;

public static partial class Optimizer
{
    private readonly record struct PreComputeFrame<TNumber>(PreComputeState<TNumber> State,
                                                            UserDefinedCallBudget Budget,
                                                            ImmutableDictionary<int, TNumber> LetValues)
        where TNumber : INumber<TNumber>;

    private readonly record struct UserDefinedCallBudget(int RemainingUserDefinedCalls)
    {
        public bool TryConsume(out UserDefinedCallBudget remainingBudget)
        {
            Debug.Assert(RemainingUserDefinedCalls >= 0);

            if (RemainingUserDefinedCalls <= 0)
            {
                remainingBudget = default;
                return false;
            }

            remainingBudget = new UserDefinedCallBudget(RemainingUserDefinedCalls - 1);
            return true;
        }
    }
}
