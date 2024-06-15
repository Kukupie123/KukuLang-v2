# KukuLang Lexer
Lexers job is to break down the source code and then generate tokens out of it. These tokens help us to values to the words in our soure code. <br>
There are severak ways we can create a lexer. One way is to use regular expressions, another way is to use Finite automata (Finite State Machine), etc. <br>

My first approach was to design a FSM. I would treat every character as a state and next input as an input and use it to transition between states. I soon scrapped this idea and went for my own way of implementation. <br>

## Overview
## Scanning
The overall idea is to scan every single character and join them as one single string until we hit a delimiter. Let's take the following code as example
```
set a to 2;
```
Here we would start scanning at "s". <br>
We would keep on concating it as "s" -> "se" -> "set". <br>
Once we hit a white space we are going to treat it as a delimiter. This would give us a string "set". <br>
We then send this as an input to a function to generate token. <br>
We woud then need to start scanning from "t" and repeat the whole process again.
## Generate Token
Generate token takes in a string as an input. It then checks if it's any pre defined token such as keyword, or maths symbols, logical symbol, etc. If its none then it simply generates an identifier

## Implementation
We keep two pointers to keep track of the string we are scanning. 
```
private int _inputStartPos;
private int _inputEndPos;
```
In our KukuLang code snippet in Scanning section where we got "set" as our string. Our _inputStartPos would be 0 and _inputEndPos would be 2.
<br>

UpdateEndPos is a function that does the scanning. It updates the position of _inputEndPos until it hits a delimiter. The exception is where the _inputStartPos is a string or a comment. In such cases it will stop when we encounter the closing string or comment symbol.
```c#
private void UpdateEndPos(){
    //Skip white space if we are at white space
    // return delimiter such as ; {}[]()
    //if startChar is ~ or " keep scanning until we reach ~ or " again depding on what we got
    //if not keep scanning until we hit a delimiter such as whitespace or ;
}
```
Once updated we call the Generate Token function which will use the _inputStartPos and _inputEndPos to determine the string. It then decides if what kind of token it is. 
```c#
private Token GenerateToken(){
    // if its predefined token return it.
    // if its int or string or float or char or bool return it as its respective literal
    // if its starts with and ends with ~ or " return it as comment or string literal
    // if none of that happens it must be returned as an identifier
}
```
This generated token will then be added to the tokens list. <br> The next step would be to move the _inputStartPos forward to _inputEndPos + 1.
```c#
private void MoveInputPositionForward()
    {
        _inputStartPos = _inputEndPos + 1;
        _inputEndPos = _inputStartPos;
        //Some other code to keep track of line number
    }
```
This whole step is repeated until we reach the end of source code. At the very end we add a EOF token to let parser know where the token ends. 
[Go To Parser](../Parser/README.MD)
