using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class MethodNode : FeatureNode
    {
        public IdNode Id { get; set; }
        public List<FormalNode> Arguments { get; set; }
        public TypeNode TypeReturn { get; set; }
        public ExpressionNode Body { get; set; }

        public MethodNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
