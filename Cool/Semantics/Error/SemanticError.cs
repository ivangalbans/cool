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
                Message = $"Circular base class dependency involving" +
                $" '{confilctClassA.Text}' (Line: {confilctClassA.Line} Column: {confilctClassA.Column}) and " +
                $" '{confilctClassB.Text}' (Line: {confilctClassB.Line} Column: {confilctClassB.Column})"
            };
        }

        public static SemanticError RepeatedClass(TypeNode node)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) The program already contains a definition for '{node.Text}'."
            };
        }

        public static SemanticError NotDeclaredVariable(IdentifierNode node)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) The name '{node.Text}' does not exist in the current context."
            };
        }

        public static SemanticError NotDeclaredType(TypeNode node)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) The type '{node.Text}' could not be found."
            };
        }

        public static SemanticError RepeatedVariable(IdNode node)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) The variable '{node.Text}' is already defined in this scope."
            };
        }

        public static SemanticError CannotConvert(ASTNode node, TypeInfo first, TypeInfo second)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) Cannot convert from '{first.Text}' to '{second.Text}'."
            };
        }

        public static SemanticError InvalidUseOfOperator(UnaryOperationNode node, TypeInfo operand)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) Operator '{node.Symbol}' cannot be applied to operands of type '{operand.Text}'."
            };
        }

        public static SemanticError InvalidUseOfOperator(BinaryOperationNode node, TypeInfo leftOperand, TypeInfo rightOperand)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) Operator '{node.Symbol}' cannot be applied to operands of type '{leftOperand.Text}' and '{rightOperand.Text}'."
            };
        }

        public override string ToString()
        {
            return $"{Message}";
        }

        public static SemanticError NotDeclareFunction(DispatchImplicitNode node, string name)
        {
            return new SemanticError
            {
                Node = node,
                Line = node.Line,
                Column = node.Column,
                Message = $"(Line: {node.Line}, Column: {node.Column}) The name '{name}' does not exist in the current context."
            };
        }
    }
}
