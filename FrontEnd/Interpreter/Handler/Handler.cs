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
        /// <summary>
        /// Handles the dispatch of different statement types to their respective handlers.
        /// </summary>
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

        /// <summary>
        /// Processes SetTo statements, deciding whether it's a simple or nested variable assignment.
        /// </summary>
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

        /// <summary>
        /// Assigns a value to a simple variable (non-nested).
        /// </summary>
        private static void AssignSimpleVariable(string variableName, ExpressionStmt value, RuntimeScope scope)
        {
            var runtimeObject = ConvertExpressionToRuntimeObject(value, scope);
            scope.UpdateScopeVariable(variableName, runtimeObject);
        }

        /// <summary>
        /// Assigns a value to a nested variable.
        /// </summary>
        private static void AssignNestedVariable(NestedVariableExp nestedVar, ExpressionStmt value, RuntimeScope scope)
        {
            //Get root variable
            var currentVar = scope.GetVariable(nestedVar.VarName)
                              ?? throw new Exception($"Variable not found: {nestedVar.VarName}");
            RuntimeObj? parentVar = currentVar;
            while (nestedVar.NextNode != null)
            {
                nestedVar = nestedVar.NextNode;
                parentVar = currentVar;
                var nestedVars = currentVar.Val as Dictionary<string, RuntimeObj>;

                // Check if the current variable is a valid nested dictionary
                if (nestedVars == null)
                {
                    throw new Exception($"Variable {currentVar.Val} is not a nested object");
                }

                if (!nestedVars.TryGetValue(nestedVar.VarName, out currentVar))
                {
                    throw new Exception($"Nested variable not found: {nestedVar.VarName}");
                }
                currentVar = nestedVars[nestedVar.VarName];
            }
            (parentVar.Val as Dictionary<string, RuntimeObj>)[nestedVar.VarName] = ConvertExpressionToRuntimeObject(value, scope);
        }

        /// <summary>
        /// Traverses to the last node in a nested variable expression.
        /// </summary>
        private static (RuntimeObj currentVar, RuntimeObj parentVar) TraverseToLastNode(NestedVariableExp nestedVar, RuntimeScope scope)
        {
            //Get root variable
            var currentVar = scope.GetVariable(nestedVar.VarName)
                              ?? throw new Exception($"Variable not found: {nestedVar.VarName}");
            RuntimeObj? parentVar = currentVar;

            while (nestedVar.NextNode != null)
            {
                nestedVar = nestedVar.NextNode;
                parentVar = currentVar;
                var nestedVars = currentVar.Val as Dictionary<string, RuntimeObj>;

                // Check if the current variable is a valid nested dictionary
                if (nestedVars == null)
                {
                    throw new Exception($"Variable {currentVar.Val} is not a nested object");
                }

                if (!nestedVars.TryGetValue(nestedVar.VarName, out currentVar))
                {
                    throw new Exception($"Nested variable not found: {nestedVar.VarName}");
                }
                currentVar = nestedVars[nestedVar.VarName];
            }

            return (currentVar, parentVar);
        }

        /// <summary>
        /// Converts an expression statement to a runtime object.
        /// </summary>
        private static RuntimeObj ConvertExpressionToRuntimeObject(ExpressionStmt exp, RuntimeScope scope)
        {
            return exp switch
            {
                IntLiteral intLiteral => new RuntimeObj(RuntimeObjType.Integer, intLiteral.Val),
                TextLiteral textLiteral => new RuntimeObj(RuntimeObjType.Text, textLiteral.Val),
                NestedVariableExp nestedVar => ResolveNestedVariable(nestedVar, scope),
                FuncCallExp or BinaryExp => throw new NotImplementedException("Function call and binary expressions are not yet implemented."),
                _ => throw new NotSupportedException($"Unsupported expression type: {exp.GetType().Name}")
            };
        }

        /// <summary>
        /// Resolves a nested variable to its corresponding runtime object.
        /// </summary>
        private static RuntimeObj ResolveNestedVariable(NestedVariableExp nestedVar, RuntimeScope scope)
        {
            // Handle the case where there is no next node
            if (nestedVar.NextNode == null)
            {
                var varRef = scope.GetVariable(nestedVar.VarName)
                              ?? CustomTypeHelper.CreateObjectFromCustomType(scope.GetCustomType(nestedVar.VarName), scope);

                return varRef ?? throw new Exception($"Custom type not found: {nestedVar.VarName}");
            }

            // Traverse through the nested nodes
            var currentVar = scope.GetVariable(nestedVar.VarName)
                              ?? throw new Exception($"Variable not found: {nestedVar.VarName}");

            while (nestedVar.NextNode != null)
            {
                nestedVar = nestedVar.NextNode;
                var nestedVars = currentVar.Val as Dictionary<string, RuntimeObj>;

                // Check if the current variable is a valid nested dictionary
                if (nestedVars == null)
                {
                    throw new Exception($"Variable {currentVar.Val} is not a nested object");
                }

                // Move to the next level in the nested structure
                if (!nestedVars.TryGetValue(nestedVar.VarName, out currentVar))
                {
                    throw new Exception($"Nested variable not found: {nestedVar.VarName}");
                }
            }

            return currentVar;
        }

        // Placeholder for processing If statements
        private static void ProcessIfStatement(IfStmt stmt, RuntimeScope scope)
        {
            // Implementation for handling If statements
        }

        // Placeholder for processing function call statements
        private static void ProcessFunctionCallStatement(FunctionCallStmt stmt, RuntimeScope scope)
        {
            // Implementation for handling function call statements
        }
    }
}
