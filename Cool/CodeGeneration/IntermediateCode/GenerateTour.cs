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
            VariableManager.CurrentClass = node.TypeClass.Text;
            IntermediateCode.DefineClass(VariableManager.CurrentClass);

            List<AttributeNode> attributes = new List<AttributeNode>();
            List<MethodNode> methods = new List<MethodNode>();

            foreach (var f in node.FeatureNodes)
                if (f is AttributeNode)
                    attributes.Add((AttributeNode)f);
                else
                {
                    methods.Add((MethodNode)f);
                    IntermediateCode.DefineMethod(VariableManager.CurrentClass, ((MethodNode)f).Id.Text);
                }


            foreach (var method in methods)
            {
                LabelLine label_function = IntermediateCode.GetMethodLabel(node.TypeClass.Text, method.Id.Text);
                IntermediateCode.AddCodeLine(label_function);
                method.Accept(this);
            }


            //begin constructor function

            int self = VariableManager.VariableCounter = 0;
            IntermediateCode.AddCodeLine(new LabelLine(VariableManager.CurrentClass, "constructor"));
            IntermediateCode.AddCodeLine(new ParamLine(self));
            VariableManager.IncrementVariableCounter();

            //calling first the parent constructor method
            if (VariableManager.CurrentClass != "Object")
            {
                IntermediateCode.AddCodeLine(new PushParamLine(self));
                LabelLine label = new LabelLine(node.TypeInherit.Text, "constructor");
                IntermediateCode.AddCodeLine(new CallLabelLine(label));
                IntermediateCode.AddCodeLine(new PopParamLine(4));
            }


            foreach (var attr in attributes)
            {
                IntermediateCode.DefineAttribute(node.TypeClass.Text, attr.Formal.Id.Text);
                VariableManager.PushVariableCounter();
                attr.Accept(this);
                VariableManager.PopVariableCounter();
                IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(self, VariableManager.VariableCounter, IntermediateCode.GetAttributeOffset(node.TypeClass.Text, attr.Formal.Id.Text)));
            }
            
            IntermediateCode.AddCodeLine(new ReturnLine(-1));

            VTableLine vt = IntermediateCode.GetVirtualTable(VariableManager.CurrentClass);

            IntermediateCode.AddCodeLine(vt);
            IntermediateCode.AddCodeLine(new HeadLine(VariableManager.CurrentClass, (3 + attributes.Count) * 4, vt));
        }

        public void Visit(AttributeNode node)
        {
            node.AssignExp.Accept(this);
        }

        public void Visit(MethodNode node)
        {

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
                int offset = IntermediateCode.GetAttributeOffset(VariableManager.CurrentClass, node.ID.Text);
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
            Console.WriteLine("------");
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
                IntermediateCode.AddCodeLine(new AssignmentMemoryToVariableLine(t, 0, IntermediateCode.GetAttributeOffset(VariableManager.CurrentClass, node.Text)));
            }

            //if (variable_link.ContainsKey(node.Text))
            //    IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(result_variable, variable_link[node.Text]));
            //else
            //{
            //    IntermediateCode.AddCodeLine(new AssignmentMemoryToVariableLine(result_variable, variable_link["class"], IntermediateCode.GetAttributeOffset(current_class.TypeClass.Text, node.Text)));
            //}
        }
        

        public void Visit(ComparisonOperation node)
        {
            BinaryOperationVisit(node);
        }

        public void Visit(DispatchExplicitNode node)
        {
            List<int> parameters = new List<int>();
            string cclass = node.IdType.Text;
            string method = node.IdMethod.Text;

            node.Expression.Accept(this);

            VariableManager.PushVariableCounter();

            int t = VariableManager.IncrementVariableCounter();
            int function_address = VariableManager.IncrementVariableCounter();
            int offset = IntermediateCode.GetMethodOffset(cclass, method);

            foreach (var p in node.Arguments)
            {
                VariableManager.IncrementVariableCounter();
                VariableManager.PushVariableCounter();
                parameters.Add(VariableManager.VariableCounter);
                p.Accept(this);
                VariableManager.PopVariableCounter();
            }

            VariableManager.PopVariableCounter();


            IntermediateCode.AddCodeLine(new AssignmentMemoryToVariableLine(t, VariableManager.PeekVariableCounter(), 2 * 4));
            IntermediateCode.AddCodeLine(new AssignmentMemoryToVariableLine(function_address, t, offset));

            foreach (var p in parameters)
            {
                IntermediateCode.AddCodeLine(new PushParamLine(p));
            }

            IntermediateCode.AddCodeLine(new CallAddressLine(function_address));
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
        }

        public void Visit(StringNode node)
        {
            IntermediateCode.AddCodeLine(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), node.Text));
        }

        public void Visit(LetNode node)
        {
            foreach (var attr in node.Initialization)
            {
                VariableManager.IncrementVariableCounter();
                VariableManager.PushVariable(attr.Formal.Id.Text);
                VariableManager.PushVariableCounter();
                attr.Accept(this);
                //IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), VariableManager.VariableCounter));
                VariableManager.PopVariableCounter();
            }

            node.ExpressionBody.Accept(this);

            foreach (var attr in node.Initialization)
            {
                VariableManager.PopVariable(attr.Formal.Id.Text);
            }
        }

        public void Visit(NewNode node)
        {
            int size = IntermediateCode.GetSizeClass(node.TypeId.Text);
            IntermediateCode.AddCodeLine(new AllocateLine(VariableManager.PeekVariableCounter(), size));
            IntermediateCode.AddCodeLine(new PushParamLine(VariableManager.PeekVariableCounter()));
            IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine(node.TypeId.Text, "constructor")));

            //IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), VariableManager.VariableCounter));

            //throw new NotImplementedException();
        }

        public void Visit(IsVoidNode node)
        {
            UnaryOperationVisit(node);
        }

        public void Visit(NegNode node)
        {
            UnaryOperationVisit(node);
        }


        public void Visit(NotNode node)
        {
            UnaryOperationVisit(node);
        }

        void UnaryOperationVisit(UnaryOperationNode node)
        {
            VariableManager.PushVariableCounter();

            VariableManager.IncrementVariableCounter();
            int t1 = VariableManager.VariableCounter;
            VariableManager.PushVariableCounter();
            node.Operand.Accept(this);

            VariableManager.PopVariableCounter();

            IntermediateCode.AddCodeLine(new ArithmeticLine(VariableManager.PeekVariableCounter(), t1, -1, node.Symbol));
        }

        public void Visit(IfNode node)
        {
            throw new NotImplementedException();
        }
        

        public void Visit(WhileNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(CaseNode node)
        {

            throw new NotImplementedException();
        }

        public void Visit(VoidNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(SelfNode node)
        {
            throw new NotImplementedException();
        }
        public void Visit(FormalNode node)
        {
            throw new NotImplementedException();
        }

    }
}
