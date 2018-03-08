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
            int[] a = { 1, 2, 3 };
            var r3 = (a.Select(e => e.ToString().Select(x => x - '0').Sum())).Sum();
            IEnumerable<string> b =  a.Select(x => x.ToString());



        }
    }
}
