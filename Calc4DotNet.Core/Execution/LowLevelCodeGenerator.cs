using System.Collections.Immutable;
using System.Diagnostics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution;

public static class LowLevelCodeGenerator
{
    public static LowLevelModule<TNumber> Generate<TNumber>(IOperator op, CompilationContext context)
        where TNumber : notnull
    {
        var constTable = new List<TNumber>();
        var userDefinedOperators = ImmutableArray.CreateBuilder<LowLevelUserDefinedOperator>();
        var operatorLabels = new Dictionary<OperatorDefinition, int>();
        var variableIndices = new Dictionary<ValueBox<string>, int>();

        // Initialize operatorLabels
        int index = 0;
        foreach (var implement in context.OperatorImplements)
        {
            operatorLabels[implement.Definition] = index++;
        }

        // Generate user-defined operators' codes
        foreach (var implement in context.OperatorImplements)
        {
            var visitor = new Visitor<TNumber>(context, constTable, operatorLabels, implement.Definition, variableIndices);

            var operatorBody = implement.Operator;
            Debug.Assert(operatorBody is not null);
            visitor.Generate(operatorBody);
            if (visitor.StackSize != 0)
            {
                throw new InvalidOperationException($"Stacksize is not zero: {visitor.StackSize}");
            }
            userDefinedOperators.Add(new LowLevelUserDefinedOperator(implement.Definition, visitor.Operations.ToImmutableArray(), visitor.MaxStackSize));
        }

        // Generate Main code
        ImmutableArray<LowLevelOperation> entryPoint;
        {
            var visitor = new Visitor<TNumber>(context, constTable, operatorLabels, null, variableIndices);
            visitor.Generate(op);
            entryPoint = visitor.Operations.ToImmutableArray();
        }

        // Assert all variable indicies are consecutive
        Debug.Assert(variableIndices.Select(pair => pair.Value).OrderBy(x => x).SequenceEqual(Enumerable.Range(0, variableIndices.Count)));

        return new LowLevelModule<TNumber>(entryPoint,
                                           constTable.ToImmutableArray(),
                                           userDefinedOperators.ToImmutable(),
                                           variableIndices.OrderBy(pair => pair.Value).Select(pair => pair.Key.Value).ToImmutableArray());
    }

