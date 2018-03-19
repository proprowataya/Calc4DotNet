grep "class.*:.*I.*Operator" ./IOperator.cs | sed "s/.*class \(.*\).* :.*/void Visit(\1 op);/g"
echo
grep "class.*:.*I.*Operator" ./IOperator.cs | sed "s/.*class \(.*\).* :.*/T Visit(\1 op);/g"
