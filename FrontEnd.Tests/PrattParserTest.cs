using System;
using System.Collections.Generic;
using System.Diagnostics;
using FrontEnd.Commons.Tokens;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace FrontEnd.Tests;
[TestClass]
[TestSubject(typeof(PrattParser))]
public class PrattParserTest
{

    [TestMethod]
    public void Test1()
    {
        List<Token> tokens = [
            new (TokenType.RoundBracketsOpening,12,0),
            new (TokenType.IntegerLiteral,12,0),
            new (TokenType.Add,1,0),
            new (TokenType.IntegerLiteral,1,0),
            new (TokenType.RoundBracketsClosing,12,0),
            new (TokenType.FullStop,".",0),
        ];

        var parser = new PrattParser(tokens, 0);
        var final = parser.Parse();
        Console.WriteLine(final);
    }
    [TestMethod]
    public void Test2()
    {
        List<Token> tokens = new List<Token>
        {
            new Token(TokenType.RoundBracketsOpening, "(", 0),
            new Token(TokenType.IntegerLiteral, 12, 0),
            new Token(TokenType.Add, "+", 0),
            new Token(TokenType.IntegerLiteral, 1, 0),
            new Token(TokenType.RoundBracketsClosing, ")", 0),
            new Token(TokenType.Multiply, "*", 0),
            new Token(TokenType.IntegerLiteral, 5, 0)
        };


        var parser = new PrattParser(tokens, 0);
        var exp = parser.Parse();
        var expString = exp.ToString();
    }
}
