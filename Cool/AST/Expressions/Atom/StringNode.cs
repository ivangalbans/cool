using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public class StringNode : AtomNode
    {
        public string Text { get; set; }

        public StringNode(ParserRuleContext context, string text) : base(context)
        {
            Text = "";

            // 0 to Length - 1 in order to remove the (Antlr delivered) ""
            for(int i = 1; i < text.Length-1; ++i)
            {
                if(text[i] != '\\')
                {
                    Text += text[i];
                }
                else
                {
                    i++;
                    if(char.IsWhiteSpace(text[i]))
                    {
                        while (char.IsWhiteSpace(text[i]))
                            i++;
                    }
                    else
                    {
                        char c = '\"';
                        if (text[i] == 'n')  c = '\n';
                        if (text[i] == 't')  c = '\t';
                        if (text[i] == 'r')  c = '\r';
                        if (text[i] == '\\') c = '\\';
                        if (text[i] == 't')  c = '\t';
                        if (text[i] == '\"') c = '\"';
                        Text += c;
                    }
                }
            }

        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
