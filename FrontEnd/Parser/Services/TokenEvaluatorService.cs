using FrontEnd.Commons.Tokens;
using FrontEnd.Parser.Models.CustomTask;
using FrontEnd.Parser.Models.CustomType;
using FrontEnd.Parser.Models.Exceptions;
using FrontEnd.Parser.Models.Expressions;
using FrontEnd.Parser.Models.Scope;
using FrontEnd.Parser.Models.Stmt;
using FrontEnd.Parser.Parsers.Pratt;
using System.Data;

namespace FrontEnd.Parser.Services;

class TokenEvaluatorService
{
    public static void EvaluateToken(dynamic ParserBase, ASTScope scope)
    {
        var parser = ParserBase as RecursiveDescentParser;
        switch (parser.CurrentToken.Type)
        {
            case TokenType.Define:
                EvaluateDefineToken(parser, scope);
                break;
            case TokenType.Set:
                EvaluateSetToken(parser, scope);
                break;
            case TokenType.If:
                EvaluateIfToken(parser, scope);
                break;
            default:
                throw new UnknownTokenException(parser.CurrentToken);
        }
    }
    private static void EvaluateDefineToken(RecursiveDescentParser ParserBase, ASTScope Scope)
    {

        var parser = ParserBase as RecursiveDescentParser;
        switch (parser.CurrentToken.Type)
        {
            case TokenType.Define:
                parser.Advance(); //Advance to identifier token
                TokenValidatorService.ValidateToken(TokenType.Identifier, parser.CurrentToken);
                Token taskNameToken = parser.ConsumeCurrentToken(); //Store identifier Advance to "returning" or "with"
                if (parser.CurrentToken.Type == TokenType.Returning)
                {
                    //Example :- "Define Square returning nothing with num(int)."
                    parser.Advance(); //Advance to "nothing".
                    var returnTypeToken = parser.ConsumeCurrentToken(); //Store "nothing" and Advance to "with" or ".".
                    TokenValidatorService.ValidateToken([TokenType.Identifier, TokenType.Nothing], returnTypeToken);

                    //Check if it has params. If not we move to body directly
                    Dictionary<string, string>? paramTypeVariables = null;
                    if (parser.CurrentToken.Type == TokenType.With)
                    {
                        parser.Advance(); //Advance to the first param
                        paramTypeVariables = StoreArgs();
                        //Once out we should be at '{'
                    }
                    //Enter the task block now and store it's instructions
                    TokenValidatorService.ValidateToken(TokenType.CurlyBracesOpening, parser.CurrentToken);
                    parser.Advance(); //Consume the '{'
                    var taskScope = new ASTScope($"{Scope.ScopeName}->{taskNameToken.Value}"); //Task body
                    while (parser.CurrentToken.Type != TokenType.CurlyBracesClosing)
                    {
                        EvaluateToken(parser, taskScope);
                    }
                    CustomTask customTask = new(taskNameToken.Value, returnTypeToken.Value,
                    paramTypeVariables, taskScope);
                    Scope.CustomTasks.Add(taskNameToken.Value, customTask);
                    parser.Advance(); //Consume the '}'
                    return;
                }
                else if (parser.CurrentToken.Type == TokenType.With)
                {
                    //Example :- "Define Human with age(int), name(text)."
                    parser.Advance(); //Advance to first property such as "age"
                    TokenValidatorService.ValidateToken(TokenType.Identifier, parser.CurrentToken);
                    Dictionary<string, string>? typeVariables = StoreArgs();
                    if (typeVariables.Count <= 0) throw new NoParameterOrPropertyException(parser.CurrentToken);
                    TokenValidatorService.ValidateToken(TokenType.FullStop, parser.CurrentToken);
                    CustomType customType = new(taskNameToken.Value, typeVariables);
                    //Add this to the scope
                    Scope.CustomTypes.Add(taskNameToken.Value, customType);
                    TokenValidatorService.ValidateToken(TokenType.FullStop, parser.CurrentToken);
                    parser.Advance();
                    return;
                }
                throw new UnexpectedTokenException([new Token(TokenType.With, "with", -1), new Token(TokenType.Returning, "returning", -1)], parser.CurrentToken);
        }

        //Iterate parameters of Define instruction and store it.
        Dictionary<string, string> StoreArgs()
        {
            Dictionary<string, string> args = new();
            while (parser.CurrentToken.Type != TokenType.FullStop && parser.CurrentToken.Type != TokenType.CurlyBracesOpening)
            {
                //For Params that come after 1st it is going to have "," so we need to advance().
                if (parser.CurrentToken.Type == TokenType.Comma)
                {
                    parser.Advance(); //Move to the next param token.
                }
                TokenValidatorService.ValidateToken(TokenType.Identifier, parser.CurrentToken);
                Token propertyToken = parser.ConsumeCurrentToken(); //Store PropertyName and Advance to "(".
                TokenValidatorService.ValidateToken(TokenType.RoundBracketsOpening, parser.CurrentToken);
                parser.Advance(); //Advance to "propertyType" such as "int".
                TokenValidatorService.ValidateToken(TokenType.Identifier, parser.CurrentToken);
                args.Add(propertyToken.Value, parser.CurrentToken.Value.ToString());
                parser.Advance(); //Advance to closing ")".
                parser.Advance(); //Advance to closing "," or ".".
            }
            return args;
        }
    }
    private static void EvaluateSetToken(RecursiveDescentParser ParserBase, ASTScope Scope)
    {
        //Example :- Set a to 12.
        //Example :- Set kuku's name to "Kuku".

        //Set
        var parser = ParserBase as RecursiveDescentParser;
        TokenValidatorService.ValidateToken(TokenType.Set, parser.CurrentToken);
        //advance to a or kuku
        parser.Advance();
        var variableNameToken = parser.CurrentToken;
        var prattParser = new PrattParser(parser.Tokens, parser._Pos);
        var variableExp = prattParser.Parse() as VariableExp ?? throw new Exception($"Variable Expression was evaluated to null for token {variableNameToken}"); //Throw exception if null
        parser._Pos = prattParser._Pos; //Update the main parser's _pos
        TokenValidatorService.ValidateToken(TokenType.To, parser.CurrentToken);
        if (variableExp == null)
        {
            throw new Exception($"Failed to parse variable name token {variableNameToken.Value}");
        }
        parser.Advance(); // Advance to the start of the value expression.
        prattParser = new PrattParser(parser.Tokens, parser._Pos);
        var value = prattParser.Parse();
        parser._Pos = prattParser._Pos; //Update the main parser's _pos
        SetToStmt setToStmt = new(variableExp, value);
        Scope.Statements.Add(setToStmt);
        parser.Advance(); //Consume the "."
        return;


    }

    private static void EvaluateIfToken(RecursiveDescentParser Parser, ASTScope Scope)
    {
        //Example :- If a is b then
        Parser.Advance(); //Advance to the start of conditional expression
        PrattParser pratt = new(Parser.Tokens, Parser._Pos);
        var condition = pratt.Parse() ?? throw new InvalidExpressionException("Conditional expression isn't valid"); //Throw exception if null
        Parser._Pos = pratt._Pos; //Update the position of the main parser.
        //Current token will be "then"
        Parser.Advance(); //Advance to "{"
        TokenValidatorService.ValidateToken(TokenType.CurlyBracesOpening, Parser.CurrentToken);
        var ifScope = new ASTScope($"{Scope}->Conditional");
        Parser.Advance(); //Advance to the first statement.
        while (Parser.CurrentToken.Type != TokenType.CurlyBracesClosing)
        {
            EvaluateToken(Parser, ifScope);
        }
        var stmt = new IfStmt(condition, ifScope);
        Scope.Statements.Add(stmt);
        Parser.Advance(); //Consume "}"
    }
}