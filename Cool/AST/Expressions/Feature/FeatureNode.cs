using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class FeatureNode : ASTNode
    {
        /// <summary>
        /// Get the text of ID in Feature.
        /// </summary>
        public string TextID { get; set; }
        /// <summary>
        /// Get the line number of ID in Feature
        /// </summary>
        public int LineID { get; set; }

        /// <summary>
        /// Get column number of ID in Feature
        /// </summary>
        public int ColumnID { get; set; }

        public FeatureNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
