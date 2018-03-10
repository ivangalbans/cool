using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool
{
    class Program
    {
        class PP
        {
            public string Name { get; set; }

            public PP(string nn)
            {
                Name = nn;
            }
        }
        


        static void Main(string[] args)
        {
            var a = new List<PP>(){ new PP("aaa"), new PP("bbb")};

            var b = a.GetRange(0, 2);

            b[0].Name = "zzzzzzzzzzzzzzz";

            Console.WriteLine(a[0].Name);


        }
    }
}
