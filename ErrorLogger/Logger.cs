using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grammars;
namespace ErrorLogger
{
    public class Error
    {
        private int Line;
        private int Column;
        private string Text;
        private string Type;
        public Error(int Line, int Column, string Text, string Type)
        {
            this.Line = Line;
            this.Column = Column;
            this.Text = Text;
            this.Type = Type;
        }
        public Error(string e)
        {
            Text = e;
        }
        public override string ToString()
        {
            return $"{Type}: Line: {Line} , Column: {Column}, Text: {Text}";
        }
    }

    public static class Errors
    {
        static List<Error> errorList = new List<Error>();
        static bool change;
        public static void Log(Error e)
        {
            change = true;
            errorList.Add(e);
        }

       
        public static void Log(Token t)
        {
            change = true;
            errorList.Add(new Error(t.Line, t.Column, t.Text, t.Type));
        }

        public static bool HasError()
        {
            return change;
        }
        public static void Report()
        {
            foreach (var item in errorList)
            {
                Console.WriteLine(item);
            }
        }


    }
}
