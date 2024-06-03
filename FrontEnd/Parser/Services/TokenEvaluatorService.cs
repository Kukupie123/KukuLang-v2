using FrontEnd.Commons.Tokens;

namespace FrontEnd;

class TokenEvaluatorService
{
    public static void EvaluateDefineToken(dynamic ParserBase, ASTScope scope)
    {

        var parser = ParserBase as RecursiveDescentParser;
        switch (parser.CurrentToken.Type)
        {
            case TokenType.Define:
                parser.Advance(); //Advance to identifier token
                TokenValidatorService.validateToken(TokenType.Identifier, parser.CurrentToken);
                Token definitionToken = parser.ConsumeCurrentToken(); //Store identifier Advance to "returning" or "with"
                if (parser.CurrentToken.Type == TokenType.Returning)
                {
                }
                else if (parser.CurrentToken.Type == TokenType.With)
                {
                    parser.Advance(); //Advance to first identifier such as "age" in "Define Human with age(int), name(text)."
                    TokenValidatorService.validateToken(TokenType.Identifier, parser.CurrentToken);
                    Dictionary<string, string>? typeVariables = null;
                    //Iterate and store each property
                    while (parser.CurrentToken.Type != TokenType.FullStop)
                    {
                        //For properties that come after 1st it is going to have "," so we need to advance().
                        if (parser.CurrentToken.Type == TokenType.Comma)
                        {
                            parser.Advance(); //Move to the next token.
                        }
                        TokenValidatorService.validateToken(TokenType.Identifier, parser.CurrentToken);
                        Token propertyToken = parser.ConsumeCurrentToken(); //Store PropertyName and Advance to "(".
                        TokenValidatorService.validateToken(TokenType.RoundBracketsOpening, parser.CurrentToken);
                        parser.Advance(); //Advance to "propertyType" such as "int" in "Define Human with age(int), name(text).".
                        TokenValidatorService.validateToken(TokenType.Identifier, parser.CurrentToken);
                        if (typeVariables == null)
                        {
                            typeVariables = new();
                        }
                        typeVariables.Add(propertyToken.Value, parser.CurrentToken.Value.ToString());
                        parser.Advance(); //Advance to closing ")".
                        TokenValidatorService.validateToken(TokenType.RoundBracketsClosing, parser.CurrentToken);
                        parser.Advance(); //Advance to , or .
                    }
                    if (typeVariables == null) throw new NoParameterOrPropertyException(definitionToken);
                    parser.Advance(); //If we are out of the loop we are at '.' token. So we need to skip over it.
                    CustomType customType = new CustomType(definitionToken.Value, typeVariables);
                    //Add this to the scope
                    scope.CustomTypes.Add(definitionToken.Value, customType);
                    return;
                }
                throw new UnexpectedTokenException([new Token(TokenType.With, "with", -1), new Token(TokenType.Returning, "returning", -1)], parser.CurrentToken);
        }
    }
}