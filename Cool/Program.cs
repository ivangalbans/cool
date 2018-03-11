using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Cool.AST;
using Cool.Parsing;

namespace Cool
{
    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("Cool Compiler version 1.0\nCopyright (C) 2018 Ivan Galban Smith\nFaculty of Mathematics and Computer Science\nUniversity of Havana");

            string preffixSuccess = "../../../Examples/success/";
            string preffixFail = "../../../Examples/fail/";

            string file = "equalsassociativity.cl";
            string inputPath = preffixFail + file;

            ASTNode root = ParseInput(inputPath);


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

                /* Print Tokens*/
                /*foreach (var item in lexer.GetAllTokens())
                {
                    Console.WriteLine(item);
                }
                */

                var parser = new CoolParser(tokens);

                parser.RemoveErrorListeners();
                parser.AddErrorListener(new ParserErrorListener(errors));

                

                IParseTree tree = parser.program();

                Console.WriteLine(tree.ToStringTree(parser));

                Console.WriteLine();
                Console.WriteLine(errors.Count);
                Console.WriteLine();

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
    }
}
