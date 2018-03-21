using System.Collections.Generic;
using System.Linq;
using Calc4DotNet.Core.Operators;

namespace Calc4DotNet.Core
{
    public sealed class Context
    {
        private readonly Dictionary<string, OperatorDefinition> definitions
            = new Dictionary<string, OperatorDefinition>();
        private readonly Dictionary<string, IOperator> implements
            = new Dictionary<string, IOperator>();

        public IEnumerable<OperatorDefinition> OperatorDefinitions => definitions.Values.ToArray();

        public void AddOperatorDefinition(OperatorDefinition operatorDefinition)
        {
            definitions.Add(operatorDefinition.Name, operatorDefinition);
        }

        public void AddOrUpdateOperatorImplement(string name, IOperator op)
        {
            implements[name] = op;
        }

        public OperatorDefinition LookUpOperatorDefinition(string name) => definitions[name];
        public IOperator LookUpOperatorImplement(string name) => implements[name];

        public bool TryLookUpOperatorDefinition(string name, out OperatorDefinition operatorDefinition)
        {
            return definitions.TryGetValue(name, out operatorDefinition);
        }

        public bool TryLookUpOperatorImplement(string name, out IOperator op)
        {
            return implements.TryGetValue(name, out op);
        }
    }
}
