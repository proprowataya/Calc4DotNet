using System.Collections.Immutable;
using System.Numerics;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core.Optimization;

public static partial class Optimizer
{
    private sealed record PotentialEffects(ImmutableHashSet<string?> WrittenVariables,
                                           bool MayWriteArray,
                                           bool MayPrintOutput,
                                           bool MayReadInput,
                                           bool MayThrow,
                                           bool MayNotTerminate)
    {
        public bool HasAnyEffect =>
            WrittenVariables.Count > 0
            || MayWriteArray
            || MayPrintOutput
            || MayReadInput
            || MayThrow
            || MayNotTerminate;
    }

    private sealed class PotentialEffectsBuilder
    {
        private readonly ImmutableHashSet<string?>.Builder writtenVariables = ImmutableHashSet.CreateBuilder<string?>();
        private bool mayWriteArray = false;
        private bool mayPrintOutput = false;
        private bool mayReadInput = false;
        private bool mayThrow = false;
        private bool mayNotTerminate = false;

        public void AddWrittenVariable(string? variableName)
        {
            writtenVariables.Add(variableName);
        }

        public void AddArrayWrite()
        {
            mayWriteArray = true;
        }

        public void AddPrintOutput()
        {
            mayPrintOutput = true;
        }

        public void AddInputRead()
        {
            mayReadInput = true;
        }

        public void AddThrow()
        {
            mayThrow = true;
        }

        public void AddMayNotTerminate()
        {
            mayNotTerminate = true;
        }

        public bool AddFrom(PotentialEffects effects)
        {
            bool changed = false;

            foreach (var variable in effects.WrittenVariables)
            {
                if (writtenVariables.Add(variable))
                {
                    changed = true;
                }
            }

            if (effects.MayWriteArray && !mayWriteArray)
            {
                mayWriteArray = true;
                changed = true;
            }

            if (effects.MayPrintOutput && !mayPrintOutput)
            {
                mayPrintOutput = true;
                changed = true;
            }

            if (effects.MayReadInput && !mayReadInput)
            {
                mayReadInput = true;
                changed = true;
            }

            if (effects.MayThrow && !mayThrow)
            {
                mayThrow = true;
                changed = true;
            }

            if (effects.MayNotTerminate && !mayNotTerminate)
            {
                mayNotTerminate = true;
                changed = true;
            }

            return changed;
        }

        public bool AddFrom(PotentialEffectsBuilder effects)
        {
            if (ReferenceEquals(this, effects))
            {
                return false;
            }

            bool changed = false;

            foreach (var variable in effects.writtenVariables)
            {
                if (writtenVariables.Add(variable))
                {
                    changed = true;
                }
            }

            if (effects.mayWriteArray && !mayWriteArray)
            {
                mayWriteArray = true;
                changed = true;
            }

            if (effects.mayPrintOutput && !mayPrintOutput)
            {
                mayPrintOutput = true;
                changed = true;
            }

            if (effects.mayReadInput && !mayReadInput)
            {
                mayReadInput = true;
                changed = true;
            }

            if (effects.mayThrow && !mayThrow)
            {
                mayThrow = true;
                changed = true;
            }

            if (effects.mayNotTerminate && !mayNotTerminate)
            {
                mayNotTerminate = true;
                changed = true;
            }

            return changed;
        }

        public PotentialEffects ToImmutable()
        {
            return new PotentialEffects(writtenVariables.ToImmutable(),
                                        mayWriteArray,
                                        mayPrintOutput,
                                        mayReadInput,
                                        mayThrow,
                                        mayNotTerminate);
        }
    }

    private static ImmutableDictionary<string, PotentialEffects> ComputePotentialEffects<TNumber>(CompilationContext context)
        where TNumber : INumber<TNumber>
    {
        Dictionary<string, PotentialEffectsBuilder> effects = [];
        Dictionary<string, HashSet<string>> callees = [];

        foreach (var implement in context.OperatorImplements)
        {
            if (implement.Operator is null)
            {
                continue;
            }

            PotentialEffectsBuilder directEffects = new();
            HashSet<string> directCallees = [];
            CollectDirectEffects<TNumber>(implement.Operator, directEffects, directCallees);
            effects[implement.Definition.Name] = directEffects;
            callees[implement.Definition.Name] = directCallees;
        }

        foreach (var name in callees.Keys)
        {
            if (CanReach(name, name, callees))
            {
                effects[name].AddMayNotTerminate();
            }
        }

        bool changed;
        do
        {
            changed = false;

            foreach (var (name, myCallees) in callees)
            {
                var myEffects = effects[name];

                foreach (var callee in myCallees)
                {
                    if (!effects.TryGetValue(callee, out var calleeEffects))
                    {
                        continue;
                    }

                    if (myEffects.AddFrom(calleeEffects))
                    {
                        changed = true;
                    }
                }
            }
        } while (changed);

        return effects.ToImmutableDictionary(static p => p.Key, static p => p.Value.ToImmutable());
    }

    private static bool CanReach(string from,
                                 string target,
                                 Dictionary<string, HashSet<string>> callees)
    {
        return CanReachCore(from, target, callees, []);

        static bool CanReachCore(string from,
                                 string target,
                                 Dictionary<string, HashSet<string>> callees,
                                 HashSet<string> visited)
        {
            if (!callees.TryGetValue(from, out var directCallees))
            {
                return false;
            }

            foreach (var callee in directCallees)
            {
                if (callee == target)
                {
                    return true;
                }

                if (visited.Add(callee) && CanReachCore(callee, target, callees, visited))
                {
                    return true;
                }
            }

            return false;
        }
    }

    private static void CollectDirectEffects<TNumber>(IOperator op, PotentialEffectsBuilder effects, HashSet<string> callees)
        where TNumber : INumber<TNumber>
    {
        CollectEffectsFromTree<TNumber>(op, effects, userDefined => callees.Add(userDefined.Definition.Name));
    }

    private static void CollectEffectsFromTree<TNumber>(IOperator op,
                                                        PotentialEffectsBuilder effects,
                                                        Action<UserDefinedOperator> collectUserDefined)
        where TNumber : INumber<TNumber>
    {
        switch (op)
        {
            case StoreVariableOperator store:
                effects.AddWrittenVariable(store.VariableName);
                break;
            case StoreArrayOperator:
                effects.AddArrayWrite();
                break;
            case PrintCharOperator:
                effects.AddPrintOutput();
                break;
            case InputOperator:
                effects.AddInputRead();
                break;
            case BinaryOperator binary when binary.Type is BinaryType.Div or BinaryType.Mod:
                if (!IsDefinitelyNonZeroConstant<TNumber>(binary.Right))
                {
                    effects.AddThrow();
                }
                break;
            case UserDefinedOperator userDefined:
                collectUserDefined(userDefined);
                break;
            case ParenthesisOperator parenthesis:
                foreach (var inner in parenthesis.Operators)
                {
                    CollectEffectsFromTree<TNumber>(inner, effects, collectUserDefined);
                }
                break;
        }

        op.ForEachOperand(operand => CollectEffectsFromTree<TNumber>(operand, effects, collectUserDefined));
    }
}
