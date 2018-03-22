using System;
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
            var (operations, userDefinedOperators) = ResolveLabels(visitor);
            return new Module<TNumber>(operations.ToImmutableArray(), visitor.ConstTable.ToImmutableArray(), userDefinedOperators);
        }

        private static (List<LowLevelOperation> Operations, ImmutableArray<(OperatorDefinition Definition, int StartAddress, int Length)> UserDefinedOperators) ResolveLabels<TNumber>(Visitor<TNumber> visitor)
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

            // Length is not determined here
            (OperatorDefinition Definition, int StartAddress, int Length)[] userDefinedOperators =
                visitor.OperatorBeginLabels.Select(p => (p.Key, labelMap[p.Value], default(int))).ToArray();

            for (int i = 0; i < userDefinedOperators.Length; i++)
            {
                int endAddress = (i + 1 < userDefinedOperators.Length)
                                 ? userDefinedOperators[i + 1].StartAddress
                                 : list.Count;
                userDefinedOperators[i].Length = endAddress - userDefinedOperators[i].StartAddress;
            }

            return (list, userDefinedOperators.ToImmutableArray());
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
                list.Add(new LowLevelOperation(Opcode.LoadArg, GetArgumentAddress(definition.NumOperands, op.Index)));
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
                void EmitComparisonBranch(Opcode opcode, bool reverse)
                {
                    int ifFalse = nextLabel++, end = nextLabel++;

                    list.Add(new LowLevelOperation(opcode));
                    list.Add(new LowLevelOperation(Opcode.LoadConst, reverse ? 1 : 0));
                    list.Add(new LowLevelOperation(Opcode.Goto, end));
                    list.Add(new LowLevelOperation(Opcode.Lavel, ifFalse));
                    list.Add(new LowLevelOperation(Opcode.LoadConst, reverse ? 0 : 1));
                    list.Add(new LowLevelOperation(Opcode.Lavel, end));
                }

                /* ******************** */

                op.Left.Accept(this);
                op.Right.Accept(this);

                switch (op.Type)
                {
                    case BinaryType.Add:
                        list.Add(new LowLevelOperation(Opcode.Add));
                        break;
                    case BinaryType.Sub:
                        list.Add(new LowLevelOperation(Opcode.Sub));
                        break;
                    case BinaryType.Mult:
                        list.Add(new LowLevelOperation(Opcode.Mult));
                        break;
                    case BinaryType.Div:
                        list.Add(new LowLevelOperation(Opcode.Div));
                        break;
                    case BinaryType.Mod:
                        list.Add(new LowLevelOperation(Opcode.Mod));
                        break;
                    case BinaryType.Equal:
                        EmitComparisonBranch(Opcode.GotoIfEqual, reverse: false);
                        break;
                    case BinaryType.NotEqual:
                        EmitComparisonBranch(Opcode.GotoIfEqual, reverse: true);
                        break;
                    case BinaryType.LessThan:
                        EmitComparisonBranch(Opcode.GotoIfLessThan, reverse: false);
                        break;
                    case BinaryType.LessThanOrEqual:
                        EmitComparisonBranch(Opcode.GotoIfLessThanOrEqual, reverse: false);
                        break;
                    case BinaryType.GreaterThanOrEqual:
                        EmitComparisonBranch(Opcode.GotoIfLessThan, reverse: true);
                        break;
                    case BinaryType.GreaterThan:
                        EmitComparisonBranch(Opcode.GotoIfLessThanOrEqual, reverse: true);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
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

                if (definition == op.Definition && (op.IsTailCallable ?? false))
                {
                    for (int i = op.Operands.Length - 1; i >= 0; i--)
                    {
                        list.Add(new LowLevelOperation(Opcode.StoreArg, GetArgumentAddress(definition.NumOperands, i)));
                    }

                    list.Add(new LowLevelOperation(Opcode.Goto, operatorBeginLabels[definition]));
                }
                else
                {
                    list.Add(new LowLevelOperation(Opcode.Call, operatorBeginLabels[op.Definition]));
                }
            }

            /* ******************** */

            private static int GetArgumentAddress(int numOperands, int index) => numOperands - index;
        }
    }
}
