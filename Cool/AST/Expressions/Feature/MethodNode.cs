using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class MethodNode : FeatureNode
    {
        public IdNode Id => Children[0] as IdNode;
        public List<FormalNode> Arguments => Children.Skip(1).Reverse().Skip(2).Reverse().Cast<FormalNode>().ToList();
        public TypeNode TypeReturn => Children[Children.Count - 2] as TypeNode;
        public ExpressionNode Epxression => Children[Children.Count - 1] as ExpressionNode;

        public MethodNode(ParserRuleContext context) : base(context) { }

    }
}
