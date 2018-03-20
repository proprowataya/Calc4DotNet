using System.Collections.Generic;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core
{
    public sealed class Context
    {
        private readonly Dictionary<string, OperatorDefinition> definitions
            = new Dictionary<string, OperatorDefinition>();

        public IEnumerable<OperatorDefinition> OperatorDefinitions => definitions.Values.ToArray();

        public void AddOrUpdateOperatorDefinition(OperatorDefinition operatorDefinition)
        {
            definitions[operatorDefinition.Name] = operatorDefinition;
        }

        public bool TryLookUpOperatorDefinition(string name, out OperatorDefinition operatorDefinition)
        {
            return definitions.TryGetValue(name, out operatorDefinition);
        }
    }
}
