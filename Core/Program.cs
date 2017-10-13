using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 1, b = 2;
            (a, b) = (0, 2);
            (a, b) = (b, a);
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine("Welcome to Compiler's Cool");
        }
    }
}
