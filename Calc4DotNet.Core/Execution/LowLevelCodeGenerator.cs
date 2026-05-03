using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Execution;

public static class LowLevelCodeGenerator
{
    public static LowLevelModule<TNumber> Generate<TNumber>(IOperator op, CompilationContext context, LowLevelCodeGenerationOption option)
        where TNumber : INumber<TNumber>
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
            var visitor = new Visitor<TNumber>(context, option, constTable, operatorLabels, implement.Definition, variableIndices);

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
            var visitor = new Visitor<TNumber>(context, option, constTable, operatorLabels, null, variableIndices);
            visitor.Generate(op);
            entryPoint = visitor.Operations.ToImmutableArray();
        }

        // Assert all variable indicies are consecutive
        Debug.Assert(variableIndices.Select(pair => pair.Value).Order().SequenceEqual(Enumerable.Range(0, variableIndices.Count)));

        return new LowLevelModule<TNumber>(entryPoint,
                                           constTable.ToImmutableArray(),
                                           userDefinedOperators.ToImmutable(),
                                           variableIndices.OrderBy(pair => pair.Value).Select(pair => pair.Key.Value).ToImmutableArray());
    }

    private sealed class Visitor<TNumber> : IOperatorVisitor
        where TNumber : INumber<TNumber>
    {
        private const int OperatorBeginLabel = 0;

        private readonly CompilationContext context;
        private readonly LowLevelCodeGenerationOption option;
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

        public Visitor(CompilationContext context, LowLevelCodeGenerationOption option, List<TNumber> constTable, Dictionary<OperatorDefinition, int> operatorLabels, OperatorDefinition? definition, Dictionary<ValueBox<string>, int> variableIndices)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.option = option ?? throw new ArgumentNullException(nameof(option));
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

            // First pass removes label operations and records their address.
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

            // Second pass resolves label operands to absolute indices.
            for (int i = 0; i < newList.Count; i++)
            {
                LowLevelOperation op = newList[i];

                switch (op.Opcode)
                {
                    case Opcode.Goto:
                    case Opcode.GotoIfTrue:
                    case Opcode.GotoIfFalse:
                    case Opcode.GotoIfEqual:
                    case Opcode.GotoIfNotEqual:
                    case Opcode.GotoIfLessThan:
                    case Opcode.GotoIfLessThanOrEqual:
                    case Opcode.GotoIfGreaterThan:
                    case Opcode.GotoIfGreaterThanOrEqual:
                        // The destination address in LowLevelOperation should be just before it,
                        // because program counter will be incremented after execution of this operation.
                        newList[i] = new LowLevelOperation(op.Opcode, labelMap[op.Value] - 1);
                        break;
                    default:
                        // Do nothing
                        break;
                }
            }

            // Third pass compresses Goto chains and replaces Goto with Return when the target is Return.
            // visitId is a marker for the current walk.
            // visited stores the last visit id for each index.
            if (newList.Count > 0)
            {
                int[] visited = new int[newList.Count];
                int visitId = 1;
                for (int i = 0; i < newList.Count; i++)
                {
                    LowLevelOperation operation = newList[i];

                    if (operation.Opcode != Opcode.Goto)
                    {
                        continue;
                    }

                    // Get a new visit id for this walk.
                    visitId++;
                    if (visitId == 0)
                    {
                        // visitId overflowed to zero so we reset markers to avoid false matches.
                        Array.Clear(visited, 0, visited.Length);
                        visitId = 1;
                    }

                    int target = operation.Value + 1;
                    while (true)
                    {
                        if (visited[target] == visitId)
                        {
                            // Cycle detected so we keep the original Goto.
                            break;
                        }
                        visited[target] = visitId;

                        LowLevelOperation targetOperation = newList[target];
                        if (targetOperation.Opcode == Opcode.Goto)
                        {
                            // Follow the next Goto in the chain.
                            target = targetOperation.Value + 1;
                        }
                        else if (targetOperation.Opcode == Opcode.Return)
                        {
                            // Replace Goto with Return to skip the jump.
                            newList[i] = targetOperation;
                            break;
                        }
                        else
                        {
                            // Replace Goto with the final target.
                            newList[i] = new LowLevelOperation(operation.Opcode, target - 1);
                            break;
                        }
                    }
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
                case Opcode.StoreArrayElement:
                case Opcode.Add:
                case Opcode.Sub:
                case Opcode.Mult:
                case Opcode.Div:
                case Opcode.DivChecked:
                case Opcode.Mod:
                case Opcode.ModChecked:
                case Opcode.GotoIfTrue:
                case Opcode.GotoIfFalse:
                case Opcode.Return:
                case Opcode.Halt:
                    StackSize--;
                    break;
                case Opcode.StoreVariable:
                case Opcode.LoadArrayElement:
                case Opcode.PrintChar:
                case Opcode.Goto:
                    // Stacksize will not change
                    break;
                case Opcode.GotoIfEqual:
                case Opcode.GotoIfNotEqual:
                case Opcode.GotoIfLessThan:
                case Opcode.GotoIfLessThanOrEqual:
                case Opcode.GotoIfGreaterThan:
                case Opcode.GotoIfGreaterThanOrEqual:
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
            try
            {
                short value = short.CreateChecked<TNumber>((TNumber)op.Value);
                AddOperation(new LowLevelOperation(Opcode.LoadConst, value));
            }
            catch (OverflowException)
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

        public void Visit(InputOperator op)
        {
            AddOperation(new LowLevelOperation(Opcode.Input));
        }

        public void Visit(LoadArrayOperator op)
        {
            op.Index.Accept(this);
            AddOperation(new LowLevelOperation(Opcode.LoadArrayElement));
        }

        public void Visit(PrintCharOperator op)
        {
            op.Character.Accept(this);
            AddOperation(new LowLevelOperation(Opcode.PrintChar));
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
            op.Value.Accept(this);
            op.Index.Accept(this);
            AddOperation(new LowLevelOperation(Opcode.StoreArrayElement));
        }

        public void Visit(BinaryOperator op)
        {
            switch (op.Type)
            {
                case BinaryType.Add:
                    op.Left.Accept(this);
                    op.Right.Accept(this);
                    AddOperation(new LowLevelOperation(Opcode.Add));
                    return;
                case BinaryType.Sub:
                    op.Left.Accept(this);
                    op.Right.Accept(this);
                    AddOperation(new LowLevelOperation(Opcode.Sub));
                    return;
                case BinaryType.Mult:
                    op.Left.Accept(this);
                    op.Right.Accept(this);
                    AddOperation(new LowLevelOperation(Opcode.Mult));
                    return;
                case BinaryType.Div:
                    op.Left.Accept(this);
                    op.Right.Accept(this);
                    AddOperation(new LowLevelOperation(option.CheckZeroDivision ? Opcode.DivChecked : Opcode.Div));
                    return;
                case BinaryType.Mod:
                    op.Left.Accept(this);
                    op.Right.Accept(this);
                    AddOperation(new LowLevelOperation(option.CheckZeroDivision ? Opcode.ModChecked : Opcode.Mod));
                    return;
                case BinaryType.Equal:
                case BinaryType.NotEqual:
                case BinaryType.LessThan:
                case BinaryType.LessThanOrEqual:
                case BinaryType.GreaterThanOrEqual:
                case BinaryType.GreaterThan:
                case BinaryType.LogicalAnd:
                case BinaryType.LogicalOr:
                    {
                        int ifTrueLabel = nextLabel++, endLabel = nextLabel++;

                        EmitConditionGotoIfTrue(op, ifTrueLabel);
                        AddOperation(new LowLevelOperation(Opcode.LoadConst, 0));
                        AddOperation(new LowLevelOperation(Opcode.Goto, endLabel));
                        AddOperation(new LowLevelOperation(Opcode.Lavel, ifTrueLabel));
                        AddOperation(new LowLevelOperation(Opcode.LoadConst, 1));
                        AddOperation(new LowLevelOperation(Opcode.Lavel, endLabel));

                        // StackSize increased by two due to two LoadConst operations.
                        // However, only one of them will be executed.
                        // We modify StackSize here.
                        StackSize--;
                        return;
                    }
                default:
                    throw new InvalidOperationException();
            }
        }

        private void EmitConditionGoto(IOperator condition, int label, bool gotoIfTrue)
        {
            if (condition is ParenthesisOperator parenthesis)
            {
                ImmutableArray<IOperator> operators = parenthesis.Operators;
                for (int i = 0; i < operators.Length; i++)
                {
                    if (i < operators.Length - 1)
                    {
                        operators[i].Accept(this);
                        AddOperation(new LowLevelOperation(Opcode.Pop));
                    }
                    else
                    {
                        EmitConditionGoto(operators[i], label, gotoIfTrue);
                    }
                }

                return;
            }

            if (condition is BinaryOperator binary)
            {
                switch (binary.Type)
                {
                    case BinaryType.Equal:
                        if (IsZeroOperatorValue(binary.Left))
                        {
                            binary.Right.Accept(this);
                            AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfFalse : Opcode.GotoIfTrue, label));
                            return;
                        }
                        if (IsZeroOperatorValue(binary.Right))
                        {
                            binary.Left.Accept(this);
                            AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfFalse : Opcode.GotoIfTrue, label));
                            return;
                        }
                        binary.Left.Accept(this);
                        binary.Right.Accept(this);
                        AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfEqual : Opcode.GotoIfNotEqual, label));
                        return;
                    case BinaryType.NotEqual:
                        if (IsZeroOperatorValue(binary.Left))
                        {
                            binary.Right.Accept(this);
                            AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfTrue : Opcode.GotoIfFalse, label));
                            return;
                        }
                        if (IsZeroOperatorValue(binary.Right))
                        {
                            binary.Left.Accept(this);
                            AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfTrue : Opcode.GotoIfFalse, label));
                            return;
                        }
                        binary.Left.Accept(this);
                        binary.Right.Accept(this);
                        AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfNotEqual : Opcode.GotoIfEqual, label));
                        return;
                    case BinaryType.LessThan:
                        binary.Left.Accept(this);
                        binary.Right.Accept(this);
                        AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfLessThan : Opcode.GotoIfGreaterThanOrEqual, label));
                        return;
                    case BinaryType.LessThanOrEqual:
                        binary.Left.Accept(this);
                        binary.Right.Accept(this);
                        AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfLessThanOrEqual : Opcode.GotoIfGreaterThan, label));
                        return;
                    case BinaryType.GreaterThanOrEqual:
                        binary.Left.Accept(this);
                        binary.Right.Accept(this);
                        AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfGreaterThanOrEqual : Opcode.GotoIfLessThan, label));
                        return;
                    case BinaryType.GreaterThan:
                        binary.Left.Accept(this);
                        binary.Right.Accept(this);
                        AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfGreaterThan : Opcode.GotoIfLessThanOrEqual, label));
                        return;
                    case BinaryType.LogicalAnd:
                        if (gotoIfTrue)
                        {
                            int ifFalseLabel = nextLabel++;
                            EmitConditionGoto(binary.Left, ifFalseLabel, false);
                            EmitConditionGoto(binary.Right, label, true);
                            AddOperation(new LowLevelOperation(Opcode.Lavel, ifFalseLabel));
                        }
                        else
                        {
                            EmitConditionGoto(binary.Left, label, false);
                            EmitConditionGoto(binary.Right, label, false);
                        }
                        return;
                    case BinaryType.LogicalOr:
                        if (gotoIfTrue)
                        {
                            EmitConditionGoto(binary.Left, label, true);
                            EmitConditionGoto(binary.Right, label, true);
                        }
                        else
                        {
                            int endLabel = nextLabel++;
                            EmitConditionGoto(binary.Left, endLabel, true);
                            EmitConditionGoto(binary.Right, label, false);
                            AddOperation(new LowLevelOperation(Opcode.Lavel, endLabel));
                        }
                        return;
                    default:
                        break;
                }
            }

            condition.Accept(this);
            AddOperation(new LowLevelOperation(gotoIfTrue ? Opcode.GotoIfTrue : Opcode.GotoIfFalse, label));
        }

        private void EmitConditionGotoIfTrue(IOperator condition, int ifTrueLabel)
        {
            EmitConditionGoto(condition, ifTrueLabel, true);
        }

        private void EmitConditionGotoIfFalse(IOperator condition, int ifFalseLabel)
        {
            EmitConditionGoto(condition, ifFalseLabel, false);
        }

        private static bool IsZeroOperatorValue(IOperator op)
        {
            if (op is ZeroOperator)
            {
                return true;
            }

            if (op is PreComputedOperator preComputed && preComputed.Value is TNumber value)
            {
                return TNumber.IsZero(value);
            }

            return false;
        }

        public void Visit(ConditionalOperator op)
        {
            int ifTrueLabel = nextLabel++, endLabel = nextLabel++;
            EmitConditionGotoIfTrue(op.Condition, ifTrueLabel);

            int savedStackSize = StackSize;
            op.IfFalse.Accept(this);
            // Always jump over the true branch after evaluating the false branch. A trailing
            // Goto inside the false branch does not necessarily mean every false-branch path
            // has left the current control flow. It may belong to a nested conditional or
            // a tail-call path only. Emitting this jump is harmless when it is unreachable.
            AddOperation(new LowLevelOperation(Opcode.Goto, endLabel));
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
