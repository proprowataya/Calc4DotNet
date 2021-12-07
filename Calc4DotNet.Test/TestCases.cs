#nullable disable

using System.Collections.Immutable;
using Calc4DotNet.Core;
using Calc4DotNet.Core.Execution;
using Calc4DotNet.Core.Operators;
using Calc4DotNet.Test;

internal static class TestCases
{
    public static readonly TestCase[] Values =
    {
        new TestCase(
            Source: "1<2",
            ExpectedValue: 1,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 2,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.LessThan,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThan, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 1
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "1<=2",
            ExpectedValue: 1,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 2,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.LessThanOrEqual,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 1
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "1>=2",
            ExpectedValue: 0,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 2,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.GreaterThanOrEqual,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThan, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 0
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "1>2",
            ExpectedValue: 0,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 2,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.GreaterThan,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 0
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "2<1",
            ExpectedValue: 0,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 2,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.LessThan,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThan, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 0
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "2<=1",
            ExpectedValue: 0,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 2,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.LessThanOrEqual,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 0
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "2>=1",
            ExpectedValue: 1,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 2,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.GreaterThanOrEqual,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThan, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 1
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "2>1",
            ExpectedValue: 1,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 2,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.GreaterThan,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 1
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "1<1",
            ExpectedValue: 0,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.LessThan,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThan, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 0
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "1<=1",
            ExpectedValue: 1,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.LessThanOrEqual,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 1
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "1>=1",
            ExpectedValue: 1,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.GreaterThanOrEqual,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThan, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 1
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "1>1",
            ExpectedValue: 0,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.GreaterThan,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 12),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 0
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "12345678",
            ExpectedValue: 12345678,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new DecimalOperator(
                        Operand: new DecimalOperator(
                            Operand: new DecimalOperator(
                                Operand: new DecimalOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 1,
                                                    SupplementaryText: null
                                                ),
                                                Value: 2,
                                                SupplementaryText: null
                                            ),
                                            Value: 3,
                                            SupplementaryText: null
                                        ),
                                        Value: 4,
                                        SupplementaryText: null
                                    ),
                                    Value: 5,
                                    SupplementaryText: null
                                ),
                                Value: 6,
                                SupplementaryText: null
                            ),
                            Value: 7,
                            SupplementaryText: null
                        ),
                        Value: 8,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 14 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 15 */ new LowLevelOperation(Opcode.LoadConst, 4),
                                /* 16 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 17 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 18 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 19 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                /* 20 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 21 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 23 */ new LowLevelOperation(Opcode.LoadConst, 6),
                                /* 24 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 27 */ new LowLevelOperation(Opcode.LoadConst, 7),
                                /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 29 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 30 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 31 */ new LowLevelOperation(Opcode.LoadConst, 8),
                                /* 32 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 33 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 12345678
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConstTable, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                                12345678
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "1+2*3-10",
            ExpectedValue: -1,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new BinaryOperator(
                            Left: new BinaryOperator(
                                Left: new DecimalOperator(
                                    Operand: new ZeroOperator(),
                                    Value: 1,
                                    SupplementaryText: null
                                ),
                                Right: new DecimalOperator(
                                    Operand: new ZeroOperator(),
                                    Value: 2,
                                    SupplementaryText: null
                                ),
                                Type: BinaryType.Add,
                                SupplementaryText: null
                            ),
                            Right: new DecimalOperator(
                                Operand: new ZeroOperator(),
                                Value: 3,
                                SupplementaryText: null
                            ),
                            Type: BinaryType.Mult,
                            SupplementaryText: null
                        ),
                        Right: new DecimalOperator(
                            Operand: new DecimalOperator(
                                Operand: new ZeroOperator(),
                                Value: 1,
                                SupplementaryText: null
                            ),
                            Value: 0,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Sub,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 13 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 14 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 15 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 16 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 17 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 18 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 19 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 20 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 21 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 22 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 23 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 24 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 25 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 26 */ new LowLevelOperation(Opcode.Sub, 0),
                                /* 27 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: -1
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, -1),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "0?1?2?3?4",
            ExpectedValue: 3,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ConditionalOperator(
                        Condition: new ConditionalOperator(
                            Condition: new DecimalOperator(
                                Operand: new ZeroOperator(),
                                Value: 0,
                                SupplementaryText: null
                            ),
                            IfTrue: new DecimalOperator(
                                Operand: new ZeroOperator(),
                                Value: 1,
                                SupplementaryText: null
                            ),
                            IfFalse: new DecimalOperator(
                                Operand: new ZeroOperator(),
                                Value: 2,
                                SupplementaryText: null
                            ),
                            SupplementaryText: null
                        ),
                        IfTrue: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 3,
                            SupplementaryText: null
                        ),
                        IfFalse: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 4,
                            SupplementaryText: null
                        ),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.GotoIfTrue, 11),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 08 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 11 */ new LowLevelOperation(Opcode.Goto, 16),
                                /* 12 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 14 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 15 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 16 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 17 */ new LowLevelOperation(Opcode.GotoIfTrue, 23),
                                /* 18 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 19 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 20 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 21 */ new LowLevelOperation(Opcode.LoadConst, 4),
                                /* 22 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 23 */ new LowLevelOperation(Opcode.Goto, 28),
                                /* 24 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 27 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 29 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 3
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[add|x,y|x+y] 12{add}23",
            ExpectedValue: 35,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new UserDefinedOperator(
                        Definition: new OperatorDefinition(Name: "add", NumOperands: 2),
                        Operands: new IOperator[]
                        {
                            new DecimalOperator(
                                Operand: new DecimalOperator(
                                    Operand: new DefineOperator(
                                        SupplementaryText: "add|x,y|x+y"
                                    ),
                                    Value: 1,
                                    SupplementaryText: null
                                ),
                                Value: 2,
                                SupplementaryText: null
                            ),
                            new DecimalOperator(
                                Operand: new DecimalOperator(
                                    Operand: new ZeroOperator(),
                                    Value: 2,
                                    SupplementaryText: null
                                ),
                                Value: 3,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        IsTailCall: null,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "add", NumOperands: 2),
                                    IsOptimized: false,
                                    Operator: new BinaryOperator(
                                        Left: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        Right: new ArgumentOperator(
                                            Index: 1,
                                            SupplementaryText: null
                                        ),
                                        Type: BinaryType.Add,
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 10 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 11 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 12 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 13 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 14 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 15 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 16 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 17 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 18 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 19 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "add", NumOperands: 2),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 02 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.Return, 2)
                                    }.ToImmutableArray(),
                                    2)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 35
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "add", NumOperands: 2),
                                    IsOptimized: true,
                                    Operator: new BinaryOperator(
                                        Left: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        Right: new ArgumentOperator(
                                            Index: 1,
                                            SupplementaryText: null
                                        ),
                                        Type: BinaryType.Add,
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 35),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "add", NumOperands: 2),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 02 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.Return, 2)
                                    }.ToImmutableArray(),
                                    2)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[get12345||12345] {get12345}+{get12345}",
            ExpectedValue: 24690,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new DefineOperator(
                                    SupplementaryText: "get12345||12345"
                                ),
                                new UserDefinedOperator(
                                    Definition: new OperatorDefinition(Name: "get12345", NumOperands: 0),
                                    Operands: new IOperator[]
                                    {
                                    }.ToImmutableArray(),
                                    IsTailCall: null,
                                    SupplementaryText: null
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new UserDefinedOperator(
                            Definition: new OperatorDefinition(Name: "get12345", NumOperands: 0),
                            Operands: new IOperator[]
                            {
                            }.ToImmutableArray(),
                            IsTailCall: null,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Add,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get12345", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new DecimalOperator(
                                        Operand: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new DecimalOperator(
                                                        Operand: new ZeroOperator(),
                                                        Value: 1,
                                                        SupplementaryText: null
                                                    ),
                                                    Value: 2,
                                                    SupplementaryText: null
                                                ),
                                                Value: 3,
                                                SupplementaryText: null
                                            ),
                                            Value: 4,
                                            SupplementaryText: null
                                        ),
                                        Value: 5,
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "get12345", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                        /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 14 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadConst, 4),
                                        /* 16 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 18 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                        /* 20 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    2)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 24690
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get12345", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new PreComputedOperator(
                                        Value: 12345
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 24690),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "get12345", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 12345),
                                        /* 01 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[fact|x,y|x==0?y?(x-1){fact}(x*y)] 10{fact}1",
            ExpectedValue: 3628800,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new UserDefinedOperator(
                        Definition: new OperatorDefinition(Name: "fact", NumOperands: 2),
                        Operands: new IOperator[]
                        {
                            new DecimalOperator(
                                Operand: new DecimalOperator(
                                    Operand: new DefineOperator(
                                        SupplementaryText: "fact|x,y|x==0?y?(x-1){fact}(x*y)"
                                    ),
                                    Value: 1,
                                    SupplementaryText: null
                                ),
                                Value: 0,
                                SupplementaryText: null
                            ),
                            new DecimalOperator(
                                Operand: new ZeroOperator(),
                                Value: 1,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        IsTailCall: null,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fact", NumOperands: 2),
                                    IsOptimized: false,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Equal,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 1,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new UserDefinedOperator(
                                            Definition: new OperatorDefinition(Name: "fact", NumOperands: 2),
                                            Operands: new IOperator[]
                                            {
                                                new BinaryOperator(
                                                    Left: new ArgumentOperator(
                                                        Index: 0,
                                                        SupplementaryText: null
                                                    ),
                                                    Right: new DecimalOperator(
                                                        Operand: new ZeroOperator(),
                                                        Value: 1,
                                                        SupplementaryText: null
                                                    ),
                                                    Type: BinaryType.Sub,
                                                    SupplementaryText: null
                                                ),
                                                new BinaryOperator(
                                                    Left: new ArgumentOperator(
                                                        Index: 0,
                                                        SupplementaryText: null
                                                    ),
                                                    Right: new ArgumentOperator(
                                                        Index: 1,
                                                        SupplementaryText: null
                                                    ),
                                                    Type: BinaryType.Mult,
                                                    SupplementaryText: null
                                                )
                                            }.ToImmutableArray(),
                                            IsTailCall: null,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 10 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 11 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 12 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 13 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 15 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fact", NumOperands: 2),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 03 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.GotoIfEqual, 18),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 16 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.Goto, 19),
                                        /* 19 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 20 */ new LowLevelOperation(Opcode.Return, 2)
                                    }.ToImmutableArray(),
                                    3)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 3628800
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fact", NumOperands: 2),
                                    IsOptimized: true,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new PreComputedOperator(
                                                Value: 0
                                            ),
                                            Type: BinaryType.Equal,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 1,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new UserDefinedOperator(
                                            Definition: new OperatorDefinition(Name: "fact", NumOperands: 2),
                                            Operands: new IOperator[]
                                            {
                                                new BinaryOperator(
                                                    Left: new ArgumentOperator(
                                                        Index: 0,
                                                        SupplementaryText: null
                                                    ),
                                                    Right: new PreComputedOperator(
                                                        Value: 1
                                                    ),
                                                    Type: BinaryType.Sub,
                                                    SupplementaryText: null
                                                ),
                                                new BinaryOperator(
                                                    Left: new ArgumentOperator(
                                                        Index: 0,
                                                        SupplementaryText: null
                                                    ),
                                                    Right: new ArgumentOperator(
                                                        Index: 1,
                                                        SupplementaryText: null
                                                    ),
                                                    Type: BinaryType.Mult,
                                                    SupplementaryText: null
                                                )
                                            }.ToImmutableArray(),
                                            IsTailCall: true,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConstTable, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                                3628800
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fact", NumOperands: 2),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.GotoIfEqual, 11),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.StoreArg, 1),
                                        /* 10 */ new LowLevelOperation(Opcode.StoreArg, 2),
                                        /* 11 */ new LowLevelOperation(Opcode.Goto, -1),
                                        /* 12 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 13 */ new LowLevelOperation(Opcode.Return, 2)
                                    }.ToImmutableArray(),
                                    3)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[fib|n|n<=1?n?(n-1){fib}+(n-2){fib}] 10{fib}",
            ExpectedValue: 55,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new UserDefinedOperator(
                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                        Operands: new IOperator[]
                        {
                            new DecimalOperator(
                                Operand: new DecimalOperator(
                                    Operand: new DefineOperator(
                                        SupplementaryText: "fib|n|n<=1?n?(n-1){fib}+(n-2){fib}"
                                    ),
                                    Value: 1,
                                    SupplementaryText: null
                                ),
                                Value: 0,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        IsTailCall: null,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.LessThanOrEqual,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new BinaryOperator(
                                            Left: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            Right: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 10 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 03 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 24),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 18 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 20 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 22 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 24 */ new LowLevelOperation(Opcode.Goto, 25),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 26 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    4)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new UserDefinedOperator(
                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                        Operands: new IOperator[]
                        {
                            new PreComputedOperator(
                                Value: 10
                            )
                        }.ToImmutableArray(),
                        IsTailCall: true,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new PreComputedOperator(
                                                Value: 1
                                            ),
                                            Type: BinaryType.LessThanOrEqual,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new BinaryOperator(
                                            Left: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 1
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: false,
                                                SupplementaryText: null
                                            ),
                                            Right: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 2
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: false,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 02 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 12),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 09 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 14 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    3)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[fibImpl|x,a,b|x ? ((x-1) ? ((x-1){fibImpl}(a+b){fibImpl}a) ? a) ? b] D[fib|x|x{fibImpl}1{fibImpl}0] 10{fib}",
            ExpectedValue: 55,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new UserDefinedOperator(
                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                        Operands: new IOperator[]
                        {
                            new DecimalOperator(
                                Operand: new DecimalOperator(
                                    Operand: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new DefineOperator(
                                                SupplementaryText: "fibImpl|x,a,b|x ? ((x-1) ? ((x-1){fibImpl}(a+b){fibImpl}a) ? a) ? b"
                                            ),
                                            new DefineOperator(
                                                SupplementaryText: "fib|x|x{fibImpl}1{fibImpl}0"
                                            )
                                        }.ToImmutableArray(),
                                        SupplementaryText: null
                                    ),
                                    Value: 1,
                                    SupplementaryText: null
                                ),
                                Value: 0,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        IsTailCall: null,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fibImpl", NumOperands: 3),
                                        Operands: new IOperator[]
                                        {
                                            new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 0,
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: null,
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fibImpl", NumOperands: 3),
                                    IsOptimized: false,
                                    Operator: new ConditionalOperator(
                                        Condition: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ConditionalOperator(
                                            Condition: new BinaryOperator(
                                                Left: new ArgumentOperator(
                                                    Index: 0,
                                                    SupplementaryText: null
                                                ),
                                                Right: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 1,
                                                    SupplementaryText: null
                                                ),
                                                Type: BinaryType.Sub,
                                                SupplementaryText: null
                                            ),
                                            IfTrue: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fibImpl", NumOperands: 3),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new ArgumentOperator(
                                                            Index: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Add,
                                                        SupplementaryText: null
                                                    ),
                                                    new ArgumentOperator(
                                                        Index: 1,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            IfFalse: new ArgumentOperator(
                                                Index: 1,
                                                SupplementaryText: null
                                            ),
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new ArgumentOperator(
                                            Index: 2,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 04 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 06 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 08 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 11 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 03 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 08 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.Call, 1),
                                        /* 12 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    4),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fibImpl", NumOperands: 3),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 01 */ new LowLevelOperation(Opcode.GotoIfTrue, 3),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 03 */ new LowLevelOperation(Opcode.Goto, 25),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.GotoIfTrue, 13),
                                        /* 12 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 13 */ new LowLevelOperation(Opcode.Goto, 25),
                                        /* 14 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 17 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 19 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 20 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 22 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 23 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 24 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 25 */ new LowLevelOperation(Opcode.Call, 1),
                                        /* 26 */ new LowLevelOperation(Opcode.Return, 3)
                                    }.ToImmutableArray(),
                                    3)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 55
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fibImpl", NumOperands: 3),
                                        Operands: new IOperator[]
                                        {
                                            new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            new PreComputedOperator(
                                                Value: 1
                                            ),
                                            new PreComputedOperator(
                                                Value: 0
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: true,
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fibImpl", NumOperands: 3),
                                    IsOptimized: true,
                                    Operator: new ConditionalOperator(
                                        Condition: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ConditionalOperator(
                                            Condition: new BinaryOperator(
                                                Left: new ArgumentOperator(
                                                    Index: 0,
                                                    SupplementaryText: null
                                                ),
                                                Right: new PreComputedOperator(
                                                    Value: 1
                                                ),
                                                Type: BinaryType.Sub,
                                                SupplementaryText: null
                                            ),
                                            IfTrue: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fibImpl", NumOperands: 3),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 1
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new ArgumentOperator(
                                                            Index: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Add,
                                                        SupplementaryText: null
                                                    ),
                                                    new ArgumentOperator(
                                                        Index: 1,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: true,
                                                SupplementaryText: null
                                            ),
                                            IfFalse: new ArgumentOperator(
                                                Index: 1,
                                                SupplementaryText: null
                                            ),
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new ArgumentOperator(
                                            Index: 2,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 55),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.Call, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    3),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fibImpl", NumOperands: 3),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 01 */ new LowLevelOperation(Opcode.GotoIfTrue, 3),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 03 */ new LowLevelOperation(Opcode.Goto, 20),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 06 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.GotoIfTrue, 9),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 09 */ new LowLevelOperation(Opcode.Goto, 20),
                                        /* 10 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 12 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 14 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 15 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 17 */ new LowLevelOperation(Opcode.StoreArg, 1),
                                        /* 18 */ new LowLevelOperation(Opcode.StoreArg, 2),
                                        /* 19 */ new LowLevelOperation(Opcode.StoreArg, 3),
                                        /* 20 */ new LowLevelOperation(Opcode.Goto, -1),
                                        /* 21 */ new LowLevelOperation(Opcode.Return, 3)
                                    }.ToImmutableArray(),
                                    3)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[f|a,b,p,q,c|c < 2 ? ((a*p) + (b*q)) ? (c % 2 ? ((a*p) + (b*q) {f} (a*q) + (b*q) + (b*p) {f} (p*p) + (q*q) {f} (2*p+q)*q {f} c/2) ? (a {f} b {f} (p*p) + (q*q) {f} (2*p+q)*q {f} c/2))] D[fib|n|0{f}1{f}0{f}1{f}n] 10{fib}",
            ExpectedValue: 55,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new UserDefinedOperator(
                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                        Operands: new IOperator[]
                        {
                            new DecimalOperator(
                                Operand: new DecimalOperator(
                                    Operand: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new DefineOperator(
                                                SupplementaryText: "f|a,b,p,q,c|c < 2 ? ((a*p) + (b*q)) ? (c % 2 ? ((a*p) + (b*q) {f} (a*q) + (b*q) + (b*p) {f} (p*p) + (q*q) {f} (2*p+q)*q {f} c/2) ? (a {f} b {f} (p*p) + (q*q) {f} (2*p+q)*q {f} c/2))"
                                            ),
                                            new DefineOperator(
                                                SupplementaryText: "fib|n|0{f}1{f}0{f}1{f}n"
                                            )
                                        }.ToImmutableArray(),
                                        SupplementaryText: null
                                    ),
                                    Value: 1,
                                    SupplementaryText: null
                                ),
                                Value: 0,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        IsTailCall: null,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "f", NumOperands: 5),
                                    IsOptimized: false,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 4,
                                                SupplementaryText: null
                                            ),
                                            Right: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 2,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.LessThan,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new BinaryOperator(
                                            Left: new BinaryOperator(
                                                Left: new ArgumentOperator(
                                                    Index: 0,
                                                    SupplementaryText: null
                                                ),
                                                Right: new ArgumentOperator(
                                                    Index: 2,
                                                    SupplementaryText: null
                                                ),
                                                Type: BinaryType.Mult,
                                                SupplementaryText: null
                                            ),
                                            Right: new BinaryOperator(
                                                Left: new ArgumentOperator(
                                                    Index: 1,
                                                    SupplementaryText: null
                                                ),
                                                Right: new ArgumentOperator(
                                                    Index: 3,
                                                    SupplementaryText: null
                                                ),
                                                Type: BinaryType.Mult,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new ConditionalOperator(
                                            Condition: new BinaryOperator(
                                                Left: new ArgumentOperator(
                                                    Index: 4,
                                                    SupplementaryText: null
                                                ),
                                                Right: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 2,
                                                    SupplementaryText: null
                                                ),
                                                Type: BinaryType.Mod,
                                                SupplementaryText: null
                                            ),
                                            IfTrue: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "f", NumOperands: 5),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 0,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Add,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new BinaryOperator(
                                                                Left: new ArgumentOperator(
                                                                    Index: 0,
                                                                    SupplementaryText: null
                                                                ),
                                                                Right: new ArgumentOperator(
                                                                    Index: 3,
                                                                    SupplementaryText: null
                                                                ),
                                                                Type: BinaryType.Mult,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new BinaryOperator(
                                                                Left: new ArgumentOperator(
                                                                    Index: 1,
                                                                    SupplementaryText: null
                                                                ),
                                                                Right: new ArgumentOperator(
                                                                    Index: 3,
                                                                    SupplementaryText: null
                                                                ),
                                                                Type: BinaryType.Mult,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Add,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Add,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Add,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new BinaryOperator(
                                                                Left: new DecimalOperator(
                                                                    Operand: new ZeroOperator(),
                                                                    Value: 2,
                                                                    SupplementaryText: null
                                                                ),
                                                                Right: new ArgumentOperator(
                                                                    Index: 2,
                                                                    SupplementaryText: null
                                                                ),
                                                                Type: BinaryType.Mult,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Add,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new ArgumentOperator(
                                                            Index: 3,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Mult,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 4,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Div,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            IfFalse: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "f", NumOperands: 5),
                                                Operands: new IOperator[]
                                                {
                                                    new ArgumentOperator(
                                                        Index: 0,
                                                        SupplementaryText: null
                                                    ),
                                                    new ArgumentOperator(
                                                        Index: 1,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Add,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new BinaryOperator(
                                                                Left: new DecimalOperator(
                                                                    Operand: new ZeroOperator(),
                                                                    Value: 2,
                                                                    SupplementaryText: null
                                                                ),
                                                                Right: new ArgumentOperator(
                                                                    Index: 2,
                                                                    SupplementaryText: null
                                                                ),
                                                                Type: BinaryType.Mult,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Add,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new ArgumentOperator(
                                                            Index: 3,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Mult,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 4,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Div,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "f", NumOperands: 5),
                                        Operands: new IOperator[]
                                        {
                                            new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: null,
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 04 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 06 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 08 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 11 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 12 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "f", NumOperands: 5),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 03 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 05 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.GotoIfLessThan, 88),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.Mod, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.GotoIfTrue, 43),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadArg, 5),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadArg, 4),
                                        /* 17 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 18 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 19 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 20 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 21 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 24 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 27 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 29 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 30 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 31 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 32 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 33 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 34 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 35 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 36 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 37 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 38 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 39 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 40 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 41 */ new LowLevelOperation(Opcode.Div, 0),
                                        /* 42 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 43 */ new LowLevelOperation(Opcode.Goto, 87),
                                        /* 44 */ new LowLevelOperation(Opcode.LoadArg, 5),
                                        /* 45 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 46 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 47 */ new LowLevelOperation(Opcode.LoadArg, 4),
                                        /* 48 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 49 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 50 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 51 */ new LowLevelOperation(Opcode.LoadArg, 5),
                                        /* 52 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 53 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 54 */ new LowLevelOperation(Opcode.LoadArg, 4),
                                        /* 55 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 56 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 57 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 58 */ new LowLevelOperation(Opcode.LoadArg, 4),
                                        /* 59 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 60 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 61 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 62 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 63 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 64 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 65 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 66 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 67 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 68 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 69 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 70 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 71 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 72 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 73 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 74 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 75 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 76 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 77 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 78 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 79 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 80 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 81 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 82 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 83 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 84 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 85 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 86 */ new LowLevelOperation(Opcode.Div, 0),
                                        /* 87 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 88 */ new LowLevelOperation(Opcode.Goto, 95),
                                        /* 89 */ new LowLevelOperation(Opcode.LoadArg, 5),
                                        /* 90 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 91 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 92 */ new LowLevelOperation(Opcode.LoadArg, 4),
                                        /* 93 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 94 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 95 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 96 */ new LowLevelOperation(Opcode.Return, 5)
                                    }.ToImmutableArray(),
                                    7),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 12 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 17 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 19 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 20 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 21 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 22 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    5)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 55
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "f", NumOperands: 5),
                                    IsOptimized: true,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 4,
                                                SupplementaryText: null
                                            ),
                                            Right: new PreComputedOperator(
                                                Value: 2
                                            ),
                                            Type: BinaryType.LessThan,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new BinaryOperator(
                                            Left: new BinaryOperator(
                                                Left: new ArgumentOperator(
                                                    Index: 0,
                                                    SupplementaryText: null
                                                ),
                                                Right: new ArgumentOperator(
                                                    Index: 2,
                                                    SupplementaryText: null
                                                ),
                                                Type: BinaryType.Mult,
                                                SupplementaryText: null
                                            ),
                                            Right: new BinaryOperator(
                                                Left: new ArgumentOperator(
                                                    Index: 1,
                                                    SupplementaryText: null
                                                ),
                                                Right: new ArgumentOperator(
                                                    Index: 3,
                                                    SupplementaryText: null
                                                ),
                                                Type: BinaryType.Mult,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new ConditionalOperator(
                                            Condition: new BinaryOperator(
                                                Left: new ArgumentOperator(
                                                    Index: 4,
                                                    SupplementaryText: null
                                                ),
                                                Right: new PreComputedOperator(
                                                    Value: 2
                                                ),
                                                Type: BinaryType.Mod,
                                                SupplementaryText: null
                                            ),
                                            IfTrue: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "f", NumOperands: 5),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 0,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Add,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new BinaryOperator(
                                                                Left: new ArgumentOperator(
                                                                    Index: 0,
                                                                    SupplementaryText: null
                                                                ),
                                                                Right: new ArgumentOperator(
                                                                    Index: 3,
                                                                    SupplementaryText: null
                                                                ),
                                                                Type: BinaryType.Mult,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new BinaryOperator(
                                                                Left: new ArgumentOperator(
                                                                    Index: 1,
                                                                    SupplementaryText: null
                                                                ),
                                                                Right: new ArgumentOperator(
                                                                    Index: 3,
                                                                    SupplementaryText: null
                                                                ),
                                                                Type: BinaryType.Mult,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Add,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Add,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Add,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new BinaryOperator(
                                                                Left: new PreComputedOperator(
                                                                    Value: 2
                                                                ),
                                                                Right: new ArgumentOperator(
                                                                    Index: 2,
                                                                    SupplementaryText: null
                                                                ),
                                                                Type: BinaryType.Mult,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Add,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new ArgumentOperator(
                                                            Index: 3,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Mult,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 4,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 2
                                                        ),
                                                        Type: BinaryType.Div,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: true,
                                                SupplementaryText: null
                                            ),
                                            IfFalse: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "f", NumOperands: 5),
                                                Operands: new IOperator[]
                                                {
                                                    new ArgumentOperator(
                                                        Index: 0,
                                                        SupplementaryText: null
                                                    ),
                                                    new ArgumentOperator(
                                                        Index: 1,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Mult,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Add,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new BinaryOperator(
                                                            Left: new BinaryOperator(
                                                                Left: new PreComputedOperator(
                                                                    Value: 2
                                                                ),
                                                                Right: new ArgumentOperator(
                                                                    Index: 2,
                                                                    SupplementaryText: null
                                                                ),
                                                                Type: BinaryType.Mult,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new ArgumentOperator(
                                                                Index: 3,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Add,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new ArgumentOperator(
                                                            Index: 3,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Mult,
                                                        SupplementaryText: null
                                                    ),
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 4,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 2
                                                        ),
                                                        Type: BinaryType.Div,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: true,
                                                SupplementaryText: null
                                            ),
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "f", NumOperands: 5),
                                        Operands: new IOperator[]
                                        {
                                            new PreComputedOperator(
                                                Value: 0
                                            ),
                                            new PreComputedOperator(
                                                Value: 1
                                            ),
                                            new PreComputedOperator(
                                                Value: 0
                                            ),
                                            new PreComputedOperator(
                                                Value: 1
                                            ),
                                            new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: true,
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 55),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "f", NumOperands: 5),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 02 */ new LowLevelOperation(Opcode.GotoIfLessThan, 72),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 05 */ new LowLevelOperation(Opcode.Mod, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.GotoIfTrue, 31),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 5),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadArg, 4),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 10 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 11 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 14 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 17 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 18 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 20 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 24 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 25 */ new LowLevelOperation(Opcode.Div, 0),
                                        /* 26 */ new LowLevelOperation(Opcode.StoreArg, 1),
                                        /* 27 */ new LowLevelOperation(Opcode.StoreArg, 2),
                                        /* 28 */ new LowLevelOperation(Opcode.StoreArg, 3),
                                        /* 29 */ new LowLevelOperation(Opcode.StoreArg, 4),
                                        /* 30 */ new LowLevelOperation(Opcode.StoreArg, 5),
                                        /* 31 */ new LowLevelOperation(Opcode.Goto, -1),
                                        /* 32 */ new LowLevelOperation(Opcode.LoadArg, 5),
                                        /* 33 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 34 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 35 */ new LowLevelOperation(Opcode.LoadArg, 4),
                                        /* 36 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 37 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 38 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 39 */ new LowLevelOperation(Opcode.LoadArg, 5),
                                        /* 40 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 41 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 42 */ new LowLevelOperation(Opcode.LoadArg, 4),
                                        /* 43 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 44 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 45 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 46 */ new LowLevelOperation(Opcode.LoadArg, 4),
                                        /* 47 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 48 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 49 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 50 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 51 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 52 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 53 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 54 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 55 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 56 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 57 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 58 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 59 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 60 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 61 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 62 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 63 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 64 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 65 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 66 */ new LowLevelOperation(Opcode.Div, 0),
                                        /* 67 */ new LowLevelOperation(Opcode.StoreArg, 1),
                                        /* 68 */ new LowLevelOperation(Opcode.StoreArg, 2),
                                        /* 69 */ new LowLevelOperation(Opcode.StoreArg, 3),
                                        /* 70 */ new LowLevelOperation(Opcode.StoreArg, 4),
                                        /* 71 */ new LowLevelOperation(Opcode.StoreArg, 5),
                                        /* 72 */ new LowLevelOperation(Opcode.Goto, -1),
                                        /* 73 */ new LowLevelOperation(Opcode.LoadArg, 5),
                                        /* 74 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 75 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 76 */ new LowLevelOperation(Opcode.LoadArg, 4),
                                        /* 77 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 78 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 79 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 80 */ new LowLevelOperation(Opcode.Return, 5)
                                    }.ToImmutableArray(),
                                    6),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    5)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: new[] { typeof(Double) }
        ),
        new TestCase(
            Source: "D[tarai|x,y,z|x <= y ? y ? (((x - 1){tarai}y{tarai}z){tarai}((y - 1){tarai}z{tarai}x){tarai}((z - 1){tarai}x{tarai}y))] 10{tarai}5{tarai}5",
            ExpectedValue: 5,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new UserDefinedOperator(
                        Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                        Operands: new IOperator[]
                        {
                            new DecimalOperator(
                                Operand: new DecimalOperator(
                                    Operand: new DefineOperator(
                                        SupplementaryText: "tarai|x,y,z|x <= y ? y ? (((x - 1){tarai}y{tarai}z){tarai}((y - 1){tarai}z{tarai}x){tarai}((z - 1){tarai}x{tarai}y))"
                                    ),
                                    Value: 1,
                                    SupplementaryText: null
                                ),
                                Value: 0,
                                SupplementaryText: null
                            ),
                            new DecimalOperator(
                                Operand: new ZeroOperator(),
                                Value: 5,
                                SupplementaryText: null
                            ),
                            new DecimalOperator(
                                Operand: new ZeroOperator(),
                                Value: 5,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        IsTailCall: null,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                    IsOptimized: false,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new ArgumentOperator(
                                                Index: 1,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.LessThanOrEqual,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 1,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new UserDefinedOperator(
                                            Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                            Operands: new IOperator[]
                                            {
                                                new UserDefinedOperator(
                                                    Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                                    Operands: new IOperator[]
                                                    {
                                                        new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 0,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new DecimalOperator(
                                                                Operand: new ZeroOperator(),
                                                                Value: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Sub,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 2,
                                                            SupplementaryText: null
                                                        )
                                                    }.ToImmutableArray(),
                                                    IsTailCall: null,
                                                    SupplementaryText: null
                                                ),
                                                new UserDefinedOperator(
                                                    Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                                    Operands: new IOperator[]
                                                    {
                                                        new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new DecimalOperator(
                                                                Operand: new ZeroOperator(),
                                                                Value: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Sub,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        )
                                                    }.ToImmutableArray(),
                                                    IsTailCall: null,
                                                    SupplementaryText: null
                                                ),
                                                new UserDefinedOperator(
                                                    Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                                    Operands: new IOperator[]
                                                    {
                                                        new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new DecimalOperator(
                                                                Operand: new ZeroOperator(),
                                                                Value: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Type: BinaryType.Sub,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 1,
                                                            SupplementaryText: null
                                                        )
                                                    }.ToImmutableArray(),
                                                    IsTailCall: null,
                                                    SupplementaryText: null
                                                )
                                            }.ToImmutableArray(),
                                            IsTailCall: null,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 10 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 11 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 12 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                /* 13 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 14 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 15 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 16 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 17 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                /* 18 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 19 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 20 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 02 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 34),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 12 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 14 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 16 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 18 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 20 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 21 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 22 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 24 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 27 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 29 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 30 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 31 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 32 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 33 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 34 */ new LowLevelOperation(Opcode.Goto, 35),
                                        /* 35 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 36 */ new LowLevelOperation(Opcode.Return, 3)
                                    }.ToImmutableArray(),
                                    5)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 5
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                    IsOptimized: true,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new ArgumentOperator(
                                                Index: 1,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.LessThanOrEqual,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 1,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new UserDefinedOperator(
                                            Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                            Operands: new IOperator[]
                                            {
                                                new UserDefinedOperator(
                                                    Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                                    Operands: new IOperator[]
                                                    {
                                                        new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 0,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new PreComputedOperator(
                                                                Value: 1
                                                            ),
                                                            Type: BinaryType.Sub,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 2,
                                                            SupplementaryText: null
                                                        )
                                                    }.ToImmutableArray(),
                                                    IsTailCall: false,
                                                    SupplementaryText: null
                                                ),
                                                new UserDefinedOperator(
                                                    Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                                    Operands: new IOperator[]
                                                    {
                                                        new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new PreComputedOperator(
                                                                Value: 1
                                                            ),
                                                            Type: BinaryType.Sub,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        )
                                                    }.ToImmutableArray(),
                                                    IsTailCall: false,
                                                    SupplementaryText: null
                                                ),
                                                new UserDefinedOperator(
                                                    Definition: new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                                    Operands: new IOperator[]
                                                    {
                                                        new BinaryOperator(
                                                            Left: new ArgumentOperator(
                                                                Index: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Right: new PreComputedOperator(
                                                                Value: 1
                                                            ),
                                                            Type: BinaryType.Sub,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        new ArgumentOperator(
                                                            Index: 1,
                                                            SupplementaryText: null
                                                        )
                                                    }.ToImmutableArray(),
                                                    IsTailCall: false,
                                                    SupplementaryText: null
                                                )
                                            }.ToImmutableArray(),
                                            IsTailCall: true,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "tarai", NumOperands: 3),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 02 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 24),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 10 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 11 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 14 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 17 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.LoadArg, 3),
                                        /* 19 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 20 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.StoreArg, 1),
                                        /* 22 */ new LowLevelOperation(Opcode.StoreArg, 2),
                                        /* 23 */ new LowLevelOperation(Opcode.StoreArg, 3),
                                        /* 24 */ new LowLevelOperation(Opcode.Goto, -1),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadArg, 2),
                                        /* 26 */ new LowLevelOperation(Opcode.Return, 3)
                                    }.ToImmutableArray(),
                                    5)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "1S",
            ExpectedValue: 1,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new StoreOperator(
                        Operand: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 06 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new StoreOperator(
                        Operand: new PreComputedOperator(
                            Value: 1
                        ),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "L",
            ExpectedValue: 0,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new LoadOperator(
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 0
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "1S[var]",
            ExpectedValue: 1,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new StoreOperator(
                        Operand: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 1,
                            SupplementaryText: null
                        ),
                        SupplementaryText: "var"
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 06 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                "var"
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new StoreOperator(
                        Operand: new PreComputedOperator(
                            Value: 1
                        ),
                        SupplementaryText: "var"
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                "var"
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "L[var]",
            ExpectedValue: 0,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new LoadOperator(
                        SupplementaryText: "var"
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                "var"
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 0
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[set|x|xS] 7{set}L",
            ExpectedValue: 7,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                Operands: new IOperator[]
                                {
                                    new DecimalOperator(
                                        Operand: new DefineOperator(
                                            SupplementaryText: "set|x|xS"
                                        ),
                                        Value: 7,
                                        SupplementaryText: null
                                    )
                                }.ToImmutableArray(),
                                IsTailCall: null,
                                SupplementaryText: null
                            ),
                            new LoadOperator(
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreOperator(
                                        Operand: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 7),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 06 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "set", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 7
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreOperator(
                                        Operand: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 7),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "set", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[set|x|xS] 7{set}LS[var1] L[zero]3{set}LS[var2] L[var1]*L[var2]",
            ExpectedValue: 21,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new UserDefinedOperator(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    Operands: new IOperator[]
                                    {
                                        new DecimalOperator(
                                            Operand: new DefineOperator(
                                                SupplementaryText: "set|x|xS"
                                            ),
                                            Value: 7,
                                            SupplementaryText: null
                                        )
                                    }.ToImmutableArray(),
                                    IsTailCall: null,
                                    SupplementaryText: null
                                ),
                                new StoreOperator(
                                    Operand: new LoadOperator(
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: "var1"
                                ),
                                new UserDefinedOperator(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    Operands: new IOperator[]
                                    {
                                        new DecimalOperator(
                                            Operand: new LoadOperator(
                                                SupplementaryText: "zero"
                                            ),
                                            Value: 3,
                                            SupplementaryText: null
                                        )
                                    }.ToImmutableArray(),
                                    IsTailCall: null,
                                    SupplementaryText: null
                                ),
                                new StoreOperator(
                                    Operand: new LoadOperator(
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: "var2"
                                ),
                                new LoadOperator(
                                    SupplementaryText: "var1"
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadOperator(
                            SupplementaryText: "var2"
                        ),
                        Type: BinaryType.Mult,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreOperator(
                                        Operand: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 7),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 06 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 08 */ new LowLevelOperation(Opcode.StoreVariable, 1),
                                /* 09 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 10 */ new LowLevelOperation(Opcode.LoadVariable, 2),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 12 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 14 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 15 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 16 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 17 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 18 */ new LowLevelOperation(Opcode.StoreVariable, 3),
                                /* 19 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 20 */ new LowLevelOperation(Opcode.LoadVariable, 1),
                                /* 21 */ new LowLevelOperation(Opcode.LoadVariable, 3),
                                /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 23 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "set", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null,
                                "var1",
                                "zero",
                                "var2"
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 21
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreOperator(
                                        Operand: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 21),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "set", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "(123S)L*L",
            ExpectedValue: 15129,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new StoreOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            Value: 2,
                                            SupplementaryText: null
                                        ),
                                        Value: 3,
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: null
                                ),
                                new LoadOperator(
                                    SupplementaryText: null
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadOperator(
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Mult,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 15 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 16 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 17 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 18 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 15129
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 15129),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "(123S[var])L[var]*L[var]",
            ExpectedValue: 15129,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new StoreOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            Value: 2,
                                            SupplementaryText: null
                                        ),
                                        Value: 3,
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: "var"
                                ),
                                new LoadOperator(
                                    SupplementaryText: "var"
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadOperator(
                            SupplementaryText: "var"
                        ),
                        Type: BinaryType.Mult,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 15 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 16 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 17 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 18 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                "var"
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 15129
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 15129),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "((100+20+3)S)L*L",
            ExpectedValue: 15129,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new StoreOperator(
                                    Operand: new BinaryOperator(
                                        Left: new BinaryOperator(
                                            Left: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new DecimalOperator(
                                                        Operand: new ZeroOperator(),
                                                        Value: 1,
                                                        SupplementaryText: null
                                                    ),
                                                    Value: 0,
                                                    SupplementaryText: null
                                                ),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 2,
                                                    SupplementaryText: null
                                                ),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        Right: new DecimalOperator(
                                            Operand: new ZeroOperator(),
                                            Value: 3,
                                            SupplementaryText: null
                                        ),
                                        Type: BinaryType.Add,
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: null
                                ),
                                new LoadOperator(
                                    SupplementaryText: null
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadOperator(
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Mult,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 14 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 15 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 16 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 17 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 18 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 19 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 20 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 21 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 22 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 23 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 24 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 25 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 26 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 27 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 29 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 30 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 31 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 32 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 33 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 34 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 15129
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 15129),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "((100+20+3)S[var])L[var]*L[var]",
            ExpectedValue: 15129,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new StoreOperator(
                                    Operand: new BinaryOperator(
                                        Left: new BinaryOperator(
                                            Left: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new DecimalOperator(
                                                        Operand: new ZeroOperator(),
                                                        Value: 1,
                                                        SupplementaryText: null
                                                    ),
                                                    Value: 0,
                                                    SupplementaryText: null
                                                ),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 2,
                                                    SupplementaryText: null
                                                ),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        Right: new DecimalOperator(
                                            Operand: new ZeroOperator(),
                                            Value: 3,
                                            SupplementaryText: null
                                        ),
                                        Type: BinaryType.Add,
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: "var"
                                ),
                                new LoadOperator(
                                    SupplementaryText: "var"
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadOperator(
                            SupplementaryText: "var"
                        ),
                        Type: BinaryType.Mult,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 14 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 15 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 16 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 17 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 18 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 19 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 20 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 21 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 22 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 23 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 24 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 25 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 26 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 27 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 29 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 30 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 31 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 32 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 33 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 34 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                "var"
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 15129
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 15129),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[op||(123S)L*L]{op}",
            ExpectedValue: 15129,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new DefineOperator(
                                SupplementaryText: "op||(123S)L*L"
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "op", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: null,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "op", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new BinaryOperator(
                                        Left: new ParenthesisOperator(
                                            Operators: new IOperator[]
                                            {
                                                new StoreOperator(
                                                    Operand: new DecimalOperator(
                                                        Operand: new DecimalOperator(
                                                            Operand: new DecimalOperator(
                                                                Operand: new ZeroOperator(),
                                                                Value: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Value: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        Value: 3,
                                                        SupplementaryText: null
                                                    ),
                                                    SupplementaryText: null
                                                ),
                                                new LoadOperator(
                                                    SupplementaryText: null
                                                )
                                            }.ToImmutableArray(),
                                            SupplementaryText: null
                                        ),
                                        Right: new LoadOperator(
                                            SupplementaryText: null
                                        ),
                                        Type: BinaryType.Mult,
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "op", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                        /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    2)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new PreComputedOperator(
                        Value: 15129
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "op", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new PreComputedOperator(
                                        Value: 15129
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 15129),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "op", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 15129),
                                        /* 01 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[op||L*L](123S){op}",
            ExpectedValue: 15129,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new DefineOperator(
                                SupplementaryText: "op||L*L"
                            ),
                            new StoreOperator(
                                Operand: new DecimalOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new DecimalOperator(
                                            Operand: new ZeroOperator(),
                                            Value: 1,
                                            SupplementaryText: null
                                        ),
                                        Value: 2,
                                        SupplementaryText: null
                                    ),
                                    Value: 3,
                                    SupplementaryText: null
                                ),
                                SupplementaryText: null
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "op", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: null,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "op", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new BinaryOperator(
                                        Left: new LoadOperator(
                                            SupplementaryText: null
                                        ),
                                        Right: new LoadOperator(
                                            SupplementaryText: null
                                        ),
                                        Type: BinaryType.Mult,
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 04 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 06 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 08 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 12 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 14 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 15 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 16 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 17 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 18 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "op", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    2)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreOperator(
                                Operand: new PreComputedOperator(
                                    Value: 123
                                ),
                                SupplementaryText: null
                            ),
                            new PreComputedOperator(
                                Value: 15129
                            )
                        }.ToImmutableArray(),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "op", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new BinaryOperator(
                                        Left: new LoadOperator(
                                            SupplementaryText: null
                                        ),
                                        Right: new LoadOperator(
                                            SupplementaryText: null
                                        ),
                                        Type: BinaryType.Mult,
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 123),
                                /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 15129),
                                /* 04 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "op", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    2)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] (20{fib}S)+L",
            ExpectedValue: 13530,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new DefineOperator(
                                    SupplementaryText: "fib|n|n<=1?n?((n-1){fib}+(n-2){fib})"
                                ),
                                new StoreOperator(
                                    Operand: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 2,
                                                    SupplementaryText: null
                                                ),
                                                Value: 0,
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: null,
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: null
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadOperator(
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Add,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.LessThanOrEqual,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new BinaryOperator(
                                            Left: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            Right: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 04 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 06 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 08 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 11 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 12 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 15 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 03 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 24),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 18 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 20 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 22 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 24 */ new LowLevelOperation(Opcode.Goto, 25),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 26 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    4)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new StoreOperator(
                            Operand: new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                Operands: new IOperator[]
                                {
                                    new PreComputedOperator(
                                        Value: 20
                                    )
                                }.ToImmutableArray(),
                                IsTailCall: true,
                                SupplementaryText: null
                            ),
                            SupplementaryText: null
                        ),
                        Right: new LoadOperator(
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Add,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new PreComputedOperator(
                                                Value: 1
                                            ),
                                            Type: BinaryType.LessThanOrEqual,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new BinaryOperator(
                                            Left: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 1
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: false,
                                                SupplementaryText: null
                                            ),
                                            Right: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 2
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: false,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 02 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 02 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 12),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 09 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 14 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    3)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[get||L] D[set|x|xS] D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] (20{fib}>=1000?10?5)S {get}",
            ExpectedValue: 10,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreOperator(
                                Operand: new ParenthesisOperator(
                                    Operators: new IOperator[]
                                    {
                                        new DefineOperator(
                                            SupplementaryText: "get||L"
                                        ),
                                        new DefineOperator(
                                            SupplementaryText: "set|x|xS"
                                        ),
                                        new DefineOperator(
                                            SupplementaryText: "fib|n|n<=1?n?((n-1){fib}+(n-2){fib})"
                                        ),
                                        new ConditionalOperator(
                                            Condition: new BinaryOperator(
                                                Left: new UserDefinedOperator(
                                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                    Operands: new IOperator[]
                                                    {
                                                        new DecimalOperator(
                                                            Operand: new DecimalOperator(
                                                                Operand: new ZeroOperator(),
                                                                Value: 2,
                                                                SupplementaryText: null
                                                            ),
                                                            Value: 0,
                                                            SupplementaryText: null
                                                        )
                                                    }.ToImmutableArray(),
                                                    IsTailCall: null,
                                                    SupplementaryText: null
                                                ),
                                                Right: new DecimalOperator(
                                                    Operand: new DecimalOperator(
                                                        Operand: new DecimalOperator(
                                                            Operand: new DecimalOperator(
                                                                Operand: new ZeroOperator(),
                                                                Value: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Value: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Value: 0,
                                                        SupplementaryText: null
                                                    ),
                                                    Value: 0,
                                                    SupplementaryText: null
                                                ),
                                                Type: BinaryType.GreaterThanOrEqual,
                                                SupplementaryText: null
                                            ),
                                            IfTrue: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 1,
                                                    SupplementaryText: null
                                                ),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            IfFalse: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 5,
                                                SupplementaryText: null
                                            ),
                                            SupplementaryText: null
                                        )
                                    }.ToImmutableArray(),
                                    SupplementaryText: null
                                ),
                                SupplementaryText: null
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: null,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.LessThanOrEqual,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new BinaryOperator(
                                            Left: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            Right: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new LoadOperator(
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreOperator(
                                        Operand: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 04 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 05 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 08 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 12 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 15 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 17 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 18 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 19 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 20 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 21 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 23 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 24 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 27 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 29 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 30 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 31 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 32 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 33 */ new LowLevelOperation(Opcode.GotoIfLessThan, 43),
                                /* 34 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 35 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 36 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 37 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 38 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 39 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 40 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 41 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 42 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 43 */ new LowLevelOperation(Opcode.Goto, 48),
                                /* 44 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 45 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 46 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 47 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                /* 48 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 49 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 50 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 51 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 52 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 03 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 24),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 18 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 20 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 22 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 24 */ new LowLevelOperation(Opcode.Goto, 25),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 26 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    4),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "get", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    1),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "set", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreOperator(
                                Operand: new ConditionalOperator(
                                    Condition: new BinaryOperator(
                                        Left: new UserDefinedOperator(
                                            Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                            Operands: new IOperator[]
                                            {
                                                new PreComputedOperator(
                                                    Value: 20
                                                )
                                            }.ToImmutableArray(),
                                            IsTailCall: false,
                                            SupplementaryText: null
                                        ),
                                        Right: new PreComputedOperator(
                                            Value: 1000
                                        ),
                                        Type: BinaryType.GreaterThanOrEqual,
                                        SupplementaryText: null
                                    ),
                                    IfTrue: new PreComputedOperator(
                                        Value: 10
                                    ),
                                    IfFalse: new PreComputedOperator(
                                        Value: 5
                                    ),
                                    SupplementaryText: null
                                ),
                                SupplementaryText: null
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: true,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new PreComputedOperator(
                                                Value: 1
                                            ),
                                            Type: BinaryType.LessThanOrEqual,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new BinaryOperator(
                                            Left: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 1
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: false,
                                                SupplementaryText: null
                                            ),
                                            Right: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 2
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: false,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new LoadOperator(
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreOperator(
                                        Operand: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 1000),
                                /* 03 */ new LowLevelOperation(Opcode.GotoIfLessThan, 5),
                                /* 04 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 05 */ new LowLevelOperation(Opcode.Goto, 6),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                /* 07 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 09 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 10 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 02 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 12),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 09 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 14 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    3),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "get", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    1),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "set", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[get||L] D[set|x|xS] D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] (20{fib}>=1000?10S?5S) {get}",
            ExpectedValue: 10,
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new DefineOperator(
                                SupplementaryText: "get||L"
                            ),
                            new DefineOperator(
                                SupplementaryText: "set|x|xS"
                            ),
                            new DefineOperator(
                                SupplementaryText: "fib|n|n<=1?n?((n-1){fib}+(n-2){fib})"
                            ),
                            new ConditionalOperator(
                                Condition: new BinaryOperator(
                                    Left: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 2,
                                                    SupplementaryText: null
                                                ),
                                                Value: 0,
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: null,
                                        SupplementaryText: null
                                    ),
                                    Right: new DecimalOperator(
                                        Operand: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 1,
                                                    SupplementaryText: null
                                                ),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            Value: 0,
                                            SupplementaryText: null
                                        ),
                                        Value: 0,
                                        SupplementaryText: null
                                    ),
                                    Type: BinaryType.GreaterThanOrEqual,
                                    SupplementaryText: null
                                ),
                                IfTrue: new StoreOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new DecimalOperator(
                                            Operand: new ZeroOperator(),
                                            Value: 1,
                                            SupplementaryText: null
                                        ),
                                        Value: 0,
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: null
                                ),
                                IfFalse: new StoreOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new ZeroOperator(),
                                        Value: 5,
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: null
                                ),
                                SupplementaryText: null
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: null,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.LessThanOrEqual,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new BinaryOperator(
                                            Left: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            Right: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 2,
                                                            SupplementaryText: null
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: null,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new LoadOperator(
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreOperator(
                                        Operand: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 04 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 05 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 08 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 12 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 15 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 17 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 18 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 19 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 20 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 21 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 23 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 24 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 27 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 29 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 30 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 31 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 32 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 33 */ new LowLevelOperation(Opcode.GotoIfLessThan, 44),
                                /* 34 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 35 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 36 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 37 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 38 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 39 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 40 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 41 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 42 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 43 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 44 */ new LowLevelOperation(Opcode.Goto, 50),
                                /* 45 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 46 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 47 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 48 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                /* 49 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 50 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 51 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 52 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 53 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 03 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 24),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 18 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 20 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 22 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 24 */ new LowLevelOperation(Opcode.Goto, 25),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 26 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    4),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "get", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    1),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "set", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new ConditionalOperator(
                                Condition: new BinaryOperator(
                                    Left: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new PreComputedOperator(
                                                Value: 20
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: false,
                                        SupplementaryText: null
                                    ),
                                    Right: new PreComputedOperator(
                                        Value: 1000
                                    ),
                                    Type: BinaryType.GreaterThanOrEqual,
                                    SupplementaryText: null
                                ),
                                IfTrue: new StoreOperator(
                                    Operand: new PreComputedOperator(
                                        Value: 10
                                    ),
                                    SupplementaryText: null
                                ),
                                IfFalse: new StoreOperator(
                                    Operand: new PreComputedOperator(
                                        Value: 5
                                    ),
                                    SupplementaryText: null
                                ),
                                SupplementaryText: null
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: true,
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new ConditionalOperator(
                                        Condition: new BinaryOperator(
                                            Left: new ArgumentOperator(
                                                Index: 0,
                                                SupplementaryText: null
                                            ),
                                            Right: new PreComputedOperator(
                                                Value: 1
                                            ),
                                            Type: BinaryType.LessThanOrEqual,
                                            SupplementaryText: null
                                        ),
                                        IfTrue: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        IfFalse: new BinaryOperator(
                                            Left: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 1
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: false,
                                                SupplementaryText: null
                                            ),
                                            Right: new UserDefinedOperator(
                                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                                Operands: new IOperator[]
                                                {
                                                    new BinaryOperator(
                                                        Left: new ArgumentOperator(
                                                            Index: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Right: new PreComputedOperator(
                                                            Value: 2
                                                        ),
                                                        Type: BinaryType.Sub,
                                                        SupplementaryText: null
                                                    )
                                                }.ToImmutableArray(),
                                                IsTailCall: false,
                                                SupplementaryText: null
                                            ),
                                            Type: BinaryType.Add,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new LoadOperator(
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreOperator(
                                        Operand: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 1000),
                                /* 03 */ new LowLevelOperation(Opcode.GotoIfLessThan, 6),
                                /* 04 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 05 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 06 */ new LowLevelOperation(Opcode.Goto, 8),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                /* 08 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 09 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 10 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 11 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "fib", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 02 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 12),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 09 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.Goto, 13),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 14 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    3),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "get", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    1),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "set", NumOperands: 1),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 1)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                null
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
    };
}
