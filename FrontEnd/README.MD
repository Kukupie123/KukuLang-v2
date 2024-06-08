# KUKULANG
A minimal programming language with natural syntax making it very easy for non programmers to Program and understand.

## Syntax
### Comments
```
~ This is a comment ~
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
define 
```