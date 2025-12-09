# KukuLang Lexer 🔡

The **Lexer** (or Scanner) is the first phase of the KukuLang compiler pipeline. Its responsibility is to convert the raw source code string into a sequence of structured **Tokens**.

## Overview

Lexical analysis gives "meaning" to the raw characters in a file. Instead of treating the code as a long string of text, the Lexer groups characters into meaningful units (Tokens) that the Parser can understand, such as Keywords, Identifiers, Operators, and Literals.

### The Scanning Strategy

KukuLang uses a **custom scanning approach** rather than a traditional Finite State Machine (FSM) or Regex-based tokenizer. This was chosen for simplicity and manual control over the scanning position.

The core algorithm is a "Search for Delimiter" loop:
1.  **Start**: Initialize `StartPos` and `EndPos` pointers at the current character.
2.  **Scan**: Advance `EndPos` character by character.
3.  **Stop**: When a "Delimiter" (space, operator, newline, special symbol) is found, the scan stops.
4.  **Extract**: The substring between `StartPos` and `EndPos` is extracted.
5.  **Tokenize**: The extracted string is analyzed to determine its Token Type (see below).
6.  **Repeat**: Move `StartPos` to the next unread character and repeat until EOF.

#### Example Trace
Source: `set a to 2;`

| Step | Scanned String | Detected Type | Action |
|------|----------------|---------------|--------|
| 1 | `set` | Keyword | Emit Token `[SET]` |
| 2 | `a` | Identifier | Emit Token `[IDENTIFIER: a]` |
| 3 | `to` | Keyword | Emit Token `[TO]` |
| 4 | `2` | IntLiteral | Emit Token `[INT: 2]` |
| 5 | `;` | Delimiter | Emit Token `[SEMICOLON]` |

## Implementation Details

The Lexer maintains two key pointers to the source code string:
- `_inputStartPos`: Typical start of the current word.
- `_inputEndPos`: The generic probe exploring ahead.

### 1. Scanning (`UpdateEndPos`)
This method advances `_inputEndPos` until a break condition is met.
- **Whitespace**: Skipped automatically.
- **Strings**: If a quote `"` is seen, it scans until the closing quote.
- **Comments**: If `~` is seen, it scans until the closing `~`.
- **Standard**: Otherwise, it scans until a delimiter (whitespace, `;`, `(`, `)`, etc.) is hit.

### 2. Token Generation (`GenerateToken`)
Once a substring is isolated, this method classifies it:
- **Keywords**: Checks against a dictionary of reserved words (`if`, `else`, `define`, `return`, etc.).
- **Literals**: Checks if it's a number, string, boolean, or char.
- **Identifiers**: If it's not a keyword or literal, it is treated as an Identifier (variable or function name).

### 3. Advancing
After a token is generated, `_inputStartPos` catches up to `_inputEndPos`, preparing for the next scan cycle.

## Next Step
Once the entire file is tokenized, the list of tokens is passed to the **[Parser](../Parser/README.MD)**.
