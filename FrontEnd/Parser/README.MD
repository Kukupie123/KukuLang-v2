# KukuLang Parser 🌲

The **Parser** is responsible for validating the syntax of the code and checking if it makes grammatical sense. It takes the flat list of **Tokens** produced by the Lexer and builds a hierarchical **Abstract Syntax Tree (AST)**.

## The Hybrid Parsing Approach

Parsing a programming language is complex because different parts of the language behave differently. KukuLang uses a **Hybrid Approach** combining two powerful algorithms:

1.  **Recursive Descent Parser**: Used for **Statements** (e.g., `if`, `while`, `define`). These have a predictable "start-to-finish" structure.
2.  **Pratt Parser**: Used for **Expressions** (e.g., `a + b * c`, `func()`). This efficiently handles **operator precedence** without deep recursion chains.

---

## 1. Statement Parsing (Recursive Descent)

Statements are the "sentences" of the language. The core of this logic resides in `TokenEvaluatorService.cs`. The logic is simple: look at the **Current Token** and switch to the correct "Evaluator" method.

### The Dispatch Loop
The `RecursiveDescentParser` simply loops through tokens and calls `TokenEvaluatorService.EvaluateToken`:

```csharp
// From TokenEvaluatorService.cs
switch (parser.CurrentToken.Type)
{
    case TokenType.Define:
        EvaluateDefineToken(parser, scope); // define Human ...
        break;
    case TokenType.Set:
        EvaluateSetToken(parser, scope);    // set a to 10...
        break;
    case TokenType.If:
        EvaluateIfToken(parser, scope);     // if a is 10 ...
        break;
    // ... handles Until, Return, Print, etc.
}
```

### Deep Dive: `EvaluateIfToken`
How does KukuLang parse an `if` statement?
1.  **Advance**: Skips the `if` token.
2.  **Expression**: Uses `PrattParser` to parse the condition (e.g., `a is 10`).
3.  **Validation**: Ensures the next token is `{`.
4.  **Scope**: Creates a new `AstScope` (e.g., `Global->Conditional`).
5.  **Recursion**: Calls `EvaluateToken` inside a loop until `}` is found, populating the new scope.
6.  **Store**: Creates an `IfStmt` node containing the condition and the populated scope.

---

## 2. Expression Parsing (Pratt Parser)

Expressions are tricky. `1 + 2 * 3` should be `7`, not `9`. A simple left-to-right parser would fail here. The **Pratt Parser** (`PrattParser.cs`) solves this using **Binding Power** (Precedence).

### Precedence Table
KukuLang uses the following precedence levels (highest binds most tightly):
1.  **Boolean And/Or** (`and`, `or`) - *Highest*
2.  **Product** (`*`, `/`, `%`)
3.  **Sum** (`+`, `-`)
4.  **Comparison** (`is`, `is_greater_than`)
5.  **Lowest** (Default)

### The `Parse` Algorithm
The magic happens in the `Parse(precedence)` method. It uses a loop to "gobble up" tokens as long as they have higher precedence than the current context.

```csharp
// Simplified logic from PrattParser.cs method Parse(int precedence)

// 1. Parse the "Left" side (e.g., the number "1" in "1 + 2")
var left = ProcessPrimaryExpAndAdvance(); 

// 2. Loop: As long as the NEXT operator is stronger than the CURRENT context...
while (precedence < GetPrecedence(CurrentToken.Type)) 
{
    // 3. Bind the operator to the left side and parse the right side recursively
    left = ProcessInfixAndAdvance(left);
}
```

### Example: `a + b * c`
1.  **Parse `a`**: `ProcessPrimaryExpAndAdvance` returns `a`.
2.  **See `+`**: Precedence is **Sum (3)**.
3.  **Recurse**: Call `Parse(3)` for the right side.
    -   **Parse `b`**: Returns `b`.
    -   **See `*`**: Precedence is **Product (4)**.
    -   **4 > 3**: The condition `precedence < Next` is True!
    -   **Bind**: `ProcessInfixAndAdvance` consumes `*` and calls `Parse(4)` for `c`.
    -   **Result**: Returns a Binary Expression `(b * c)`.
4.  **Combine**: The outer call creates `(a + (b * c))`.

---

## Output: The Abstract Syntax Tree (AST)

The Parser outputs an `AstScope` which essentially holds a list of these Statement Nodes.

**Node Hierarchy Example:**
```
AstScope (Global)
├── Variable: a
├── Variable: b
├── SetToStmt
│   ├── Variable: a
│   └── Value: Literal(10)
└── IfStmt
    ├── Condition: BinaryExp(a is 10)
    └── Body: AstScope (Conditional)
        └── PrintStmt(...)
```

## Next Step
This tree is passed to the **[Interpreter](../Interpreter/README.MD)**, which walks these nodes to execute the program.
