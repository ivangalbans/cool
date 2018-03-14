using Cool.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    public class SemanticError
    {
        public string Message { get; set; }
        public ASTNode Node { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public static SemanticError InvalidUseOfOperator(string op, string type, string member, ASTNode node)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) Invalid use of {op} operator with a non-{type} {member} value"
            };
        }

        public static SemanticError InvalidClassDependency(ClassNode confilctClassA, ClassNode confilctClassB)
        {
            return new SemanticError
            {
                Message = $"The class dependency is not a DAG. Circular base class dependency involving" +
                $" '{confilctClassA.TypeClass}' (Line: {confilctClassA.Line} Column: {confilctClassA.Column}) and " +
                $"'{confilctClassB.TypeClass} (Line: {confilctClassB.Line} Column: {confilctClassB.Column})'"
            };
        }

        public static SemanticError RepeatedClass(ClassNode classNode)
        {
            return new SemanticError
            {
                Node = classNode,
                Line = classNode.Line,
                Column = classNode.Column,
                Message = $"(Line: {classNode.Line}, Column: {classNode.Column}) The class '{classNode.TypeClass}' is already exist"
            };
        }

        public override string ToString()
        {
            return $"{Message}.";
        }
    }
}
