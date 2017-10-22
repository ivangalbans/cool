using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using BottomUpParsing;

namespace Core
{
    class Program
    {
        static void Main(string[] args)
        {
            CoolGrammar grammar = new CoolGrammar();

            var crono = new Stopwatch();
            crono.Start();
            new WriteTable(grammar._coolGrammar);
            crono.Stop();
            Console.WriteLine(crono.ElapsedMilliseconds / 1000.0);
        }
    }
}
