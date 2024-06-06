


namespace FrontEnd.Parser.Models.Expressions
{
    /// <summary>
    /// Represents a function call
    /// </summary>
    public class FuncCallExp : ExpressionStmt
    {
        public FuncCallExp(string functionName, Dictionary<string, string>? paramAndValPair) : base("Function Call Exp")
        {
            ParamAndValPair = paramAndValPair;
            FunctionName = functionName;
        }

        public Dictionary<string, string>? ParamAndValPair { get; set; } //Parameters of the function
        public string FunctionName;
    }
}
