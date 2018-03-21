using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution
{
    public static class LowLevelCodeGenerator
    {
        public static Module Generate(IOperator op, Context context)
        {
            Visitor visitor = new Visitor(context);
            visitor.Generate(op);
            var (Operations, StartAddresses) = ResolveLabels(visitor);
            return new Module(Operations.ToImmutableArray(), visitor.ConstTable.ToImmutableArray(), StartAddresses);
        }

        private static (List<LowLevelOperation> Operations, ImmutableArray<(OperatorDefinition, int)> StartAddresses) ResolveLabels(Visitor visitor)
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

        private sealed class Visitor : IOperatorVisitor
        {
            private readonly Context context;

            private List<LowLevelOperation> list = new List<LowLevelOperation>();
            private List<Number> constTable = new List<Number>();
            private Dictionary<OperatorDefinition, int> operatorBeginLabels = new Dictionary<OperatorDefinition, int>();
            private OperatorDefinition definition = null;
            private int nextLabel = 0;

            public List<LowLevelOperation> Operations => list;
            public List<Number> ConstTable => constTable;
            public Dictionary<OperatorDefinition, int> OperatorBeginLabels => operatorBeginLabels;

            public Visitor(Context context)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public void Generate(IOperator op)
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

            public void Visit(ZeroOperator op)
            {
                list.Add(new LowLevelOperation(Opcode.LoadConst, 0));
            }

            public void Visit(PreComputedOperator op)
            {
                if (unchecked((short)op.Value) == op.Value)
                {
                    list.Add(new LowLevelOperation(Opcode.LoadConst, (short)op.Value));
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

            public void Visit(ArgumentOperator op)
            {
                list.Add(new LowLevelOperation(Opcode.LoadArg, definition.NumOperands - op.Index));
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
                Opcode ConvertArithmeticType(BinaryOperator.ArithmeticType type)
                {
                    switch (type)
                    {
                        case BinaryOperator.ArithmeticType.Add:
                            return Opcode.Add;
                        case BinaryOperator.ArithmeticType.Sub:
                            return Opcode.Sub;
                        case BinaryOperator.ArithmeticType.Mult:
                            return Opcode.Mult;
                        case BinaryOperator.ArithmeticType.Div:
                            return Opcode.Div;
                        case BinaryOperator.ArithmeticType.Mod:
                            return Opcode.Mod;
                        case BinaryOperator.ArithmeticType.LessThan:
                            return Opcode.LessThan;
                        case BinaryOperator.ArithmeticType.LessThanOrEqual:
                            return Opcode.LessThanOrEqual;
                        case BinaryOperator.ArithmeticType.GreaterThanOrEqual:
                            return Opcode.GreaterThanOrEqual;
                        case BinaryOperator.ArithmeticType.GreaterThan:
                            return Opcode.GreaterThan;
                        default:
                            throw new InvalidOperationException();
                    }
                }

                op.Left.Accept(this);
                op.Right.Accept(this);
                list.Add(new LowLevelOperation(ConvertArithmeticType(op.Type)));
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
                        list.Add(new LowLevelOperation(Opcode.Goto, endLabel));
                        list.Add(new LowLevelOperation(Opcode.Lavel, ifTrueLabel));
                        ifTrue.Accept(this);
                        list.Add(new LowLevelOperation(Opcode.Lavel, endLabel));
                    }

                    switch (binary.Type)
                    {
                        case BinaryOperator.ArithmeticType.LessThan:
                            Emit(Opcode.GotoIfLessThan, ifTrue: op.IfTrue, ifFalse: op.IfFalse);
                            return;
                        case BinaryOperator.ArithmeticType.LessThanOrEqual:
                            Emit(Opcode.GotoIfLessThanOrEqual, ifTrue: op.IfTrue, ifFalse: op.IfFalse);
                            return;
                        case BinaryOperator.ArithmeticType.GreaterThanOrEqual:
                            // "a >= b ? c ? d" is equivalent to "a < b ? d ? c"
                            Emit(Opcode.GotoIfLessThan, ifTrue: op.IfFalse, ifFalse: op.IfTrue);
                            return;
                        case BinaryOperator.ArithmeticType.GreaterThan:
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

            public void Visit(UserDefinedOperator op)
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
