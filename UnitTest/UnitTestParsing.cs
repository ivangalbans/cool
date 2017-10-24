using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System.IO;
using ErrorLogger;
using System.Linq;
using Parsing;

namespace UnitTest
{
    [TestClass]
    public class UnitTestParsing
    {
        [TestMethod]
        public void success()
        {
            CoolGrammar grammar = new CoolGrammar();
            CoolTable table = new CoolTable(grammar._coolGrammar);
            CoolLexer lexer = new CoolLexer();

            StreamReader file = new StreamReader("../../../Examples/success/hello_world.cl");
            var input = file.ReadToEnd();
            var tokens = lexer.Lex(input, grammar._coolGrammar).ToList();

            table.table.TryParse(grammar._coolGrammar, tokens, out DerivationTree tree);

            foreach (var error in Errors.Report())
                Assert.Fail(error);
        }
    }
}
