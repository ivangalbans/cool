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
                testParsing.ParsingFile(file.FullName);
                var astBuilder = new ASTBuilder();
                ProgramNode program = astBuilder.Visit(testParsing.tree) as ProgramNode;

                Algorithm.TopologicalSort(program.Classes);
                Assert.IsTrue(IsTopologicalSort(program.Classes), $"Class dependency in file {file.Name} is not a DAG.");
            }
        }

        private bool IsTopologicalSort(List<ClassNode> classes)
        {
            Dictionary<string, bool> _mk = new Dictionary<string, bool>
            {
                { "Object", true },
                { "IO", true }
            };
            for(int i = classes.Count - 1; i >= 0; --i)
            {
                if (!_mk.ContainsKey(classes[i].TypeInherit.TypeId))
                    return false;
                _mk.Add(classes[i].TypeClass.TypeId, true);
            }
            return true;
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
                testParsing.ParsingFile(file.FullName);
                var astBuilder = new ASTBuilder();
                ProgramNode program = astBuilder.Visit(testParsing.tree) as ProgramNode;

                Algorithm.TopologicalSort(program.Classes);
                Assert.IsFalse(IsTopologicalSort(program.Classes), $"Not found cycles in class dependency in file {file.Name}");
            }
        }
    }
}
