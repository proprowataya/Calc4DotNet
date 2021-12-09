using System.Collections.Immutable;
using Calc4DotNet.Core.Operators;
using Xunit;

namespace Calc4DotNet.Test;

public class EqualityTest
{
    [Fact]
    public void TestParenthesisOperatorEquality()
    {
        static ParenthesisOperator CreateDummy1() =>
            new ParenthesisOperator(
                ImmutableArray.Create<IOperator>(
                    new BinaryOperator(
                        new PreComputedOperator(1), // Difference
                        new DecimalOperator(
                            new ZeroOperator(),
                            0),
                        BinaryType.Sub,
                        "dummy"),
                    new StoreVariableOperator(
                        new ZeroOperator(),
                        "var")));

        static ParenthesisOperator CreateDummy2() =>
            new ParenthesisOperator(
                ImmutableArray.Create<IOperator>(
                    new BinaryOperator(
                        new PreComputedOperator(2), // Difference
                        new DecimalOperator(
                            new ZeroOperator(),
                            0),
                        BinaryType.Sub,
                        "dummy"),
                    new StoreVariableOperator(
                        new ZeroOperator(),
                        "var")));

        ParenthesisOperator dummy11 = CreateDummy1();
        ParenthesisOperator dummy12 = CreateDummy1();
        ParenthesisOperator dummy2 = CreateDummy2();

        Assert.Equal(dummy11, dummy11);
        Assert.Equal(dummy11, dummy12);
        Assert.NotEqual(dummy11, dummy2);

        Assert.Equal(dummy11.GetHashCode(), dummy12.GetHashCode());
    }

    [Fact]
    public void TestUserDefinedOperatorEquality()
    {
        static UserDefinedOperator CreateDummy1() =>
            new UserDefinedOperator(
                new OperatorDefinition("testfunc", 1),  // Difference
                ImmutableArray.Create<IOperator>(
                    new ZeroOperator(),
                    new LoadVariableOperator("var")),
                true,
                "testfunc");

        static UserDefinedOperator CreateDummy2() =>
            new UserDefinedOperator(
                new OperatorDefinition("testfunc", 2),  // Difference
                ImmutableArray.Create<IOperator>(
                    new ZeroOperator(),
                    new LoadVariableOperator("var")),
                true,
                "testfunc");

        UserDefinedOperator dummy11 = CreateDummy1();
        UserDefinedOperator dummy12 = CreateDummy1();
        UserDefinedOperator dummy2 = CreateDummy2();

        Assert.Equal(dummy11, dummy11);
        Assert.Equal(dummy11, dummy12);
        Assert.NotEqual(dummy11, dummy2);

        Assert.Equal(dummy11.GetHashCode(), dummy12.GetHashCode());
    }
}
