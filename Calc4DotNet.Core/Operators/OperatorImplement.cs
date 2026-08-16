namespace Calc4DotNet.Core.Operators;

public closed record OperatorImplement(OperatorDefinition Definition);

public sealed record UnresolvedOperatorImplement(OperatorDefinition Definition)
    : OperatorImplement(Definition);

public sealed record ResolvedOperatorImplement(OperatorDefinition Definition, IOperator Body, bool IsOptimized)
    : OperatorImplement(Definition);
