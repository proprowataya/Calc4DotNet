grep "record.*:.*I.*Operator" ./IOperator.cs | sed "s/.*record \(\w*\).* :.*/void Visit(\1 op);/g"
echo
grep "record.*:.*I.*Operator" ./IOperator.cs | sed "s/.*record \(\w*\).* :.*/TResult Visit(\1 op);/g"
echo
grep "record.*:.*I.*Operator" ./IOperator.cs | sed "s/.*record \(\w*\).* :.*/TResult Visit(\1 op, TParam param);/g"
