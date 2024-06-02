using System;
using System.Collections.Generic;
using FrontEnd.Commons.Tokens;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontEnd.Tests;
[TestClass]
[TestSubject(typeof(PrattParser))]
public class PrattParserTest
{

    [TestMethod]
    public void Test1()
    {
        List<Token> tokens = [
            new (TokenType.Integer,12,0),
            new (TokenType.Plus,1,0),
            new (TokenType.Integer,1,0),
            new (TokenType.FullStop,".",0),
        ];

        var parser = new PrattParser(tokens, 0);
        parser.Parse(0);
    }
}
