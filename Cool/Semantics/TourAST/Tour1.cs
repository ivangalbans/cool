using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cool.AST;

namespace Cool.Semantics
{
    class Tour1 :   IVisitor<ProgramNode>,
                    IVisitor<ClassNode>,
                    IVisitor<AttributeNode>,
                    IVisitor<MethodNode>
    {
        public void Visit(ProgramNode node, IScope scope, List<SemanticError> errors)
        {
            Algorithm.TopologicalSort(node.Classes);
            foreach (var classNode in node.Classes)
            {
                Console.WriteLine($"Entrando a {classNode}");
                this.Visit(classNode, scope, errors);
                Console.WriteLine($"Saliendo de {classNode}");
            }
        }

        public void Visit(ClassNode node, IScope scope, List<SemanticError> errors)
        {
        }

        public void Visit(AttributeNode node, IScope scope, List<SemanticError> errors)
        {
        }

        public void Visit(MethodNode node, IScope scope, List<SemanticError> errors)
        {
        }
    }
}
