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


            DirectoryInfo directory = new DirectoryInfo("../../../Examples/success/");
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                StreamReader sr = new StreamReader(file.FullName);
                var input = sr.ReadToEnd();
                var tokens = lexer.Lex(input, grammar._coolGrammar).ToList();

                table.table.TryParse(grammar._coolGrammar, tokens, out DerivationTree tree);

                foreach (var error in Errors.Report())
                    Assert.Fail(file.Name + "    " + error);

                Errors.Clear();
            }
        }
    }
}
