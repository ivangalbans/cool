using System.Collections.Generic;
using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    abstract class ASTNode
    {
        public int Line { get; }

        public int Column { get; }

        public List<ASTNode> Children { get; set; } = new List<ASTNode>();

        public ASTNode(ParserRuleContext context)
        {   
            Line = context.Start.Line;
            Column = context.Start.Column;
        }

        public ASTNode(int line, int column)
        {
            Line = line;
            Column = column;
        }

    }
}
