using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AST.Nodes.Abstract
{
    public abstract class FeatureNode : ASTNode
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
    }
}
