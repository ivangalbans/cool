using Antlr4.Runtime;

namespace Cool.AST
{
    public abstract class FeatureNode : ASTNode
    {
        public FeatureNode(ParserRuleContext context) : base(context) { }

    }
}
