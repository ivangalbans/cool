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

        public static string InvalidClassDependency(TypeNode confilctClassA, TypeNode confilctClassB)
        {
            return  $"Circular base class dependency involving" +
                    $" '{confilctClassA.Text}' (Line: {confilctClassA.Line} Column: {confilctClassA.Column}) and " +
                    $" '{confilctClassB.Text}' (Line: {confilctClassB.Line} Column: {confilctClassB.Column})"
                    ;
        }

        public static string RepeatedClass(TypeNode node)
        {
            return  $"(Line: {node.Line}, Column: {node.Column})" +
                    $"The program already contains a definition for '{node.Text}'."
                    ;
        }

        public static string NotFoundClassMain()
        {
            return  $"Couldn't found the class 'Main'.";
        }

        public static string NotFoundMethodmain(ClassNode node)
        {
            return  $"(Line: {node.Line}, Column: {node.Column})" +
                    $"The class '{node.TypeClass.Text}' has not a method 'main' without parameters."
                    ;
        }

        public static string NotDeclaredVariable(IdentifierNode node)
        {
            return  $"(Line: {node.Line}, Column: {node.Column}) The name " +
                    $"'{node.Text}' does not exist in the current context."
                    ;
        }

        public static string NotDeclaredType(TypeNode node)
        {
            return  $"(Line: {node.Line}, Column: {node.Column})" +
                    $"The type '{node.Text}' could not be found."
                    ;
        }

        public static string RepeatedVariable(IdentifierNode node)
        {
            return  $"(Line: {node.Line}, Column: {node.Column})" +
                    $"The variable '{node.Text}' is already defined in this scope."
                    ;
        }

        public static string CannotConvert(ASTNode node, TypeInfo first, TypeInfo second)
        {
            return  $"(Line: {node.Line}, Column: {node.Column})" +
                    $"Cannot convert from '{first.Text}' to '{second.Text}'."
                    ;
        }

        public static string NotInheritsOf(ClassNode node, TypeInfo type)
        {
            return $"(Line: {node.Line}, Column: {node.Column})" +
                   $"Is not allowed inherit of {type.Text}"
                   ;
        }

        public static string InvalidUseOfOperator(UnaryOperationNode node, TypeInfo operand)
        {
            return  $"(Line: {node.Line}, Column: {node.Column})" +
                    $"Operator '{node.Symbol}' cannot be applied to operands of type '{operand.Text}'."
                    ;
        }

        public static string InvalidUseOfOperator(ArithmeticOperation node)
        {
            return  $"(Line: {node.Line}, Column: {node.Column})" +
                    $"Operator '{node.Symbol}' must be applied to types 'Int'."
                    ;
        }

        public static string InvalidUseOfOperator(BinaryOperationNode node, TypeInfo leftOperand, TypeInfo rightOperand)
        {
            return  $"(Line: {node.Line}, Column: {node.Column})" +
                    $"Operator '{node.Symbol}' cannot be applied to operands of type '{leftOperand.Text}' and '{rightOperand.Text}'."
                    ;
        }

        public static string NotDeclareFunction(DispatchNode node, string name)
        {
            return  $"(Line: {node.Line}, Column: {node.Column})" +
                    $"The name '{name}' does not exist in the current context."
                    ;
        }

        public static string NotMatchedBranch(CaseNode node)
        {
            return  $"(Line: {node.Line}, Column: {node.Column})" +
                    $"At least one branch must be matched."
                    ;
        }
    }
}
