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

        public override ASTNode VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            var node = new ClassNode(context);

            var typeClass = new TypeNode(context.TYPE(0).Symbol.Line, 
                            context.TYPE(0).Symbol.Column, context.TYPE(0).GetText());

            var typeInherit = context.TYPE(1) == null ? TypeNode.NULL : new TypeNode(context.TYPE(1).Symbol.Line,
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

        public override ASTNode VisitMethod([NotNull] CoolParser.MethodContext context)
        {
            var node = new MethodNode(context);

            var idMethod = new IdNode(context, context.ID().GetText());
            node.Children.Add(idMethod);

            node.Children.AddRange(from x in context.formal() select Visit(x));

            var typeReturn = new TypeNode(context.TYPE().Symbol.Line, context.TYPE().Symbol.Column, context.TYPE().GetText());
            node.Children.Add(typeReturn);

            node.Children.Add(Visit(context.expression()));
            return node;
        }

        public override ASTNode VisitProperty([NotNull] CoolParser.PropertyContext context)
        {
            var node = new AttributeNode(context);
            node.Children.Add(Visit(context.formal()));
            node.Children.Add(Visit(context.expression()));
            return node;
        }

        public override ASTNode VisitFormal([NotNull] CoolParser.FormalContext context)
        {
            var node = new FormalNode(context);

            var idNode = new IdNode(context.ID().Symbol.Line,
                            context.ID().Symbol.Column, context.ID().GetText());
            var typeNode = new TypeNode(context.TYPE().Symbol.Line,
                            context.TYPE().Symbol.Column, context.TYPE().GetText());

            node.Children.Add(idNode);
            node.Children.Add(typeNode);

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

        public override ASTNode VisitDispatchExplicit([NotNull] CoolParser.DispatchExplicitContext context)
        {
            var node = new DispatchExplicitNode(context);
            node.Children.Add(Visit(context.expression(0)));

            var typeSuperClass = context.TYPE() == null ? TypeNode.NULL : new TypeNode(context.TYPE().Symbol.Line,
                        context.TYPE().Symbol.Column, context.TYPE().GetText());
            node.Children.Add(typeSuperClass);

            var idNode = new IdNode(context.ID().Symbol.Line, context.ID().Symbol.Column, context.ID().GetText());
            node.Children.Add(idNode);

            node.Children.AddRange(from x in context.expression().Skip(1) select Visit(x));
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

        public override ASTNode VisitBlock([NotNull] CoolParser.BlockContext context)
        {
            var node = new BlockNode(context);
            node.Children = context.expression().Select(x => Visit(x)).ToList();
            node.Expressions = node.Children.Select(x => x as ExpressionNode).ToList();
            return node;
        }

        public override ASTNode VisitDispatchImplicit([NotNull] CoolParser.DispatchImplicitContext context)
        {
            var node = new DispatchImplicitNode(context);
            node.Children.Add(new IdNode(context, context.ID().GetText()));
            node.Children.AddRange(from x in context.expression() select Visit(x));
            return node;
        }

        public override ASTNode VisitWhile([NotNull] CoolParser.WhileContext context)
        {
            var node = new WhileNode(context);
            node.Children.Add(Visit(context.expression(0))); // CONDITION
            node.Children.Add(Visit(context.expression(1))); // BODY
            return node;
        }

        public override ASTNode VisitId([NotNull] CoolParser.IdContext context)
        {
            return new IdNode(context, context.ID().GetText());
        }

        public override ASTNode VisitBoolNot([NotNull] CoolParser.BoolNotContext context)
        {
            var node = new NotNode(context);
            node.Children.Add(Visit(context.expression()));
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
            var nodeId = new IdNode(context.ID().Symbol.Line, context.ID().Symbol.Column, context.ID().GetText());
            node.Children.Add(nodeId);
            node.Children.Add(Visit(context.expression()));
            return node;
        }

        public override ASTNode VisitNew([NotNull] CoolParser.NewContext context)
        {
            var node = new NewNode(context);
            node.Children.Add(new TypeNode(context.TYPE().Symbol.Line,
                    context.TYPE().Symbol.Column, context.TYPE().GetText()));
            return node;
        }

        public override ASTNode VisitLetIn([NotNull] CoolParser.LetInContext context)
        {
            var node = new LetNode(context);
            node.Children.AddRange(from x in context.property() select Visit(x));
            node.Children.Add(Visit(context.expression()));
            return node;
        }

        public override ASTNode VisitIf([NotNull] CoolParser.IfContext context)
        {
            var node = new IfNode(context);
            node.Children.Add(Visit(context.expression(0)));    //  if expression
            node.Children.Add(Visit(context.expression(1)));    //then expression
            node.Children.Add(Visit(context.expression(2)));    //else expression
            return node;
        }

        public override ASTNode VisitString([NotNull] CoolParser.StringContext context)
        {
            return new StringNode(context.STRING().Symbol.Line, context.STRING().Symbol.Column, context.STRING().GetText());
        }

        public override ASTNode VisitBoolean([NotNull] CoolParser.BooleanContext context)
        {
            return new BoolNode(context, context.value.Text);
        }

        public override ASTNode VisitCase([NotNull] CoolParser.CaseContext context)
        {
            var node = new CaseNode(context);
            node.Children.Add(Visit(context.expression(0)));

            List<FormalNode> formals = context.formal().Select(x => Visit(x) as FormalNode).ToList();
            List<ExpressionNode> expressions = new List<ExpressionNode>();

            for (int i = 1; i <= formals.Count; ++i)
                expressions.Add(Visit(context.expression(i)) as ExpressionNode);

            for (int i = 0; i < formals.Count; ++i)
            {
                node.Children.Add(formals[i]);
                node.Children.Add(expressions[i]);
                node.Branches.Add((formals[i], expressions[i]));
            }

            return node;
        }

        public override ASTNode VisitNegative([NotNull] CoolParser.NegativeContext context)
        {
            var node = new NegNode(context);
            node.Children.Add(Visit(context.expression()));
            return node;
        }
    }
}
