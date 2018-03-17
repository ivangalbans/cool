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

        public static SemanticError InvalidClassDependency(TypeNode confilctClassA, TypeNode confilctClassB)
        {
            return new SemanticError
            {
                Message = $"The class dependency is not a DAG. Circular base class dependency involving" +
                $" '{confilctClassA.TypeId}' (Line: {confilctClassA.Line} Column: {confilctClassA.Column}) and " +
                $" '{confilctClassB.TypeId}' (Line: {confilctClassB.Line} Column: {confilctClassB.Column})"
            };
        }

        public static SemanticError RepeatedClass(TypeNode node)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) The program already contains a definition for '{node.TypeId}'."
            };
        }

        public static SemanticError NotDeclaredType(TypeNode node)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) The type '{node.TypeId}' could not be found."
            };
        }

        public static SemanticError RepeatedVariable(IdNode node)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) The variable '{node.Name}' is already defined in this scope."
            };
        }

        public override string ToString()
        {
            return $"{Message}.";
        }
    }
}
