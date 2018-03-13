using System;
using System.IO;
using Cool.AST;
using Cool.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTestAST
    {
        [TestMethod]
        public void ASTBuilderSuccess()
        {
            UnitTestParsing testParsing = new UnitTestParsing();
            string directorySuccess = "../../../Examples/Parsing/success/";
            DirectoryInfo directory = new DirectoryInfo(directorySuccess);
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                testParsing.ParsingFile(file.FullName);
                var astBuilder = new ASTBuilder();
                ASTNode root = astBuilder.Visit(testParsing.tree);

                Assert.IsFalse(root is null, "AST no created. (root is null)");
                Assert.IsTrue(root is ProgramNode, $"AST created with big problems. (root is not a ProgramNode, root is {root})");
            }
        }
    }
}
