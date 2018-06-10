using System;
using System.Collections.Generic;
using Cool.AST;
using Cool.Semantics;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;

namespace Cool.CodeGeneration.IntermediateCode
{
    class GenerateTour : IVisitor
    {
        IntermediateCode IntermediateCode;
        IScope Scope;
        ClassNode current_class;
        VariableManager VariableManager;
        
        public IIntermediateCode GetIntermediateCode(ProgramNode node, IScope scope)
        {
            Scope = scope;
            IntermediateCode = new IntermediateCode(scope);
            VariableManager = new VariableManager();

            node.Accept(this);
            return IntermediateCode;
        }

        public void Visit(ProgramNode node)
        {
            List<ClassNode> sorted = new List<ClassNode>();
            sorted.AddRange(node.Classes);
            sorted.Sort((x, y) => (Scope.GetType(x.TypeClass.Text) <= Scope.GetType(y.TypeClass.Text) ? 1 : -1));

            foreach (var c in sorted)
                c.Accept(this);
        }

        public void Visit(ClassNode node)
        {
            current_class = node;
            string cclass = current_class.TypeClass.Text;
            IntermediateCode.DefineClass(cclass);

            List<AttributeNode> attributes = new List<AttributeNode>();
            List<MethodNode> methods = new List<MethodNode>();

            foreach (var f in node.FeatureNodes)
                if (f is AttributeNode)
                    attributes.Add((AttributeNode)f);
                else
                    methods.Add((MethodNode)f);


            foreach (var method in methods)
            {
                method.Accept(this);
            }


            //begin constructor function

            int self = VariableManager.VariableCounter = 0;
            IntermediateCode.AddCodeLine(new LabelLine(cclass, "constructor"));
            IntermediateCode.AddCodeLine(new ParamLine(self));
            VariableManager.IncrementVariableCounter();

            //calling first the parent constructor method
            if (cclass != "Object")
            {
                IntermediateCode.AddCodeLine(new PushParamLine(self));
                LabelLine label = new LabelLine(current_class.TypeInherit.Text, "constructor");
                IntermediateCode.AddCodeLine(new CallLine(label));
                IntermediateCode.AddCodeLine(new PopParamLine(4));
            }


            foreach (var attr in attributes)
            {
                IntermediateCode.DefineAttribute(current_class.TypeClass.Text, attr.Formal.Id.Text);
                VariableManager.PushVariableCounter();
                attr.Accept(this);
                VariableManager.PopVariableCounter();
                IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(self, VariableManager.VariableCounter, IntermediateCode.GetAttributeOffset(current_class.TypeClass.Text, attr.Formal.Id.Text)));
            }
            
            IntermediateCode.AddCodeLine(new ReturnLine(-1));

            VTableLine vt = IntermediateCode.GetVirtualTable(cclass);

            IntermediateCode.AddCodeLine(vt);
            IntermediateCode.AddCodeLine(new HeadLine(cclass, (3 + attributes.Count) * 4, vt));
        }

        public void Visit(AttributeNode node)
        {
            node.AssignExp.Accept(this);



            //IntermediateCode.DefineAttribute(current_class.TypeClass.Text, node.Formal.Id.Text);
            //LabelLine label_function = new LabelLine(current_class.TypeClass.Text + ".constructor", "set_" + node.Formal.Id.Text);
            //IntermediateCode.AddCodeLine(label_function);

            //IntermediateCode.AddCodeLine(new ParamLine(variable_counter));
            //int this_var = variable_counter;

            //int t1 = result_variable = ++variable_counter;
            //node.AssignExp.Accept(this);

            //IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(this_var, t1, IntermediateCode.GetAttributeOffset(current_class.TypeClass.Text, node.Formal.Id.Text)));
            //IntermediateCode.AddCodeLine(new ReturnLine(-1));
        }

        public void Visit(MethodNode node)
        {
            IntermediateCode.DefineMethod(current_class.TypeClass.Text, node.Id.Text);

            LabelLine label_function = IntermediateCode.GetMethodLabel(current_class.TypeClass.Text, node.Id.Text);
            Console.WriteLine(label_function);
            IntermediateCode.AddCodeLine(label_function);

            int self = VariableManager.VariableCounter = 0;
            IntermediateCode.AddCodeLine(new ParamLine(self));

            VariableManager.IncrementVariableCounter();

            foreach (var formal in node.Arguments)
            {
                IntermediateCode.AddCodeLine(new ParamLine(VariableManager.VariableCounter));
                VariableManager.PushVariable(formal.Id.Text);
                VariableManager.IncrementVariableCounter();
            }

            VariableManager.PushVariableCounter();
            node.Body.Accept(this);
            IntermediateCode.AddCodeLine(new ReturnLine(VariableManager.PeekVariableCounter()));
            VariableManager.PopVariableCounter();

            foreach (var formal in node.Arguments)
            {
                VariableManager.PopVariable(formal.Id.Text);
            }
        }

