using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.AST;
using Cool.Semantics;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;

namespace Cool.CodeGeneration.IntermediateCode
{
    class OptimizationTour : IVisitor
    {
        IScope Scope;

        public ProgramNode Optimize(ProgramNode node, IScope scope)
        {
            Scope = scope;

            node.Accept(this);

            return node;
        }

        public void Visit(ArithmeticOperation node)
        {
            node.LeftOperand.Accept(this);
            node.RightOperand.Accept(this);

            if(node.LeftOperand.Attributes.ContainsKey("integer_constant_value") &&
               node.RightOperand.Attributes.ContainsKey("integer_constant_value"))
            {
                int t1 = node.LeftOperand.Attributes["integer_constant_value"];
                int t2 = node.RightOperand.Attributes["integer_constant_value"];
                switch (node.Symbol)
                {
                    case "+":
                        node.Attributes["integer_constant_value"] = t1 + t2;
                        break;
                    case "-":
                        node.Attributes["integer_constant_value"] = t1 - t2;
                        break;
                    case "*":
                        node.Attributes["integer_constant_value"] = t1 * t2;
                        break;
                    case "/":
                        if(t2 != 0)
                            node.Attributes["integer_constant_value"] = t1 / t2;
                        break;
                    default:
                        break;
                }
            }

        }

        public void Visit(AssignmentNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(AttributeNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(BoolNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(CaseNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(ClassNode node)
        {
            foreach (var f in node.FeatureNodes)
            {
                f.Accept(this);
            }


        }

        public void Visit(ComparisonOperation node)
        {
           // throw new NotImplementedException();
        }

        public void Visit(DispatchExplicitNode node)
        {
           // throw new NotImplementedException();
        }

        public void Visit(DispatchImplicitNode node)
        {
            foreach (var a in node.Arguments)
                a.Accept(this);
        }

        public void Visit(EqualNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(FormalNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(IdentifierNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(IfNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(IntNode node)
        {
            node.Attributes["integer_constant_value"] = node.Value;
        }

        public void Visit(IsVoidNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(LetNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(MethodNode node)
        {
            node.Body.Accept(this);
        }

        public void Visit(NegNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(NewNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(NotNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(ProgramNode node)
        {
            foreach (var c in node.Classes)
                c.Accept(this);
        }

        public void Visit(SelfNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(SequenceNode node)
        {
            foreach (var s in node.Sequence)
                s.Accept(this);
        }

        public void Visit(StringNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(VoidNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(WhileNode node)
        {
            //throw new NotImplementedException();
        }
    }
}
