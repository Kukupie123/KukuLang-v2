# KukuSpeak Technical doc
A programming language with natural English syntax, making it very easy to understand and write for non-programmers.
```
~This is a comment~
~Complete Code Snippet Demo~

~Creating Custom Types~
Define Keyword with layoutType(text), buttonCount(int, initialized to 100);
Define Computer with typer(Keyboard), name(text);

~Creating Custom Functions~
Define ChangeKeyboardLayout returning nothing with comp(Computer), newLayoutType(text){
    set layoutType of typer of comp to newLayoutType.
}
Define GetButtonCount returning int with comp(Computer){
    return buttonCount of typer of comp.
}

~Performing Task~
(ChangeKeyboardLayout with comp as asus, newLayoutType as "QWERTY").

~Variables~
set hp to Computer.
set name of hp to "HP".

set asus to Computer with name as "ASUS.

~Set to Task's output~
set bc to (GetButtonCount with comp as hp)'s output.

~Instance access~
asus's typer.

~Nested instance~
set kb to hp's typer.
set bc to hp's typer's buttonCount.

```
# Models
## CustomType
```
Model :-
class CustomType {
    string typeName;
    map<string,string> variableNameAndType;
}
```
## Custom Task
```
Model :-
class CustomTask{
    string taskName;
    map<string,string> parameterNameAndType;
    List<Statements> instructions;
}
```
```
Define Foo returning nothing{
    ~body~
}
Define Square returning float with Number(float) {
    return float*float;
}
```

# Brain Storming
Lets take the following syntax as example.
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
~We can also do~
~Set "Kuchuk" to Human with age as 24.~

Set nickName of kuku to "Kuku".

~Demonstration of compoistion~
Define Keyboard with type(text).
Define Computer with keyboardVar(Keyboard, initialized to Keyboard with "QWERTY" as type).

Set "comp" to Computer.

Set type of keyboardVar of comp to "ZXCV".

set keyboardRef to keyboardVar of comp.

~Imagine we defined a function called startTyping which returns nothing~
execute startTyping with keyboard as keyboardRef.
execute Square with num1 as 21 and set "ress" to output.
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
    private string _customType;
    private Dictionary<string, Expression> parameters;
    //We can also pass property values as we instantiate it.
    //I just didn't show it
}
```
```
CustomTypeInstance obj = new CustomTypeInstance("Human",{age:IntegerLiteral(24), nickName: StringLiteral("Nameless")});
var instruction = new SetDirectInstruction("kuchuk","Human",obj)
```
###### Set nickName of kuku to "KukuNickName".
Set to is a bit more complex set instruction. Unlike typical set this is used to update properties of structures

```
class SetOfInstruction{
    string propertyName;
    string variableName;
    string value;
}
```
```
var setOfIntruction = new SetOfInstruction("nickName","kuku","KukuNickName");
```

###### Define Computer with keyboardVar(Keyboard, initialized to Keyboard with "QWERTY" as type).

```
var computerStruct = new CustomType("Computer",{"keyboardVar":"Keyboard"})
```
*Next, we store it in structures dictionary*

###### Set "comp" to Computer.

```
var computerInstance = new CustomTypeInstance("Computer",{
    keyboardVar : new CustomTypeInstance("Keyboard", {
        type:"QWERTY"
    })
});
//We defined the initial value of "type" in keyboard so this is how it's going to be.
var setInstruction = new SetInstruction("comp","Computer",)
```

###### Set type of keyboardVar of comp to "ZXCV".
We can define the composition as a linked list
```
var setOfInstruction = new SetOfInstruction("type","comp->keyboard","ZXCV");
```
##### set keyboardRef to keyboardVar of comp.
This is how we can get properties of structures.
We can ignore type. Let that be done in semantics analysis.
```
var setInstruction = new SetDirectInstruction("keyboardRef","unknownType","comp->keyboardVar")
```
Designing PrattParsing for this might be tricky. We will see

###### Execute startTyping with keyboardRef as keyboard.

```
class FunctionCallInstruction{
    string functionName;
    Dictionary<string,string> params; //parameterName, parameter value
    //Types checks will be handled in semantic anaysis
}
```
```
var functionINstruction = new FunctionCallInstruction("startTyping",{"keyboard":"keyboardRef"});
```
###### Execute Square with num1 as 21 and set "ress" to output.
This one is an assignment statement. For value the function will be executed in runtime.
```
var functionINstruction = new FunctionCallInstruction("Square",{"num":"12"});
var setDirect = new SetDirectInstruction("ress","unknown", functionINstruction)
```
