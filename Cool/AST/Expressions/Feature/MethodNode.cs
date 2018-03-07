using System.Collections.Generic;
using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class MethodNode : FeatureNode
    {
        public TypeInfo TypeReturn { get; set; }

        public string TextTypeReturn { get; set; }

        public int LineTypeReturn { get; set; }

        public int ColumnTypeReturn { get; set; }

        public List<(string TextID, string TextType)> Parameters { get; set; }

        public List<((int LineID, int ColumnID), (int LineType, int ColumnType))> LineColumnFormals { get; set; }

        public ExpressionNode Expression { get; set; }

        public MethodNode(ParserRuleContext context) : base(context)
        {
            
        }

    }
}
