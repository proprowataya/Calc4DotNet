﻿namespace Calc4DotNet.Core.Operators;

public interface IOperatorVisitor
{
    void Visit(ZeroOperator op);
    void Visit(PreComputedOperator op);
    void Visit(ArgumentOperator op);
    void Visit(DefineOperator op);
    void Visit(LoadVariableOperator op);
    void Visit(InputOperator op);
    void Visit(LoadArrayOperator op);
    void Visit(PrintCharOperator op);
    void Visit(ParenthesisOperator op);
    void Visit(DecimalOperator op);
    void Visit(StoreVariableOperator op);
    void Visit(StoreArrayOperator op);
    void Visit(BinaryOperator op);
    void Visit(ConditionalOperator op);
    void Visit(UserDefinedOperator op);
}

public interface IOperatorVisitor<TResult>
{
    TResult Visit(ZeroOperator op);
    TResult Visit(PreComputedOperator op);
    TResult Visit(ArgumentOperator op);
    TResult Visit(DefineOperator op);
    TResult Visit(LoadVariableOperator op);
    TResult Visit(InputOperator op);
    TResult Visit(LoadArrayOperator op);
    TResult Visit(PrintCharOperator op);
    TResult Visit(ParenthesisOperator op);
    TResult Visit(DecimalOperator op);
    TResult Visit(StoreVariableOperator op);
    TResult Visit(StoreArrayOperator op);
    TResult Visit(BinaryOperator op);
    TResult Visit(ConditionalOperator op);
    TResult Visit(UserDefinedOperator op);
}

public interface IOperatorVisitor<TResult, TParam>
{
    TResult Visit(ZeroOperator op, TParam param);
    TResult Visit(PreComputedOperator op, TParam param);
    TResult Visit(ArgumentOperator op, TParam param);
    TResult Visit(DefineOperator op, TParam param);
    TResult Visit(LoadVariableOperator op, TParam param);
    TResult Visit(InputOperator op, TParam param);
    TResult Visit(LoadArrayOperator op, TParam param);
    TResult Visit(PrintCharOperator op, TParam param);
    TResult Visit(ParenthesisOperator op, TParam param);
    TResult Visit(DecimalOperator op, TParam param);
    TResult Visit(StoreVariableOperator op, TParam param);
    TResult Visit(StoreArrayOperator op, TParam param);
    TResult Visit(BinaryOperator op, TParam param);
    TResult Visit(ConditionalOperator op, TParam param);
    TResult Visit(UserDefinedOperator op, TParam param);
}
