using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = { 1, 2, 3 , 4, 5, 6, 7, 8, 9, 10};

            foreach(var item in a.Skip(1).Reverse().Skip(2).Reverse().Cast<int>().ToList())
            {
                Console.WriteLine(item);
            }

        }
    }
}