        public void Visit(IntNode node)
        {
            IntermediateCode.AddCodeLine(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), node.Value));
        }

        public void Visit(BoolNode node)
        {
            IntermediateCode.AddCodeLine(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), node.Value ? 1 : 0));
        }

        public void Visit(ArithmeticOperation node)
        {
            BinaryOperationVisit(node);
        }

        public void Visit(AssignmentNode node)
        {

            node.ExpressionRight.Accept(this);

            int t = VariableManager.GetVariable(node.ID.Text);
            if (t != -1)
            {
                IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
            }
            else
            {
                int offset = IntermediateCode.GetAttributeOffset(current_class.TypeClass.Text, node.ID.Text);
                IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(0, VariableManager.PeekVariableCounter(), offset));
            }

            //int t = result_variable;
            //if (variable_link.ContainsKey(node.ID.Text))
            //{
            //    result_variable = variable_link[node.ID.Text];
            //    node.ExpressionRight.Accept(this);
            //    IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(t, variable_link[node.ID.Text]));
            //}
            //else
            //{
            //    int offset = IntermediateCode.GetAttributeOffset(current_class.TypeClass.Text, node.ID.Text);
            //    int t1 = result_variable = ++variable_counter;
            //    node.ExpressionRight.Accept(this);
            //    IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(variable_link["class"], t1, offset));
            //}
        }

        public void Visit(SequenceNode node)
        {
            foreach (var s in node.Sequence)
            {
                s.Accept(this);
            }

            //int t = result_variable;
            //foreach (var s in node.Sequence)
            //{
            //    result_variable = t;
            //    s.Accept(this);
            //}
        }

        public void Visit(IdentifierNode node)
        {
            int t = VariableManager.GetVariable(node.Text);
            if (t != -1)
            {
                IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
            }
            else
            {
                IntermediateCode.AddCodeLine(new AssignmentMemoryToVariableLine(t, 0, IntermediateCode.GetAttributeOffset(current_class.TypeClass.Text, node.Text)));
            }

            //if (variable_link.ContainsKey(node.Text))
            //    IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(result_variable, variable_link[node.Text]));
            //else
            //{
            //    IntermediateCode.AddCodeLine(new AssignmentMemoryToVariableLine(result_variable, variable_link["class"], IntermediateCode.GetAttributeOffset(current_class.TypeClass.Text, node.Text)));
            //}
        }

        public void Visit(CaseNode node)
        {
            throw new NotImplementedException();
        }
        

        public void Visit(ComparisonOperation node)
        {
            BinaryOperationVisit(node);
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
            BinaryOperationVisit(node);
        }

        void BinaryOperationVisit(BinaryOperationNode node)
        {
            //throw new NotImplementedException();

            VariableManager.PushVariableCounter();

            VariableManager.IncrementVariableCounter();
            int t1 = VariableManager.VariableCounter;
            VariableManager.PushVariableCounter();
            node.LeftOperand.Accept(this);
            VariableManager.PopVariableCounter();

            VariableManager.IncrementVariableCounter();
            int t2 = VariableManager.VariableCounter;
            VariableManager.PushVariableCounter();
            node.RightOperand.Accept(this);
            VariableManager.PopVariableCounter();

            VariableManager.PopVariableCounter();
            IntermediateCode.AddCodeLine(new ArithmeticLine(VariableManager.PeekVariableCounter(), t1, t2, node.Symbol));


            //int t = result_variable;

            //int t1 = result_variable = ++variable_counter;
            //node.LeftOperand.Accept(this);

            //int t2 = result_variable = ++variable_counter;
            //node.RightOperand.Accept(this);

            //IntermediateCode.AddCodeLine(new ArithmeticLine(t, t1, t2, node.Symbol));
        }

        public void Visit(StringNode node)
        {
            throw new NotImplementedException();
            //IntermediateCode.AddCodeLine(new AssignmentStringToVariableLine(result_variable, node.Text));
        }

        public void Visit(LetNode node)
        {
            
            throw new NotImplementedException();
        }

        public void Visit(FormalNode node)
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
