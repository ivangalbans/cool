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
        public List<ClassNode> ClassNodeList { get; set; }

        public ProgramNode(ClassNode classNode)
        {
            ClassNodeList = new List<ClassNode>() { classNode };
        }

        public ProgramNode(ClassNode classNode, ProgramNode programNode)
        {
            ClassNodeList = new List<ClassNode>() { classNode };
            ClassNodeList.AddRange(programNode.ClassNodeList);
        }
    }
}
