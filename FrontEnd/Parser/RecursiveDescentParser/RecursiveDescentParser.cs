using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public class RecursiveDescentParser(List<Token> tokens)
{
    int _index;
    private Token _currentToken => tokens[_index];
    private void _advance() => _index++;
    public BlockBase Parse()
    {
        var rootBlock = new BlockBase();
        while (_currentToken.Type != TokenType.EOF)
        {

        }
        return rootBlock;
    }

    public void StatementHandler()
    {
        var token = _currentToken;
        switch (token.Type)
        {
            case TokenType.Keyword:
                break;
        }
    }

    public void KeywordHandler()
    {
        var token = _currentToken;
        switch (token.Value)
        {
            case "define":
                break;
        }
    }
    public void defineHandler()
    {
        var token = _currentToken; //keyword : define
        _tokenValidator(token, TokenType.Keyword, "define");
        _advance();
        token = _currentToken; //variable or function or structure
        if (token.Type != TokenType.Keyword) throw new Exception($"Expected token of type keyword but got {token}");
        switch (token.Value)
        {
            case "variable":
                _advance();
                token = _currentToken; //identifier : <name>
                if (token.Type != TokenType.Identifier) throw new Exception($"Expected token of type identifier but got {token}");
                string variableName = token.Value;
                _advance();
                token = _currentToken; //keyword : of_type
                _tokenValidator(token, TokenType.Keyword, "of_type");
                token = _currentToken; //identifier : <data_type>
                if (token.Type != TokenType.Identifier) throw new Exception($"Expected token of type identifier but got {token}");
                switch (token.Value)
                {
                    case "number":
                        break;
                    case "text":
                        break;
                    default:
                        //TODO: Check if the custom struture exists in this or parent scope
                        break;
                }
                break;
            default:
                throw new Exception($"Expected keyword token and value(variable, task, structure) but got {token}");
        }

    }

    private void _tokenValidator(Token token, TokenType tokenType, dynamic tokenValue)
    {
        if (token.Type != tokenType || token.Value! - tokenValue) throw new Exception($"Expected token {new Token(tokenType, tokenValue)} but got {token}");
    }
}
