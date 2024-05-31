using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FrontEnd.Commons.Tokens;

namespace FrontEnd.Lexer
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
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
            string currentCharacter = _kukuLangSourceCode[_inputStartPos].ToString();

            // Directly check for special cases to avoid deep nesting
            if (currentCharacter is "~" or "\'" or "\"")
            {
                if (currentCharacter == "~")
                {
                    int i = _inputStartPos + 1;
                    while (i < _kukuLangSourceCode.Length)
                    {
                        if (_kukuLangSourceCode[i] == '~')
                        {
                            _inputEndPos = i;
                            return;
                        }

                        i++;
                    }

                    throw new Exception($"Comment opened at {_inputStartPos} is never closed");
                }

                if (currentCharacter == "\'")
                {
                    _inputEndPos = _inputStartPos + 2; // Assuming character can only extend up to this value
                    if (_kukuLangSourceCode[_inputEndPos] != '\'')
                        throw new Exception($"Invalid character at {_inputEndPos}. Expected '");
                }
                else if (currentCharacter == "\"")
                {
                    int i = _inputStartPos + 1;
                    while (i < _kukuLangSourceCode.Length)
                    {
                        if (_kukuLangSourceCode[i] == '"')
                        {
                            _inputEndPos = i;
                            return;
                        }

                        i++;
                    }

                    throw new Exception($"Invalid string. String opened at {_inputStartPos} was never closed.");
                }
            }

            // Check for terminators, math operators, or parentheses
            if (
                TokenMap.IsValueAGenericToken(currentCharacter) && (
                    TokenMap.GetTypeByValue(currentCharacter) is TokenType.Terminator ||
                    TokenMap.GetTypeByValue(currentCharacter) is TokenType.Math ||
                    TokenMap.GetTypeByValue(currentCharacter) is TokenType.Parenthesis
                )
            )
            {
                _inputEndPos = _inputStartPos;
                return;
            }

            // Iterate to find the end position based on token type
            for (int i = _inputStartPos; i < _kukuLangSourceCode.Length; i++)
            {
                currentCharacter = _kukuLangSourceCode[i].ToString();
                if (TokenMap.IsValueAGenericToken(currentCharacter) &&
                    TokenMap.GetTypeByValue(currentCharacter) is TokenType.Terminator or TokenType.Math
                        or TokenType.Parenthesis)
                {
                    int endPos = i - 1; //-1 because the current position will have the terminator
                    _inputEndPos =
                        endPos < _inputStartPos
                            ? _inputStartPos
                            : endPos; // Safety net for scenarios when we start off with terminator
                    break;
                }
            }
        }

        private Token? GenerateToken()
        {
            var input = Input().Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            // Check for comments
            if (Regex.IsMatch(input, "^~.*~$"))
            {
                return new Token(TokenType.Comment, input);
            }

            // Check for character literals
            if (input.StartsWith("'") && input.EndsWith("'"))
            {
                var replaced = Regex.Replace(input, "'", "");
                return new Token(TokenType.Char, replaced);
            }

            // Check for string literals
            if (input.StartsWith("\"") && input.EndsWith("\""))
            {
                var removed = input.Substring(1, input.Length - 2);
                return new Token(TokenType.String, removed);
            }

            // Check for numbers
            if (float.TryParse(input, out float f))
            {
                int i = (int)f;
                bool same = i == f;
                return new Token(same ? TokenType.Integer : TokenType.Float, same ? i : f);
            }

            // Check for identifiers
            if (!char.IsDigit(input[0]))
            {
                return new Token(TokenType.Identifier, input);
            }

            // Check against a predefined token map
            if (TokenMap.IsValueAGenericToken(input))
            {
                return new Token(TokenMap.GetTypeByValue(input), input);
            }

            // Default case: Unknown token type
            throw new Exception($"Unknown Input {input}");
        }


        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();
            while (_inputStartPos < _kukuLangSourceCode.Length)
            {
                UpdateEndPos();
                var generatedToken = GenerateToken();
                if (generatedToken != null)
                {
                    tokens.Add(generatedToken);
                }

                MoveInputPositionForward();
            }

            tokens.Add(new Token(TokenType.EOF, "EOF"));
            return tokens;
        }
    }
}