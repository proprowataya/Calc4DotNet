using System;
using System.Collections.Immutable;
using System.Linq;

namespace Calc4DotNet.Core.Operators
{
    public interface IOperator
    {
        string SupplementaryText { get; }
        ImmutableArray<IOperator> Operands { get; }
        bool ThisTypeIsPreComputable { get; }
        Number Evaluate(Context context, ReadOnlySpan<Number> arguments);
    }

    public interface IPrimitiveOperator : IOperator { }

    public sealed class ZeroOperator : IPrimitiveOperator
    {
        public string SupplementaryText => null;
        public ImmutableArray<IOperator> Operands => ImmutableArray<IOperator>.Empty;
        public bool ThisTypeIsPreComputable => true;
        public Number Evaluate(Context context, ReadOnlySpan<Number> arguments) => Number.Zero;
    }

    public sealed class ArgumentOperator : IPrimitiveOperator
    {
        public int Index { get; }
        public string SupplementaryText { get; }

        public ArgumentOperator(int index, string supplementaryText = null)
        {
            Index = index;
            SupplementaryText = supplementaryText;
        }

        public ImmutableArray<IOperator> Operands => ImmutableArray<IOperator>.Empty;
        public bool ThisTypeIsPreComputable => true;

        public Number Evaluate(Context context, ReadOnlySpan<Number> arguments) => arguments[Index];
    }

    public sealed class DefineOperator : IPrimitiveOperator
    {
        public string SupplementaryText { get; }

        public DefineOperator(string supplementaryText = null)
        {
            SupplementaryText = supplementaryText;
        }

        public ImmutableArray<IOperator> Operands => ImmutableArray<IOperator>.Empty;
        public bool ThisTypeIsPreComputable => true;

        public Number Evaluate(Context context, ReadOnlySpan<Number> arguments) => Number.Zero;
    }

    public sealed class ParenthesisOperator : IPrimitiveOperator
    {
        public ImmutableArray<IOperator> Operators { get; }
        public string SupplementaryText { get; }

        public ParenthesisOperator(ImmutableArray<IOperator> operators, string supplementaryText = null)
        {
            Operators = operators;
            SupplementaryText = supplementaryText;
        }

        public ImmutableArray<IOperator> Operands => ImmutableArray<IOperator>.Empty;
        public bool ThisTypeIsPreComputable => Operators.All(op => op.ThisTypeIsPreComputable);

        public Number Evaluate(Context context, ReadOnlySpan<Number> arguments)
        {
            Number result = Number.Zero;

            for (int i = 0; i < Operators.Length; i++)
            {
                result = Operators[i].Evaluate(context, arguments);
            }

            return result;
        }
    }

    public sealed class DecimalOperator : IPrimitiveOperator
    {
        public IOperator Operand { get; }
        public int Value { get; }
        public string SupplementaryText { get; }

        public DecimalOperator(IOperator operand, int value, string supplementaryText = null)
        {
            Operand = operand ?? throw new ArgumentNullException(nameof(operand));
            Value = value;
            SupplementaryText = supplementaryText;
            if (value < 0 || value > 9)
                throw new ArgumentException(nameof(value));
        }

        public ImmutableArray<IOperator> Operands => ImmutableArray.Create(Operand);
        public bool ThisTypeIsPreComputable => true;

        public Number Evaluate(Context context, ReadOnlySpan<Number> arguments)
            => Operand.Evaluate(context, arguments) * 10 + Value;
    }

    public sealed class BinaryOperator : IPrimitiveOperator
    {
        public enum ArithmeticType { Add, Sub, Mult, Div, Mod, LessThan, LessThanOrEqual, GreaterThanOrEqual, GreaterThan }

        public IOperator Left { get; }
        public IOperator Right { get; }
        public ArithmeticType Type { get; }
        public string SupplementaryText { get; }

        public BinaryOperator(IOperator left, IOperator right, ArithmeticType type, string supplementaryText = null)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
            Type = type;
            SupplementaryText = supplementaryText;
        }

        public ImmutableArray<IOperator> Operands => ImmutableArray.Create(Left, Right);
        public bool ThisTypeIsPreComputable => true;

        public Number Evaluate(Context context, ReadOnlySpan<Number> arguments)
        {
            switch (Type)
            {
                case ArithmeticType.Add:
                    return Left.Evaluate(context, arguments) + Right.Evaluate(context, arguments);
                case ArithmeticType.Sub:
                    return Left.Evaluate(context, arguments) - Right.Evaluate(context, arguments);
                case ArithmeticType.Mult:
                    return Left.Evaluate(context, arguments) * Right.Evaluate(context, arguments);
                case ArithmeticType.Div:
                    return Left.Evaluate(context, arguments) / Right.Evaluate(context, arguments);
                case ArithmeticType.Mod:
                    return Left.Evaluate(context, arguments) % Right.Evaluate(context, arguments);
                case ArithmeticType.LessThan:
                    return Left.Evaluate(context, arguments) < Right.Evaluate(context, arguments) ? 1 : 0;
                case ArithmeticType.LessThanOrEqual:
                    return Left.Evaluate(context, arguments) <= Right.Evaluate(context, arguments) ? 1 : 0;
                case ArithmeticType.GreaterThanOrEqual:
                    return Left.Evaluate(context, arguments) >= Right.Evaluate(context, arguments) ? 1 : 0;
                case ArithmeticType.GreaterThan:
                    return Left.Evaluate(context, arguments) > Right.Evaluate(context, arguments) ? 1 : 0;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public sealed class ConditionalOperator : IPrimitiveOperator
    {
        public IOperator Condition { get; }
        public IOperator IfTrue { get; }
        public IOperator IfFalse { get; }
        public string SupplementaryText { get; }

        public ConditionalOperator(IOperator condition, IOperator ifTrue, IOperator ifFalse, string supplementaryText = null)
        {
            Condition = condition ?? throw new ArgumentNullException(nameof(condition));
            IfTrue = ifTrue ?? throw new ArgumentNullException(nameof(ifTrue));
            IfFalse = ifFalse ?? throw new ArgumentNullException(nameof(ifFalse));
            SupplementaryText = supplementaryText;
        }

        public ImmutableArray<IOperator> Operands => ImmutableArray.Create(Condition, IfTrue, IfFalse);
        public bool ThisTypeIsPreComputable => true;

        public Number Evaluate(Context context, ReadOnlySpan<Number> arguments)
            => Condition.Evaluate(context, arguments) != Number.Zero ? IfTrue.Evaluate(context, arguments) : IfFalse.Evaluate(context, arguments);
    }

    public sealed class UserDefinedOperator : IOperator
    {
        public OperatorDefinition Definition { get; }
        public ImmutableArray<IOperator> Operands { get; }
        public string SupplementaryText { get; }

        public UserDefinedOperator(OperatorDefinition definition, ImmutableArray<IOperator> operands, string supplementaryText = null)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Operands = operands;
            SupplementaryText = supplementaryText;

            if (operands.Length != definition.NumOperands)
                throw new ArgumentException($"Number of operands does not match between {nameof(definition)} and {nameof(operands)}");
        }

        public bool ThisTypeIsPreComputable => true;

        public Number Evaluate(Context context, ReadOnlySpan<Number> arguments)
        {
            Span<Number> stack = stackalloc Number[Operands.Length];

            for (int i = 0; i < Operands.Length; i++)
            {
                stack[i] = Operands[i].Evaluate(context, arguments);
            }

            return Definition.Root.Evaluate(context, stack);
        }
    }
}
