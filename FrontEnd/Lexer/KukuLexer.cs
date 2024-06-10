using FrontEnd.Commons.Tokens;
using System.Text;

namespace FrontEnd.Lexer
{
    public class KukuLexer
    {
        private int _inputStartPos;
        private int _inputEndPos;
        private int _currentLine = 1;
        private int _currentLinePosition = 0;
        private readonly string _kukuLangSourceCode;

        public KukuLexer(string kukuLangSourceCode)
        {
            _kukuLangSourceCode = kukuLangSourceCode;
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

            // Update the line and line position when encountering a newline character
            while (_inputStartPos < _kukuLangSourceCode.Length && (_kukuLangSourceCode[_inputStartPos] == '\n' || _kukuLangSourceCode[_inputStartPos] == '\r'))
            {
                if (_kukuLangSourceCode[_inputStartPos] == '\n')
                {
                    _currentLine++;
                    _currentLinePosition = 0;
                }
                _inputStartPos++;
                _inputEndPos = _inputStartPos;
            }

            if (_inputStartPos < _kukuLangSourceCode.Length && _kukuLangSourceCode[_inputStartPos] != '\n' && _kukuLangSourceCode[_inputStartPos] != '\r')
            {
                _currentLinePosition++;
            }
        }

        private void UpdateEndPos()
        {
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

            while (string.IsNullOrWhiteSpace(startChar))
            {
                _inputStartPos++;
                if (_inputStartPos >= _kukuLangSourceCode.Length)
                    return;
                startChar = _kukuLangSourceCode[_inputStartPos].ToString();
            }

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
                            _inputStartPos = i == _kukuLangSourceCode.Length - 1 ? _kukuLangSourceCode.Length : i + 1;
                            _inputEndPos = i == _kukuLangSourceCode.Length - 1 ? _kukuLangSourceCode.Length : i + 1;
                            UpdateEndPos();
                            return;
                        }
                    }
                    if (throwException)
                        throw new Exception($"Comment opened in line {_currentLine} was never closed");
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
                            _inputEndPos = i;
                            return;
                        }
                    }
                    throw new Exception($"String opened in line {_currentLine} was never closed");
                }
            }

            for (int i = _inputStartPos; i < _kukuLangSourceCode.Length; i++)
            {
                var endChar = _kukuLangSourceCode[i].ToString();
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
            if (string.IsNullOrEmpty(input))
            {
                throw new Exception($"Input is empty with range {_inputStartPos}-{_inputEndPos}");
            }

            if (TokenMap.IsValueAGenericToken(input))
            {
                return new Token(TokenMap.GetTypeByValue(input), input, _currentLinePosition, _currentLine);
            }

            if (float.TryParse(input, out var f))
            {
                int i = (int)f;
                if (i == f)
                    return new Token(TokenType.IntegerLiteral, i, _currentLinePosition, _currentLine);
                return new Token(TokenType.FloatLiteral, f, _currentLinePosition, _currentLine);
            }

            else if (input[0] == '"' && input[^1] == '"')
            {
                return new Token(TokenType.TextLiteral, input[1..^1], _currentLinePosition, _currentLine);
            }

            return new Token(TokenType.Identifier, input, _currentLinePosition, _currentLine);
        }

        public List<Token> Tokenize()
        {
            List<Token> tokens = new();
            while (_inputStartPos < _kukuLangSourceCode.Length)
            {
                UpdateEndPos();

                if (_inputStartPos >= _kukuLangSourceCode.Length) break;

                tokens.Add(GenerateToken());
                MoveInputPositionForward();
                _currentLinePosition = _inputStartPos - _inputEndPos;
            }
            tokens.Add(new Token(TokenType.EOF, "EOF", _currentLinePosition, _currentLine));
            return tokens;
        }
    }
}
