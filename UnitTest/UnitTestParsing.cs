using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System.IO;
using ErrorLogger;
using System.Linq;
using Parsing;
using Cool_Grammar;

namespace UnitTest
{
    [TestClass]
    public class UnitTestParsing
    {
        static CoolGrammar grammar = new CoolGrammar();
        static CoolTable table = new CoolTable(grammar._coolGrammar);
        static CoolLexer lexer = new CoolLexer();

        [TestMethod]
        public void ParsingSuccess()
        {
            DirectoryInfo directory = new DirectoryInfo("../../../Examples/success/");
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                Errors.Clear();

                StreamReader sr = new StreamReader(file.FullName);
                var input = sr.ReadToEnd();
                var tokens = lexer.Lex(input, grammar._coolGrammar).ToList();

                table.table.TryParse(grammar._coolGrammar, tokens, out DerivationTree tree);

                foreach (var error in Errors.Report())
                    Assert.Fail(file.Name + "    " + error);
            }
        }

        [TestMethod]
        public void ParsingFail()
        {
            DirectoryInfo directory = new DirectoryInfo("../../../Examples/fail/");
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                Errors.Clear();

                StreamReader sr = new StreamReader(file.FullName);
                var input = sr.ReadToEnd();
                var tokens = lexer.Lex(input, grammar._coolGrammar).ToList();

                table.table.TryParse(grammar._coolGrammar, tokens, out DerivationTree tree);

                Assert.IsTrue(Errors.HasError());
            }
        }

    }
}
