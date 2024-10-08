﻿#nullable disable

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
            Source: "200",
            StandardInput: "",
            ExpectedValue: 200,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new DecimalOperator(
                        Operand: new DecimalOperator(
                            Operand: new DecimalOperator(
                                Operand: new ZeroOperator(),
                                Value: 2,
                                SupplementaryText: null
                            ),
                            Value: 0,
                            SupplementaryText: null
                        ),
                        Value: 0,
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
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.Halt, 0)
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
                        Value: 200
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
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 200),
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
            Source: "1<2",
            StandardInput: "",
            ExpectedValue: 1,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 1,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 1,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 1,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 1,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 1,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 12345678,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: -1,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 3,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            Source: "72P101P108P108P111P10P",
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new PrintCharOperator(
                        Character: new DecimalOperator(
                            Operand: new DecimalOperator(
                                Operand: new PrintCharOperator(
                                    Character: new DecimalOperator(
                                        Operand: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new PrintCharOperator(
                                                    Character: new DecimalOperator(
                                                        Operand: new DecimalOperator(
                                                            Operand: new DecimalOperator(
                                                                Operand: new PrintCharOperator(
                                                                    Character: new DecimalOperator(
                                                                        Operand: new DecimalOperator(
                                                                            Operand: new DecimalOperator(
                                                                                Operand: new PrintCharOperator(
                                                                                    Character: new DecimalOperator(
                                                                                        Operand: new DecimalOperator(
                                                                                            Operand: new DecimalOperator(
                                                                                                Operand: new PrintCharOperator(
                                                                                                    Character: new DecimalOperator(
                                                                                                        Operand: new DecimalOperator(
                                                                                                            Operand: new ZeroOperator(),
                                                                                                            Value: 7,
                                                                                                            SupplementaryText: null
                                                                                                        ),
                                                                                                        Value: 2,
                                                                                                        SupplementaryText: null
                                                                                                    ),
                                                                                                    SupplementaryText: null
                                                                                                ),
                                                                                                Value: 1,
                                                                                                SupplementaryText: null
                                                                                            ),
                                                                                            Value: 0,
                                                                                            SupplementaryText: null
                                                                                        ),
                                                                                        Value: 1,
                                                                                        SupplementaryText: null
                                                                                    ),
                                                                                    SupplementaryText: null
                                                                                ),
                                                                                Value: 1,
                                                                                SupplementaryText: null
                                                                            ),
                                                                            Value: 0,
                                                                            SupplementaryText: null
                                                                        ),
                                                                        Value: 8,
                                                                        SupplementaryText: null
                                                                    ),
                                                                    SupplementaryText: null
                                                                ),
                                                                Value: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Value: 0,
                                                            SupplementaryText: null
                                                        ),
                                                        Value: 8,
                                                        SupplementaryText: null
                                                    ),
                                                    SupplementaryText: null
                                                ),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            Value: 1,
                                            SupplementaryText: null
                                        ),
                                        Value: 1,
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: null
                                ),
                                Value: 1,
                                SupplementaryText: null
                            ),
                            Value: 0,
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
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 7),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 10 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 11 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 12 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 13 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 14 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 15 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 17 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 18 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 19 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 20 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 21 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 22 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 23 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 24 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 25 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 26 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 27 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 28 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 29 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 30 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 31 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 32 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 33 */ new LowLevelOperation(Opcode.LoadConst, 8),
                                /* 34 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 35 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 36 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 37 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 38 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 39 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 40 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 41 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 42 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 43 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 44 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 45 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 46 */ new LowLevelOperation(Opcode.LoadConst, 8),
                                /* 47 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 48 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 49 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 50 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 51 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 52 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 53 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 54 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 55 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 56 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 57 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 58 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 59 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 60 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 61 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 62 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 63 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 64 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 65 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 66 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 67 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 68 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 69 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 70 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 71 */ new LowLevelOperation(Opcode.Halt, 0)
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 72
                                ),
                                SupplementaryText: null
                            ),
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 101
                                ),
                                SupplementaryText: null
                            ),
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 108
                                ),
                                SupplementaryText: null
                            ),
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 108
                                ),
                                SupplementaryText: null
                            ),
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 111
                                ),
                                SupplementaryText: null
                            ),
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 10
                                ),
                                SupplementaryText: null
                            ),
                            new PreComputedOperator(
                                Value: 0
                            )
                        }.ToImmutableArray(),
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
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 72),
                                /* 01 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 101),
                                /* 04 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 05 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 108),
                                /* 07 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 108),
                                /* 10 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 11 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 12 */ new LowLevelOperation(Opcode.LoadConst, 111),
                                /* 13 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 15 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 16 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 17 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 18 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 19 */ new LowLevelOperation(Opcode.Halt, 0)
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
            SkipTypes: null,
            ExpectedConsoleOutput: "Hello\n"
        ),
        new TestCase(
            Source: "1+// C++ style comment\n2",
            StandardInput: "",
            ExpectedValue: 3,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
                        Type: BinaryType.Add,
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
                                /* 11 */ new LowLevelOperation(Opcode.Halt, 0)
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
            Source: "1+/* C style comment*/2",
            StandardInput: "",
            ExpectedValue: 3,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
                        Type: BinaryType.Add,
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
                                /* 11 */ new LowLevelOperation(Opcode.Halt, 0)
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
            Source: "D[print||72P101P108P108P111P10P] {print}",
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new DefineOperator(
                                SupplementaryText: "print||72P101P108P108P111P10P"
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "print", NumOperands: 0),
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
                                    Definition: new OperatorDefinition(Name: "print", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new PrintCharOperator(
                                        Character: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new PrintCharOperator(
                                                    Character: new DecimalOperator(
                                                        Operand: new DecimalOperator(
                                                            Operand: new DecimalOperator(
                                                                Operand: new PrintCharOperator(
                                                                    Character: new DecimalOperator(
                                                                        Operand: new DecimalOperator(
                                                                            Operand: new DecimalOperator(
                                                                                Operand: new PrintCharOperator(
                                                                                    Character: new DecimalOperator(
                                                                                        Operand: new DecimalOperator(
                                                                                            Operand: new DecimalOperator(
                                                                                                Operand: new PrintCharOperator(
                                                                                                    Character: new DecimalOperator(
                                                                                                        Operand: new DecimalOperator(
                                                                                                            Operand: new DecimalOperator(
                                                                                                                Operand: new PrintCharOperator(
                                                                                                                    Character: new DecimalOperator(
                                                                                                                        Operand: new DecimalOperator(
                                                                                                                            Operand: new ZeroOperator(),
                                                                                                                            Value: 7,
                                                                                                                            SupplementaryText: null
                                                                                                                        ),
                                                                                                                        Value: 2,
                                                                                                                        SupplementaryText: null
                                                                                                                    ),
                                                                                                                    SupplementaryText: null
                                                                                                                ),
                                                                                                                Value: 1,
                                                                                                                SupplementaryText: null
                                                                                                            ),
                                                                                                            Value: 0,
                                                                                                            SupplementaryText: null
                                                                                                        ),
                                                                                                        Value: 1,
                                                                                                        SupplementaryText: null
                                                                                                    ),
                                                                                                    SupplementaryText: null
                                                                                                ),
                                                                                                Value: 1,
                                                                                                SupplementaryText: null
                                                                                            ),
                                                                                            Value: 0,
                                                                                            SupplementaryText: null
                                                                                        ),
                                                                                        Value: 8,
                                                                                        SupplementaryText: null
                                                                                    ),
                                                                                    SupplementaryText: null
                                                                                ),
                                                                                Value: 1,
                                                                                SupplementaryText: null
                                                                            ),
                                                                            Value: 0,
                                                                            SupplementaryText: null
                                                                        ),
                                                                        Value: 8,
                                                                        SupplementaryText: null
                                                                    ),
                                                                    SupplementaryText: null
                                                                ),
                                                                Value: 1,
                                                                SupplementaryText: null
                                                            ),
                                                            Value: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        Value: 1,
                                                        SupplementaryText: null
                                                    ),
                                                    SupplementaryText: null
                                                ),
                                                Value: 1,
                                                SupplementaryText: null
                                            ),
                                            Value: 0,
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
                                /* 02 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "print", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 7),
                                        /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 11 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 13 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 15 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 19 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 20 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 21 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 22 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 24 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 26 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 27 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 28 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 29 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 30 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 31 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 32 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 33 */ new LowLevelOperation(Opcode.LoadConst, 8),
                                        /* 34 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 35 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 36 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 37 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 38 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 39 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 40 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 41 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 42 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 43 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 44 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 45 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 46 */ new LowLevelOperation(Opcode.LoadConst, 8),
                                        /* 47 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 48 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 49 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 50 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 51 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 52 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 53 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 54 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 55 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 56 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 57 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 58 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 59 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 60 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 61 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 62 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 63 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 64 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 65 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 66 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 67 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 68 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 69 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 70 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 71 */ new LowLevelOperation(Opcode.Return, 0)
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 72
                                ),
                                SupplementaryText: null
                            ),
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 101
                                ),
                                SupplementaryText: null
                            ),
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 108
                                ),
                                SupplementaryText: null
                            ),
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 108
                                ),
                                SupplementaryText: null
                            ),
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 111
                                ),
                                SupplementaryText: null
                            ),
                            new PrintCharOperator(
                                Character: new PreComputedOperator(
                                    Value: 10
                                ),
                                SupplementaryText: null
                            ),
                            new PreComputedOperator(
                                Value: 0
                            )
                        }.ToImmutableArray(),
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "print", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new PrintCharOperator(
                                                Character: new PreComputedOperator(
                                                    Value: 72
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new PrintCharOperator(
                                                Character: new PreComputedOperator(
                                                    Value: 101
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new PrintCharOperator(
                                                Character: new PreComputedOperator(
                                                    Value: 108
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new PrintCharOperator(
                                                Character: new PreComputedOperator(
                                                    Value: 108
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new PrintCharOperator(
                                                Character: new PreComputedOperator(
                                                    Value: 111
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new PrintCharOperator(
                                                Character: new PreComputedOperator(
                                                    Value: 10
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new PreComputedOperator(
                                                Value: 0
                                            )
                                        }.ToImmutableArray(),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 72),
                                /* 01 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 101),
                                /* 04 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 05 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 108),
                                /* 07 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 108),
                                /* 10 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 11 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 12 */ new LowLevelOperation(Opcode.LoadConst, 111),
                                /* 13 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 15 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 16 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                /* 17 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 18 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 19 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "print", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 72),
                                        /* 01 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 101),
                                        /* 04 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.LoadConst, 108),
                                        /* 07 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 08 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 108),
                                        /* 10 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.LoadConst, 111),
                                        /* 13 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 16 */ new LowLevelOperation(Opcode.PrintChar, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null,
            ExpectedConsoleOutput: "Hello\n"
        ),
        new TestCase(
            Source: "D[add|x,y|x+y] 12{add}23",
            StandardInput: "",
            ExpectedValue: 35,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 24690,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 3628800,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 55,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 55,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 55,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
                                        /* 13 */ new LowLevelOperation(Opcode.ModChecked, 0),
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
                                        /* 41 */ new LowLevelOperation(Opcode.DivChecked, 0),
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
                                        /* 86 */ new LowLevelOperation(Opcode.DivChecked, 0),
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
                                        /* 05 */ new LowLevelOperation(Opcode.ModChecked, 0),
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
                                        /* 25 */ new LowLevelOperation(Opcode.DivChecked, 0),
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
                                        /* 66 */ new LowLevelOperation(Opcode.DivChecked, 0),
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
            StandardInput: "",
            ExpectedValue: 5,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
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
            StandardInput: "",
            ExpectedValue: 1,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 1),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new StoreVariableOperator(
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
                    Operator: new StoreVariableOperator(
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
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new LoadVariableOperator(
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
            StandardInput: "",
            ExpectedValue: 1,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create("var"), 1),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new StoreVariableOperator(
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
                    Operator: new StoreVariableOperator(
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
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new LoadVariableOperator(
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
            Source: "D[get||L[var]] D[set|x|xS[var]] 123{set} {get} * {get}",
            StandardInput: "",
            ExpectedValue: 15129,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create("var"), 123),
                    }
                ),
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
                                            Operand: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new ParenthesisOperator(
                                                        Operators: new IOperator[]
                                                        {
                                                            new DefineOperator(
                                                                SupplementaryText: "get||L[var]"
                                                            ),
                                                            new DefineOperator(
                                                                SupplementaryText: "set|x|xS[var]"
                                                            )
                                                        }.ToImmutableArray(),
                                                        SupplementaryText: null
                                                    ),
                                                    Value: 1,
                                                    SupplementaryText: null
                                                ),
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
                        Right: new UserDefinedOperator(
                            Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                            Operands: new IOperator[]
                            {
                            }.ToImmutableArray(),
                            IsTailCall: null,
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Mult,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new LoadVariableOperator(
                                        SupplementaryText: "var"
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreVariableOperator(
                                        Operand: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: "var"
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
                                /* 15 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 16 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 17 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 18 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 19 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 20 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
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
                                "var"
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 123
                                ),
                                SupplementaryText: "var"
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
                                    Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new LoadVariableOperator(
                                        SupplementaryText: "var"
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreVariableOperator(
                                        Operand: new ArgumentOperator(
                                            Index: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: "var"
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
                                "var"
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[set|x|xS] 7{set}L",
            StandardInput: "",
            ExpectedValue: 7,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 7),
                    }
                ),
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
                            new LoadVariableOperator(
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
                                    Operator: new StoreVariableOperator(
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 7
                                ),
                                SupplementaryText: null
                            ),
                            new PreComputedOperator(
                                Value: 7
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
                                    IsOptimized: true,
                                    Operator: new StoreVariableOperator(
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
                                /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 7),
                                /* 04 */ new LowLevelOperation(Opcode.Halt, 0)
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
            StandardInput: "",
            ExpectedValue: 21,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 3),
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create("var1"), 7),
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create("var2"), 3),
                    }
                ),
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
                                new StoreVariableOperator(
                                    Operand: new LoadVariableOperator(
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: "var1"
                                ),
                                new UserDefinedOperator(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    Operands: new IOperator[]
                                    {
                                        new DecimalOperator(
                                            Operand: new LoadVariableOperator(
                                                SupplementaryText: "zero"
                                            ),
                                            Value: 3,
                                            SupplementaryText: null
                                        )
                                    }.ToImmutableArray(),
                                    IsTailCall: null,
                                    SupplementaryText: null
                                ),
                                new StoreVariableOperator(
                                    Operand: new LoadVariableOperator(
                                        SupplementaryText: null
                                    ),
                                    SupplementaryText: "var2"
                                ),
                                new LoadVariableOperator(
                                    SupplementaryText: "var1"
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadVariableOperator(
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
                                    Operator: new StoreVariableOperator(
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 3
                                ),
                                SupplementaryText: null
                            ),
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 7
                                ),
                                SupplementaryText: "var1"
                            ),
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 3
                                ),
                                SupplementaryText: "var2"
                            ),
                            new PreComputedOperator(
                                Value: 21
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
                                    IsOptimized: true,
                                    Operator: new StoreVariableOperator(
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
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 7),
                                /* 04 */ new LowLevelOperation(Opcode.StoreVariable, 1),
                                /* 05 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 07 */ new LowLevelOperation(Opcode.StoreVariable, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 21),
                                /* 10 */ new LowLevelOperation(Opcode.Halt, 0)
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
                                "var2"
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "(123S)L*L",
            StandardInput: "",
            ExpectedValue: 15129,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 123),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new StoreVariableOperator(
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
                                new LoadVariableOperator(
                                    SupplementaryText: null
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadVariableOperator(
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
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
            Source: "(123S[var])L[var]*L[var]",
            StandardInput: "",
            ExpectedValue: 15129,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create("var"), 123),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new StoreVariableOperator(
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
                                new LoadVariableOperator(
                                    SupplementaryText: "var"
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadVariableOperator(
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 123
                                ),
                                SupplementaryText: "var"
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
            Source: "((100+20+3)S)L*L",
            StandardInput: "",
            ExpectedValue: 15129,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 123),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new StoreVariableOperator(
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
                                new LoadVariableOperator(
                                    SupplementaryText: null
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadVariableOperator(
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
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
            Source: "((100+20+3)S[var])L[var]*L[var]",
            StandardInput: "",
            ExpectedValue: 15129,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create("var"), 123),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new StoreVariableOperator(
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
                                new LoadVariableOperator(
                                    SupplementaryText: "var"
                                )
                            }.ToImmutableArray(),
                            SupplementaryText: null
                        ),
                        Right: new LoadVariableOperator(
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 123
                                ),
                                SupplementaryText: "var"
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
            Source: "D[op||(123S)L*L]{op}",
            StandardInput: "",
            ExpectedValue: 15129,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 123),
                    }
                ),
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
                                                new StoreVariableOperator(
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
                                                new LoadVariableOperator(
                                                    SupplementaryText: null
                                                )
                                            }.ToImmutableArray(),
                                            SupplementaryText: null
                                        ),
                                        Right: new LoadVariableOperator(
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
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
                                    Operator: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new StoreVariableOperator(
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
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 123),
                                        /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 15129),
                                        /* 04 */ new LowLevelOperation(Opcode.Return, 0)
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
            Source: "D[op||L*L](123S){op}",
            StandardInput: "",
            ExpectedValue: 15129,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 123),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new DefineOperator(
                                SupplementaryText: "op||L*L"
                            ),
                            new StoreVariableOperator(
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
                                        Left: new LoadVariableOperator(
                                            SupplementaryText: null
                                        ),
                                        Right: new LoadVariableOperator(
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
                            new StoreVariableOperator(
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
                                        Left: new LoadVariableOperator(
                                            SupplementaryText: null
                                        ),
                                        Right: new LoadVariableOperator(
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
            StandardInput: "",
            ExpectedValue: 13530,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 6765),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new ParenthesisOperator(
                            Operators: new IOperator[]
                            {
                                new DefineOperator(
                                    SupplementaryText: "fib|n|n<=1?n?((n-1){fib}+(n-2){fib})"
                                ),
                                new StoreVariableOperator(
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
                        Right: new LoadVariableOperator(
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
                        Left: new StoreVariableOperator(
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
                        Right: new LoadVariableOperator(
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
            StandardInput: "",
            ExpectedValue: 10,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 10),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
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
                                    Operator: new LoadVariableOperator(
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreVariableOperator(
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
                            new StoreVariableOperator(
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
                                    Operator: new LoadVariableOperator(
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreVariableOperator(
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
            StandardInput: "",
            ExpectedValue: 10,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 10),
                    }
                ),
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
                                IfTrue: new StoreVariableOperator(
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
                                IfFalse: new StoreVariableOperator(
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
                                    Operator: new LoadVariableOperator(
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreVariableOperator(
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
                                IfTrue: new StoreVariableOperator(
                                    Operand: new PreComputedOperator(
                                        Value: 10
                                    ),
                                    SupplementaryText: null
                                ),
                                IfFalse: new StoreVariableOperator(
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
                                    Operator: new LoadVariableOperator(
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreVariableOperator(
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
        new TestCase(
            Source: "D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 3{set} {fib2}",
            StandardInput: "",
            ExpectedValue: 2,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 3),
                    }
                ),
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
                                        Operand: new ParenthesisOperator(
                                            Operators: new IOperator[]
                                            {
                                                new DefineOperator(
                                                    SupplementaryText: "fib|n|n<=1?n?((n-1){fib}+(n-2){fib})"
                                                ),
                                                new DefineOperator(
                                                    SupplementaryText: "fib2||L{fib}"
                                                ),
                                                new DefineOperator(
                                                    SupplementaryText: "set|x|xS"
                                                )
                                            }.ToImmutableArray(),
                                            SupplementaryText: null
                                        ),
                                        Value: 3,
                                        SupplementaryText: null
                                    )
                                }.ToImmutableArray(),
                                IsTailCall: null,
                                SupplementaryText: null
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
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
                                    Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new LoadVariableOperator(
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: null,
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreVariableOperator(
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
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.Call, 2),
                                /* 10 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 11 */ new LowLevelOperation(Opcode.Call, 1),
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
                                    new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 0)
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
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 3
                                ),
                                SupplementaryText: null
                            ),
                            new PreComputedOperator(
                                Value: 2
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
                                    Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new LoadVariableOperator(
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: true,
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreVariableOperator(
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
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 04 */ new LowLevelOperation(Opcode.Halt, 0)
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
                                    new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 0)
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
            Source: "D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 20{set} {fib2}",
            StandardInput: "",
            ExpectedValue: 6765,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 20),
                    }
                ),
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
                                        Operand: new DecimalOperator(
                                            Operand: new ParenthesisOperator(
                                                Operators: new IOperator[]
                                                {
                                                    new DefineOperator(
                                                        SupplementaryText: "fib|n|n<=1?n?((n-1){fib}+(n-2){fib})"
                                                    ),
                                                    new DefineOperator(
                                                        SupplementaryText: "fib2||L{fib}"
                                                    ),
                                                    new DefineOperator(
                                                        SupplementaryText: "set|x|xS"
                                                    )
                                                }.ToImmutableArray(),
                                                SupplementaryText: null
                                            ),
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
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
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
                                    Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new LoadVariableOperator(
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: null,
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreVariableOperator(
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
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.Call, 2),
                                /* 14 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 15 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 16 */ new LowLevelOperation(Opcode.Halt, 0)
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
                                    new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 0)
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
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 20
                                ),
                                SupplementaryText: null
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
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
                                    Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new LoadVariableOperator(
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: true,
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreVariableOperator(
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
                                /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Halt, 0)
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
                                    new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 0)
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
            Source: "D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 3S {fib2}",
            StandardInput: "",
            ExpectedValue: 2,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 3),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
                                Operand: new DecimalOperator(
                                    Operand: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new DefineOperator(
                                                SupplementaryText: "fib|n|n<=1?n?((n-1){fib}+(n-2){fib})"
                                            ),
                                            new DefineOperator(
                                                SupplementaryText: "fib2||L{fib}"
                                            ),
                                            new DefineOperator(
                                                SupplementaryText: "set|x|xS"
                                            )
                                        }.ToImmutableArray(),
                                        SupplementaryText: null
                                    ),
                                    Value: 3,
                                    SupplementaryText: null
                                ),
                                SupplementaryText: null
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
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
                                    Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new LoadVariableOperator(
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: null,
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreVariableOperator(
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
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 10 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 11 */ new LowLevelOperation(Opcode.Call, 1),
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
                                    new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 0)
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
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 3
                                ),
                                SupplementaryText: null
                            ),
                            new PreComputedOperator(
                                Value: 2
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
                                    Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new LoadVariableOperator(
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: true,
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreVariableOperator(
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
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 04 */ new LowLevelOperation(Opcode.Halt, 0)
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
                                    new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 0)
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
            Source: "D[fib|n|n<=1?n?((n-1){fib}+(n-2){fib})] D[fib2||L{fib}] D[set|x|xS] 20S {fib2}",
            StandardInput: "",
            ExpectedValue: 6765,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 20),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreVariableOperator(
                                Operand: new DecimalOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new ParenthesisOperator(
                                            Operators: new IOperator[]
                                            {
                                                new DefineOperator(
                                                    SupplementaryText: "fib|n|n<=1?n?((n-1){fib}+(n-2){fib})"
                                                ),
                                                new DefineOperator(
                                                    SupplementaryText: "fib2||L{fib}"
                                                ),
                                                new DefineOperator(
                                                    SupplementaryText: "set|x|xS"
                                                )
                                            }.ToImmutableArray(),
                                            SupplementaryText: null
                                        ),
                                        Value: 2,
                                        SupplementaryText: null
                                    ),
                                    Value: 0,
                                    SupplementaryText: null
                                ),
                                SupplementaryText: null
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
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
                                    Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new LoadVariableOperator(
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: null,
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: false,
                                    Operator: new StoreVariableOperator(
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
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 15 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 16 */ new LowLevelOperation(Opcode.Halt, 0)
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
                                    new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 0)
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
                            new StoreVariableOperator(
                                Operand: new PreComputedOperator(
                                    Value: 20
                                ),
                                SupplementaryText: null
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
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
                                    Definition: new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new UserDefinedOperator(
                                        Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                        Operands: new IOperator[]
                                        {
                                            new LoadVariableOperator(
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        IsTailCall: true,
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "set", NumOperands: 1),
                                    IsOptimized: true,
                                    Operator: new StoreVariableOperator(
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
                                /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 04 */ new LowLevelOperation(Opcode.Halt, 0)
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
                                    new OperatorDefinition(Name: "fib2", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 0)
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
            Source: "D[fib|n|10S(n<=1?n?((n-1){fib}+(n-2){fib}))S] 20{fib} L",
            StandardInput: "",
            ExpectedValue: 6765,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                        new KeyValuePair<ValueBox<string>, Int32>(ValueBox.Create((String)null), 6765),
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "fib", NumOperands: 1),
                                Operands: new IOperator[]
                                {
                                    new DecimalOperator(
                                        Operand: new DecimalOperator(
                                            Operand: new DefineOperator(
                                                SupplementaryText: "fib|n|10S(n<=1?n?((n-1){fib}+(n-2){fib}))S"
                                            ),
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
                            new LoadVariableOperator(
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
                                    Operator: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new StoreVariableOperator(
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
                                            new StoreVariableOperator(
                                                Operand: new ConditionalOperator(
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
                                                ),
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
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
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 10 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadVariable, 0),
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
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 12 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 14 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 16 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 35),
                                        /* 18 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 19 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 20 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 21 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 22 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 23 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 24 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 25 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 26 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 27 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 28 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 29 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 30 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 31 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 32 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 33 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 34 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 35 */ new LowLevelOperation(Opcode.Goto, 36),
                                        /* 36 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 37 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 38 */ new LowLevelOperation(Opcode.Return, 1)
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new UserDefinedOperator(
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
                            new LoadVariableOperator(
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
                                    Operator: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new StoreVariableOperator(
                                                Operand: new PreComputedOperator(
                                                    Value: 10
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new StoreVariableOperator(
                                                Operand: new ConditionalOperator(
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
                                                ),
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
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
                                /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 03 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 04 */ new LowLevelOperation(Opcode.Halt, 0)
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
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 01 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 05 */ new LowLevelOperation(Opcode.GotoIfLessThanOrEqual, 15),
                                        /* 06 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 08 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 12 */ new LowLevelOperation(Opcode.Sub, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.Call, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 15 */ new LowLevelOperation(Opcode.Goto, 16),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadArg, 1),
                                        /* 17 */ new LowLevelOperation(Opcode.StoreVariable, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.Return, 1)
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
            Source: "0@",
            StandardInput: "",
            ExpectedValue: 0,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new LoadArrayOperator(
                        Index: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 0,
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
                                /* 05 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
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
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new LoadArrayOperator(
                        Index: new PreComputedOperator(
                            Value: 0
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
                                /* 01 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
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
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "5->0",
            StandardInput: "",
            ExpectedValue: 5,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new StoreArrayOperator(
                        Value: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 5,
                            SupplementaryText: null
                        ),
                        Index: new DecimalOperator(
                            Operand: new ZeroOperator(),
                            Value: 0,
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
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                /* 11 */ new LowLevelOperation(Opcode.Halt, 0)
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
                    Operator: new StoreArrayOperator(
                        Value: new PreComputedOperator(
                            Value: 5
                        ),
                        Index: new PreComputedOperator(
                            Value: 0
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
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 5),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 02 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Halt, 0)
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
            Source: "(10->20)L[zero]20@",
            StandardInput: "",
            ExpectedValue: 10,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new LoadArrayOperator(
                        Index: new DecimalOperator(
                            Operand: new DecimalOperator(
                                Operand: new ParenthesisOperator(
                                    Operators: new IOperator[]
                                    {
                                        new StoreArrayOperator(
                                            Value: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 1,
                                                    SupplementaryText: null
                                                ),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            Index: new DecimalOperator(
                                                Operand: new DecimalOperator(
                                                    Operand: new ZeroOperator(),
                                                    Value: 2,
                                                    SupplementaryText: null
                                                ),
                                                Value: 0,
                                                SupplementaryText: null
                                            ),
                                            SupplementaryText: null
                                        ),
                                        new LoadVariableOperator(
                                            SupplementaryText: "zero"
                                        )
                                    }.ToImmutableArray(),
                                    SupplementaryText: null
                                ),
                                Value: 2,
                                SupplementaryText: null
                            ),
                            Value: 0,
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
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 10 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 11 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 12 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 13 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 14 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 15 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 17 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 18 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                /* 19 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 20 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                /* 21 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 23 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 24 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 27 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 29 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                /* 30 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                            }.ToImmutableArray(),
                            new String[]
                            {
                                "zero"
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new LoadArrayOperator(
                        Index: new DecimalOperator(
                            Operand: new DecimalOperator(
                                Operand: new ParenthesisOperator(
                                    Operators: new IOperator[]
                                    {
                                        new StoreArrayOperator(
                                            Value: new PreComputedOperator(
                                                Value: 10
                                            ),
                                            Index: new PreComputedOperator(
                                                Value: 20
                                            ),
                                            SupplementaryText: null
                                        ),
                                        new PreComputedOperator(
                                            Value: 0
                                        )
                                    }.ToImmutableArray(),
                                    SupplementaryText: null
                                ),
                                Value: 2,
                                SupplementaryText: null
                            ),
                            Value: 0,
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
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                /* 02 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 04 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
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
            SkipTypes: null
        ),
        new TestCase(
            Source: "((4+6)->(10+10))(20@)",
            StandardInput: "",
            ExpectedValue: 10,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreArrayOperator(
                                Value: new BinaryOperator(
                                    Left: new DecimalOperator(
                                        Operand: new ZeroOperator(),
                                        Value: 4,
                                        SupplementaryText: null
                                    ),
                                    Right: new DecimalOperator(
                                        Operand: new ZeroOperator(),
                                        Value: 6,
                                        SupplementaryText: null
                                    ),
                                    Type: BinaryType.Add,
                                    SupplementaryText: null
                                ),
                                Index: new BinaryOperator(
                                    Left: new DecimalOperator(
                                        Operand: new DecimalOperator(
                                            Operand: new ZeroOperator(),
                                            Value: 1,
                                            SupplementaryText: null
                                        ),
                                        Value: 0,
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
                                    Type: BinaryType.Add,
                                    SupplementaryText: null
                                ),
                                SupplementaryText: null
                            ),
                            new LoadArrayOperator(
                                Index: new DecimalOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new ZeroOperator(),
                                        Value: 2,
                                        SupplementaryText: null
                                    ),
                                    Value: 0,
                                    SupplementaryText: null
                                ),
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
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
                                /* 03 */ new LowLevelOperation(Opcode.LoadConst, 4),
                                /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 08 */ new LowLevelOperation(Opcode.LoadConst, 6),
                                /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 13 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 14 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 15 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 16 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 17 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 18 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 19 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 20 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 21 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 23 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                /* 24 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 27 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 29 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 30 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                /* 31 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 32 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 33 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 34 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 35 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 36 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 37 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 38 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 39 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 40 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 41 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                /* 42 */ new LowLevelOperation(Opcode.Halt, 0)
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new StoreArrayOperator(
                                Value: new PreComputedOperator(
                                    Value: 10
                                ),
                                Index: new PreComputedOperator(
                                    Value: 20
                                ),
                                SupplementaryText: null
                            ),
                            new LoadArrayOperator(
                                Index: new PreComputedOperator(
                                    Value: 20
                                ),
                                SupplementaryText: null
                            )
                        }.ToImmutableArray(),
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
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 01 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                /* 02 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 04 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                /* 05 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
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
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[func||(10->20)L[zero]20@] {func} (20@)",
            StandardInput: "",
            ExpectedValue: 10,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new DefineOperator(
                                SupplementaryText: "func||(10->20)L[zero]20@"
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: null,
                                SupplementaryText: null
                            ),
                            new LoadArrayOperator(
                                Index: new DecimalOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new ZeroOperator(),
                                        Value: 2,
                                        SupplementaryText: null
                                    ),
                                    Value: 0,
                                    SupplementaryText: null
                                ),
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
                                    Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new LoadArrayOperator(
                                        Index: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new ParenthesisOperator(
                                                    Operators: new IOperator[]
                                                    {
                                                        new StoreArrayOperator(
                                                            Value: new DecimalOperator(
                                                                Operand: new DecimalOperator(
                                                                    Operand: new ZeroOperator(),
                                                                    Value: 1,
                                                                    SupplementaryText: null
                                                                ),
                                                                Value: 0,
                                                                SupplementaryText: null
                                                            ),
                                                            Index: new DecimalOperator(
                                                                Operand: new DecimalOperator(
                                                                    Operand: new ZeroOperator(),
                                                                    Value: 2,
                                                                    SupplementaryText: null
                                                                ),
                                                                Value: 0,
                                                                SupplementaryText: null
                                                            ),
                                                            SupplementaryText: null
                                                        ),
                                                        new LoadVariableOperator(
                                                            SupplementaryText: "zero"
                                                        )
                                                    }.ToImmutableArray(),
                                                    SupplementaryText: null
                                                ),
                                                Value: 2,
                                                SupplementaryText: null
                                            ),
                                            Value: 0,
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
                                /* 02 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 04 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "func", NumOperands: 0),
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
                                        /* 12 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 13 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 15 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 20 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 24 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 27 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 29 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 30 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    3)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                "zero"
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: false,
                                SupplementaryText: null
                            ),
                            new LoadArrayOperator(
                                Index: new PreComputedOperator(
                                    Value: 20
                                ),
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
                                    Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new LoadArrayOperator(
                                        Index: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new ParenthesisOperator(
                                                    Operators: new IOperator[]
                                                    {
                                                        new StoreArrayOperator(
                                                            Value: new PreComputedOperator(
                                                                Value: 10
                                                            ),
                                                            Index: new PreComputedOperator(
                                                                Value: 20
                                                            ),
                                                            SupplementaryText: null
                                                        ),
                                                        new LoadVariableOperator(
                                                            SupplementaryText: "zero"
                                                        )
                                                    }.ToImmutableArray(),
                                                    SupplementaryText: null
                                                ),
                                                Value: 2,
                                                SupplementaryText: null
                                            ),
                                            Value: 0,
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
                                /* 00 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                /* 03 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                /* 04 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "func", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                        /* 02 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    2)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                "zero"
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[func||((4+6)->(10+10))(20@)] {func} (20@)",
            StandardInput: "",
            ExpectedValue: 10,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new DefineOperator(
                                SupplementaryText: "func||((4+6)->(10+10))(20@)"
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: null,
                                SupplementaryText: null
                            ),
                            new LoadArrayOperator(
                                Index: new DecimalOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new ZeroOperator(),
                                        Value: 2,
                                        SupplementaryText: null
                                    ),
                                    Value: 0,
                                    SupplementaryText: null
                                ),
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
                                    Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new StoreArrayOperator(
                                                Value: new BinaryOperator(
                                                    Left: new DecimalOperator(
                                                        Operand: new ZeroOperator(),
                                                        Value: 4,
                                                        SupplementaryText: null
                                                    ),
                                                    Right: new DecimalOperator(
                                                        Operand: new ZeroOperator(),
                                                        Value: 6,
                                                        SupplementaryText: null
                                                    ),
                                                    Type: BinaryType.Add,
                                                    SupplementaryText: null
                                                ),
                                                Index: new BinaryOperator(
                                                    Left: new DecimalOperator(
                                                        Operand: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        Value: 0,
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
                                                    Type: BinaryType.Add,
                                                    SupplementaryText: null
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new LoadArrayOperator(
                                                Index: new DecimalOperator(
                                                    Operand: new DecimalOperator(
                                                        Operand: new ZeroOperator(),
                                                        Value: 2,
                                                        SupplementaryText: null
                                                    ),
                                                    Value: 0,
                                                    SupplementaryText: null
                                                ),
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
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
                                /* 03 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 04 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                /* 14 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "func", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 4),
                                        /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 6),
                                        /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 13 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 15 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 17 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 20 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 24 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 27 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 29 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 30 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                        /* 31 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 32 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 33 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 34 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 35 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 36 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 37 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 38 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 39 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 40 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 41 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 42 */ new LowLevelOperation(Opcode.Return, 0)
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: false,
                                SupplementaryText: null
                            ),
                            new LoadArrayOperator(
                                Index: new PreComputedOperator(
                                    Value: 20
                                ),
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
                                    Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new StoreArrayOperator(
                                                Value: new PreComputedOperator(
                                                    Value: 10
                                                ),
                                                Index: new PreComputedOperator(
                                                    Value: 20
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new LoadArrayOperator(
                                                Index: new PreComputedOperator(
                                                    Value: 20
                                                ),
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                /* 03 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                /* 04 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "func", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                        /* 02 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.Return, 0)
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
            Source: "D[func||(10->20)L[zero]20@] D[get||20@] {func} (20@)",
            StandardInput: "",
            ExpectedValue: 10,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new DefineOperator(
                                SupplementaryText: "func||(10->20)L[zero]20@"
                            ),
                            new DefineOperator(
                                SupplementaryText: "get||20@"
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: null,
                                SupplementaryText: null
                            ),
                            new LoadArrayOperator(
                                Index: new DecimalOperator(
                                    Operand: new DecimalOperator(
                                        Operand: new ZeroOperator(),
                                        Value: 2,
                                        SupplementaryText: null
                                    ),
                                    Value: 0,
                                    SupplementaryText: null
                                ),
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
                                    Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new LoadArrayOperator(
                                        Index: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new ParenthesisOperator(
                                                    Operators: new IOperator[]
                                                    {
                                                        new StoreArrayOperator(
                                                            Value: new DecimalOperator(
                                                                Operand: new DecimalOperator(
                                                                    Operand: new ZeroOperator(),
                                                                    Value: 1,
                                                                    SupplementaryText: null
                                                                ),
                                                                Value: 0,
                                                                SupplementaryText: null
                                                            ),
                                                            Index: new DecimalOperator(
                                                                Operand: new DecimalOperator(
                                                                    Operand: new ZeroOperator(),
                                                                    Value: 2,
                                                                    SupplementaryText: null
                                                                ),
                                                                Value: 0,
                                                                SupplementaryText: null
                                                            ),
                                                            SupplementaryText: null
                                                        ),
                                                        new LoadVariableOperator(
                                                            SupplementaryText: "zero"
                                                        )
                                                    }.ToImmutableArray(),
                                                    SupplementaryText: null
                                                ),
                                                Value: 2,
                                                SupplementaryText: null
                                            ),
                                            Value: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new LoadArrayOperator(
                                        Index: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 2,
                                                SupplementaryText: null
                                            ),
                                            Value: 0,
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
                                /* 04 */ new LowLevelOperation(Opcode.Call, 0),
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
                                /* 15 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                /* 16 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "func", NumOperands: 0),
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
                                        /* 12 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 13 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 15 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 17 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 20 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 24 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 27 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 29 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 30 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    3),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "get", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    2)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                "zero"
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: false,
                                SupplementaryText: null
                            ),
                            new LoadArrayOperator(
                                Index: new PreComputedOperator(
                                    Value: 20
                                ),
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
                                    Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new LoadArrayOperator(
                                        Index: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new ParenthesisOperator(
                                                    Operators: new IOperator[]
                                                    {
                                                        new StoreArrayOperator(
                                                            Value: new PreComputedOperator(
                                                                Value: 10
                                                            ),
                                                            Index: new PreComputedOperator(
                                                                Value: 20
                                                            ),
                                                            SupplementaryText: null
                                                        ),
                                                        new LoadVariableOperator(
                                                            SupplementaryText: "zero"
                                                        )
                                                    }.ToImmutableArray(),
                                                    SupplementaryText: null
                                                ),
                                                Value: 2,
                                                SupplementaryText: null
                                            ),
                                            Value: 0,
                                            SupplementaryText: null
                                        ),
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new LoadArrayOperator(
                                        Index: new PreComputedOperator(
                                            Value: 20
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
                                /* 00 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                /* 03 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                /* 04 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "func", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                        /* 02 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadVariable, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 10 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 13 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    2),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "get", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                                "zero"
                            }.ToImmutableArray()
                        )
                ),
            SkipTypes: null
        ),
        new TestCase(
            Source: "D[func||((4+6)->(10+10))(20@)] D[get||20@] {func} {get}",
            StandardInput: "",
            ExpectedValue: 10,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new DefineOperator(
                                SupplementaryText: "func||((4+6)->(10+10))(20@)"
                            ),
                            new DefineOperator(
                                SupplementaryText: "get||20@"
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: null,
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
                                    Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new StoreArrayOperator(
                                                Value: new BinaryOperator(
                                                    Left: new DecimalOperator(
                                                        Operand: new ZeroOperator(),
                                                        Value: 4,
                                                        SupplementaryText: null
                                                    ),
                                                    Right: new DecimalOperator(
                                                        Operand: new ZeroOperator(),
                                                        Value: 6,
                                                        SupplementaryText: null
                                                    ),
                                                    Type: BinaryType.Add,
                                                    SupplementaryText: null
                                                ),
                                                Index: new BinaryOperator(
                                                    Left: new DecimalOperator(
                                                        Operand: new DecimalOperator(
                                                            Operand: new ZeroOperator(),
                                                            Value: 1,
                                                            SupplementaryText: null
                                                        ),
                                                        Value: 0,
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
                                                    Type: BinaryType.Add,
                                                    SupplementaryText: null
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new LoadArrayOperator(
                                                Index: new DecimalOperator(
                                                    Operand: new DecimalOperator(
                                                        Operand: new ZeroOperator(),
                                                        Value: 2,
                                                        SupplementaryText: null
                                                    ),
                                                    Value: 0,
                                                    SupplementaryText: null
                                                ),
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new LoadArrayOperator(
                                        Index: new DecimalOperator(
                                            Operand: new DecimalOperator(
                                                Operand: new ZeroOperator(),
                                                Value: 2,
                                                SupplementaryText: null
                                            ),
                                            Value: 0,
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
                                /* 04 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 05 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 06 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 07 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "func", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 4),
                                        /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 07 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 08 */ new LowLevelOperation(Opcode.LoadConst, 6),
                                        /* 09 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 11 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 12 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 13 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 14 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 15 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 16 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 17 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 18 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 19 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 20 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 21 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 22 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 23 */ new LowLevelOperation(Opcode.LoadConst, 1),
                                        /* 24 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 25 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 26 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 27 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 28 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 29 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 30 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                        /* 31 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 32 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 33 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 34 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 35 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 36 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 37 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 38 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 39 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 40 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 41 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 42 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    4),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "get", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 02 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.LoadConst, 2),
                                        /* 04 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 06 */ new LowLevelOperation(Opcode.Mult, 0),
                                        /* 07 */ new LowLevelOperation(Opcode.LoadConst, 0),
                                        /* 08 */ new LowLevelOperation(Opcode.Add, 0),
                                        /* 09 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 10 */ new LowLevelOperation(Opcode.Return, 0)
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
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                Operands: new IOperator[]
                                {
                                }.ToImmutableArray(),
                                IsTailCall: false,
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
                                    Definition: new OperatorDefinition(Name: "func", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new ParenthesisOperator(
                                        Operators: new IOperator[]
                                        {
                                            new StoreArrayOperator(
                                                Value: new PreComputedOperator(
                                                    Value: 10
                                                ),
                                                Index: new PreComputedOperator(
                                                    Value: 20
                                                ),
                                                SupplementaryText: null
                                            ),
                                            new LoadArrayOperator(
                                                Index: new PreComputedOperator(
                                                    Value: 20
                                                ),
                                                SupplementaryText: null
                                            )
                                        }.ToImmutableArray(),
                                        SupplementaryText: null
                                    )
                                ),
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "get", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new LoadArrayOperator(
                                        Index: new PreComputedOperator(
                                            Value: 20
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
                                /* 00 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Pop, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Call, 1),
                                /* 03 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "func", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 10),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                        /* 02 */ new LowLevelOperation(Opcode.StoreArrayElement, 0),
                                        /* 03 */ new LowLevelOperation(Opcode.Pop, 0),
                                        /* 04 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                        /* 05 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 06 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    2),
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "get", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.LoadConst, 20),
                                        /* 01 */ new LowLevelOperation(Opcode.LoadArrayElement, 0),
                                        /* 02 */ new LowLevelOperation(Opcode.Return, 0)
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
            Source: "I",
            StandardInput: "A",
            ExpectedValue: 65,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new InputOperator(
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
                                /* 00 */ new LowLevelOperation(Opcode.Input, 0),
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
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new InputOperator(
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
                                /* 00 */ new LowLevelOperation(Opcode.Input, 0),
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
            Source: "I+I",
            StandardInput: "AB",
            ExpectedValue: 131,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
                        Left: new InputOperator(
                            SupplementaryText: null
                        ),
                        Right: new InputOperator(
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Add,
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
                                /* 00 */ new LowLevelOperation(Opcode.Input, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Input, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Halt, 0)
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
                    Operator: new BinaryOperator(
                        Left: new InputOperator(
                            SupplementaryText: null
                        ),
                        Right: new InputOperator(
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Add,
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
                                /* 00 */ new LowLevelOperation(Opcode.Input, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Input, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Halt, 0)
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
            Source: "1+2+I",
            StandardInput: "A",
            ExpectedValue: 68,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new BinaryOperator(
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
                        Right: new InputOperator(
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Add,
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
                                /* 11 */ new LowLevelOperation(Opcode.Input, 0),
                                /* 12 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 13 */ new LowLevelOperation(Opcode.Halt, 0)
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
                    Operator: new BinaryOperator(
                        Left: new PreComputedOperator(
                            Value: 3
                        ),
                        Right: new InputOperator(
                            SupplementaryText: null
                        ),
                        Type: BinaryType.Add,
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
                                /* 00 */ new LowLevelOperation(Opcode.LoadConst, 3),
                                /* 01 */ new LowLevelOperation(Opcode.Input, 0),
                                /* 02 */ new LowLevelOperation(Opcode.Add, 0),
                                /* 03 */ new LowLevelOperation(Opcode.Halt, 0)
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
            Source: "D[Input||I]{Input}",
            StandardInput: "A",
            ExpectedValue: 65,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new ParenthesisOperator(
                        Operators: new IOperator[]
                        {
                            new DefineOperator(
                                SupplementaryText: "Input||I"
                            ),
                            new UserDefinedOperator(
                                Definition: new OperatorDefinition(Name: "Input", NumOperands: 0),
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
                                    Definition: new OperatorDefinition(Name: "Input", NumOperands: 0),
                                    IsOptimized: false,
                                    Operator: new InputOperator(
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
                                    new OperatorDefinition(Name: "Input", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.Input, 0),
                                        /* 01 */ new LowLevelOperation(Opcode.Return, 0)
                                    }.ToImmutableArray(),
                                    1)
                            }.ToImmutableArray(),
                            new String[]
                            {
                            }.ToImmutableArray()
                        )
                ),
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new UserDefinedOperator(
                        Definition: new OperatorDefinition(Name: "Input", NumOperands: 0),
                        Operands: new IOperator[]
                        {
                        }.ToImmutableArray(),
                        IsTailCall: true,
                        SupplementaryText: null
                    ),
                    Context:
                        CompilationContext.Empty.WithAddOrUpdateOperatorImplements(
                            new OperatorImplement[]
                            {
                                new OperatorImplement(
                                    Definition: new OperatorDefinition(Name: "Input", NumOperands: 0),
                                    IsOptimized: true,
                                    Operator: new InputOperator(
                                        SupplementaryText: null
                                    )
                                )
                            }
                        ),
                    Module:
                        new LowLevelModule<Int32>(
                            new LowLevelOperation[]
                            {
                                /* 00 */ new LowLevelOperation(Opcode.Call, 0),
                                /* 01 */ new LowLevelOperation(Opcode.Halt, 0)
                            }.ToImmutableArray(),
                            new Int32[]
                            {
                            }.ToImmutableArray(),
                            new LowLevelUserDefinedOperator[]
                            {
                                new LowLevelUserDefinedOperator(
                                    new OperatorDefinition(Name: "Input", NumOperands: 0),
                                    new LowLevelOperation[]
                                    {
                                        /* 00 */ new LowLevelOperation(Opcode.Input, 0),
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
            Source: "I",
            StandardInput: "",
            ExpectedValue: -1,
            VariablesAfterExecution:
                ImmutableDictionary.CreateRange(
                    new KeyValuePair<ValueBox<string>, Int32>[]
                    {
                    }
                ),
            ExpectedWhenNotOptimized:
                new CompilationResult<Int32>(
                    Operator: new InputOperator(
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
                                /* 00 */ new LowLevelOperation(Opcode.Input, 0),
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
            ExpectedWhenOptimized:
                new CompilationResult<Int32>(
                    Operator: new InputOperator(
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
                                /* 00 */ new LowLevelOperation(Opcode.Input, 0),
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
    };
}
