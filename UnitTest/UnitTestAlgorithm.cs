using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Cool.AST;
using Cool.Parsing;
using Cool.Semantics;

namespace UnitTest
{
    [TestClass]
    public class UnitTestAlgorithm
    {
        [TestMethod]
        public void TopologicalSortSuccess()
        {
            UnitTestParsing testParsing = new UnitTestParsing();
            string directorySuccess = "../../../Examples/Algorithm/success/";
            DirectoryInfo directory = new DirectoryInfo(directorySuccess);
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                List<SemanticError> errors = new List<SemanticError>();
                testParsing.ParsingFile(file.FullName);

                var astBuilder = new ASTBuilder();
                ProgramNode program = astBuilder.Visit(testParsing.tree) as ProgramNode;

                Algorithm.TopologicalSort(program.Classes, errors);
                foreach (var item in errors)
                    Assert.Fail(file.Name + ": " + item.ToString());
            }
        }

        [TestMethod]
        public void TopologicalSortFail()
        {
            UnitTestParsing testParsing = new UnitTestParsing();
            string directorySuccess = "../../../Examples/Algorithm/fail/";
            DirectoryInfo directory = new DirectoryInfo(directorySuccess);
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                List<SemanticError> errors = new List<SemanticError>();
                testParsing.ParsingFile(file.FullName);
                var astBuilder = new ASTBuilder();
                ProgramNode program = astBuilder.Visit(testParsing.tree) as ProgramNode;
                Algorithm.TopologicalSort(program.Classes, errors);
                Assert.IsTrue(errors.Count != 0, $"Not found cycles in class dependency in file {file.Name}");
            }
        }
    }
}
