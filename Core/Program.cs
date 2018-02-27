using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

using BottomUpParsing;
using Error;
using ErrorLogger;
using Parsing;
using Cool_Grammar;

namespace Core
{
    class Program
    {
        static bool ReportErrors(string phase)
        {
            Console.WriteLine();
            Console.WriteLine($"Finding error in {phase}.");
            if (Errors.HasError())
            {
                foreach (var errors in Errors.Report())
                {
                    Console.WriteLine(errors);
                }
                Console.WriteLine();
                return true;
            }
            Console.WriteLine();
            return false;
        }

        static void Main(string[] args)
        {
            #region Generate CoolTable
            /*
            var crono = new Stopwatch();
            crono.Start();
            new WriteTable(grammar._coolGrammar);
            crono.Stop();
            Console.WriteLine(crono.ElapsedMilliseconds / 1000.0);
            */
            #endregion

            #region Testing Parsing

            /*CoolGrammar grammar = new CoolGrammar();
            CoolTable table = new CoolTable(grammar._coolGrammar);
            CoolLexer lexer = new CoolLexer();

            Console.WriteLine();
            Console.WriteLine("*******************************************");
            Console.WriteLine();

            DirectoryInfo directory = new DirectoryInfo("../../../Examples/success/hello_world.cl");

            Errors.Clear();

            StreamReader sr = new StreamReader(directory.FullName);
            var input = sr.ReadToEnd();
            var tokens = lexer.Lex(input, grammar._coolGrammar.EOF.Name).ToList();

            if (ReportErrors("Lexer"))
                return;

            table.table.TryParse(grammar._coolGrammar, tokens, out DerivationTree tree);

            if (ReportErrors("Parsing"))
                return;

            //var evaluatedTree = tree.Evaluate();

            //ReportErrors("Evaluated Tree");*/

            #endregion

            CoolGrammar grammar = new CoolGrammar();
            LrAutomaton at = new LrAutomaton(grammar._coolGrammar);

            var table = new SlrTable(at);

            CoolLexer a = new CoolLexer();

            string preffixPath = "../../../Examples/success/";
            StreamReader asd = new StreamReader(preffixPath + "fact.cl");

            var g = asd.ReadToEnd();

            var tok = a.Lex(g, grammar._coolGrammar.EOF.Name).ToList();

            if (ReportErrors("Lexer Phase"))
             return;

            /*if(table.TryParse(grammar._coolGrammar, tok, out DerivationTree tree))
            {

                var tr = tree.Evaluate();

                Console.WriteLine(tr);

                if (ReportErrors("Evaluating Tree Phase"))
                    return;

            }*/

        }
    }
}
