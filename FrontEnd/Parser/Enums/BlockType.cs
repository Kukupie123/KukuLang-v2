namespace FrontEnd;

public enum BlockType
{
    Structure, //Structure declaration body. Can't have functions or structures declarations
    Function, //Function statement body. Can't have functions or structure declarations
    Generic, //Generic body for loops and conditionals. Can have functions and structures declarations
}
