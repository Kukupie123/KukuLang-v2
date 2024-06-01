```
define structure Human {
    ~Variables declaration~
    define name of_type text;
    ~Variable declaration with assignment~
    define age of_type number as -1;
}

define localAge of_type number;
set localAge to 24;
define kuchuk of_type Human;
set kuchuk to Human;
set name of kuchuk to "Kuku";
set age of kuchuk to localAge;
```
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
