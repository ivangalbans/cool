using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Cool.AST;
using Cool.Parsing;
using Cool.Semantics;

namespace Cool
{

    class Program
    {
        static readonly int ErrorCode = 1;

        static void Main(string[] args)
        {

            /*Console.WriteLine("Cool Language Compiler. Version 1.0");
            Console.WriteLine("Faculty of Mathematics and Computer Science");
            Console.WriteLine("University of Havana");
            Console.WriteLine();
            Console.WriteLine("Copyright (c) 2018 Ivan Galban Smith");
            Console.WriteLine("All Rights Reserved.\n");*/

            string preffixSuccess = "../../../Examples/Semantics/success/";
            string preffixFail = "../../../Examples/Semantics/fail/";

            string file = "sum.cl";
            string inputPath = preffixSuccess + file;
            string outputPath = "";


            ASTNode root = ParseInput(inputPath);

            if (root == null)
            {
                Console.WriteLine("AST no created");
                Environment.ExitCode = ErrorCode;
                return;
            }

            if (!(root is ProgramNode))
            {
                Console.WriteLine("AST created with big problems. (root is not a ProgramNode)");
                Environment.ExitCode = ErrorCode;
                return;
            }

            var scope = new Scope();
            ProgramNode rootProgram = root as ProgramNode;
            if(!CheckSemantics(rootProgram, scope))
            {
                Environment.ExitCode = ErrorCode;
                return;
            }


            GenerateCode(rootProgram, outputPath, scope);

        }

        private static ASTNode ParseInput(string inputPath)
        {
            //try
            {
                var input = new AntlrFileStream(inputPath);
                var lexer = new CoolLexer(input);

                var errors = new List<string>();
                lexer.RemoveErrorListeners();
                lexer.AddErrorListener(new LexerErrorListener(errors));

                var tokens = new CommonTokenStream(lexer);
                var parser = new CoolParser(tokens);

                parser.RemoveErrorListeners();
                parser.AddErrorListener(new ParserErrorListener(errors));

                IParseTree tree = parser.program();

                if (errors.Any())
                {
                    Console.WriteLine();
                    foreach (var item in errors)
                        Console.WriteLine(item);
                    return null;
                }

                var astBuilder = new ASTBuilder();
                ASTNode ast = astBuilder.Visit(tree);
                return ast;
            }
            /*catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }*/
        }

        private static bool CheckSemantics(ProgramNode root, Scope scope)
        {
            var errors = new List<SemanticError>();

            var programNode = new Tour1().CheckSemantic(root, scope, errors);
            programNode = new Tour2().CheckSemantic(programNode, scope, errors);

            if (errors.Count == 0)
                return true;

            Console.WriteLine();
            foreach (var error in errors)
                Console.WriteLine(error);
            return false;
        }

        private static void GenerateCode(ProgramNode root, string outputPath, Scope scope)
        {

        }
        
    }
}
