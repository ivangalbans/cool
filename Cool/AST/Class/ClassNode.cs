using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class ClassNode : ASTNode, IVisit
    {
        public TypeNode TypeClass { get; set; }

        public TypeNode TypeInherit { get; set; }

        public List<FeatureNode> FeatureNodes { get; set; }

        public IScope Scope { get; set; }

        public ClassNode(ParserRuleContext context) : base(context)
        {
        }

        public ClassNode(int line, int column, string className, string classInherit) : base(line, column)
        {
            TypeClass = new TypeNode(line, column, className);
            TypeInherit = new TypeNode(line, column, classInherit);
        }

        public override string ToString()
        {
            string repr = $"Class Node (Line: {Line}, Column: {Column}) class {TypeClass} inherits {TypeInherit} \n";
            foreach (var f in FeatureNodes)
            {
                repr += f.ToString() + "\n";
            }
            return repr.Replace("\n","\n| ");
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
