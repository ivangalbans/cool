using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.Semantics;

namespace Cool.AST
{
    public class VoidNode : AtomNode
    {
        public string GetStaticType { get; }
        public VoidNode(string type, int line = 0, int column = 0) : base(line, column)
        {
            GetStaticType = type;
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<string> errors)
        {
            visitor.Visit(this, scope, errors);
        }

        public override string ToString()
        {
            return $"Void {GetStaticType}";
        }
    }
}
