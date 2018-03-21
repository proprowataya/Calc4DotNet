namespace Calc4DotNet.Core.Operators
{
    public interface IOperatorVisitor<TNumber>
    {
        void Visit(ZeroOperator<TNumber> op);
        void Visit(PreComputedOperator<TNumber> op);
        void Visit(ArgumentOperator<TNumber> op);
        void Visit(DefineOperator<TNumber> op);
        void Visit(ParenthesisOperator<TNumber> op);
        void Visit(DecimalOperator<TNumber> op);
        void Visit(BinaryOperator<TNumber> op);
        void Visit(ConditionalOperator<TNumber> op);
        void Visit(UserDefinedOperator<TNumber> op);
    }

    public interface IOperatorVisitor<TNumber, TResult>
    {
        TResult Visit(ZeroOperator<TNumber> op);
        TResult Visit(PreComputedOperator<TNumber> op);
        TResult Visit(ArgumentOperator<TNumber> op);
        TResult Visit(DefineOperator<TNumber> op);
        TResult Visit(ParenthesisOperator<TNumber> op);
        TResult Visit(DecimalOperator<TNumber> op);
        TResult Visit(BinaryOperator<TNumber> op);
        TResult Visit(ConditionalOperator<TNumber> op);
        TResult Visit(UserDefinedOperator<TNumber> op);
    }

    public interface IOperatorVisitor<TNumber, TResult, TParam>
    {
        TResult Visit(ZeroOperator<TNumber> op, TParam param);
        TResult Visit(PreComputedOperator<TNumber> op, TParam param);
        TResult Visit(ArgumentOperator<TNumber> op, TParam param);
        TResult Visit(DefineOperator<TNumber> op, TParam param);
        TResult Visit(ParenthesisOperator<TNumber> op, TParam param);
        TResult Visit(DecimalOperator<TNumber> op, TParam param);
        TResult Visit(BinaryOperator<TNumber> op, TParam param);
        TResult Visit(ConditionalOperator<TNumber> op, TParam param);
        TResult Visit(UserDefinedOperator<TNumber> op, TParam param);
    }
}
