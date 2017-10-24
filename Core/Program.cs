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
            
            CoolGrammar grammar = new CoolGrammar();
            CoolTable table = new CoolTable(grammar._coolGrammar);
            CoolLexer lexer = new CoolLexer();

            DirectoryInfo directory = new DirectoryInfo("../../../Examples/success/hello_world.cl");

            Errors.Clear();

            StreamReader sr = new StreamReader(directory.FullName);
            var input = sr.ReadToEnd();
            var tokens = lexer.Lex(input, grammar._coolGrammar).ToList();
            
            table.table.TryParse(grammar._coolGrammar, tokens, out DerivationTree tree);

            foreach (var error in Errors.Report())
                Console.WriteLine($"test.4.cl     {error}");
            
            #endregion

        }
    }
}
