using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution
{
    public static class LowLevelCodeGenerator
    {
        internal static LowLevelModule<TNumber> Generate<TNumber>(IOperator op, CompilationContext context, TNumber dummy)
        {
            return Generate<TNumber>(op, context);
        }

        public static LowLevelModule<TNumber> Generate<TNumber>(IOperator op, CompilationContext context)
        {
            var constTable = new List<TNumber>();
            var userDefinedOperators = ImmutableArray.CreateBuilder<(OperatorDefinition Definition, ImmutableArray<LowLevelOperation> Operations)>();
            var operatorLabels = new Dictionary<OperatorDefinition, int>();

            // Initialize operatorLabels
            int index = 0;
            foreach (var implement in context.OperatorImplements)
            {
                operatorLabels[implement.Definition] = index++;
            }

            // Generate user-defined operators' codes
            foreach (var implement in context.OperatorImplements)
            {
                var visitor = new Visitor<TNumber>(context, constTable, operatorLabels, implement.Definition);
                visitor.Generate(implement.Operator);
                userDefinedOperators.Add((implement.Definition, visitor.Operations.ToImmutableArray()));
            }

            // Generate Main code
            ImmutableArray<LowLevelOperation> entryPoint;
            {
                var visitor = new Visitor<TNumber>(context, constTable, operatorLabels, null);
                visitor.Generate(op);
                entryPoint = visitor.Operations.ToImmutableArray();
            }

            return new LowLevelModule<TNumber>(entryPoint,
                                               constTable.ToImmutableArray(),
                                               userDefinedOperators.ToImmutable());
        }

        private sealed class Visitor<TNumber> : IOperatorVisitor
        {
            private const int OperatorBeginLabel = 0;

            private readonly CompilationContext context;
            private readonly List<TNumber> constTable;
            private readonly Dictionary<OperatorDefinition, int> operatorLabels;
            private readonly OperatorDefinition definition;

            private List<LowLevelOperation> list = new List<LowLevelOperation>();
            private int nextLabel = OperatorBeginLabel;

            public List<LowLevelOperation> Operations => list;

            public Visitor(CompilationContext context, List<TNumber> constTable, Dictionary<OperatorDefinition, int> operatorLabels, OperatorDefinition definition)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.constTable = constTable ?? throw new ArgumentNullException(nameof(constTable));
                this.operatorLabels = operatorLabels ?? throw new ArgumentNullException(nameof(operatorLabels));
                this.definition = definition;
            }

            public void Generate(IOperator op)
            {
                // Add lavel at begin
                Debug.Assert(nextLabel == OperatorBeginLabel);
                list.Add(new LowLevelOperation(Opcode.Lavel, nextLabel++));

                if (definition == null)
                {
                    // Generate Main code
                    op.Accept(this);
                    list.Add(new LowLevelOperation(Opcode.Halt));
                }
                else
                {
                    // Generate user-defined operators' codes
                    op.Accept(this);
                    list.Add(new LowLevelOperation(Opcode.Return, definition.NumOperands));
                }

                ResolveLavels();
            }

            private void ResolveLavels()
            {
                List<LowLevelOperation> newList = new List<LowLevelOperation>();
                Dictionary<int, int> labelMap = new Dictionary<int, int>();

                foreach (var op in list)
                {
                    switch (op.Opcode)
                    {
                        case Opcode.Lavel:
                            labelMap[op.Value] = newList.Count;
                            break;
                        default:
                            newList.Add(op);
                            break;
                    }
                }

                for (int i = 0; i < newList.Count; i++)
                {
                    LowLevelOperation op = newList[i];

                    switch (op.Opcode)
                    {
                        case Opcode.Goto:
                        case Opcode.GotoIfTrue:
                        case Opcode.GotoIfEqual:
                        case Opcode.GotoIfLessThan:
                        case Opcode.GotoIfLessThanOrEqual:
                            // The destination address in LowLevelOperation should be just before it,
                            // because program counter will be incremented after execution of this operation.
                            newList[i] = new LowLevelOperation(op.Opcode, labelMap[op.Value] - 1);
                            break;
                        default:
                            // Do nothing
                            break;
                    }
                }

                list = newList;
            }

            /* ******************** */

            public void Visit(ZeroOperator op)
            {
                list.Add(new LowLevelOperation(Opcode.LoadConst, 0));
            }

            public void Visit(PreComputedOperator op)
            {
                bool TryCastToShort(TNumber number, out short casted)
                {
                    try
                    {
                        casted = checked((short)(dynamic)number);
                        return true;
                    }
                    catch (OverflowException)
                    {
                        casted = default;
                        return false;
                    }
                }

                if (TryCastToShort((TNumber)op.Value, out var value))
                {
                    list.Add(new LowLevelOperation(Opcode.LoadConst, value));
                }
                else
                {
                    // We cannot emit Opcode.LoadConst,
                    // because the constant value exceeds limit of 16-bit integer.
                    // So we use constTable.
                    int no = constTable.Count;
                    constTable.Add((TNumber)op.Value);
                    list.Add(new LowLevelOperation(Opcode.LoadConstTable, no));
                }
            }

            public void Visit(ArgumentOperator op)
            {
                list.Add(new LowLevelOperation(Opcode.LoadArg, GetArgumentAddress(definition.NumOperands, op.Index)));
            }

            public void Visit(DefineOperator op)
            {
                list.Add(new LowLevelOperation(Opcode.LoadConst, 0));
            }

            public void Visit(ParenthesisOperator op)
            {
                ImmutableArray<IOperator> operators = op.Operators;
                for (int i = 0; i < operators.Length; i++)
                {
                    operators[i].Accept(this);
                    if (i < operators.Length - 1)
                        list.Add(new LowLevelOperation(Opcode.Pop));
                }
            }

            public void Visit(DecimalOperator op)
            {
                op.Operand.Accept(this);
                list.Add(new LowLevelOperation(Opcode.LoadConst, 10));
                list.Add(new LowLevelOperation(Opcode.Mult));
                list.Add(new LowLevelOperation(Opcode.LoadConst, op.Value));
                list.Add(new LowLevelOperation(Opcode.Add));
            }

            public void Visit(BinaryOperator op)
            {
                void EmitComparisonBranch(Opcode opcode, bool reverse)
                {
                    int ifFalse = nextLabel++, end = nextLabel++;

                    list.Add(new LowLevelOperation(opcode, ifFalse));
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

            public void Visit(ConditionalOperator op)
            {
                int ifTrueLabel = nextLabel++, endLabel = nextLabel++;

                if (op.Condition is BinaryOperator binary)
                {
                    // Special optimiztion for comparisons
                    void Emit(Opcode opcode, IOperator ifTrue, IOperator ifFalse)
                    {
                        binary.Left.Accept(this);
                        binary.Right.Accept(this);
                        list.Add(new LowLevelOperation(opcode, ifTrueLabel));
                        ifFalse.Accept(this);
                        if (list.Last(x => x.Opcode != Opcode.Lavel).Opcode != Opcode.Goto)
                        {
                            // "list.Last().Opcode == Opcode.Goto" means
                            // elimination of "Call" (tail-call)
                            list.Add(new LowLevelOperation(Opcode.Goto, endLabel));
                        }
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
                if (list.Last(x => x.Opcode != Opcode.Lavel).Opcode != Opcode.Goto)
                {
                    // "list.Last().Opcode == Opcode.Goto" means
                    // elimination of "Call" (tail-call)
                    list.Add(new LowLevelOperation(Opcode.Goto, endLabel));
                }
                list.Add(new LowLevelOperation(Opcode.Lavel, ifTrueLabel));
                op.IfTrue.Accept(this);
                list.Add(new LowLevelOperation(Opcode.Lavel, endLabel));
            }

            public void Visit(UserDefinedOperator op)
            {
                for (int i = 0; i < op.Operands.Length; i++)
                {
                    op.Operands[i].Accept(this);
                }

                if (OptimizableTailCall(op))
                {
                    for (int i = op.Operands.Length - 1; i >= 0; i--)
                    {
                        list.Add(new LowLevelOperation(Opcode.StoreArg, GetArgumentAddress(definition.NumOperands, i)));
                    }

                    list.Add(new LowLevelOperation(Opcode.Goto, OperatorBeginLabel));
                }
                else
                {
                    list.Add(new LowLevelOperation(Opcode.Call, operatorLabels[op.Definition]));
                }
            }

            private bool OptimizableTailCall(IOperator op)
            {
                return op is UserDefinedOperator userDefined
                        && definition == userDefined.Definition
                        && (userDefined.IsTailCallable ?? false);
            }

            /* ******************** */

            private static int GetArgumentAddress(int numOperands, int index) => numOperands - index;
        }
    }
}
