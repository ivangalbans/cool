using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.AST;
using Cool.Semantics;

namespace Cool.AST
{
    class StringTour : IVisitor
    {
        public string Repr;

        public static string ToString(ProgramNode node)
        {
            var st = new StringTour();
            st.Visit(node);
            return st.Repr;
        }

        public StringTour()
        {
            Repr = "";
        }

        public void Visit(ArithmeticOperation node)
        {
            string t = $"Arithmetic Node (Line: {node.Line}, Column: {node.Column})\n";
            var st = new StringTour();
            node.LeftOperand.Accept(st);
            t += st.Repr;
            t += $"\n";
            t += $"{node.Symbol} \n";
            st = new StringTour();
            node.RightOperand.Accept(st);
            t += st.Repr;
            t += $"\n";
            Repr += t.Replace("\n", "\n| ");
        }

        public void Visit(AssignmentNode node)
        {
            string t = $"Assignment Node (Line: {node.Line}, Column: {node.Column}),\n";
            var st = new StringTour();
            node.ID.Accept(st);
            t += st.Repr;
            t += $" <-\n";
            st = new StringTour();
            node.ExpressionRight.Accept(st);
            t += st.Repr;
            t += $"\n";
            Repr += t.Replace("\n", "\n| ");
        }

        public void Visit(AttributeNode node)
        {
            string t = $"Attribute Node (Line: {node.Line}, Column: {node.Column}) ";

            var st = new StringTour();
            node.Formal.Accept(st);
            t += st.Repr;

            t += "\n| ";

            st = new StringTour();
            node.AssignExp.Accept(st);
            t += st.Repr;

            Repr += t;
        }

        public void Visit(BoolNode node)
        {
            Repr += node.Value.ToString();
        }

        public void Visit(CaseNode node)
        {
            string t = $"Case Node (Line: {node.Line}, Column: {node.Column}),\n";
            t += "Evaluation:\n| ";

            var st = new StringTour();
            node.ExpressionCase.Accept(st);
            t += st.Repr;

            t += $"\n";

            for (int i = 1; i <= node.Branches.Count; ++i)
            {
                var (f, e) = node.Branches[i - 1];
                t += $"Condition {i}\n| ";

                st = new StringTour();
                f.Accept(st);
                t += st.Repr;

                t += $"\n";

                t += $"Body {i}\n| ";

                st = new StringTour();
                e.Accept(st);
                t += st.Repr;

                t += $"\n";
            }
            
            Repr += t.Replace("\n", "\n| ");
        }

        public void Visit(ClassNode node)
        {
            string t = $"Class Node (Line: {node.Line}, Column: {node.Column}) class {node.TypeClass} inherits {node.TypeInherit} \n";
            foreach (var f in node.FeatureNodes)
            {
                var st = new StringTour();
                f.Accept(st);
                t += st.Repr;
                t += "\n";
            }
            Repr += t.Replace("\n", "\n| ");
        }

        public void Visit(ComparisonOperation node)
        {
            string t = $"Comparison Node (Line: {node.Line}, Column: {node.Column})\n";
            var st = new StringTour();
            node.LeftOperand.Accept(st);
            t += st.Repr;
            t += $"\n";
            t += $"{node.Symbol} \n";
            st = new StringTour();
            node.RightOperand.Accept(st);
            t += st.Repr;
            t += $"\n";
            Repr += t.Replace("\n", "\n| ");
        }

        public void Visit(DispatchExplicitNode node)
        {
            string t = $"Dispatch Explicit Node (Line: {node.Line}, Column: {node.Column})\n";

            t += "Object:\n| ";

            var st = new StringTour();
            node.Expression.Accept(st);
            t += st.Repr;
            t += $"\n";

            t += "@Type:\n| ";
            t += $"{node.IdType.Text}\n";
            t += "Dispatch:\n| ";

            string t2 = $"{node.IdMethod.Text}(\n";
            foreach (var a in node.Arguments)
            {
                st = new StringTour();
                a.Accept(st);
                t2 += st.Repr + "\n";
            }
            t2 += ")\n";

            t += t2.Replace("\n", "\n| ");

            Repr = t.Replace("\n", "\n| ");
        }

        public void Visit(DispatchImplicitNode node)
        {
            string t = $"Dispatch Implicit Node (Line: {node.Line}, Column: {node.Column})\n";

            t += $"{node.IdMethod.Text}(\n";
            foreach (var a in node.Arguments)
            {
                var st = new StringTour();
                a.Accept(st);
                t += st.Repr + "\n";
            }
            t += ")\n";

            Repr = t;
        }

