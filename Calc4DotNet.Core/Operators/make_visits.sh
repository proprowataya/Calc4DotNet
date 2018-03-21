grep "class.*:.*I.*Operator" ./IOperator.cs | sed "s/.*class \(.*\).* :.*/void Visit(\1 op);/g"
echo
grep "class.*:.*I.*Operator" ./IOperator.cs | sed "s/.*class \(.*\).* :.*/TResult Visit(\1 op);/g"
echo
grep "class.*:.*I.*Operator" ./IOperator.cs | sed "s/.*class \(.*\).* :.*/TResult Visit(\1 op, TParam param);/g"
