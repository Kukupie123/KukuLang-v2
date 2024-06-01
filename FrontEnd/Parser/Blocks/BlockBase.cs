namespace FrontEnd;

public class BlockBase
{
    public BlockBase? ParentBlock;
    public BlockType BlockType;
    public List<dynamic> Variables = []; //Declared variables and structures
    public List<dynamic> Functions = []; //Declared functions
    public List<dynamic> Statements = []; //Instruction sequence
}