    private sealed class Visitor<TNumber> : IOperatorVisitor
        where TNumber : notnull
    {
        private const int OperatorBeginLabel = 0;

        private readonly CompilationContext context;
        private readonly List<TNumber> constTable;
        private readonly Dictionary<OperatorDefinition, int> operatorLabels;
        private readonly OperatorDefinition? definition;
        private readonly Dictionary<ValueBox<string>, int> variableIndices;

        private List<LowLevelOperation> list = new List<LowLevelOperation>();
        private int nextLabel = OperatorBeginLabel;
        private int stackSize = 0;
        private int maxStackSize = 0;

        public List<LowLevelOperation> Operations => list;

        public int StackSize
        {
            get => stackSize;

            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException($"Stacksize is negative: {value}");
                }
                stackSize = value;
                maxStackSize = Math.Max(maxStackSize, value);
            }
        }

        public int MaxStackSize => maxStackSize;

        public Visitor(CompilationContext context, List<TNumber> constTable, Dictionary<OperatorDefinition, int> operatorLabels, OperatorDefinition? definition, Dictionary<ValueBox<string>, int> variableIndices)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.constTable = constTable ?? throw new ArgumentNullException(nameof(constTable));
            this.operatorLabels = operatorLabels ?? throw new ArgumentNullException(nameof(operatorLabels));
            this.definition = definition;
            this.variableIndices = variableIndices ?? throw new ArgumentNullException(nameof(variableIndices));
        }

        public void Generate(IOperator op)
        {
            // Add lavel at begin
            Debug.Assert(nextLabel == OperatorBeginLabel);
            AddOperation(new LowLevelOperation(Opcode.Lavel, nextLabel++));

            if (definition == null)
            {
                // Generate Main code
                op.Accept(this);
                AddOperation(new LowLevelOperation(Opcode.Halt));
            }
            else
            {
                // Generate user-defined operators' codes
                op.Accept(this);
                AddOperation(new LowLevelOperation(Opcode.Return, definition.NumOperands));
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

        private void AddOperation(LowLevelOperation operation)
        {
            list.Add(operation);

            // Change stacksize
            switch (operation.Opcode)
            {
                case Opcode.Push:
                case Opcode.LoadConst:
                case Opcode.LoadConstTable:
                case Opcode.LoadArg:
                case Opcode.LoadVariable:
                case Opcode.Input:
                    StackSize++;
                    break;
                case Opcode.Pop:
                case Opcode.StoreArg:
                case Opcode.Add:
                case Opcode.Sub:
                case Opcode.Mult:
                case Opcode.Div:
                case Opcode.Mod:
                case Opcode.GotoIfTrue:
                case Opcode.Return:
                case Opcode.Halt:
                    StackSize--;
                    break;
                case Opcode.StoreVariable:
                case Opcode.Goto:
                    // Stacksize will not change
                    break;
                case Opcode.GotoIfEqual:
                case Opcode.GotoIfLessThan:
                case Opcode.GotoIfLessThanOrEqual:
                    StackSize -= 2;
                    break;
                case Opcode.Call:
                    StackSize -= operatorLabels.Single(p => p.Value == operation.Value).Key.NumOperands - 1;
                    break;
                case Opcode.Lavel:
                    // Do nothing
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /* ******************** */

        public void Visit(ZeroOperator op)
        {
            AddOperation(new LowLevelOperation(Opcode.LoadConst, 0));
        }

        public void Visit(PreComputedOperator op)
        {
            static bool TryCastToShort(TNumber number, out short casted)
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
                AddOperation(new LowLevelOperation(Opcode.LoadConst, value));
            }
            else
            {
                // We cannot emit Opcode.LoadConst,
                // because the constant value exceeds limit of 16-bit integer.
                // So we use constTable.
                int no = constTable.Count;
                constTable.Add((TNumber)op.Value);
                AddOperation(new LowLevelOperation(Opcode.LoadConstTable, no));
            }
        }

        public void Visit(ArgumentOperator op)
        {
            Debug.Assert(definition is not null);
            AddOperation(new LowLevelOperation(Opcode.LoadArg, GetArgumentAddress(definition.NumOperands, op.Index)));
        }

        public void Visit(DefineOperator op)
        {
            AddOperation(new LowLevelOperation(Opcode.LoadConst, 0));
        }

        public void Visit(LoadVariableOperator op)
        {
            AddOperation(new LowLevelOperation(Opcode.LoadVariable, GetOrCreateVariableIndex(op.VariableName)));
        }

        public void Visit(LoadArrayOperator op)
        {
            throw new NotImplementedException();
        }

        public void Visit(ParenthesisOperator op)
        {
            ImmutableArray<IOperator> operators = op.Operators;
            for (int i = 0; i < operators.Length; i++)
            {
                operators[i].Accept(this);
                if (i < operators.Length - 1)
                    AddOperation(new LowLevelOperation(Opcode.Pop));
            }
        }

        public void Visit(DecimalOperator op)
        {
            op.Operand.Accept(this);
            AddOperation(new LowLevelOperation(Opcode.LoadConst, 10));
            AddOperation(new LowLevelOperation(Opcode.Mult));
            AddOperation(new LowLevelOperation(Opcode.LoadConst, op.Value));
            AddOperation(new LowLevelOperation(Opcode.Add));
        }

        public void Visit(StoreVariableOperator op)
        {
            op.Operand.Accept(this);
            AddOperation(new LowLevelOperation(Opcode.StoreVariable, GetOrCreateVariableIndex(op.VariableName)));
        }

        public void Visit(StoreArrayOperator op)
        {
            throw new NotImplementedException();
        }

        public void Visit(BinaryOperator op)
        {
            void EmitComparisonBranch(Opcode opcode, bool reverse)
            {
                int ifFalse = nextLabel++, end = nextLabel++;

                AddOperation(new LowLevelOperation(opcode, ifFalse));
                AddOperation(new LowLevelOperation(Opcode.LoadConst, reverse ? 1 : 0));
                AddOperation(new LowLevelOperation(Opcode.Goto, end));
                AddOperation(new LowLevelOperation(Opcode.Lavel, ifFalse));
                AddOperation(new LowLevelOperation(Opcode.LoadConst, reverse ? 0 : 1));
                AddOperation(new LowLevelOperation(Opcode.Lavel, end));
            }

            /* ******************** */

            op.Left.Accept(this);
            op.Right.Accept(this);

            switch (op.Type)
            {
                case BinaryType.Add:
                    AddOperation(new LowLevelOperation(Opcode.Add));
                    break;
                case BinaryType.Sub:
                    AddOperation(new LowLevelOperation(Opcode.Sub));
                    break;
                case BinaryType.Mult:
                    AddOperation(new LowLevelOperation(Opcode.Mult));
                    break;
                case BinaryType.Div:
                    AddOperation(new LowLevelOperation(Opcode.Div));
                    break;
                case BinaryType.Mod:
                    AddOperation(new LowLevelOperation(Opcode.Mod));
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
                    AddOperation(new LowLevelOperation(opcode, ifTrueLabel));

                    int savedStackSize = StackSize;
                    ifFalse.Accept(this);
                    if (list.Last(x => x.Opcode != Opcode.Lavel).Opcode != Opcode.Goto)
                    {
                        // "list.Last().Opcode == Opcode.Goto" means
                        // elimination of "Call" (tail-call)
                        AddOperation(new LowLevelOperation(Opcode.Goto, endLabel));
                    }
                    AddOperation(new LowLevelOperation(Opcode.Lavel, ifTrueLabel));
                    StackSize = savedStackSize;
                    ifTrue.Accept(this);
                    AddOperation(new LowLevelOperation(Opcode.Lavel, endLabel));
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
            AddOperation(new LowLevelOperation(Opcode.GotoIfTrue, ifTrueLabel));

            int savedStackSize = StackSize;
            op.IfFalse.Accept(this);
            if (list.Last(x => x.Opcode != Opcode.Lavel).Opcode != Opcode.Goto)
            {
                // "list.Last().Opcode == Opcode.Goto" means
                // elimination of "Call" (tail-call)
                AddOperation(new LowLevelOperation(Opcode.Goto, endLabel));
            }
            AddOperation(new LowLevelOperation(Opcode.Lavel, ifTrueLabel));
            StackSize = savedStackSize;
            op.IfTrue.Accept(this);
            AddOperation(new LowLevelOperation(Opcode.Lavel, endLabel));
        }

        public void Visit(UserDefinedOperator op)
        {
            for (int i = 0; i < op.Operands.Length; i++)
            {
                op.Operands[i].Accept(this);
            }

            if (IsReplaceableWithJump(op))
            {
                for (int i = op.Operands.Length - 1; i >= 0; i--)
                {
                    Debug.Assert(definition is not null);
                    AddOperation(new LowLevelOperation(Opcode.StoreArg, GetArgumentAddress(definition.NumOperands, i)));
                }

                AddOperation(new LowLevelOperation(Opcode.Goto, OperatorBeginLabel));

                // If we eliminate tail-call, there are no values left in the stack.
                // We treat as if there are one returning value in the stack.
                StackSize++;
            }
            else
            {
                AddOperation(new LowLevelOperation(Opcode.Call, operatorLabels[op.Definition]));
            }
        }

        private bool IsReplaceableWithJump(UserDefinedOperator op)
        {
            return definition == op.Definition && (op.IsTailCall ?? false);
        }

        /* ******************** */

        private int GetOrCreateVariableIndex(string? variableName)
        {
            if (variableIndices.TryGetValue(ValueBox.Create(variableName), out var index))
            {
                return index;
            }
            else
            {
                index = variableIndices.Count;
                variableIndices[ValueBox.Create(variableName)] = index;
                return index;
            }
        }

        private static int GetArgumentAddress(int numOperands, int index) => numOperands - index;
    }
}
