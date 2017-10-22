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

namespace Core
{
    class Program
    {
        static void Main(string[] args)
        {

            CoolGrammar grammar = new CoolGrammar();

            #region Generate CoolTable
            /*
            var crono = new Stopwatch();
            crono.Start();
            new WriteTable(grammar._coolGrammar);
            crono.Stop();
            Console.WriteLine(crono.ElapsedMilliseconds / 1000.0);
            */
            #endregion


            //Console.WriteLine(grammar._coolGrammar);
            //LrAutomaton at = new LrAutomaton(grammar._coolGrammar);
            //Console.WriteLine(at);

            /*CoolTable t = new CoolTable(grammar._coolGrammar);
            CoolLexer a = new CoolLexer();
            StreamReader asd = new StreamReader("test1.cl");

            var g = asd.ReadToEnd();

            var tok = a.Lex(g, grammar._coolGrammar).ToList();
            if (Errors.HasError())
                Errors.Report();
            else
            {
                t.table.TryParse(grammar._coolGrammar, tok, out DerivationTree tree);
                Console.WriteLine(tree);
                var tr = tree.Evaluate();

                if (Errors.HasError())
                    Errors.Report();

                Console.WriteLine();
            }*/


        }
    }
}
