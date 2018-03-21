﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution
{
    public static class LowLevelCodeGenerator
    {
        public static Module<TNumber> Generate<TNumber>(IOperator<TNumber> op, Context<TNumber> context)
        {
            Visitor<TNumber> visitor = new Visitor<TNumber>(context);
            visitor.Generate(op);
            var (Operations, StartAddresses) = ResolveLabels(visitor);
            return new Module<TNumber>(Operations.ToImmutableArray(), visitor.ConstTable.ToImmutableArray(), StartAddresses);
        }

        private static (List<LowLevelOperation> Operations, ImmutableArray<(OperatorDefinition, int)> StartAddresses) ResolveLabels<TNumber>(Visitor<TNumber> visitor)
        {
            List<LowLevelOperation> list = new List<LowLevelOperation>();
            Dictionary<int, int> labelMap = new Dictionary<int, int>();

            foreach (var op in visitor.Operations)
            {
                switch (op.Opcode)
                {
                    case Opcode.Lavel:
                        labelMap[op.Value] = list.Count;
                        break;
                    default:
                        list.Add(op);
                        break;
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                LowLevelOperation op = list[i];

                switch (op.Opcode)
                {
                    case Opcode.Goto:
                    case Opcode.GotoIfTrue:
                    case Opcode.GotoIfFalse:
                    case Opcode.GotoIfEqual:
                    case Opcode.GotoIfLessThan:
                    case Opcode.GotoIfLessThanOrEqual:
                    case Opcode.Call:
                        // The destination address in LowLevelOperation should be just before it,
                        // because program counter will be incremented after execution of this operation.
                        list[i] = new LowLevelOperation(op.Opcode, labelMap[op.Value] - 1);
                        break;
                    default:
                        // Do nothing
                        break;
                }
            }

            return (list, visitor.OperatorBeginLabels.Select(p => (p.Key, labelMap[p.Value])).ToImmutableArray());
        }

        private sealed class Visitor<TNumber> : IOperatorVisitor<TNumber>
        {
            private readonly Context<TNumber> context;

            private List<LowLevelOperation> list = new List<LowLevelOperation>();
            private List<TNumber> constTable = new List<TNumber>();
            private Dictionary<OperatorDefinition, int> operatorBeginLabels = new Dictionary<OperatorDefinition, int>();
            private OperatorDefinition definition = null;
            private int nextLabel = 0;

            public List<LowLevelOperation> Operations => list;
            public List<TNumber> ConstTable => constTable;
            public Dictionary<OperatorDefinition, int> OperatorBeginLabels => operatorBeginLabels;

            public Visitor(Context<TNumber> context)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public void Generate(IOperator<TNumber> op)
            {
                // Determine user-defined operators' label
                foreach (var item in context.OperatorDefinitions)
                {
                    operatorBeginLabels[item] = nextLabel++;
                }

                // Generate Main code
                definition = null;
                op.Accept(this);
                list.Add(new LowLevelOperation(Opcode.Halt));

                // Generate user-defined operators' codes
                foreach (var item in context.OperatorDefinitions)
                {
                    definition = item;
                    list.Add(new LowLevelOperation(Opcode.Lavel, operatorBeginLabels[definition]));
                    context.LookUpOperatorImplement(definition.Name).Accept(this);
                    list.Add(new LowLevelOperation(Opcode.Return, definition.NumOperands));
                }
            }

            /* ******************** */

            public void Visit(ZeroOperator<TNumber> op)
            {
                list.Add(new LowLevelOperation(Opcode.LoadConst, 0));
            }

            public void Visit(PreComputedOperator<TNumber> op)
            {
                bool TryCastToShort(TNumber number, out short casted)
                {
                    try
                    {
                        casted = unchecked((short)(dynamic)number);
                        return ((TNumber)(dynamic)casted).Equals(number);
                    }
                    catch
                    {
                        casted = default;
                        return false;
                    }
                }

                if (TryCastToShort(op.Value, out var value))
                {
                    list.Add(new LowLevelOperation(Opcode.LoadConst, value));
                }
                else
                {
                    // We cannot emit Opcode.LoadConst,
                    // because the constant value exceeds limit of 16-bit integer.
                    // So we use constTable.
                    int no = constTable.Count;
                    constTable.Add(op.Value);
                    list.Add(new LowLevelOperation(Opcode.LoadConstTable, no));
                }
            }

            public void Visit(ArgumentOperator<TNumber> op)
            {
                list.Add(new LowLevelOperation(Opcode.LoadArg, definition.NumOperands - op.Index));
            }

            public void Visit(DefineOperator<TNumber> op)
            {
                list.Add(new LowLevelOperation(Opcode.LoadConst, 0));
            }

            public void Visit(ParenthesisOperator<TNumber> op)
            {
                ImmutableArray<IOperator<TNumber>> operators = op.Operators;
                for (int i = 0; i < operators.Length; i++)
                {
                    operators[i].Accept(this);
                    if (i < operators.Length - 1)
                        list.Add(new LowLevelOperation(Opcode.Pop));
                }
            }

            public void Visit(DecimalOperator<TNumber> op)
            {
                op.Operand.Accept(this);
                list.Add(new LowLevelOperation(Opcode.LoadConst, 10));
                list.Add(new LowLevelOperation(Opcode.Mult));
                list.Add(new LowLevelOperation(Opcode.LoadConst, op.Value));
                list.Add(new LowLevelOperation(Opcode.Add));
            }

            public void Visit(BinaryOperator<TNumber> op)
            {
                Opcode ConvertArithmeticType(BinaryType type)
                {
                    switch (type)
                    {
                        case BinaryType.Add:
                            return Opcode.Add;
                        case BinaryType.Sub:
                            return Opcode.Sub;
                        case BinaryType.Mult:
                            return Opcode.Mult;
                        case BinaryType.Div:
                            return Opcode.Div;
                        case BinaryType.Mod:
                            return Opcode.Mod;
                        case BinaryType.Equal:
                            return Opcode.Equal;
                        case BinaryType.NotEqual:
                            return Opcode.NotEqual;
                        case BinaryType.LessThan:
                            return Opcode.LessThan;
                        case BinaryType.LessThanOrEqual:
                            return Opcode.LessThanOrEqual;
                        case BinaryType.GreaterThanOrEqual:
                            return Opcode.GreaterThanOrEqual;
                        case BinaryType.GreaterThan:
                            return Opcode.GreaterThan;
                        default:
                            throw new InvalidOperationException();
                    }
                }

                op.Left.Accept(this);
                op.Right.Accept(this);
                list.Add(new LowLevelOperation(ConvertArithmeticType(op.Type)));
            }

            public void Visit(ConditionalOperator<TNumber> op)
            {
                int ifTrueLabel = nextLabel++, endLabel = nextLabel++;

                if (op.Condition is BinaryOperator<TNumber> binary)
                {
                    // Special optimiztion for comparisons
                    void Emit(Opcode opcode, IOperator<TNumber> ifTrue, IOperator<TNumber> ifFalse)
                    {
                        binary.Left.Accept(this);
                        binary.Right.Accept(this);
                        list.Add(new LowLevelOperation(opcode, ifTrueLabel));
                        ifFalse.Accept(this);
                        list.Add(new LowLevelOperation(Opcode.Goto, endLabel));
                        list.Add(new LowLevelOperation(Opcode.Lavel, ifTrueLabel));
                        ifTrue.Accept(this);
                        list.Add(new LowLevelOperation(Opcode.Lavel, endLabel));
                    }

                    switch (binary.Type)
                    {
                        case BinaryType.Equal:
                            Emit(Opcode.GotoIfEqual, ifTrue: op.IfTrue, ifFalse: op.IfFalse);
                            return;
                        case BinaryType.NotEqual:
                            // "a != b ? c ? d" is equivalent to "a == b ? d ? c"
                            Emit(Opcode.GotoIfEqual, ifTrue: op.IfFalse, ifFalse: op.IfTrue);
                            return;
                        case BinaryType.LessThan:
                            Emit(Opcode.GotoIfLessThan, ifTrue: op.IfTrue, ifFalse: op.IfFalse);
                            return;
                        case BinaryType.LessThanOrEqual:
                            Emit(Opcode.GotoIfLessThanOrEqual, ifTrue: op.IfTrue, ifFalse: op.IfFalse);
                            return;
                        case BinaryType.GreaterThanOrEqual:
                            // "a >= b ? c ? d" is equivalent to "a < b ? d ? c"
                            Emit(Opcode.GotoIfLessThan, ifTrue: op.IfFalse, ifFalse: op.IfTrue);
                            return;
                        case BinaryType.GreaterThan:
                            // "a > b ? c ? d" is equivalent to "a <= b ? d ? c"
                            Emit(Opcode.GotoIfLessThanOrEqual, ifTrue: op.IfFalse, ifFalse: op.IfTrue);
                            return;
                        default:
                            break;
                    }
                }

                op.Condition.Accept(this);
                list.Add(new LowLevelOperation(Opcode.GotoIfTrue, ifTrueLabel));
                op.IfFalse.Accept(this);
                list.Add(new LowLevelOperation(Opcode.Goto, endLabel));
                list.Add(new LowLevelOperation(Opcode.Lavel, ifTrueLabel));
                op.IfTrue.Accept(this);
                list.Add(new LowLevelOperation(Opcode.Lavel, endLabel));
            }

            public void Visit(UserDefinedOperator<TNumber> op)
            {
                for (int i = 0; i < op.Operands.Length; i++)
                {
                    op.Operands[i].Accept(this);
                }

                list.Add(new LowLevelOperation(Opcode.Call, operatorBeginLabels[op.Definition]));
            }
        }
    }
}