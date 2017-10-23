using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grammars;
using Lexer;
using ErrorLogger;

namespace Core
{
    class CoolLexer : ILexer<Token>
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
                (".", "."),
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

        public IEnumerable<Token> Lex(string str, Grammar g)
        {
            var listStatements = str.Split('\n');
            var lineNumber = 1;
            var tokens = new List<Token>();

            foreach(var statement in listStatements)
            {
                foreach(var item in _engine.Lex(statement, g))
                {
                    item.Line = lineNumber;
                    tokens.Add(item);
                }
                if(tokens[tokens.Count - 1].Type == "EOF")
                {
                    tokens[tokens.Count - 1].Text = "\n";
                    tokens[tokens.Count - 1].Type = "newline";
                }
                ++lineNumber;
            }

            if (tokens[tokens.Count - 1].Type == "newline")
            {
                tokens[tokens.Count - 1].Text = g.EOF.Name;
                tokens[tokens.Count - 1].Type = "EOF";
            }

            /*
             * 
             * To implement a cleaner for the comments
             * 
            */

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
            for (var i = 0; i < tokens.Count;)
            {
                var tmp = tokens[i++];
                if (tmp.Type == "blockCommentInit")
                {
                    while (i < tokens.Count && tokens[i].Type != "blockCommentEnd")
                        tmp.Text += tokens[i++].Text;
                    if (i < tokens.Count)
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
                    while (i < tokens.Count && tokens[i].Type != "EOF" && tokens[i].Type != "newline")
                        tmp.Text += tokens[i++].Text;
                    if (i < tokens.Count)
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
                    while (i < tokens.Count && tokens[i].Type != "strdelimiter")
                    {
                        if (tokens[i].Type == "newline" || tokens[i].Type == "0" || tokens[i].Type == "EOF")
                            break;
                        if (tokens[i].Type == "\\" && tokens[i + 1].Type == "newline")
                        {
                            i += 2;
                            continue;
                        }
                        if (tokens[i].Type == "\\" && specialDict.TryGetValue(tokens[i + 1].Text, out string value))
                        {
                            i += 2;
                            tmp.Text += value;
                            continue;
                        }

                        if (tokens[i].Type == "\\")
                            i++;
                        tmp.Text += tokens[i++].Text;
                    }
                    if (i < tokens.Count && tokens[i].Type != "newline" && tokens[i].Type != "0" && tokens[i].Type != "EOF" &&
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
