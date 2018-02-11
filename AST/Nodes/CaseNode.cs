using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using Grammars;

namespace AST.Nodes
{
    public class CaseNode : KeywordNode
    {
        public ExpressionNode Expression { get; set; }

        public List<(string TextID, string TextType, ExpressionNode Expression)> Body { get; set; }

        public CaseNode(ExpressionNode expression, IEnumerable<(Token ID, Token Type, ExpressionNode Expression)> body)
        {
            Expression = expression;
            Body = body.Select(x => (x.ID.Text, x.Type.Text, x.Expression)).ToList();
        }
    }
}
