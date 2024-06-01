# Kuku Lexer

## Syntax

### Declaring variable
```
define variable a of_type integer;
set a to 12;
```
Creates a new variable called a of type integer and sets its value to 12. <br/>

### Conditional statements
```
if a is b then {

}
else{

}
```
Conditional keywords are :- <br/>
1. is
2. is_not
3. is_less_than
4. is_greater_than
5. is_less_or_equal_to
6. is_greater_or_equal_to

### Loop
```
while a is b do {
    set a to 12;
}
```

### Function
```
define task sum (age as integer) which returns integer {

}
```

### Structures
```
define structure Human{
    define variable name of type text;
    define variable age of type number;
}
```