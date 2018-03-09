using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class FeatureNode : ASTNode
    {
        public FeatureNode(ParserRuleContext context) : base(context) { }

    }
}
