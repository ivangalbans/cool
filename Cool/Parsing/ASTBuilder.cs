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
            node.Children.Add(Visit(context.classDefine()));
            node.Children.AddRange(Visit(context.program()).Children);
            node.ClassNodes = node.Children.Select(x => x as ClassNode).ToList();
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
            var nodeId = new IdentifierNode(context.ID().Symbol.Line, context.ID().Symbol.Column, context.ID().Symbol.Text);
            node.Children.Add(nodeId);
            node.Children.Add(Visit(context.expression()));
            return node;
        }

        public override ASTNode VisitBlock([NotNull] CoolParser.BlockContext context)
        {
            var node = new BlockNode(context);
            node.Children = context.expression().Select(x => Visit(x)).ToList();
            node.Expressions = node.Children.Select(x => x as ExpressionNode).ToList();
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
            
            List<FormalNode> formals = context.formal().Select(x => Visit(x) as FormalNode).ToList();
            List<ExpressionNode> expressions = new List<ExpressionNode>();

            for (int i = 1; i <= formals.Count; ++i)
                expressions.Add(Visit(context.expression(i)) as ExpressionNode);

            for(int i = 0; i < formals.Count; ++i)
            {
                node.Children.Add(formals[i]);
                node.Children.Add(expressions[i]);
                node.Branches.Add((formals[i], expressions[i]));
            }

            return node;
        }

        public override ASTNode VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            var node = new ClassNode(context);

            IdentifierNode typeClass = new IdentifierNode(context.TYPE(0).Symbol.Line, 
                            context.TYPE(0).Symbol.Column, context.TYPE(0).GetText());
            IdentifierNode typeInherit = new IdentifierNode(context.TYPE(1).Symbol.Line,
                            context.TYPE(1).Symbol.Column, context.TYPE(1).GetText());

            node.Children.Add(typeClass);
            node.Children.Add(typeInherit);

            foreach (var item in context.feature())
            {
                var nodeFeature = Visit(item);
                node.Children.Add(nodeFeature);
                node.FeatureNodes.Add(nodeFeature as FeatureNode);
            }

            return node;
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

        public override ASTNode VisitBoolean([NotNull] CoolParser.BooleanContext context)
        {
            return new BoolNode(context, context.value.Text);
        }

        public override ASTNode VisitFormal([NotNull] CoolParser.FormalContext context)
        {
            var node = new FormalNode(context);
            node.Children.Add(Visit(context.ID()));
            node.Children.Add(Visit(context.TYPE()));
            return node;
        }

        public override ASTNode VisitId([NotNull] CoolParser.IdContext context)
        {
            return new IdentifierNode(context, context.ID().GetText());
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
            var node = new LetNode(context);

            return node;
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
            return new StringNode(context.STRING().Symbol.Line, context.STRING().Symbol.Column, context.STRING().Symbol.Text);
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
