using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core
{
    public sealed class CompilationContext
    {
        #region Class

        internal sealed class Boxed
        {
            public CompilationContext Value { get; set; }

            public Boxed(CompilationContext value)
            {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        #endregion

        public static readonly CompilationContext Empty = new CompilationContext();

        private readonly ImmutableDictionary<string, OperatorImplement> userDefinedOperators;

        #region Constructors

        private CompilationContext()
        {
            this.userDefinedOperators = ImmutableDictionary<string, OperatorImplement>.Empty;
        }

        internal CompilationContext(ImmutableDictionary<string, OperatorImplement> userDefinedOperators)
        {
            this.userDefinedOperators = userDefinedOperators ?? throw new ArgumentNullException(nameof(userDefinedOperators));
        }

        #endregion

        public IEnumerable<OperatorImplement> OperatorImplements => userDefinedOperators.Values;

        public OperatorImplement LookupOperatorImplement(string name)
        {
            return this.userDefinedOperators[name];
        }

        public bool TryLookupOperatorImplement(string name, out OperatorImplement value)
        {
            return this.userDefinedOperators.TryGetValue(name, out value);
        }

        public CompilationContext WithAddOrUpdateOperatorImplement(OperatorImplement implement)
        {
            return new CompilationContext(
                this.userDefinedOperators.SetItem(implement.Definition.Name, implement));
        }

        public CompilationContext WithAddOrUpdateOperatorImplements(IEnumerable<OperatorImplement> implements)
        {
            return new CompilationContext(
                this.userDefinedOperators.SetItems(implements.Select(implement => KeyValuePair.Create(implement.Definition.Name, implement))));
        }
    }
}
