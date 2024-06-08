using FrontEnd.Parser.Models.Expressions;
using FrontEnd.Parser.Models.Stmt;
using KukuLang.Interpreter.Model.RuntimeObj;
using KukuLang.Interpreter.Model.Scope;
using KukuLang.Interpreter.Service;
using KukuLang.Parser.Models.Expressions.Literals;

namespace KukuLang.Interpreter.Handler
{
    public static class StatementHandler
    {
        public static void HandleStatement(Stmt statement, RuntimeScope scope)
        {
            switch (statement)
            {
                case SetToStmt setToStmt:
                    ProcessSetToStatement(setToStmt, scope);
                    break;
                case IfStmt ifStmt:
                    ProcessIfStatement(ifStmt, scope);
                    break;
                case FunctionCallStmt funcCallStmt:
                    ProcessFunctionCallStatement(funcCallStmt, scope);
                    break;
                default:
                    throw new NotSupportedException($"Unsupported statement type: {statement.GetType().Name}");
            }
        }

        private static void ProcessSetToStatement(SetToStmt stmt, RuntimeScope scope)
        {
            var variableToSet = stmt.VariableToSet;
            var valueToSet = stmt.VarVal;

            if (variableToSet.NextNode == null)
            {
                AssignSimpleVariable(variableToSet.VarName, valueToSet, scope);
            }
            else
            {
                AssignNestedVariable(variableToSet, valueToSet, scope);
            }
        }

        private static void AssignSimpleVariable(string variableName, ExpressionStmt value, RuntimeScope scope)
        {
            var runtimeObject = ConvertExpressionToRuntimeObject(value, scope);
            var isUpdate = scope.GetVariable(variableName) != null;
            scope.UpdateScopeVariable(variableName, runtimeObject);
            Console.WriteLine($"{(isUpdate ? "Updated" : "Created")} variable '{variableName}' with value '{runtimeObject}'");
        }

        private static void AssignNestedVariable(NestedVariableExp nestedVar, ExpressionStmt value, RuntimeScope scope)
        {
            var currentVar = scope.GetVariable(nestedVar.VarName)
                                  ?? throw new Exception($"Variable not found: {nestedVar.VarName}");
            RuntimeObj? parentVar = currentVar;

            // Build the full path dynamically
            var fullPath = nestedVar.VarName;

            while (nestedVar.NextNode != null)
            {
                nestedVar = nestedVar.NextNode;
                parentVar = currentVar;
                var nestedVars = currentVar.Val as Dictionary<string, RuntimeObj>;

                if (nestedVars == null)
                {
                    throw new Exception($"Variable {currentVar.Val} is not a nested object");
                }
                if (!nestedVars.ContainsKey(nestedVar.VarName))
                {
                    throw new Exception($"Nested variable not found: {nestedVar.VarName}");

                }
                currentVar = nestedVars[nestedVar.VarName];

                // Append to the path
                fullPath += $".{nestedVar.VarName}";
            }

            var updatedValue = ConvertExpressionToRuntimeObject(value, scope);
            (parentVar.Val as Dictionary<string, RuntimeObj>)[nestedVar.VarName] = updatedValue;
            Console.WriteLine($"Updated nested variable '{fullPath}' with value '{updatedValue}'");
        }


        private static RuntimeObj? ConvertExpressionToRuntimeObject(ExpressionStmt exp, RuntimeScope scope)
        {
            return exp switch
            {
                IntLiteral intLiteral => new RuntimeObj(RuntimeObjType.Integer, intLiteral.Val),
                TextLiteral textLiteral => new RuntimeObj(RuntimeObjType.Text, textLiteral.Val),
                NestedVariableExp nestedVar => ResolveNestedVariable(nestedVar, scope),
                FuncCallExp funcCallExp => ResolveFunctionCallExp(funcCallExp, scope),
                BinaryExp => throw new Exception("Binary not implemented yet"),
                _ => throw new NotSupportedException($"Unsupported expression type: {exp.GetType().Name}")
            };
        }

        private static RuntimeObj? ResolveFunctionCallExp(FuncCallExp funcCallExp, RuntimeScope scope)
        {
            var task = scope.GetCustomTask(funcCallExp.FunctionName) ?? throw new Exception($"Function {funcCallExp.FunctionName} not found");

            //Validate if all params exist and their types are valid, add them to dictionary too.
            Dictionary<string, RuntimeObj> paramObj = [];
            if (task.ParamNameParamTypePair != null)
                foreach (var kv in task.ParamNameParamTypePair)
                {
                    string paramName = kv.Key;
                    if (!funcCallExp.ParamAndValPair.ContainsKey(paramName)) throw new Exception($"Function call {funcCallExp.FunctionName} Missing param {paramName}");
                    var runtimeObj = ConvertExpressionToRuntimeObject(funcCallExp.ParamAndValPair[paramName], scope);
                    paramObj.Add(paramName, runtimeObj);
                }
            var statements = task.TaskScope;
            var funcScope = new RuntimeScope([], [], scope);
            //Add param as variables to the funcScope
            foreach (var item in paramObj)
            {
                funcScope.CreatedObjects.Add(item.Key, item.Value);
            }
            foreach (var s in statements.Statements)
            {
                if (s is ReturnStmt)
                {
                    return ConvertExpressionToRuntimeObject((s as ReturnStmt).Expression, funcScope);
                }
                else
                {
                    HandleStatement(s, funcScope);
                }
            }
            return null;
        }

        private static RuntimeObj ResolveNestedVariable(NestedVariableExp nestedVar, RuntimeScope scope)
        {
            if (nestedVar.NextNode == null)
            {
                var varRef = scope.GetVariable(nestedVar.VarName)
                              ?? CustomTypeHelper.CreateObjectFromCustomType(scope.GetCustomType(nestedVar.VarName), scope);

                return varRef ?? throw new Exception($"Custom type not found: {nestedVar.VarName}");
            }

            var currentVar = scope.GetVariable(nestedVar.VarName)
                              ?? throw new Exception($"Variable not found: {nestedVar.VarName}");

            while (nestedVar.NextNode != null)
            {
                nestedVar = nestedVar.NextNode;
                var nestedVars = currentVar.Val as Dictionary<string, RuntimeObj>;

                if (nestedVars == null)
                {
                    throw new Exception($"Variable {currentVar.Val} is not a nested object");
                }

                if (!nestedVars.TryGetValue(nestedVar.VarName, out currentVar))
                {
                    throw new Exception($"Nested variable not found: {nestedVar.VarName}");
                }
            }

            return currentVar;
        }

        private static void ProcessIfStatement(IfStmt stmt, RuntimeScope scope)
        {
            // Implementation for handling If statements
        }

        private static void ProcessFunctionCallStatement(FunctionCallStmt stmt, RuntimeScope scope)
        {
            //Check if function exists
            var task = scope.GetCustomTask(stmt.FunctionName);
            if (task == null)
            {
                throw new Exception($"Function {stmt.FunctionName} not found");
            }
            var functionExp = stmt.FunctionExp;
            var functionReturnVal = ConvertExpressionToRuntimeObject(functionExp, scope);
        }
    }
}
