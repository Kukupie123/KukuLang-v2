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

namespace FrontEnd.Parser.Services
{
    /** <summary>
     * Evaluates token instructions.
     * Each instruction statement must consume the terminator or scope completely.
     * </summary>
     */
    class TokenEvaluatorService
    {
        public static void EvaluateToken<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> parser, ASTScope scope)
        {
            // Each sub-evaluate method needs to consume the . or }
            switch (parser.CurrentToken.Type)
            {
                case TokenType.Define:
                    EvaluateDefineToken(parser, scope);
                    break;
                case TokenType.Set:
                    EvaluateSetToken(parser, scope);
                    break;
                case TokenType.If:
                case TokenType.Until:
                    EvaluateIfToken(parser, scope);
                    break;
                default:
                    throw new UnknownTokenException(parser.CurrentToken);
            }
        }

        // Handles "define" tokens
        private static void EvaluateDefineToken<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> parser, ASTScope scope)
        {
            parser.Advance(); // Advance to the identifier token
            TokenValidatorService.ValidateToken(TokenType.Identifier, parser.CurrentToken);
            Token taskNameToken = parser.ConsumeCurrentToken(); // Store identifier and advance to "returning" or "with"

            switch (parser.CurrentToken.Type)
            {
                case TokenType.Returning:
                    EvaluateDefineReturningToken(parser, scope, taskNameToken);
                    break;

                case TokenType.With:
                    EvaluateDefineWithToken(parser, scope, taskNameToken);
                    break;

                default:
                    throw new UnexpectedTokenException(
                        [new Token(TokenType.With, "with", -1), new Token(TokenType.Returning, "returning", -1)],
                        parser.CurrentToken
                    );
            }
        }

        // Handles "define ... returning" tokens
        private static void EvaluateDefineReturningToken<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> parser, ASTScope scope, Token taskNameToken)
        {
            parser.Advance(); // Advance to "nothing" or return type
            var returnTypeToken = parser.ConsumeCurrentToken(); // Store return type and advance to "with" or "."
            TokenValidatorService.ValidateToken([TokenType.Identifier, TokenType.Nothing], returnTypeToken);

            // Check for parameters
            Dictionary<string, string>? paramTypeVariables = null;
            if (parser.CurrentToken.Type == TokenType.With)
            {
                parser.Advance(); // Advance to the first parameter
                paramTypeVariables = StoreArgs(parser);
            }

            // Parse the task block
            TokenValidatorService.ValidateToken(TokenType.CurlyBracesOpening, parser.CurrentToken);
            parser.Advance(); // Consume the '{'
            var taskScope = new ASTScope($"{scope.ScopeName}->{taskNameToken.Value}");
            while (parser.CurrentToken.Type != TokenType.CurlyBracesClosing)
            {
                EvaluateToken(parser, taskScope);
            }

            // Create and store the custom task
            CustomTaskBase customTask = new(taskNameToken.Value, returnTypeToken.Value, paramTypeVariables, taskScope);
            scope.CustomTasks.Add(taskNameToken.Value, customTask);
            parser.Advance(); // Consume the '}'
        }

        // Handles "define ... with" tokens
        private static void EvaluateDefineWithToken<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> parser, ASTScope scope, Token taskNameToken)
        {
            parser.Advance(); // Advance to the first property
            TokenValidatorService.ValidateToken(TokenType.Identifier, parser.CurrentToken);
            Dictionary<string, string>? typeVariables = StoreArgs(parser);
            if (typeVariables.Count <= 0) throw new NoPropertyException(parser.CurrentToken);

            // Create and store the custom type
            TokenValidatorService.ValidateToken(TokenType.FullStop, parser.CurrentToken);
            CustomTypeBase customType = new(taskNameToken.Value, typeVariables);
            scope.CustomTypes.Add(taskNameToken.Value, customType);
            parser.Advance(); // Consume the '.'
        }

        // Handles "set" tokens
        private static void EvaluateSetToken<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> parser, ASTScope scope)
        {
            TokenValidatorService.ValidateToken(TokenType.Set, parser.CurrentToken);
            parser.Advance(); // Advance to the variable name

            var variableNameToken = parser.CurrentToken;
            var prattParser = new PrattParser(parser.Tokens, parser._Pos);
            var variableExp = prattParser.Parse() as VariableExp
                ?? throw new Exception($"Variable Expression was evaluated to null for token {variableNameToken}");

            parser._Pos = prattParser._Pos; // Update the main parser's _pos
            TokenValidatorService.ValidateToken(TokenType.To, parser.CurrentToken);

            parser.Advance(); // Advance to the start of the value expression
            prattParser = new PrattParser(parser.Tokens, parser._Pos);
            var value = prattParser.Parse();
            parser._Pos = prattParser._Pos; // Update the main parser's _pos

            // Create and store the set statement
            var setToStmt = new SetToStmt(variableExp, value);
            scope.Statements.Add(setToStmt);
            parser.Advance(); // Consume the "."
        }

        // Handles "if" token
        private static void EvaluateIfToken<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> parser, ASTScope scope)
        {
            parser.Advance(); // Advance to the conditional expression

            var prattParser = new PrattParser(parser.Tokens, parser._Pos);
            var condition = prattParser.Parse() ?? throw new InvalidExpressionException("Conditional expression isn't valid");
            parser._Pos = prattParser._Pos; // Update the position of the main parser

            parser.Advance(); // Advance to the "{"
            TokenValidatorService.ValidateToken(TokenType.CurlyBracesOpening, parser.CurrentToken);

            var ifScope = new ASTScope($"{scope.ScopeName}->Conditional");
            parser.Advance(); // Advance to the first statement

            // Parse the if block
            while (parser.CurrentToken.Type != TokenType.CurlyBracesClosing)
            {
                EvaluateToken(parser, ifScope);
            }

            // Create and store the if statement
            var ifStmt = new IfStmt(condition, ifScope);
            scope.Statements.Add(ifStmt);
            parser.Advance(); // Consume the "}"
        }

        /** <summary>
         * Gathers all the properties until . or {
         * Doesn't consume the . or {
         * Returns a map of argument and value for the given token.
         * The initial token needs to be the FIRST identifier.
         * </summary>
         */
        public static Dictionary<string, string> StoreArgs<ParserReturnType, ParserArgument>(ParserBase<ParserReturnType, ParserArgument> parser)
        {
            var args = new Dictionary<string, string>();

            // Keep iterating until we reach . or {
            while (parser.CurrentToken.Type != TokenType.FullStop && parser.CurrentToken.Type != TokenType.CurlyBracesOpening)
            {
                // For params that come after the first, advance past the ","
                if (parser.CurrentToken.Type == TokenType.Comma)
                {
                    parser.Advance();
                }

                // Validate and store the property name and type
                TokenValidatorService.ValidateToken([TokenType.Identifier, TokenType.IntegerLiteral, TokenType.FloatLiteral, TokenType.TextLiteral], parser.CurrentToken);
                var propertyToken = parser.ConsumeCurrentToken(); // Store property name and advance to "("
                TokenValidatorService.ValidateToken(TokenType.RoundBracketsOpening, parser.CurrentToken);

                parser.Advance(); // Advance to the property type
                TokenValidatorService.ValidateToken([TokenType.Identifier, TokenType.IntegerLiteral, TokenType.FloatLiteral, TokenType.TextLiteral], parser.CurrentToken);

                args.Add(propertyToken.Value, parser.CurrentToken.Value.ToString());

                parser.Advance(); // Advance past ")"
                parser.Advance(); // Advance past "," or "."
            }

            return args;
        }
    }
}
