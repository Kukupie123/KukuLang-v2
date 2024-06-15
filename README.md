
# KUKULANG
A minimal but powerful programming language with natural syntax making it very easy for non-programmers to program and understand.  
[Demonstration of Kukulang (12th July 2024)](https://www.youtube.com/watch?v=LJWrLUUPAKY)  

## Features
1. Natural Spoken English Grammar based syntax
2. Static typing
3. Structures
4. Tasks
5. Input and Print
6. Core statements such as assignment, conditionals, and loops
7. Complex chaining of nested variables and tasks as seen in other languages such as `var.foo.bar = sum(12,2);`

## Syntax

### Comments
```kuku
~ This is a comment ~
```

### Displaying
```kuku
print with "Hello World";
```

### Taking Input
```kuku
set a to input;
```

### Setting Simple Variable
```kuku
set var to 12;
```

### Defining Custom Data Types
```kuku
define Human with name(text), age(int);
define Student with humanData(Human), class(int), rollNumber(int);
```

### Initializing Object of Custom Data Types
```kuku
set sto to Student;
```

### Nested Variable
```kuku
~ Equivalent to sto.class = 69; ~
set sto's class to 69;

~ Equivalent to sto.humanData.name = "Jack Billdickson"; ~
set sto's humanData's name to "Jack Billdickson";

~ Equivalent to sto.humanData.age = sto.rollNumber; ~
set sto's humanData's age to sto's rollNumber;
```

### Defining Tasks
```kuku
define HumanCreator returning Human with age(int), name(text) {
    set tempHuman to Human; ~creating human object~
    set tempHuman's age to age;
    set tempHuman's name to name;  ~setting the name correctly~
    return tempHuman;
}

~function with no return type and no params~
define Foo returning nothing {
    set a to (12+6)*2;
}
```

### Executing Tasks
```kuku
Foo;
HumanCreator with age(24), name("kuku");
```

### Setting Values to Task Execution Output
```kuku
set hum to HumanCreator with age(1), name("kuchuk");

~Nested example~
set hum to HumanCreator with age(1), name(sto's humanData's name);

define Human with name(text);
define Summ returning text {
    return "Summ return text";
}
print with Summ with nothing;
```

### Conditionals
```kuku
if <condition> then {
    ~do stuff~
}
else {}
```
Conditions:  
1. is
2. is_not
3. is_less_than
4. is_less_or_is
5. is_greater_than
6. is_greater_or_is

### Loops
```kuku
until <condition = false> repeat {}
```
In `until`, the loop will continue as long as the condition is false and stop when it's true.

```kuku
as_long_as <condition = true> repeat {}
```
In `as_long_as`, the loop will continue as long as the condition is true and will stop when it's false.

## Additional Notes
The `return` keyword takes you out of the scope you are currently in. It doesn't work on function scope. It is localized to the current scope.

## Future Plans
- Make exceptions better and show more precisely what's wrong and where.
- DONE (Console Print and Input)
- List (Will not do it until I gain back interest in this project)
- Map (Will not do it until I gain back interest in this project)
- Importing other KukuLang files. (Quite easy)

## KukuLang Interpreter 
Sure we have this cool language to work with but it's useless if we can't even run it. I think now is a good time to tell that the whole reason behind the existence of KukuLang is because I wanted to know how high level languages are compiled and executed. <br>
As of now I took the approach of creating an interpreter in C# which takes in an "AST" and then interprets it one instruction at a time. This is one of the ways we can get it up and running without having to dive into low level stuff such as generate platform specific assembly code or generating IR which is considered Mid level. Another approach would be to compile Source code of kukulang into another language such as C#. This is known as transpiling.<br>
I am also interested in Generating an IR and using LLVM to generate platform specific machine code but these things require a lot of time and investment to learn so I am not ready to commit as I wanna work on other projects too.

### Creating KukuLang Interpreter
The process of creating the interpreter goes as follows :- <br>
1. Creating a lexer to generate tokens. <br>
2. Creating a parser which generates AST using the tokens. <br>
3. Creating the Interpreter which reads the AST and executes it. <br>
#### Lexical Analysis
**TL;DR**: Lexical analysis is the phase where the source code is broken down into tokens. Tokens contain data such as the type of token and the value of the token.  

This is the first step. In this phase, we read the source code and break it down into chunks known as "tokens." Each token holds info about the type of token it is and its value. For example, if we take the following source code:
```kuku
int foo = 21;
```
The tokens generated may look like:
```json
[
    {type: "keyword", val: "int"},
    {type: "identifier", val: "foo"},
    {type: "operator", val: "="},
    {type: "intLiteral", val: 21}
]
```
[Implementation of Lexical Analyser](./FrontEnd/Lexer/README.MD)

#### Parser
**TL;DR**: The parser is responsible for using the tokens generated in the lexical analysis phase and generating a tree structure representing the program execution.

The second step is usually creating a parser. A parser will take the tokens as an input and use them to generate a tree-based data structure that represents the whole program. It will contain data that represents the flow of the program, information about different custom classes and functions created, and so on. A parser can have many internal phases, and its output can also vary depending on how it was programmed. Typically, a parse tree is generated first, which is a tree representation of the instructions and statements in the program. For example, if we take the following source code:
```kuku
int a = 12 + 2;
int square = a * a;
```
A parse tree may look like:
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
After generating a parse tree, we can either stop here and let our interpreter interpret this parse tree or generate an AST (Abstract Syntax Tree), which is a more abstract representation of the parse tree. We can follow up the parse tree/AST with semantic analysis.  

Semantic analysis is usually done to check if the types are correct, if the syntax makes sense, etc. For example, the sentence "The dog flew inside the rock" is syntactically correct but semantically nonsensical. The job of semantic analysis would be to find such syntax where the grammar is valid but doesn't make sense.

In our project, we are content with generating the parse tree and letting the interpreter interpret it. We have not seen the need for generating an AST or doing a semantic analysis yet. However, more extensive testing may lead us to find strong reasons to implement a semantic analysis phase.  
[Implementation of Parser](./FrontEnd/Parser/README.MD)

### Backend
#### Interpreter
We opted for creating an interpreter for now. We might use LLVM or similar tools to create platform-specific machine code directly in the future. Our interpreter is going to read the tree-like data structure generated by our parser and interpret the instructions one by one.
The benifit of creating an Interpreter is that we can the existing compiler of the language which we are using to create our interpreter. In our case we are creating an interpreter using C# so we leverage the power of C# compiler to handle everything. All we have to do is make sure to translate statements and expressions of KukuLang into valid C# statements. <br>
[Implementation of Interpreter](./FrontEnd/Interpreter/README.MD)
