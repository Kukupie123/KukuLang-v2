using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public class RecursiveDescentParser(List<Token> tokens, int startingPosition = 0) : ParserBase<ASTScope, dynamic>(tokens, startingPosition)
{
    private ASTScope currentScope = new ASTScope();

    //Parameter not used so we set it as null to keep ParserBase happy.
    public override ASTScope Parse(dynamic? arg = null)
    {

        while (CurrentToken.Type != TokenType.EOF)
        {
            //Handle Custom Type declaration
            TokenEvaluatorService.EvaluateToken(this, currentScope);
            //Handle Set Statements
        }
        return currentScope;
    }




}
