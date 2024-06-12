# KUKULANG
A minimal but powerful programming language with natural syntax making it very easy for non programmers to Program and understand. <br>
[Demonstration of Kukulang (12th July 2024)](https://www.youtube.com/watch?v=LJWrLUUPAKY) <br>
## Features
1. Natural Spoken English Grammer based syntax
2. Static typing
3. Structures
4. Tasks
1. Input and Print
5. Core statements such as assignment, conditionals and loops.
6. Complex Chaining nested variable and tasks as seen in other languages such as var.foo.bar = sum(12,2);

## Syntax
### Comments
```
~ This is a comment ~
```
### Printing
```
print with "Hello World";
```
### Taking input
```
set a to input;
```
### Setting simple Variable
```
set var to 12;
```
### Defining custom data types
```
define Human with name(text), age(int);
define Student with humanData(Human), class(int), rollNumber(int);
```
### Initializing object of custom data types
```
set sto to Student;
```
### Nested variable
```
~ Equivalent to sto.class = 69; ~
set sto's class to 69;

~ Equalivalent to sto.humanData.name = "Jack Billdickson"; ~
set sto's humanData's name to "Jack Billdickson";

~ Equivalent to sto.humanData.age = sto.rollNumber; ~
set sto's humanData's age to sto's rollNumber;
```
### Defining Tasks
```
define HumanCreator returning Human with age(int), name(text){
	set tempHuman to Human; ~creating human object~
	set tempHuman's age to age;
	set tempHuman's age to name;
	return tempHuman;
}

~function with no return type and no params~
define Foo returning nothing {
	set a to (12+6)*2;
}
```
### Executing Tasks
```
Foo;
HumanCreator with age(24), name("kuku");
```
### Setting values to task execution output
```
set hum to HumanCreator with age(1), name("kuchuk");

~Nested example~
set hum to HumanCreator with age(1), name(sto's humanData's name);

define Human with name(text);
define Summ returning text{
	return "Summ return text";
}
print with Summ with nothing;
```

### Conditionals
```
if <condition> then{
	~do stuff~
}
else {}
```
conditions :- <br>
1. is
1. is_not
1. is_less_than
1. is_less_or_is
1. is_greater_than
1. is_greater_or_is

### Loops
```
until <condition = false> repeat {}
```
in until the loop will continue as long as tthe condition is false and stop when it's true
```
as_long_as <condition = true> repeat {}
```
in as long as loop, the loop will continue as long as the condition is true and will stop when it's false.

## Additional Notes
return keyword takes you out of the scope you are currently in. It doesn't work on function scope. It is localised to the current scope. <br>

## Future plans
Make exception better and show more precisely whats wrong and where.
DONE(Console Print and Input) <br>
List (Will not do it until I gain back interest in this project) <br>
Map (Will not do it until I gain back interest in this project) <br>

## Compilers and Interpreters
The source code that we write cannot be understood by a machine. We have to convert this high level representation of instructions and statements into something that can be understood by the machine.
A compiler is responsible for converting a language into another language. In most cases, to machine code which can then be run by the target machine. <br>
We also have Interpreters which can interpret the source code or Intermediate code or byte code generated ditectly.

### Common phases of creating a compiler/Interpreter
The Task is divided into two parts, frontend and backend. The frontend is where we break down the source code, validate the correctness of the code and then create a meaning full data structure out of the source code. The backend is a bit more complex, it is responsible for using the meaningful data produced by the frontend to generate machine code which can then be interpreted by the target machine. There are a lot of ways to go about this, such as using a tool such as LLVM to generate platform specific code and then interpret it, building a virtual machine and generating byte code that can be interpreted by the Virtual machine, interpreting the data produced by frontend directly using existing compiler and programming languages, etc. In our approach we decided to go forth with creating an interpreter in C# for the backend as it's probably the easiest to do and also because of my lack of knowledge about the low level world.

### Frontend
#### Lexical Analysis
TLDR : Lexical analysis is the phase where the source code is broken down into tokens. Tokens contain data such as the type of token it is and the value of the token. <br>
This is the first Step. In this phase we read the source code and break down the source code into chunks known as "Tokens". Each token holds info about the type of token it is and the value of the token. If we take the following source code as an example :-
```
int foo = 21;
```
The tokens generated may look like :-
```
[
	{type : keyword, val : int},
	{type : identifier, val : "foo"},	
	{type : intLiteral, val 21},
]
```
[Implementation of Lexical Analyser](./FrontEnd/Lexer/README.MD)
#### Parser
TLDR: Parser is responsible for using the tokens generated in Lexical Anaylsis phase and generating a tree structure representing the program execution.
The second step is usually creating a parser. A parser will take the tokens as an input and use it to generate a tree based data structure that represents the whole program. It will contain data that represents the flow of the program, contain information about different custom classes and functions created, and so on.
A parser can have many internal phase and it's output can also vary depending on how it was programmed. But going with the usual definition, at first, a Parse tree is generated which is a tree representation of the instructions and statements in the program. We can take the following source code as an example :-
```
int a = 12 + 2;
int square = a * a;
```
A parse tree may look like
```
Program
├── AssignmentStatement
│   ├── Type: int
│   ├── Identifier: a
│   ├── Operator: =
│   └── Expression
│       ├── LeftOperand: 12
│       ├── Operator: +
│       └── RightOperand: 2
│
└── AssignmentStatement
    ├── Type: int
    ├── Identifier: square
    ├── Operator: =
    └── Expression
        ├── LeftOperand: a
        ├── Operator: *
        └── RightOperand: a
```
After generating a parse tree we can move to either stop here and let our interpreter interpret this parse tree. We can also instead generate an AST(Abstract Syntax Tree) which is a more abstract tree representation of the parse tree. We can follow up the parse tree/AST with a semantic analysis. <br>
Semantic Analysis is usually done to check if the types are correct, if the syntax sense, etc. <br>
An example would be the sentence "The dog flew inside the rock". This syntax is correct grammatically but it doesn't make sense. The job of Semantic analysis would be to find out such syntax where the grammer is valid but doesn't really make any sense. <br>
In our project we are more than fine with generating the parse tree and letting the interpreter interpret it. I have not seen the need for generating AST or doing a semantic analysis yet. To be fair I have not tested the compiler extensively, more testing may lead us to find us strong reasons to implement a semantic analysis phase.
[Implementation of Parser](./FrontEnd/Parser/README.MD)
### Backend
#### Interpreter
We opted for creating an interpreter for now, we can maybe use LLVM or such similar tools to create platform specific machine code directly. Our interpreter is going to read the tree like data structure generated by our parser and interpret the instructions one by one.
[Implementation of Interpreter](./FrontEnd/Interpreter/README.MD)
