﻿
using FrontEnd.Parser.Models.CustomTask;
using FrontEnd.Parser.Models.CustomType;

namespace KukuLang.Interpreter.Model.Scope
{
    public class RuntimeScope(Dictionary<string, CustomTypeBase> declaredTypes,
     Dictionary<string, CustomTaskBase> declaredTasks, RuntimeScope? parentScope)
    {
        public Dictionary<string, CustomTypeBase> DeclaredTypes = declaredTypes;
        public Dictionary<string, CustomTaskBase> DeclaredTasks = declaredTasks;
        public Dictionary<string, RuntimeObj.RuntimeObj> CreatedObjects = [];
        public RuntimeScope? ParentScope = parentScope;

        /// <summary>
        /// Attempts to update variable in current scope or parents scope.
        /// If none found creates a new one in this scope
        /// </summary>
        public void UpdateScopeVariable(string varName, RuntimeObj.RuntimeObj instance)
        {
            Stack<RuntimeScope> stack = [];
            stack.Push(this);
            while (stack.Count > 0)
            {
                var currentScope = stack.Pop();
                if (currentScope.CreatedObjects.ContainsKey(varName))
                {
                    currentScope.CreatedObjects[varName] = instance;
                    Console.WriteLine($"Updating variable {varName} : {instance.Val} in {this}");
                    return;
                }
                if (ParentScope != null)
                {
                    stack.Push(ParentScope);
                }
            }

            //If we reach here it means we have not created this variable yet
            CreatedObjects.Add(varName, instance);
            Console.WriteLine($"Creating variable {varName} : {instance.Val} in {this}");
        }
        public RuntimeObj.RuntimeObj? GetVariable(string name)
        {
            if (CreatedObjects.ContainsKey(name))
                return CreatedObjects[name];
            if (ParentScope != null)
            {
                return ParentScope.GetVariable(name);
            }
            return null;
        }

        public CustomTypeBase? GetCustomType(string typeName)
        {
            if (declaredTypes.ContainsKey(typeName))
            {
                return declaredTypes[typeName];
            }
            if (ParentScope != null)
            {
                return ParentScope.GetCustomType(typeName);
            }
            return null;
        }
    }


}
