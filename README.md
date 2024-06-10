# KUKULANG
A minimal programming language with natural syntax making it very easy for non programmers to Program and understand.

## Features
1. Natural Spoken English Grammer based syntax
2. static typing
3. Structures
4. Tasks
1. Input and Print
5. Basic statements such as assignment, conditionals and loops.
6. Chaining nested variable as seen in other languages such as var.foo.bar = 12;

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
```

### Conditionals
```
if <condition> then{
	~do stuff~
}
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
