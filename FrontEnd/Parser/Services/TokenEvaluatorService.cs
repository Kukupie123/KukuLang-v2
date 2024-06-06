using FrontEnd.Commons.Tokens;
using FrontEnd.Parser.Models.CustomTask;
using FrontEnd.Parser.Models.CustomType;
using FrontEnd.Parser.Models.Exceptions;
using FrontEnd.Parser.Models.Expressions;
using FrontEnd.Parser.Models.Scope;
using FrontEnd.Parser.Models.Stmt;
using FrontEnd.Parser.Parsers;
using FrontEnd.Parser.Parsers.Pratt;
using System.Data;

namespace FrontEnd.Parser.Services;

class TokenEvaluatorService
{
    public static void EvaluateToken<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> ParserBase, ASTScope scope)
    {
        //Note to self : Each sub evaluate statement needs to consume the . or }
        switch (ParserBase.CurrentToken.Type)
        {
            case TokenType.Define:
                EvaluateDefineToken(ParserBase, scope);
                break;
            case TokenType.Set:
                EvaluateSetToken(ParserBase, scope);
                break;
            case TokenType.If:
                EvaluateIfToken(ParserBase, scope);
                break;
            default:
                throw new UnknownTokenException(ParserBase.CurrentToken);
        }
    }
    private static void EvaluateDefineToken<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> Parser, ASTScope Scope)
    {
        switch (Parser.CurrentToken.Type)
        {
            case TokenType.Define:
                Parser.Advance(); //Advance to identifier token
                TokenValidatorService.ValidateToken(TokenType.Identifier, Parser.CurrentToken);
                Token taskNameToken = Parser.ConsumeCurrentToken(); //Store identifier Advance to "returning" or "with"
                if (Parser.CurrentToken.Type == TokenType.Returning)
                {
                    //Example :- "Define Square returning nothing with num(int)."
                    Parser.Advance(); //Advance to "nothing".
                    var returnTypeToken = Parser.ConsumeCurrentToken(); //Store "nothing" and Advance to "with" or ".".
                    TokenValidatorService.ValidateToken([TokenType.Identifier, TokenType.Nothing], returnTypeToken);

                    //Check if it has params. If not we move to body directly
                    Dictionary<string, string>? paramTypeVariables = null;
                    if (Parser.CurrentToken.Type == TokenType.With)
                    {
                        Parser.Advance(); //Advance to the first param
                        paramTypeVariables = StoreArgs(Parser);
                        //Once out we should be at '{'
                    }
                    //Enter the task block now and store it's instructions
                    TokenValidatorService.ValidateToken(TokenType.CurlyBracesOpening, Parser.CurrentToken);
                    Parser.Advance(); //Consume the '{'
                    var taskScope = new ASTScope($"{Scope.ScopeName}->{taskNameToken.Value}"); //Task body
                    while (Parser.CurrentToken.Type != TokenType.CurlyBracesClosing)
                    {
                        EvaluateToken(Parser, taskScope);
                    }
                    CustomTaskBase customTask = new(taskNameToken.Value, returnTypeToken.Value,
                    paramTypeVariables, taskScope);
                    Scope.CustomTasks.Add(taskNameToken.Value, customTask);
                    Parser.Advance(); //Consume the '}'
                    return;
                }
                else if (Parser.CurrentToken.Type == TokenType.With)
                {
                    //Example :- "Define Human with age(int), name(text)."
                    Parser.Advance(); //Advance to first property such as "age"
                    TokenValidatorService.ValidateToken(TokenType.Identifier, Parser.CurrentToken);
                    Dictionary<string, string>? typeVariables = StoreArgs(Parser);
                    if (typeVariables.Count <= 0) throw new NoPropertyException(Parser.CurrentToken);
                    TokenValidatorService.ValidateToken(TokenType.FullStop, Parser.CurrentToken);
                    CustomTypeBase customType = new(taskNameToken.Value, typeVariables);
                    //Add this to the scope
                    Scope.CustomTypes.Add(taskNameToken.Value, customType);
                    TokenValidatorService.ValidateToken(TokenType.FullStop, Parser.CurrentToken);
                    Parser.Advance(); //Consume the .
                    return;
                }
                throw new UnexpectedTokenException([new Token(TokenType.With, "with", -1), new Token(TokenType.Returning, "returning", -1)], Parser.CurrentToken);
        }


    }
    private static void EvaluateSetToken<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> Parser, ASTScope Scope)
    {
        //Example :- Set a to 12.
        //Example :- Set kuku's name to "Kuku".

        TokenValidatorService.ValidateToken(TokenType.Set, Parser.CurrentToken);
        //advance to a or kuku
        Parser.Advance();
        var variableNameToken = Parser.CurrentToken;
        var prattParser = new PrattParser(Parser.Tokens, Parser._Pos);
        var variableExp = prattParser.Parse() as VariableExp ?? throw new Exception($"Variable Expression was evaluated to null for token {variableNameToken}"); //Throw exception if null
        Parser._Pos = prattParser._Pos; //Update the main parser's _pos
        TokenValidatorService.ValidateToken(TokenType.To, Parser.CurrentToken);
        if (variableExp == null)
        {
            throw new Exception($"Failed to parse variable name token {variableNameToken.Value}");
        }
        Parser.Advance(); // Advance to the start of the value expression.
        prattParser = new PrattParser(Parser.Tokens, Parser._Pos);
        var value = prattParser.Parse();
        Parser._Pos = prattParser._Pos; //Update the main parser's _pos
        SetToStmt setToStmt = new(variableExp, value);
        Scope.Statements.Add(setToStmt);
        Parser.Advance(); //Consume the "."
    }

    private static void EvaluateIfToken<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> Parser, ASTScope Scope)
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


    /** <summary>
     * Gathers all the properties until . or {
     * Doesn't consume the . or {
     * </summary>
     */
    public static Dictionary<string, string> StoreArgs<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> Parser)
    {
        Dictionary<string, string> args = [];

        //Keep iterating until we reach . or {
        while (Parser.CurrentToken.Type != TokenType.FullStop && Parser.CurrentToken.Type != TokenType.CurlyBracesOpening)
        {
            //For Params that come after 1st it is going to have "," so we need to advance().
            if (Parser.CurrentToken.Type == TokenType.Comma)
            {
                Parser.Advance(); //Move to the next param token.
            }
            TokenValidatorService.ValidateToken([TokenType.Identifier, TokenType.IntegerLiteral, TokenType.FloatLiteral, TokenType.TextLiteral], Parser.CurrentToken);
            Token propertyToken = Parser.ConsumeCurrentToken(); //Store PropertyName and Advance to "(".
            TokenValidatorService.ValidateToken(TokenType.RoundBracketsOpening, Parser.CurrentToken);
            Parser.Advance(); //Advance to "propertyType" such as "int".
            TokenValidatorService.ValidateToken([TokenType.Identifier, TokenType.IntegerLiteral, TokenType.FloatLiteral, TokenType.TextLiteral], Parser.CurrentToken);
            args.Add(propertyToken.Value, Parser.CurrentToken.Value.ToString());
            Parser.Advance(); //Advance to closing ")".
            Parser.Advance(); //Advance to closing "," or "." or "}".
        }
        return args;
    }
}