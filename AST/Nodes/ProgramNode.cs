using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;

namespace AST.Nodes
{
    public class ProgramNode : ASTNode
    {
        public List<ClassNode> ClassNodes { get; set; }

        public ProgramNode(ClassNode classNode)
        {
            ClassNodes = new List<ClassNode>() { classNode };
        }

        public ProgramNode(ClassNode classNode, ProgramNode programNode)
        {
            ClassNodes = new List<ClassNode>() { classNode };
            ClassNodes.AddRange(programNode.ClassNodes);
        }
    }
}
