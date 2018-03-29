using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core
{
    public sealed class CompilationContext<TNumber>
    {
        #region Class

        internal sealed class Boxed
        {
            public CompilationContext<TNumber> Value { get; set; }

            public Boxed(CompilationContext<TNumber> value)
            {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        #endregion

        public static readonly CompilationContext<TNumber> Empty = new CompilationContext<TNumber>();

        private readonly ImmutableDictionary<string, OperatorImplement<TNumber>> userDefinedOperators;

        #region Constructors

        private CompilationContext()
        {
            this.userDefinedOperators = ImmutableDictionary<string, OperatorImplement<TNumber>>.Empty;
        }

        internal CompilationContext(ImmutableDictionary<string, OperatorImplement<TNumber>> userDefinedOperators)
        {
            this.userDefinedOperators = userDefinedOperators ?? throw new ArgumentNullException(nameof(userDefinedOperators));
        }

        #endregion

        public IEnumerable<OperatorImplement<TNumber>> OperatorImplements => userDefinedOperators.Values;

        public OperatorImplement<TNumber> LookupOperatorImplement(string name)
        {
            return this.userDefinedOperators[name];
        }

        public bool TryLookupOperatorImplement(string name, out OperatorImplement<TNumber> value)
        {
            return this.userDefinedOperators.TryGetValue(name, out value);
        }

        public CompilationContext<TNumber> WithAddOrUpdateOperatorImplement(OperatorImplement<TNumber> implement)
        {
            return new CompilationContext<TNumber>(
                this.userDefinedOperators.SetItem(implement.Definition.Name, implement));
        }

        public CompilationContext<TNumber> WithAddOrUpdateOperatorImplements(IEnumerable<OperatorImplement<TNumber>> implements)
        {
            return new CompilationContext<TNumber>(
                this.userDefinedOperators.SetItems(implements.Select(implement => KeyValuePair.Create(implement.Definition.Name, implement))));
        }
    }
}
