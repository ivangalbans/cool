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
        public IParseTree tree;

        public List<string> ParsingFile(string file)
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

            tree = parser.program();

            return errors;
        }

        [TestMethod]
        public void ParsingSuccess()
        {
            string directorySuccess = "../../../Examples/Parsing/success/";
            DirectoryInfo directory = new DirectoryInfo(directorySuccess);
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                List<string> errors = ParsingFile(file.FullName);
                foreach (var item in errors)
                {
                    Assert.Fail($"{file.Name}. {item}");
                }
            }
        }

        [TestMethod]
        public void ParsingFail()
        {
            string directoryFail = "../../../Examples/Parsing/fail/";
            DirectoryInfo directory = new DirectoryInfo(directoryFail);
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                List<string> errors = ParsingFile(file.FullName);
                Assert.IsTrue(errors.Any(), file.Name);
            }
        }
    }
}
