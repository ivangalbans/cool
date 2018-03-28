using Antlr4.Runtime.Misc;
using Cool.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cool.Parsing
{
    public class ASTBuilder : CoolBaseVisitor<ASTNode>
    {
        public override ASTNode VisitProgram([NotNull] CoolParser.ProgramContext context)
        {
            return new ProgramNode(context)
            {
                Classes = context.classDefine().Select(x => Visit(x) as ClassNode).ToList()
            };
        }

        public override ASTNode VisitClassDefine([NotNull] CoolParser.ClassDefineContext context)
        {
            var node = new ClassNode(context);
            var typeClass = new TypeNode(context.TYPE(0).Symbol.Line, context.TYPE(0).Symbol.Column, context.TYPE(0).GetText());
            var typeInherit = context.TYPE(1) == null ? TypeNode.OBJECT : new TypeNode(context.TYPE(1).Symbol.Line,
                                                        context.TYPE(1).Symbol.Column, context.TYPE(1).GetText());

            node.TypeClass = typeClass;
            node.TypeInherit = typeInherit;
            node.FeatureNodes = (from x in context.feature() select Visit(x) as FeatureNode).ToList();

            return node;
        }

        public override ASTNode VisitMethod([NotNull] CoolParser.MethodContext context)
        {
            var node = new MethodNode(context);

            var idMethod = new IdNode(context, context.ID().GetText());
            node.Id = idMethod;

            node.Arguments = (from x in context.formal() select Visit(x) as FormalNode).ToList();

            var typeReturn = new TypeNode(context.TYPE().Symbol.Line, context.TYPE().Symbol.Column, context.TYPE().GetText());
            node.TypeReturn = typeReturn;

            node.Body = Visit(context.expression()) as ExpressionNode;
            return node;
        }

        public override ASTNode VisitProperty([NotNull] CoolParser.PropertyContext context)
        {
            var node = new AttributeNode(context)
            {
                Formal = Visit(context.formal()) as FormalNode
            };

            if (context.expression() != null)
                node.AssignExp = Visit(context.expression()) as ExpressionNode;
            else if (node.Formal.Type.Text == "Int")
                node.AssignExp = new IntNode(context, "0");
            else if (node.Formal.Type.Text == "Bool")
                node.AssignExp = new BoolNode(context, "false");
            else if (node.Formal.Type.Text == "String")
                node.AssignExp = new StringNode(context, "");
            else
                node.AssignExp = new VoidNode(node.Formal.Type.Text);

            return node;
        }

        public override ASTNode VisitFormal([NotNull] CoolParser.FormalContext context)
        {
            return new FormalNode(context)
            {
                Id = new IdentifierNode(context, context.ID().GetText()),
                Type = new TypeNode(context.TYPE().Symbol.Line, context.TYPE().Symbol.Column, context.TYPE().GetText())
            };
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

            
            node.LeftOperand = Visit(context.expression(0)) as ExpressionNode;      // LEFT EXPRESSION
            node.RightOperand = Visit(context.expression(1)) as ExpressionNode;     //RIGHT EXPRESSION
            return node;
        }

        public override ASTNode VisitDispatchExplicit([NotNull] CoolParser.DispatchExplicitContext context)
        {
            var node = new DispatchExplicitNode(context)
            {
                Expression = Visit(context.expression(0)) as ExpressionNode
            };

            var typeSuperClass = context.TYPE() == null ? new TypeNode(node.Expression.Line, node.Expression.Column, node.Expression.StaticType.Text) :
                                                          new TypeNode(context.TYPE().Symbol.Line, context.TYPE().Symbol.Column, context.TYPE().GetText());
            node.IdType = typeSuperClass;

            var idNode = new IdNode(context.ID().Symbol.Line, context.ID().Symbol.Column, context.ID().GetText());
            node.IdMethod = idNode;

            node.Arguments = (from x in context.expression().Skip(1) select Visit(x) as ExpressionNode).ToList();
            return node;
        }

        public override ASTNode VisitInt([NotNull] CoolParser.IntContext context)
        {
            return new IntNode(context, context.INT().GetText());
        }

        public override ASTNode VisitIsvoid([NotNull] CoolParser.IsvoidContext context)
        {
            return new IsVoidNode(context)
            {
                Operand = Visit(context.expression()) as ExpressionNode
            };
        }

        public override ASTNode VisitBlock([NotNull] CoolParser.BlockContext context)
        {
            return new SequenceNode(context)
            {
                Sequence = context.expression().Select(x => Visit(x) as ExpressionNode).ToList()
            };
        }

        public override ASTNode VisitDispatchImplicit([NotNull] CoolParser.DispatchImplicitContext context)
        {
            return new DispatchImplicitNode(context)
            {
                IdMethod = new IdNode(context, context.ID().GetText()),
                Arguments = (from x in context.expression() select Visit(x) as ExpressionNode).ToList()
            };
        }

        public override ASTNode VisitWhile([NotNull] CoolParser.WhileContext context)
        {
            return new WhileNode(context)
            {
                Condition = Visit(context.expression(0)) as ExpressionNode,     // CONDITION
                Body = Visit(context.expression(1)) as ExpressionNode           // BODY
            };
        }

        public override ASTNode VisitId([NotNull] CoolParser.IdContext context)
        {
            if (context.ID().GetText() == "self")
                return new SelfNode(context);
            return new IdentifierNode(context, context.ID().GetText());
        }

        public override ASTNode VisitBoolNot([NotNull] CoolParser.BoolNotContext context)
        {
            return new NotNode(context)
            {
                Operand = Visit(context.expression()) as ExpressionNode
            };
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

            node.LeftOperand = Visit(context.expression(0)) as ExpressionNode;      // LEFT EXPRESSION
            node.RightOperand = Visit(context.expression(1)) as ExpressionNode;     //RIGHT EXPRESSION

            return node;
        }

        public override ASTNode VisitAssignment([NotNull] CoolParser.AssignmentContext context)
        {
            return new AssignmentNode(context)
            {
                ID = new IdentifierNode(context, context.ID().GetText()),
                ExpressionRight = Visit(context.expression()) as ExpressionNode
            };
        }

        public override ASTNode VisitNew([NotNull] CoolParser.NewContext context)
        {
            return new NewNode(context)
            {
                TypeId = new TypeNode(context.TYPE().Symbol.Line, context.TYPE().Symbol.Column, context.TYPE().GetText())
            };
        }

        public override ASTNode VisitLetIn([NotNull] CoolParser.LetInContext context)
        {
            var node = new LetNode(context)
            {
                Initialization = (from x in context.property() select Visit(x) as AttributeNode).ToList(),
                ExpressionBody = Visit(context.expression()) as ExpressionNode
            };
            return node;
        }

        public override ASTNode VisitIf([NotNull] CoolParser.IfContext context)
        {
            return new IfNode(context)
            {
                Condition   = Visit(context.expression(0)) as ExpressionNode,   //  if expression
                Body        = Visit(context.expression(1)) as ExpressionNode,   //then expression
                ElseBody    = Visit(context.expression(2)) as ExpressionNode    //else expression
            };
        }

        public override ASTNode VisitString([NotNull] CoolParser.StringContext context)
        {
            return new StringNode(context, context.STRING().GetText());
        }

        public override ASTNode VisitBoolean([NotNull] CoolParser.BooleanContext context)
        {
            return new BoolNode(context, context.value.Text);
        }

        public override ASTNode VisitCase([NotNull] CoolParser.CaseContext context)
        {
            var node = new CaseNode(context)
            {
                ExpressionCase = Visit(context.expression(0)) as ExpressionNode
            };

            var formals = context.formal().Select(x => Visit(x)).ToList();
            var expressions = context.expression().Skip(1).Select(x => Visit(x)).ToList();
            for (int i = 0; i < formals.Count; ++i)
                node.Branches.Add((formals[i] as FormalNode, expressions[i] as ExpressionNode));

            return node;
        }

        public override ASTNode VisitNegative([NotNull] CoolParser.NegativeContext context)
        {
            return new NegNode(context)
            {
                Operand = Visit(context.expression()) as ExpressionNode
            };
        }

        public override ASTNode VisitParentheses([NotNull] CoolParser.ParenthesesContext context)
        {
            return Visit(context.expression());
        }
        
    }
}
