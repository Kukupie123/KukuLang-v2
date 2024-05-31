using System;
using FrontEnd;
using FrontEnd.Lexer;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontEnd.Tests;

[TestClass]
[TestSubject(typeof(Program))]
public class LexerTest
{

    [TestMethod]
    public void Tokenize1()
    {
        string source = "set a to 1;";
        var lexer = new KukuLexer(source);
        lexer.Tokenize().ForEach(token => Console.WriteLine(token));
        
    }
}