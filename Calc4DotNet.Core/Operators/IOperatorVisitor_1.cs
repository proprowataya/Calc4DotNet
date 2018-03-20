namespace Calc4DotNet.Core.Operators
{
    public interface IOperatorVisitor<T>
    {
        T Visit(ZeroOperator op);
        T Visit(PreComputedOperator op);
        T Visit(ArgumentOperator op);
        T Visit(DefineOperator op);
        T Visit(ParenthesisOperator op);
        T Visit(DecimalOperator op);
        T Visit(BinaryOperator op);
        T Visit(ConditionalOperator op);
        T Visit(UserDefinedOperator op);
    }
}
