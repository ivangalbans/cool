using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class CaseNode : KeywordNode
    {
        public ExpressionNode ExpressionCase { get; set; }
        public List<(FormalNode Formal, ExpressionNode Expression)> Branches { get; set; }
        public int BranchSelected { get; set; }

        public CaseNode(ParserRuleContext context) : base(context)
        {
            Branches = new List<(FormalNode, ExpressionNode)>();
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        //public override string ToString()
        //{
        //    string repr = $"Case Node (Line: {Line}, Column: {Column}),\n";
        //    repr += "Evaluation:\n| ";
        //    repr += $"{ExpressionCase}\n";
        //    for(int i = 1; i <= Branches.Count; ++i)
        //    {
        //        var (f, e) = Branches[i-1];
        //        repr += $"Condition {i}\n| ";
        //        repr += $"{f}\n";
        //        repr += $"Body {i}\n| ";
        //        repr += $"{e}\n";
        //    }
        //    return repr.Replace("\n", "\n| ");
        //}
    }
}
