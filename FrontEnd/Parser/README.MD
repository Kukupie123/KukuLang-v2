```
~Defining a struct~
Define Human with age (number), nickName (text, initialized to "Nameless").
~Defining a function~
Define Square with num (number) returning float {
    return num*num.
}

~Setting variables~
Set "number" to 21.
Set "number" to (21+1)5.

Set "num" to 12 as float. ~as float is optional to enforce a type~
Set "num" to 12.4. ~Reassigning~

Set "Kuchuk" to Human.

Set nickName of kuku to "Kuku".

```

###### Define Human with age (number), nickName (text, initialized to "Nameless").
```
class CustomType{
    string name; //Name of the type
    Map<string,string>; //variable name and data type
}
```
```
var HumanStruct = new CustomType("Human",{"age":"number", "nickName","text"})
```
*We then save it in Scope's Structures Map.*

###### Define Square with num (number) returning float { return num*num. }
```
class CustomTask {
    string taskName; //Name of the task
    string returnType; //return type
    Map<string,string> params; //variable name and type
}
```
```
var sum = new CustomTask("Square","float",{"num":"number"})
```
*We then save it in Scope's Functions Map.*

###### Set "number" to 21.
There are two kinds of set. This is the direct set.
```
class SetDirectInstruction{
    string name;
    string type;
    string value;
}
```
```
var instruction = new SetDirectInstruction("number","integer","21")
```
*Next we add it to instructions list*
###### Set "number" to (21+1)5.
Now we have a value for expression so let's update our SetDirectInstruction class to be more scalable
```
class SetDirectInstruction{
    string name;
    string type;
    Expression value;
}
```
Let's define some expression classes
```
class BinaryExpression{
    Expression left;
    Operation op; //+ - * /
    Expression right;
}
```
```
Lets define instance expression.
These are literal strings, numbers or object of custom structure
class StringInstance{
    string value;
}
class IntegerInstance{
    int value;
}
class FloatInstance{
    float value;
}
```
```
var num21 = new IntegerInstance(21);
var num1 = new IntegerInstance(1);
var num5 = new IntegerInstance(5);
var binOp1 = new BinaryExpression(literal21, +, literal1);
var binOpMain = new BinaryExpression(binOp1, *, literal5);
//Evaluates to (21+1)*5
var instruction = new SetDirectInstruction("num","float",binOpMain)
```
*Next we add it to instructions list*
###### Set "Kuchuk" to Human.
Since the "value" variable is always an expression let's create a new class to express Custom Structure objects as an expression
```

class CustomTypeInstance:  {
    private CustomType _customType;
    private Dictionary<string, Expression> parameters;
}
```
```
CustomTypeInstance obj = new CustomTypeInstance("Human",{age:IntegerLiteral(24), nickName: StringLiteral("Nameless")});
var instruction = new SetDirectInstruction("kuchuk","Human",obj)
```