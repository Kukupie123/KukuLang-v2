# KukuLang 🚀

**A powerful, statically-typed programming language with natural, spoken-English syntax.**

[![KukuLang Demonstation](https://img.youtube.com/vi/LJWrLUUPAKY/0.jpg)](https://www.youtube.com/watch?v=LJWrLUUPAKY)  
*> Watch the demonstration video (July 12th, 2024)*

---

## 📖 Introduction

**KukuLang** is designed to bridge the gap between human language and machine code. By utilizing a syntax based on natural English grammar, KukuLang makes programming accessible to non-programmers while retaining the power of modern static typing, custom data structures, and complex logic handles.

Whether you are a beginner looking to understand how code works or a developer interested in language design, KukuLang offers a unique perspective on readability and structure.

### 📚 Documentation Center

- **[Design & Architecture](DesignDocumentation.md)**: A deep dive into how KukuLang is built (Architecture, Type System, Memory Management).
- **[Frontend Internals](FrontEnd/README.md)**: Explore the core components of the compiler/interpreter:
  - [Lexer](FrontEnd/Lexer/README.MD): How code becomes tokens.
  - [Parser](FrontEnd/Parser/README.MD): How tokens become structure.
  - [Interpreter](FrontEnd/Interpreter/README.MD): How structure becomes action.

---

## ✨ Features

- **🗣️ Natural Syntax**: Write code that reads like English sentences.
- **🛡️ Static Typing**: Catch errors at compile-time with a robust type system.
- **📦 Structures (Custom Types)**: Define your own data shapes easily.
- **⚡ Tasks (Functions)**: Create reusable blocks of logic with clear inputs and outputs.
- **🔗 Deep Nesting**: Support for complex object chains like `set user's profile's name`.
- **🎮 Interactive**: Built-in support for console Input and Output.

---

## 📝 Syntax Guide

### Comments
comments are essential for documenting your code.
```kuku
~ This is a single line comment ~
```

### Variables & Assignment
Defining variables is intuitive. You simply "set" a value to a name.

**Simple Assignment:**
```kuku
set score to 100;
set message to "Hello World";
```

**Taking User Input:**
```kuku
set userName to input;
```

### Output
Displaying text to the console.
```kuku
print with "Welcome to KukuLang!";
```

### Custom Data Types (Structs)
You can define your own data structures to represent real-world objects.

**Definition:**
```kuku
define Human with name(text), age(int);
define Student with personalInfo(Human), grade(int);
```

**Initialization:**
```kuku
set studentObj to Student;
```

**Nested Property Access:**
KukuLang shines with its possessive syntax for nested objects.
```kuku
~ Equivalent to: studentObj.personalInfo.name = "John"; ~
set studentObj's personalInfo's name to "John";

~ Equivalent to: studentObj.grade = 12; ~
set studentObj's grade to 12;
```

### Tasks (Functions)
Functions in KukuLang are called "Tasks". They can take parameters ("with") and return values ("returning").

**Defining a Task:**
```kuku
define CreateHuman returning Human with names(text), ages(int) {
    set newHuman to Human;
    set newHuman's name to names;
    set newHuman's age to ages;
    return newHuman;
}
```

**Task without return value:**
```kuku
define Greet returning nothing {
    print with "Hello there!";
}
```

**Executing a Task:**
```kuku
set myHuman to CreateHuman with names("Alice"), ages(25);
Greet;
```

### Control Flow

**Conditionals:**
```kuku
if score is_greater_than 50 then {
    print with "You pass!";
}
else {
    print with "Try again.";
}
```

**Available Conditions:**
- `is` (Equal to)
- `is_not` (Not equal to)
- `is_less_than`
- `is_greater_than`
- `is_less_or_is` (Less than or equal)
- `is_greater_or_is` (Greater than or equal)

**Loops:**

*Run **until** a condition becomes true:*
```kuku
set count to 0;
until count is 5 repeat {
    set count to count + 1;
}
```

*Run **as long as** a condition is true:*
```kuku
as_long_as count is_greater_than 0 repeat {
    set count to count - 1;
}
```

---

## 🛠️ building & Running

1. **Prerequisites**: Ensure you have the .NET SDK installed.
2. **Open Project**: Open `KukuLang.sln` in Visual Studio or your preferred C# IDE.
3. **Run**: Build and run the `ConsoleApp1` (or whichever project is the entry point) to start the REPL environment or execute a script.

---

## 🔮 Future Roadmap

- **Enhanced Error Messages**: clearer explanations when things go wrong.
- **Collection Types**: Native support for Lists and Maps.
- **Module System**: Importing other KukuLang files.
- **Compilation**: Potentially moving from interpretation to LLVM-based compilation.
