using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Cool.AST;
using Cool.Parsing;
using Cool.Semantics;
using Cool.CodeGeneration.IntermediateCode;

namespace Cool
{

    class Program
    {

        static readonly int ErrorCode = 1;


        static void f(ref List<int> pp)
        {
            List<int> LOL = new List<int>();
            pp = LOL;
        }

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

            string[] folder = { preffixFail, preffixSuccess };
            //string file = "book_list.cl";
            string file = "fibo.cl";
            string inputPath = folder[1] + file;

            string outputPath = "";

            if(!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file path '{inputPath}' is not valid, does not exist or user has no sufficient permission to read it.");
                Environment.ExitCode = ErrorCode;
                return;
            }

            ASTNode root = ParseInput(inputPath);
            //return;
            if (root == null)
            {
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

            Console.WriteLine(rootProgram);
            

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
            var errors = new List<string>();

            var programNode = new Tour1().CheckSemantic(root, scope, errors);
            if (Algorithm.ReportError(errors))
                return false;

            programNode = new Tour2().CheckSemantic(programNode, scope, errors);
            if (Algorithm.ReportError(errors))
                return false;

            return true;
        }

        private static void GenerateCode(ProgramNode root, string outputPath, Scope scope)
        {
            new GenerateTour().Visit(root);
            Console.WriteLine("CODE GENERATED OK!!!");
        }

    }
}
