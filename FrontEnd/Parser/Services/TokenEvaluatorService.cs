using FrontEnd.Commons.Tokens;

namespace FrontEnd;

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
        }
    }
    private static void EvaluateDefineToken(RecursiveDescentParser ParserBase, ASTScope scope)
    {

        var parser = ParserBase as RecursiveDescentParser;
        switch (parser.CurrentToken.Type)
        {
            case TokenType.Define:
                parser.Advance(); //Advance to identifier token
                TokenValidatorService.validateToken(TokenType.Identifier, parser.CurrentToken);
                Token taskNameToken = parser.ConsumeCurrentToken(); //Store identifier Advance to "returning" or "with"
                if (parser.CurrentToken.Type == TokenType.Returning)
                {
                    //Example :- "Define Square returning nothing with num(int)."
                    parser.Advance(); //Advance to "nothing".
                    var returnTypeToken = parser.ConsumeCurrentToken(); //Store "nothing" and Advance to "with" or ".".
                    TokenValidatorService.validateToken([TokenType.Identifier, TokenType.Nothing], returnTypeToken);

                    //Check if it has params. If not we move to body directly
                    Dictionary<string, string>? paramTypeVariables = null;
                    if (parser.CurrentToken.Type == TokenType.With)
                    {
                        parser.Advance(); //Advance to the first param
                        paramTypeVariables = StoreArgs();
                        //Once out we should be at '{'
                    }
                    //Enter the task block now and store it's instructions
                    TokenValidatorService.validateToken(TokenType.CurlyBracesOpening, parser.CurrentToken);
                    parser.Advance(); //Consume the '{'
                    var taskScope = new ASTScope(); //Task body
                    while (parser.CurrentToken.Type != TokenType.CurlyBracesClosing)
                    {
                        TokenEvaluatorService.EvaluateToken(parser, taskScope);
                    }
                    CustomTask customTask = new CustomTask(taskNameToken.Value, returnTypeToken.Value,
                    paramTypeVariables, taskScope);
                    scope.CustomTasks.Add(taskNameToken.Value, customTask);
                    parser.Advance(); //Consume the '}'
                    return;
                }
                else if (parser.CurrentToken.Type == TokenType.With)
                {
                    //Example :- "Define Human with age(int), name(text)."
                    parser.Advance(); //Advance to first property such as "age"
                    TokenValidatorService.validateToken(TokenType.Identifier, parser.CurrentToken);
                    Dictionary<string, string>? typeVariables = StoreArgs();
                    if (typeVariables.Count <= 0) throw new NoParameterOrPropertyException(parser.CurrentToken);
                    TokenValidatorService.validateToken(TokenType.FullStop, parser.CurrentToken);
                    CustomType customType = new CustomType(taskNameToken.Value, typeVariables);
                    //Add this to the scope
                    scope.CustomTypes.Add(taskNameToken.Value, customType);
                    TokenValidatorService.validateToken(TokenType.FullStop, parser.CurrentToken);
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
                TokenValidatorService.validateToken(TokenType.Identifier, parser.CurrentToken);
                Token propertyToken = parser.ConsumeCurrentToken(); //Store PropertyName and Advance to "(".
                TokenValidatorService.validateToken(TokenType.RoundBracketsOpening, parser.CurrentToken);
                parser.Advance(); //Advance to "propertyType" such as "int".
                TokenValidatorService.validateToken(TokenType.Identifier, parser.CurrentToken);
                args.Add(propertyToken.Value, parser.CurrentToken.Value.ToString());
                parser.Advance(); //Advance to closing ")".
                parser.Advance(); //Advance to closing "," or ".".
            }
            return args;
        }
    }
    private static void EvaluateSetToken(RecursiveDescentParser ParserBase, ASTScope scope)
    {
        //Example :- Set a to 12.
        //Example :- Set kuku's name to "Kuku".

        //Set
        var parser = ParserBase as RecursiveDescentParser;
        TokenValidatorService.validateToken(TokenType.Set, parser.CurrentToken);
        //advance to a or kuku
        parser.Advance();
        var variableNameToken = parser.CurrentToken;
        var prattParser = new PrattParser(parser.Tokens, parser._Pos);
        var variableExp = prattParser.Parse() as VariableExp;
        parser._Pos = prattParser._Pos; //Update the main parser's _pos
        TokenValidatorService.validateToken(TokenType.To, parser.CurrentToken);
        if (variableExp == null)
        {
            throw new Exception($"Failed to parse variable name token {variableNameToken.Value}");
        }
        parser.Advance(); // Advance to the start of the value expression.
        prattParser = new PrattParser(parser.Tokens, parser._Pos);
        var value = prattParser.Parse();
        parser._Pos = prattParser._Pos; //Update the main parser's _pos
        SetToStmt setToStmt = new SetToStmt(variableExp, value);
        scope.Statements.Add(setToStmt);
        parser.Advance(); //Consume the "."
        return;


    }
}