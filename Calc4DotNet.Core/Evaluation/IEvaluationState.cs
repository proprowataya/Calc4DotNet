using System.Numerics;

namespace Calc4DotNet.Core.Evaluation;

public interface IEvaluationState<TNumber>
    where TNumber : INumber<TNumber>
{
    IVariableSource<TNumber> Variables { get; }
    IArraySource<TNumber> GlobalArray { get; }
    IIOService IOService { get; }
    IEvaluationState<TNumber> Clone();
}

public sealed class SimpleEvaluationState<TNumber> : IEvaluationState<TNumber>
    where TNumber : INumber<TNumber>
{
    public IVariableSource<TNumber> Variables { get; }
    public IArraySource<TNumber> GlobalArray { get; }
    public IIOService IOService { get; }

    public SimpleEvaluationState(IVariableSource<TNumber> variables, IArraySource<TNumber> globalArray, IIOService ioService)
    {
        Variables = variables;
        GlobalArray = globalArray;
        IOService = ioService;
    }

    public SimpleEvaluationState<TNumber> Clone()
    {
        return new SimpleEvaluationState<TNumber>(Variables.Clone(), GlobalArray.Clone(), IOService.Clone());
    }

    IEvaluationState<TNumber> IEvaluationState<TNumber>.Clone()
    {
        return Clone();
    }
}
