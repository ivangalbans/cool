using System.Collections.Generic;
using System.Linq;
using Grammars;
using Lexer;
using ErrorLogger;
namespace Cool_Grammar
{
    public class CoolLexer : ILexer<Token>
    {
        private readonly LexerEngine _engine;

        public CoolLexer()
        {
            var regex = new List<(string, string)>
            {
                ("[cC][lL][aA][sS][sS]", "class"),
                ("[iI][nN][hH][eE][rR][iI][tT][sS]", "inherits"),
                (";", ";"),
                ("\\(", "("),
                ("\\)", ")"),
                (",", ","),
                (":", ":"),
                ("\\.", "."),
                ("{", "{"),
                ("}", "}"),
                ("<\\-", "<-"),
                ("[iI][fF]", "if"),
                ("[tT][hH][eE][nN]", "then"),
                ("@", "@"),
                ("[eE][lL][sS][eE]", "else"),
                ("[fF][iI]", "fi"),
                ("[wW][hH][iI][lL][eE]", "while"),
                ("[lL][oO][oO][pP]", "loop"),
                ("[pP][oO][oO][lL]", "pool"),
                ("[lL][eE][tT]", "let"),
                ("[iI][nN]", "in"),
                ("[cC][aA][sS][eE]", "case"),
                ("[oO][fF]", "of"),
                ("=>", "=>"),
                ("[eE][sS][aA][cC]", "esac"),
                ("[nN][eE][wW]", "new"),
                ("[iI][sS][vV][oO][iI][dD]", "isvoid"),
                ("[vV][oO][iI][dD]", "void"),
                ("\\+", "+"),
                ("\\-", "-"),
                ("\\*", "*"),
                ("/", "/"),
                ("~", "~"),
                ("<", "<"),
                ("<=", "<="),
                ("=", "="),
                ("not", "not"),
                ("[0-9]*", "integer"),
                ("[tT][rR][uU][eE]", "true"),
                ("[fF][aA][lL][Ss][eE]", "false"),
                ("[A-Z][_a-zA-Z0-9]*", "TYPE"),
                ("[a-z][_a-zA-Z0-9]*", "ID"),
                ("\"", "strdelimiter"),
                ("\\-\\-", "lineComment"),
                ("\\(\\*", "blockCommentInit"),
                ("\\*\\)", "blockCommentEnd"),
                ("\b", "backspace"),
                ("[ \f\t\r]*", "spaces"),
                ("\n*", "newline"),
                ("\0", "0"),
                ("\\\\", "\\")
            };
            _engine = new LexerEngine(regex);
        }

        public IEnumerable<Token> Lex(string str, string eof)
        {
            var lines = str.Split('\n');
            var lineNumb = 1;
            var list = new List<Token>();
            foreach (var line in lines)
            {
                foreach (var x in _engine.Lex(line, eof))
                {
                    x.Line = lineNumb;
                    list.Add(x);
                }
                if (list[list.Count - 1].Type == "EOF")
                {
                    list[list.Count - 1].Type = "newline";
                    list[list.Count - 1].Text = "\n";
                }
                lineNumb++;
            }
            if (list[list.Count - 1].Type == "newline")
            {
                list[list.Count - 1].Type = "EOF";
                list[list.Count - 1].Text = eof;
            }
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item);
            //}
            var specialDict = new Dictionary<string, string>
            {
                ["n"] = "\n",
                ["b"] = "\b",
                ["f"] = "\f",
                ["\""] = "\"",
                ["\\"] = "\\",
                ["t"] = "\t"
            };
            var ls = new List<Token>();
            for (var i = 0; i < list.Count;)
            {
                var tmp = list[i++];
                if (tmp.Type == "blockCommentInit")
                {
                    while (i < list.Count && list[i].Type != "blockCommentEnd")
                        tmp.Text += list[i++].Text;
                    if (i < list.Count)
                    {
                        tmp.Type = "comment";
                        tmp.Text += "*)";
                        i++;
                    }
                    else
                    {
                        tmp.Type = "ErrorToken";
                    }
                }
                if (tmp.Type == "lineComment")
                {
                    while (i < list.Count && list[i].Type != "EOF" && list[i].Type != "newline")
                        tmp.Text += list[i++].Text;
                    if (i < list.Count)
                    {
                        tmp.Type = "comment";
                        i++;
                    }
                    else
                    {
                        tmp.Type = "ErrorToken";
                    }
                }
                if (tmp.Type == "strdelimiter")
                {
                    tmp.Text = "";
                    while (i < list.Count && list[i].Type != "strdelimiter")
                    {
                        if (list[i].Type == "newline" || list[i].Type == "0" || list[i].Type == "EOF")
                            break;
                        if (list[i].Type == "\\" && list[i + 1].Type == "newline")
                        {
                            i += 2;
                            continue;
                        }
                        if (list[i].Type == "\\" && specialDict.TryGetValue(list[i + 1].Text, out string value))
                        {
                            i += 2;
                            tmp.Text += value;
                            continue;
                        }

                        if (list[i].Type == "\\")
                            i++;
                        tmp.Text += list[i++].Text;
                    }
                    if (i < list.Count && list[i].Type != "newline" && list[i].Type != "0" && list[i].Type != "EOF" &&
                        tmp.Text.Length < 1024)
                    {
                        tmp.Type = "string";
                        i++;
                    }

                    else
                    {
                        tmp.Type = "ErrorToken";
                    }
                }
                if (tmp.Type != "spaces" && tmp.Type != "newline" && tmp.Type != "comment" && (tmp.Type == "string" || tmp.Text != ""))
                {
                    if (tmp.Type == "ErrorToken") Errors.Log(tmp);
                    ls.Add(tmp);
                }
            }

            return ls;
        }
    }
}