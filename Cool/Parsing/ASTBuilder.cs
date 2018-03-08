using Antlr4.Runtime.Misc;
using Cool.AST;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var node = new ProgramBlockNode(context);
            node.Children.Add(Visit(context.classDefine()));
            node.Children.AddRange(Visit(context.programBlock()).Children);
            return node;
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
            var node = new AssignmentNode(context);
            var nodeId = new IdentifierNode(context.ID().Symbol.Line, context.ID().Symbol.Column, context.ID().GetText());
            node.Children.Add(nodeId);
            node.Children.Add(Visit(context.expression()));
            return node;
        }

        public override ASTNode VisitBlock([NotNull] CoolParser.BlockContext context)
        {
            var node = new BlockNode(context);
            foreach (var item in context.expression())
                node.Children.Add(Visit(item));
            return node;
        }

        public override ASTNode VisitBoolNot([NotNull] CoolParser.BoolNotContext context)
        {
            var node = new NotNode(context);
            node.Children.Add(Visit(context.expression()));
            return node;
        }

        public override ASTNode VisitCase([NotNull] CoolParser.CaseContext context)
        {
            var node = new CaseNode(context);
            node.Children.Add(Visit(context.expression(0)));

            List<IdentifierNode> idNodes = new List<IdentifierNode>();
            List<IdentifierNode> typeNodes = new List<IdentifierNode>();
            List<ExpressionNode> expressions = new List<ExpressionNode>();

            foreach (var item in context.ID())
                idNodes.Add(new IdentifierNode(item.Symbol.Line, item.Symbol.Column, item.GetText()));
            foreach (var item in context.TYPE())
                typeNodes.Add(new IdentifierNode(item.Symbol.Line, item.Symbol.Column, item.GetText()));
            foreach (var item in context.expression())
                expressions.Add(Visit(item) as ExpressionNode);
            for (int i = 0; i < idNodes.Count; ++i)
                node.Branches.Add((idNodes[i], typeNodes[i], expressions[i]));

            return node;
        }

        public override ASTNode VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            return base.VisitClassDefine(context);
        }

        public override ASTNode VisitComparisson([NotNull] CoolParser.ComparissonContext context)
        {
            ComparisonOperation node;
            switch (context.op.Text)
            {
                case "<=":
                    node = new LessEqual(context);
                    break;
                case "<":
                    node = new Less(context);
                    break;
                case "=":
                    node = new EqualNode(context);
                    break;
                default:
                    throw new NotSupportedException();
            }

            node.Children.Add(Visit(context.expression(0)));    // LEFT EXPRESSION
            node.Children.Add(Visit(context.expression(1)));    //RIGHT EXPRESSION
            return node;
        }

        public override ASTNode VisitEof([NotNull] CoolParser.EofContext context)
        {
            return base.VisitEof(context);
        }

        public override ASTNode VisitBoolean([NotNull] CoolParser.BooleanContext context)
        {
            return new BoolNode(context, context.value.Text);
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
            var node = new IfNode(context);
            node.Children.Add(Visit(context.expression(0)));    //  if expression
            node.Children.Add(Visit(context.expression(1)));    //then expression
            node.Children.Add(Visit(context.expression(2)));    //else expression
            return node;
        }

        public override ASTNode VisitInt([NotNull] CoolParser.IntContext context)
        {
            return new IntNode(context, context.INT().GetText());
        }

        public override ASTNode VisitIsvoid([NotNull] CoolParser.IsvoidContext context)
        {
            return new IsVoidNode(context);
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
            return new StringNode(context, context.STRING().GetText());
        }

        public override ASTNode VisitWhile([NotNull] CoolParser.WhileContext context)
        {
            var node = new WhileNode(context);
            node.Children.Add(Visit(context.expression(0))); // CONDITION
            node.Children.Add(Visit(context.expression(1))); // BODY
            return node;
        }
    }
}
