# Syntax
## Define
Defining stores them in the Scope.
### Variables
```
~Two ways to define variables~
define num as float;
define num as float initialized to 12.5;
```
### Structures
```
define Human{
    with age as number;
    with nickName as text initialized to "Nameless";
}
~3 ways to define structure variables~
define corpse as Human;

define Kuchuk as Human with nickName "Kuku";

define "kuku" as Human with nickName "kuku",age 24;
```
## Set
Set
### Setting generic variables
```
set num to 12;
```
### Setting structure variables
```
set kuku to new Human;
set kuku to new Human with nickName "Kuku", age 24;
```
### Setting variables within structure variables
```
set nickName of kuku to "kuku";
```
