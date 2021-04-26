using System;

namespace Calc4DotNet.Core.Exceptions
{
    public abstract class Calc4Exception : Exception
    {
        public Calc4Exception(string message)
            : base(message)
        { }
    }

    public sealed class OperatorOrOperandNotDefinedException : Calc4Exception
    {
        public string Name { get; }

        public OperatorOrOperandNotDefinedException(string name)
            : base($"Operator or operand \"{name}\" is not defined")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }

    public sealed class DefinitionTextIsEmptyException : Calc4Exception
    {
        public DefinitionTextIsEmptyException()
            : base("The definition text is empty")
        { }
    }

    public sealed class DefinitionTextNotSplittedProperlyException : Calc4Exception
    {
        public string Text { get; }

        public DefinitionTextNotSplittedProperlyException(string text)
            : base($"The following definition text is not splitted by two '|'s: \"{text}\"")
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }
    }

    public sealed class SomeOperandsMissingException : Calc4Exception
    {
        public SomeOperandsMissingException()
            : base("Some operand(s) is missing")
        { }
    }

    public sealed class TokenExpectedException : Calc4Exception
    {
        public string Name { get; }

        public TokenExpectedException(string name)
            : base($"\"{name}\" is expected")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }

    public sealed class UnexpectedTokenException : Calc4Exception
    {
        public string Name { get; }

        public UnexpectedTokenException(string name)
            : base($"Unexpected token \"{name}\"")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }

    public sealed class TypeNotSupportedException : Calc4Exception
    {
        public Type Type { get; }

        public TypeNotSupportedException(Type type)
            : base($"Type \"{type}\" is not supported")
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }

    public sealed class StackOverflowException : Calc4Exception
    {
        public StackOverflowException()
            : base("Stack Overflow")
        { }
    }

    public sealed class CodeIsEmptyException : Calc4Exception
    {
        public CodeIsEmptyException()
            : base("Code is empty")
        { }
    }
}
