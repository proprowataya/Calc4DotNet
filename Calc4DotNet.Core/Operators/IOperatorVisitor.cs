namespace Calc4DotNet.Core.Operators
{
    public interface IOperatorVisitor
    {
        void Visit(ZeroOperator op);
        void Visit(PreComputedOperator op);
        void Visit(ArgumentOperator op);
        void Visit(DefineOperator op);
        void Visit(ParenthesisOperator op);
        void Visit(DecimalOperator op);
        void Visit(BinaryOperator op);
        void Visit(ConditionalOperator op);
        void Visit(UserDefinedOperator op);
    }
}
