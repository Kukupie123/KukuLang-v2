using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FrontEnd.Commons.Tokens;
using System.Net;

namespace FrontEnd.Lexer;

public class KukuLexer
{
    private int _inputStartPos;
    private int _inputEndPos;
    private readonly string _kukuLangSourceCode;

    public KukuLexer(string kukuLangSourceCode)
    {
        this._kukuLangSourceCode = kukuLangSourceCode;
    }

    private string Input()
    {
        var sb = new StringBuilder();
        for (int i = _inputStartPos; i <= _inputEndPos; i++)
        {
            sb.Append(_kukuLangSourceCode[i]);
        }

        return sb.ToString();
    }

    private void MoveInputPositionForward()
    {
        _inputStartPos = _inputEndPos + 1;
        _inputEndPos = _inputStartPos;
    }

    private void UpdateEndPos()
    {
        /*
        The logic is to increment _inputEndPos and then checking if the string is a token.
        We stop when we meet a white space, ", ~
        */

        //Scoped function for ease of access
        void setEndPos(int i)
        {
            int updatedI = i < _inputStartPos ? _inputStartPos : i;
            int lastIndex = _kukuLangSourceCode.Length - 1;
            _inputEndPos = updatedI >= lastIndex ? lastIndex : updatedI;
        }
        bool HitDelimiter(string character)
        {
            return string.IsNullOrWhiteSpace(character) || TokenMap.IsValueAGenericToken(character);
        }

        string startChar = _kukuLangSourceCode[_inputEndPos].ToString();

        //If we start off with a white space we need to move to the next valid character
        while (string.IsNullOrWhiteSpace(startChar))
        {
            _inputStartPos++;
            startChar = _kukuLangSourceCode[_inputStartPos].ToString();
        }

        //Handle if startChar is currently ' or " or ~ first
        if (startChar is "~" or "'" or "\"")
        {
            if (startChar == "~")
            {
                bool throwException = true;
                for (int i = _inputStartPos + 1; i < _kukuLangSourceCode.Length; i++)
                {
                    var endChar = _kukuLangSourceCode[i].ToString();
                    if (endChar == "~")
                    {
                        //If ~ is the last element of the source code then we can't set _inputStartPos to i+1 as it will exceed the range.
                        //Updating _inputStartPos instead of _inputEndPos as we are meant to ignore comments.
                        _inputStartPos = i + 1 < _kukuLangSourceCode.Length ? i + 1 : i;
                        //We do not return unlike the rest of the if branch as we are meant to ignore comments and process what's next.
                        UpdateEndPos();
                        return;
                    }
                }
                if (throwException)
                    throw new Exception($"Comment opened in line {_inputStartPos} was never closed");
            }
            else if (startChar == "'")
            {
                if (_kukuLangSourceCode[_inputStartPos + 1] != 's')
                {
                    throw new Exception($"Expected s after ' to create an accessor token ('s) at position {_inputStartPos}");
                }
                _inputEndPos = _inputStartPos + 1;
                return;
            }
            else
            {
                for (int i = _inputStartPos + 1; i < _kukuLangSourceCode.Length; i++)
                {
                    var endChar = _kukuLangSourceCode[i].ToString();
                    if (endChar == "\"")
                    {
                        //We reached the end of the string and can set it as the end position and return.
                        _inputEndPos = i;
                        return;
                    }
                }
                throw new Exception($"String opened in line {_inputStartPos} was never closed");
            }
        }


        //Increment _inputEndPos upto source code length checking if the input is a string every iteration
        for (int i = _inputStartPos; i < _kukuLangSourceCode.Length; i++)
        {
            var endChar = _kukuLangSourceCode[i].ToString();
            //Delimiter check
            if (HitDelimiter(endChar))
            {
                setEndPos(i - 1);
                return;
            }
        }
    }

    private Token GenerateToken()
    {
        var input = Input();
        //Check if its in token map
        if (TokenMap.IsValueAGenericToken(input))
        {
            return new Token(TokenMap.GetTypeByValue(input), input, _inputStartPos);
        }
        //Check if its number
        if (float.TryParse(input, out var f))
        {
            int i = (int)f;
            if (i == f)
                return new Token(TokenType.Integer, i, _inputStartPos);
            return new Token(TokenType.Float, f, _inputStartPos);
        }
        //Check if its a string
        else if (input[0] == '"' && input[input.Length - 1] == '"')
        {
            //Remove the starting and ending "
            return new Token(TokenType.String, input[1..^1], _inputStartPos);
        }
        //Add it as identifier
        return new Token(TokenType.Identifier, input, _inputStartPos);
    }


    public List<Token> Tokenize()
    {
        List<Token> tokens = [];
        while (_inputStartPos < _kukuLangSourceCode.Length)
        {
            UpdateEndPos();
            tokens.Add(GenerateToken());
            MoveInputPositionForward();
        }
        tokens.Add(new Token(TokenType.EOF, "EOF", _inputEndPos + 1));
        return tokens;
    }
}