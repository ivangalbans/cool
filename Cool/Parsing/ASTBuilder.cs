using Antlr4.Runtime.Misc;
using Cool.AST;
using System;

namespace Cool.Parsing
{
    class ASTBuilder : CoolBaseVisitor<ASTNode>
    {
        public override ASTNode VisitProgram([NotNull] CoolParser.ProgramContext context)
        {
            var node = new ProgramNode(context);
            node.Children.Add(Visit(context.programBlock()));
            return node;
        }

        public override ASTNode VisitClasses([NotNull] CoolParser.ClassesContext context)
        {
            return base.Visit(context);
        }

        public override ASTNode VisitArithmetic([NotNull] CoolParser.ArithmeticContext context)
        {
            ArithmeticOperation node;
            switch (context.op.Text)
            {
                case "*":
                    node = new MulNode(context);
                    break;
                case "/":
                    node = new DivNode(context);
                    break;
                case "+":
                    node = new AddNode(context);
                    break;
                case "-":
                    node = new SubNode(context);
                    break;
                default:
                    throw new NotSupportedException();
            }

            node.Children.Add(Visit(context.expression(0))); // LEFT EXPRESSION
            node.Children.Add(Visit(context.expression(1))); // RIGHT EXPRESSION
            return node;
        }

        public override ASTNode VisitAssignment([NotNull] CoolParser.AssignmentContext context)
        {
            return base.VisitAssignment(context);
        }

        public override ASTNode VisitBlock([NotNull] CoolParser.BlockContext context)
        {
            return base.VisitBlock(context);
        }

        public override ASTNode VisitBoolNot([NotNull] CoolParser.BoolNotContext context)
        {
            return base.VisitBoolNot(context);
        }

        public override ASTNode VisitCase([NotNull] CoolParser.CaseContext context)
        {
            return base.VisitCase(context);
        }

        public override ASTNode VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            return base.VisitClassDefine(context);
        }

        public override ASTNode VisitComparisson([NotNull] CoolParser.ComparissonContext context)
        {
            return base.VisitComparisson(context);
        }

        public override ASTNode VisitEof([NotNull] CoolParser.EofContext context)
        {
            return base.VisitEof(context);
        }

        public override ASTNode VisitFalse([NotNull] CoolParser.FalseContext context)
        {
            return base.VisitFalse(context);
        }

        public override ASTNode VisitFormal([NotNull] CoolParser.FormalContext context)
        {
            return base.VisitFormal(context);
        }

        public override ASTNode VisitId([NotNull] CoolParser.IdContext context)
        {
            return base.VisitId(context);
        }

        public override ASTNode VisitIf([NotNull] CoolParser.IfContext context)
        {
            return base.VisitIf(context);
        }

        public override ASTNode VisitInt([NotNull] CoolParser.IntContext context)
        {
            return base.VisitInt(context);
        }

        public override ASTNode VisitIsvoid([NotNull] CoolParser.IsvoidContext context)
        {
            return base.VisitIsvoid(context);
        }

        public override ASTNode VisitLetIn([NotNull] CoolParser.LetInContext context)
        {
            return base.VisitLetIn(context);
        }

        public override ASTNode VisitMethod([NotNull] CoolParser.MethodContext context)
        {
            return base.VisitMethod(context);
        }

        public override ASTNode VisitMethodCall([NotNull] CoolParser.MethodCallContext context)
        {
            return base.VisitMethodCall(context);
        }

        public override ASTNode VisitNegative([NotNull] CoolParser.NegativeContext context)
        {
            return base.VisitNegative(context);
        }

        public override ASTNode VisitNew([NotNull] CoolParser.NewContext context)
        {
            return base.VisitNew(context);
        }

        public override ASTNode VisitOwnMethodCall([NotNull] CoolParser.OwnMethodCallContext context)
        {
            return base.VisitOwnMethodCall(context);
        }

        public override ASTNode VisitParentheses([NotNull] CoolParser.ParenthesesContext context)
        {
            return base.VisitParentheses(context);
        }

        public override ASTNode VisitProperty([NotNull] CoolParser.PropertyContext context)
        {
            return base.VisitProperty(context);
        }

        public override ASTNode VisitString([NotNull] CoolParser.StringContext context)
        {
            return base.VisitString(context);
        }

        public override ASTNode VisitTrue([NotNull] CoolParser.TrueContext context)
        {
            return base.VisitTrue(context);
        }

        public override ASTNode VisitWhile([NotNull] CoolParser.WhileContext context)
        {
            return base.VisitWhile(context);
        }
    }
}