        public void Visit(EqualNode node)
        {
            string t = $"Comparison Node (Line: {node.Line}, Column: {node.Column})\n";
            var st = new StringTour();
            node.LeftOperand.Accept(st);
            t += st.Repr;
            t += $"\n";
            t += $"{node.Symbol} \n";
            st = new StringTour();
            node.RightOperand.Accept(st);
            t += st.Repr;
            t += $"\n";
            Repr += t.Replace("\n", "\n| ");
        }

        public void Visit(FormalNode node)
        {
            Repr += $"{node.Id.Text} : {node.Type.Text}";
        }

        public void Visit(IdentifierNode node)
        {
            Repr += node.Text;
        }

        public void Visit(IfNode node)
        {
            string t = $"If Node (Line: {node.Line}, Column: {node.Column}),\n";
            t += "Condition:\n| ";

            var st = new StringTour();
            node.Condition.Accept(st);
            t += st.Repr;

            t += $"\n";

            t += "Then:\n| ";

            st = new StringTour();
            node.Body.Accept(st);
            t += st.Repr;

            t += $"\n";

            t += "Else:\n| ";

            st = new StringTour();
            node.ElseBody.Accept(st);
            t += st.Repr;

            t += $"\n";

            Repr = t.Replace("\n", "\n| ");
        }

        public void Visit(IntNode node)
        {
            Repr += node.Value;
        }

        public void Visit(IsVoidNode node)
        {
            string t = $"Is Void Node (Line: {node.Line}, Column: {node.Column})\n";
            t += $"{node.Symbol} \n";

            var st = new StringTour();
            node.Operand.Accept(st);
            t += st.Repr + "\n";

            Repr = t.Replace("\n", "\n| ");
        }

        public void Visit(LetNode node)
        {
            string t = $"Let Node (Line: {node.Line}, Column: {node.Column}),\n";
            t += "Initialization:\n| ";

            StringTour st;
            foreach (var a in node.Initialization)
            {
                st = new StringTour();
                a.Accept(st);
                t += st.Repr + "\n";
            }
            t += $"Body:\n| ";

            st = new StringTour();
            node.ExpressionBody.Accept(st);
            t += st.Repr + "\n";

            Repr += t.Replace("\n", "\n| ");
        }

        public void Visit(MethodNode node)
        {
            string t = $"Method Node (Line: {node.Line}, Column: {node.Column}) ";
            t += $"{node.Id.Text} (";
            StringTour st;
            foreach (var a in node.Arguments)
            {
                st = new StringTour();
                a.Accept(st);
                t += st.Repr + ", ";
            }

            t += $") : {node.TypeReturn.Text}\n";

            st = new StringTour();
            node.Body.Accept(st);
            t += st.Repr;

            Repr += t.Replace("\n", "\n| ");
        }

        public void Visit(NegNode node)
        {
            string t = $"Neg Node (Line: {node.Line}, Column: {node.Column})\n";
            t += $"{node.Symbol} \n";

            var st = new StringTour();
            node.Operand.Accept(st);
            t += st.Repr;

            t += $"\n";

            Repr = t.Replace("\n", "\n| ");
        }

        public void Visit(NewNode node)
        {
            Repr += $"New Node (Line: {node.Line}, Column: {node.Column}) {node.TypeId.Text}";
        }

        public void Visit(NotNode node)
        {
            string t = $"Not Node (Line: {node.Line}, Column: {node.Column})\n";
            t += $"{node.Symbol} \n";

            var st = new StringTour();
            node.Operand.Accept(st);
            t += st.Repr;

            t += $"\n";

            Repr = t.Replace("\n", "\n| ");
        }

        public void Visit(ProgramNode node)
        {
            string t = $"Program Node, Number of Classes: {node.Classes.Count}:\n";

            foreach (var c in node.Classes)
            {
                var st = new StringTour();
                c.Accept(st);
                t += st.Repr + "\n";
            }

            Repr = t.Replace("\n", "\n| ");
        }

        public void Visit(SelfNode node)
        {
            Repr += "SELF";
        }

        public void Visit(SequenceNode node)
        {
            string t = "";
            foreach (var e in node.Sequence)
            {
                var st = new StringTour();
                e.Accept(st);
                t += st.Repr + "\n";
            }
            Repr = t;
        }

        public void Visit(StringNode node)
        {
            Repr += node.Text;
        }

        public void Visit(VoidNode node)
        {
            Repr += $"Void {node.GetStaticType}";
        }

        public void Visit(WhileNode node)
        {
            string t = $"While Node (Line: {node.Line}, Column: {node.Column}),\n";
            t += "Condition:\n| ";

            var st = new StringTour();
            node.Condition.Accept(st);
            t += st.Repr + "\n";

            t += "Then:\n| ";

            st = new StringTour();
            node.Body.Accept(st);
            t += st.Repr + "\n";

            Repr += t.Replace("\n", "\n| ");
        }
    }
}
