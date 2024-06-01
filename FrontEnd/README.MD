# Syntax

## Define
Define is used for defining structures and functions. <br/>
These definitions are stored in the scope that they were defined.

### Structures
```
Define Human with age (number), nickName (text, initialized to "Nameless").
``````
We define Properties (Eg :- age, nickName) and optionally set it's initial value (Eg :- we set nickName to "Nameless")

### Functions
```
Define Add with num1 (number), num2 (number, initialized to 0), returning float {
    return num1 + num2.
}
Define Print with content (text) returning nothing {
    ~interpreter will print the text in console~
}
```
We define parameters with initial value(optional), we define the return type, we define the body.

## Set
Set is used to create/update variables.
When you set a value its type is going to be parsed, unless type has been mentioned explicitly.
Set attempts to find the variable in the current or parent scopes. If not found, it creates a new one in the current scope.

### Setting generic variables
Set "num" to 12.
Set "num" to 12 as a float.

### Setting structure variables
Set kuku to new Human.
Set kuku to new Human with nickName "Kuku", age 24.

### Setting variables within structure variables
Set nickName of kuku to "kuku".
