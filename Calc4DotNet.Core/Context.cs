using System;
using System.Collections.Generic;
using System.Text;

namespace Calc4DotNet.Core
{
    public sealed class Context
    {
        private readonly Dictionary<string, OperatorDefinition> definitions
            = new Dictionary<string, OperatorDefinition>();

        public void AddOperatorDefinition(OperatorDefinition operatorDefinition)
        {
            definitions.Add(operatorDefinition.Name, operatorDefinition);
        }

        public bool TryLookUpOperatorDefinition(string name, out OperatorDefinition operatorDefinition)
        {
            return definitions.TryGetValue(name, out operatorDefinition);
        }
    }
}
