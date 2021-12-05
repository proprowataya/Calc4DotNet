namespace Calc4DotNet.Core.Operators;

public sealed record OperatorImplement(OperatorDefinition Definition, bool IsOptimized, IOperator? Operator = null);
