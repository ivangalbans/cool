using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Cool.Parsing;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTest
{
    [TestClass]
    public class UnitTestParsing
    {
        CommonTokenStream tokens;
        CoolParser parser;


        private List<string> ParsingFile(string file)
        {
            var input = new AntlrFileStream(file);
            var lexer = new CoolLexer(input);
            var errors = new List<string>();

            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new LexerErrorListener(errors));

            tokens = new CommonTokenStream(lexer);
            parser = new CoolParser(tokens);

            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ParserErrorListener(errors));

            return errors;
        }

        [TestMethod]
        public void TestMethodParsingSuccess()
        {
            string directorySuccess = "../../../Examples/success/";
            DirectoryInfo directory = new DirectoryInfo(directorySuccess);
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                List<string> errors = ParsingFile(file.FullName);
                foreach (var error in errors)
                {
                    Assert.Fail(file.Name + " " + error);
                }
            }
        }

        [TestMethod]
        public void TestMethodParsingFail()
        {
            string directoryFail = "../../../Examples/fail/";
            DirectoryInfo directory = new DirectoryInfo(directoryFail);
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                List<string> errors = ParsingFile(file.FullName);
                Assert.IsTrue(errors.Any());
            }
        }
    }
}
