using FrontEnd.Commons.Tokens;

namespace FrontEnd;

class TokenEvaluatorService
{
    public static void EvaluateDefineToken(dynamic ParserBase, ASTScope scope)
    {

        var parser = ParserBase as RecursiveDescentParser;
        var currentToken = parser.CurrentToken;

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
                    //Check if it has params. If not we stop here.
                    if (parser.CurrentToken.Type != TokenType.With)
                    {
                        TokenValidatorService.validateToken(TokenType.FullStop, parser.CurrentToken);
                        var task = new CustomTask(taskNameToken.Value, returnTypeToken.Value, new Dictionary<string, string>());
                        scope.CustomTasks.Add(taskNameToken.Value, task);
                        parser.Advance(); //Move to the next Statement.
                        return;
                    }
                    TokenValidatorService.validateToken([TokenType.Identifier, TokenType.Nothing], returnTypeToken);
                    TokenValidatorService.validateToken(TokenType.With, parser.CurrentToken);
                    parser.Advance(); //Advance to the first param such as "num"
                    Dictionary<string, string>? paramTypeVariables = StoreArgs();
                    //StoreArgs will process upto a terminator.
                    CustomTask customTask = new CustomTask(taskNameToken.Value, returnTypeToken.Value, paramTypeVariables);
                    //Add this to the scope
                    scope.CustomTasks.Add(taskNameToken.Value, customTask);
                    return;
                }
                else if (parser.CurrentToken.Type == TokenType.With)
                {
                    //Example :- "Define Human with age(int), name(text)."
                    parser.Advance(); //Advance to first property such as "age"
                    TokenValidatorService.validateToken(TokenType.Identifier, parser.CurrentToken);
                    Dictionary<string, string>? typeVariables = StoreArgs();
                    if (typeVariables.Count <= 0) throw new NoParameterOrPropertyException(parser.CurrentToken);
                    CustomType customType = new CustomType(taskNameToken.Value, typeVariables);
                    //Add this to the scope
                    scope.CustomTypes.Add(taskNameToken.Value, customType);
                    return;
                }
                throw new UnexpectedTokenException([new Token(TokenType.With, "with", -1), new Token(TokenType.Returning, "returning", -1)], parser.CurrentToken);
        }

        //Iterate parameters of Define instruction and store it.
        Dictionary<string, string> StoreArgs()
        {
            Dictionary<string, string> args = new();
            while (parser.CurrentToken.Type != TokenType.FullStop)
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
            parser.Advance(); //If we are out of the loop we are at '.' token. So we need to skip over it and move to the next statement.
            return args;
        }
    }
}