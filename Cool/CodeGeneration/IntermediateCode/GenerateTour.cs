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
    class GenerateTour : IVisitor
    {
        IIntermediateCode IntermediateCode;
        IScope Scope;

        Dictionary<string,int> variable_link;
        ClassNode current_class;
        int variable_counter;
        int jump_label_counter;
        int result_variable;

        public IIntermediateCode GetIntermediateCode(ProgramNode node, IScope scope)
        {
            variable_counter = 0;
            jump_label_counter = 0;
            Scope = scope;
            IntermediateCode = new IntermediateCode(scope);
            node.Accept(this);
            return IntermediateCode;
        }

        public void Visit(ProgramNode node)
        {
            List<ClassNode> sorted = new List<ClassNode>();
            sorted.AddRange(node.Classes);
            sorted.Sort((x,y) => (Scope.GetType(x.TypeClass.Text) <= Scope.GetType(y.TypeClass.Text) ? 1 : -1));
            
            foreach (var c in sorted)
            {
                c.Accept(this);
            }
        }

        public void Visit(ClassNode node)
        {
            current_class = node;
            string cclass = current_class.TypeClass.Text;
            IntermediateCode.DefineClass(cclass);

            foreach (var f in node.FeatureNodes)
            {
                f.Accept(this);
            }

            IntermediateCode.AddCodeLine(new LabelLine(cclass, "constructor"));
            
            IntermediateCode.AddCodeLine(new ParamLine(variable_counter));

            if (cclass != "Object")
            {
                IntermediateCode.AddCodeLine(new PushParamLine(variable_counter));
                LabelLine label = new LabelLine(current_class.TypeInherit.Text, "constructor");
                IntermediateCode.AddCodeLine(new CallLine(label));
                IntermediateCode.AddCodeLine(new PopParamLine(4));
            }

            foreach (var attr in IntermediateCode.GetAttributeTable(cclass))
            {
                IntermediateCode.AddCodeLine(new PushParamLine(variable_counter));
                LabelLine label = new LabelLine(cclass + ".constructor", "set_" + attr);
                IntermediateCode.AddCodeLine(new CallLine(label));
                IntermediateCode.AddCodeLine(new PopParamLine(4));
            }
            IntermediateCode.AddCodeLine(new ReturnLine(-1));
            ++variable_counter;

            IntermediateCode.AddCodeLine(IntermediateCode.GetVirtualTable(cclass));
        }

        public void Visit(MethodNode node)
        {
            IntermediateCode.DefineMethod(current_class.TypeClass.Text, node.Id.Text);

            LabelLine label_function = IntermediateCode.GetMethodLabel(current_class.TypeClass.Text, node.Id.Text);
            IntermediateCode.AddCodeLine(label_function);

            IntermediateCode.AddCodeLine(new ParamLine(variable_counter));
            int this_var = variable_counter;
            variable_counter++;

            variable_link = new Dictionary<string, int>
            {
                ["class"] = this_var
            };

            foreach (var formal in node.Arguments)
            {
                IntermediateCode.AddCodeLine(new ParamLine(variable_counter));
                variable_link[formal.Id.Text] = variable_counter;
                variable_counter++;
            }

            int t = result_variable = variable_counter;
            node.Body.Accept(this);
            IntermediateCode.AddCodeLine(new ReturnLine(t));
        }

        public void Visit(AttributeNode node)
        {
            IntermediateCode.DefineAttribute(current_class.TypeClass.Text, node.Formal.Id.Text);
            LabelLine label_function = new LabelLine(current_class.TypeClass.Text + ".constructor", "set_" + node.Formal.Id.Text);
            IntermediateCode.AddCodeLine(label_function);

            IntermediateCode.AddCodeLine(new ParamLine(variable_counter));
            int this_var = variable_counter;

            int t1 = result_variable = ++variable_counter;
            node.AssignExp.Accept(this);

            IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(this_var, t1, IntermediateCode.GetAttributeOffset(current_class.TypeClass.Text, node.Formal.Id.Text)));
            IntermediateCode.AddCodeLine(new ReturnLine(-1));
        }

        public void Visit(IntNode node)
        {
            IntermediateCode.AddCodeLine(new AssignmentConstantToVariableLine(result_variable, node.Value));
        }

        public void Visit(BoolNode node)
        {
            IntermediateCode.AddCodeLine(new AssignmentConstantToVariableLine(result_variable, node.Value ? 1 : 0));
        }

        public void Visit(ArithmeticOperation node)
        {
            int t = result_variable;

            int t1 = result_variable = ++variable_counter;
            node.LeftOperand.Accept(this);

            int t2 = result_variable = ++variable_counter;
            node.RightOperand.Accept(this);

            IntermediateCode.AddCodeLine(new ArithmeticLine(t, t1, t2, node.Symbol));
        }

        public void Visit(AssignmentNode node)
        {
            int t = result_variable;
            if (variable_link.ContainsKey(node.ID.Text))
            {
                result_variable = variable_link[node.ID.Text];
                node.ExpressionRight.Accept(this);
                IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(t, variable_link[node.ID.Text]));
            }
            else
            {
                int offset = IntermediateCode.GetAttributeOffset(current_class.TypeClass.Text, node.ID.Text);
                int t1 = result_variable = ++variable_counter;
                node.ExpressionRight.Accept(this);
                IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(variable_link["class"], t1, offset));
            }
        }

        public void Visit(SequenceNode node)
        {
            int t = result_variable;
            foreach (var s in node.Sequence)
            {
                result_variable = t;
                s.Accept(this);
            }
        }

        public void Visit(CaseNode node)
        {
            throw new NotImplementedException();
        }
        

        public void Visit(ComparisonOperation node)
        {
            throw new NotImplementedException();
        }

        public void Visit(DispatchExplicitNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(DispatchImplicitNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(EqualNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(FormalNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IdentifierNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IfNode node)
        {
            throw new NotImplementedException();
        }
        
        public void Visit(IsVoidNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(LetNode node)
        {
            throw new NotImplementedException();
        }

        

        public void Visit(NegNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(NewNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(NotNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(SelfNode node)
        {
            throw new NotImplementedException();
        }
        
        public void Visit(StringNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(VoidNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(WhileNode node)
        {
            throw new NotImplementedException();
        }
    }
}
