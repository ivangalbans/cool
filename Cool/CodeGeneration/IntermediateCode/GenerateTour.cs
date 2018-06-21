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
        VirtualTable VirtualTable;
        IScope Scope;
        VariableManager VariableManager;
        bool note_object_return_type = false;
        
        public IntermediateCode GetIntermediateCode(ProgramNode node, IScope scope)
        {
            Scope = scope;
            IntermediateCode = new IntermediateCode(scope);
            VariableManager = new VariableManager();
            VirtualTable = new VirtualTable(scope);

            VariableManager.PushVariableCounter();
            InitCode();
            VariableManager.PopVariableCounter();

            node.Accept(this);

            VariableManager.PushVariableCounter();
            StartFunctionCode();
            VariableManager.PopVariableCounter();

            return IntermediateCode;
        }

        public void Visit(ProgramNode node)
        {
            List<ClassNode> sorted = new List<ClassNode>();
            sorted.AddRange(node.Classes);
            sorted.Sort((x, y) => (Scope.GetType(x.TypeClass.Text) <= Scope.GetType(y.TypeClass.Text) ? 1 : -1));

            foreach (var c in sorted)
            {
                VirtualTable.DefineClass(c.TypeClass.Text);

                List<AttributeNode> attributes = new List<AttributeNode>();
                List<MethodNode> methods = new List<MethodNode>();

                foreach (var f in c.FeatureNodes)
                    if (f is AttributeNode)
                    {
                        attributes.Add((AttributeNode)f);
                    }
                    else
                    {
                        methods.Add((MethodNode)f);
                    }

                foreach (var method in methods)
                {
                    List<string> params_type = new List<string>();
                    foreach (var x in method.Arguments)
                        params_type.Add(x.Type.Text);

                    VirtualTable.DefineMethod(c.TypeClass.Text, method.Id.Text, params_type);
                }

                foreach (var attr in attributes)
                    VirtualTable.DefineAttribute(c.TypeClass.Text, attr.Formal.Id.Text);
            }

            foreach (var c in sorted)
                c.Accept(this);
        }

        public void Visit(CaseNode node)
        {
            string static_type = node.ExpressionCase.StaticType.Text;

            int expr = VariableManager.IncrementVariableCounter();

            VariableManager.PushVariableCounter();
            node.ExpressionCase.Accept(this);
            VariableManager.PopVariableCounter();

            if (static_type == "String" ||
                static_type == "Int" ||
                static_type == "Bool")
            {
                //int index = node.BranchSelected
                //string v = node.Branches[index].Formal.Id.Text;
                int index = node.Branches.FindIndex((x) => x.Formal.Type.Text == static_type);
                string v = node.Branches[index].Formal.Id.Text;

                VariableManager.PushVariable(v);// v <- expr

                int t = VariableManager.IncrementVariableCounter();
                VariableManager.PushVariableCounter();

                node.Branches[index].Expression.Accept(this);

                VariableManager.PopVariableCounter();

                VariableManager.PopVariable(v);

                IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
            }
            else if (static_type == "Object")
            {

            }
            else
            {
                string tag = IntermediateCode.CountLines().ToString();

                List<(FormalNode Formal, ExpressionNode Expression)> sorted = new List<(FormalNode Formal, ExpressionNode Expression)>();
                sorted.AddRange(node.Branches);
                sorted.Sort((x, y) => (Scope.GetType(x.Formal.Type.Text) <= Scope.GetType(y.Formal.Type.Text) ? -1 : 1));



                //VariableManager.IncrementVariableCounter();
                for (int i = 0; i < sorted.Count; ++i)
                {
                    VariableManager.PushVariable(sorted[i].Formal.Id.Text);
                    VariableManager.PushVariableCounter();


                    int t = VariableManager.IncrementVariableCounter();


                    IntermediateCode.AddCodeLine(new LabelLine("_case", tag + "." + i));
                    string type = sorted[i].Formal.Type.Text;

                    VariableManager.PushVariableCounter();
                    sorted[i].Expression.Accept(this);
                    VariableManager.PopVariableCounter();



                    IntermediateCode.AddCodeLine(new GotoJumpLine(new LabelLine("_endcase", tag)));


                    VariableManager.PopVariableCounter();
                    VariableManager.PopVariable(sorted[i].Formal.Id.Text);
                }

                IntermediateCode.AddCodeLine(new LabelLine("_case", tag + "." + sorted.Count));
                IntermediateCode.AddCodeLine(new GotoJumpLine(new LabelLine("_caseselectionexception")));

                IntermediateCode.AddCodeLine(new LabelLine("_endcase", tag));
            }


        }

        void InitCode()
        {
            int self = VariableManager.PeekVariableCounter();
            (string, string) label;
            List<string> obj = new List<string> { "abort", "type_name", "copy" };

            IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("start")));

            IntermediateCode.AddCodeLine(new LabelLine("Object", "constructor"));
            IntermediateCode.AddCodeLine(new ParamLine(self));
            foreach (var f in VirtualTable.Object)
            {
                label = VirtualTable.GetDefinition("Object", f);
                IntermediateCode.AddCodeLine(new CommentLine("set method: " + label.Item1 + "." + label.Item2));
                IntermediateCode.AddCodeLine(new AssignmentLabelToMemoryLine(self, new LabelLine(label.Item1, label.Item2), VirtualTable.GetOffset("Object", f)));
            }
            IntermediateCode.AddCodeLine(new ReturnLine());

            IntermediateCode.AddCodeLine(new LabelLine("IO", "constructor"));

            IntermediateCode.AddCodeLine(new ParamLine(self));
            IntermediateCode.AddCodeLine(new PushParamLine(self));
            IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("Object", "constructor")));
            IntermediateCode.AddCodeLine(new PopParamLine(1));

            foreach (var f in VirtualTable.IO)
            {
                label = VirtualTable.GetDefinition("IO", f);
                IntermediateCode.AddCodeLine(new CommentLine("set method: " + label.Item1 + "." + label.Item2));
                IntermediateCode.AddCodeLine(new AssignmentLabelToMemoryLine(self, new LabelLine(label.Item1, label.Item2), VirtualTable.GetOffset("IO", f)));
            }
            IntermediateCode.AddCodeLine(new ReturnLine());

            IntermediateCode.AddCodeLine(new InheritLine("IO", "Object"));
            IntermediateCode.AddCodeLine(new InheritLine("Int", "Object"));
            IntermediateCode.AddCodeLine(new InheritLine("Bool", "Object"));
            IntermediateCode.AddCodeLine(new InheritLine("String", "Object"));

            //Int wrapper for runtime check typing
            IntermediateCode.AddCodeLine(new LabelLine("_wrapper", "Int"));
            IntermediateCode.AddCodeLine(new ParamLine(self));
            IntermediateCode.AddCodeLine(new AllocateLine(self + 1, VirtualTable.GetSizeClass("Int") + 1));
            IntermediateCode.AddCodeLine(new PushParamLine(self + 1));
            IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("Object", "constructor")));
            IntermediateCode.AddCodeLine(new PopParamLine(1));
            IntermediateCode.AddCodeLine(new AssignmentStringToMemoryLine(self + 1, "Int", 0));
            IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(self + 1, self, VirtualTable.GetSizeClass("Int")));
            IntermediateCode.AddCodeLine(new ReturnLine(self + 1));

            //Bool wrapper for runtime check typing
            IntermediateCode.AddCodeLine(new LabelLine("_wrapper", "Bool"));
            IntermediateCode.AddCodeLine(new ParamLine(self));
            IntermediateCode.AddCodeLine(new AllocateLine(self + 1, VirtualTable.GetSizeClass("Bool") + 1));
            IntermediateCode.AddCodeLine(new PushParamLine(self + 1));
            IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("Object", "constructor")));
            IntermediateCode.AddCodeLine(new PopParamLine(1));
            IntermediateCode.AddCodeLine(new AssignmentStringToMemoryLine(self + 1, "Bool", 0));
            IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(self + 1, self, VirtualTable.GetSizeClass("Bool")));
            IntermediateCode.AddCodeLine(new ReturnLine(self + 1));

            //String wrapper for runtime check typing
            IntermediateCode.AddCodeLine(new LabelLine("_wrapper", "String"));
            IntermediateCode.AddCodeLine(new ParamLine(self));
            IntermediateCode.AddCodeLine(new AllocateLine(self + 1, VirtualTable.GetSizeClass("String") + 1));
            IntermediateCode.AddCodeLine(new PushParamLine(self + 1));
            IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("Object", "constructor")));
            IntermediateCode.AddCodeLine(new PopParamLine(1));
            IntermediateCode.AddCodeLine(new AssignmentStringToMemoryLine(self + 1, "String", 0));
            IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(self + 1, self, VirtualTable.GetSizeClass("String")));
            IntermediateCode.AddCodeLine(new ReturnLine(self + 1));
        }

        void StartFunctionCode()
        {
            IntermediateCode.AddCodeLine(new LabelLine("start"));
            New("Main");
            IntermediateCode.AddCodeLine(new PushParamLine(VariableManager.PeekVariableCounter()));
            IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("Main", "main")));
            IntermediateCode.AddCodeLine(new PopParamLine(1));
            //IntermediateCode.AddCodeLine(new PushParamLine(VariableManager.PeekVariableCounter()));
            //IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("Object", "abort")));
            //IntermediateCode.AddCodeLine(new PopParamLine(1));
        }

        public void Visit(ClassNode node)
        {
            string cclass;
            cclass = VariableManager.CurrentClass = node.TypeClass.Text;
            IntermediateCode.AddCodeLine(new InheritLine(node.TypeClass.Text, Scope.GetType(node.TypeClass.Text).Parent.Text));

            //VirtualTable.DefineClass(VariableManager.CurrentClass);
            int self = VariableManager.VariableCounter = 0;
            VariableManager.IncrementVariableCounter();
            VariableManager.PushVariableCounter();

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

            IntermediateCode.AddCodeLine(new LabelLine(VariableManager.CurrentClass, "constructor"));
            IntermediateCode.AddCodeLine(new ParamLine(self));

            //calling first the parent constructor method
            if (VariableManager.CurrentClass != "Object")
            {
                IntermediateCode.AddCodeLine(new PushParamLine(self));
                LabelLine label = new LabelLine(node.TypeInherit.Text, "constructor");
                IntermediateCode.AddCodeLine(new CallLabelLine(label));
                IntermediateCode.AddCodeLine(new PopParamLine(1));
            }


            foreach (var method in methods)
            {
                (string, string) label = VirtualTable.GetDefinition(node.TypeClass.Text, method.Id.Text);
                IntermediateCode.AddCodeLine(new CommentLine("set method: " + label.Item1 + "." + label.Item2));
                IntermediateCode.AddCodeLine(new AssignmentLabelToMemoryLine(self, new LabelLine(label.Item1, label.Item2), VirtualTable.GetOffset(node.TypeClass.Text, method.Id.Text)));
                //IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(self, VariableManager.VariableCounter, IntermediateCode.GetVirtualTableOffset(node.TypeClass.Text, attr.Formal.Id.Text)));
            }


            foreach (var attr in attributes)
            {
                VariableManager.PushVariableCounter();
                attr.Accept(this);
                VariableManager.PopVariableCounter();
                IntermediateCode.AddCodeLine(new CommentLine("set attribute: " + attr.Formal.Id.Text));
                IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(self, VariableManager.PeekVariableCounter(), VirtualTable.GetOffset(node.TypeClass.Text, attr.Formal.Id.Text)));
            }
            

            IntermediateCode.AddCodeLine(new CommentLine("set class name: " + node.TypeClass.Text));
            IntermediateCode.AddCodeLine(new AssignmentStringToMemoryLine(0, node.TypeClass.Text, 0));
            IntermediateCode.AddCodeLine(new CommentLine("set class size: " + VirtualTable.GetSizeClass(node.TypeClass.Text) + " words"));
            IntermediateCode.AddCodeLine(new AssignmentConstantToMemoryLine(0, VirtualTable.GetSizeClass(node.TypeClass.Text), 1));
            IntermediateCode.AddCodeLine(new CommentLine("set class generation label"));
            IntermediateCode.AddCodeLine(new AssignmentLabelToMemoryLine(0, new LabelLine("_class", node.TypeClass.Text), 2));

            IntermediateCode.AddCodeLine(new ReturnLine(-1));

            VariableManager.PopVariableCounter();
        }

        public void Visit(AttributeNode node)
        {
            node.AssignExp.Accept(this);

            if ((node.AssignExp.StaticType.Text == "Int" ||
                node.AssignExp.StaticType.Text == "Bool" ||
                node.AssignExp.StaticType.Text == "String") &&
                node.Formal.Type.Text == "Object")
            {
                IntermediateCode.AddCodeLine(new PushParamLine(VariableManager.PeekVariableCounter()));
                IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("_wrapper", node.AssignExp.StaticType.Text), VariableManager.PeekVariableCounter()));
                IntermediateCode.AddCodeLine(new PopParamLine(1));
            }
        }

        public void Visit(MethodNode node)
        {
            IntermediateCode.AddCodeLine(new LabelLine(VariableManager.CurrentClass, node.Id.Text));

            if (node.TypeReturn.Text == "Object")
                note_object_return_type = true;

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

            if (!note_object_return_type)
                IntermediateCode.AddCodeLine(new ReturnLine(VariableManager.PeekVariableCounter()));
            else
                IntermediateCode.AddCodeLine(new SpecialObjectReturn(VariableManager.PeekVariableCounter()));


            VariableManager.PopVariableCounter();

            foreach (var formal in node.Arguments)
            {
                VariableManager.PopVariable(formal.Id.Text);
            }

            note_object_return_type = false;
        }

        public void Visit(IntNode node)
        {
            IntermediateCode.AddCodeLine(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), node.Value));
            if (note_object_return_type)
                IntermediateCode.AddCodeLine(new ReturnTypeLine("Int"));
        }

        public void Visit(BoolNode node)
        {
            IntermediateCode.AddCodeLine(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), node.Value ? 1 : 0));
            if (note_object_return_type)
                IntermediateCode.AddCodeLine(new ReturnTypeLine("Bool"));
        }

        public void Visit(ArithmeticOperation node)
        {
            BinaryOperationVisit(node);
            if (note_object_return_type)
                IntermediateCode.AddCodeLine(new ReturnTypeLine("Int"));
        }

        public void Visit(AssignmentNode node)
        {

            node.ExpressionRight.Accept(this);

            if ((node.ExpressionRight.StaticType.Text == "Int" ||
                node.ExpressionRight.StaticType.Text == "Bool" ||
                node.ExpressionRight.StaticType.Text == "String") &&
                node.StaticType.Text == "Object")
            {
                IntermediateCode.AddCodeLine(new PushParamLine(VariableManager.PeekVariableCounter()));
                IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("_wrapper", node.ExpressionRight.StaticType.Text), VariableManager.PeekVariableCounter()));
                IntermediateCode.AddCodeLine(new PopParamLine(1));
            }

            int t = VariableManager.GetVariable(node.ID.Text);
            if (t != -1)
            {
                //IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
                IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(t, VariableManager.PeekVariableCounter()));
            }
            else
            {
                int offset = VirtualTable.GetOffset(VariableManager.CurrentClass, node.ID.Text);
                IntermediateCode.AddCodeLine(new AssignmentVariableToMemoryLine(0, VariableManager.PeekVariableCounter(), offset));
            }

            
        }

        public void Visit(SequenceNode node)
        {
            foreach (var s in node.Sequence)
            {
                s.Accept(this);
            }
        }

        public void Visit(IdentifierNode node)
        {
            int t = VariableManager.GetVariable(node.Text);
            if (t != -1)
            {
                IntermediateCode.AddCodeLine(new CommentLine("get veriable: " + node.Text));
                IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
            }
            else
            {
                IntermediateCode.AddCodeLine(new CommentLine("get attribute: " + VariableManager.CurrentClass + "." + node.Text));
                IntermediateCode.AddCodeLine(new AssignmentMemoryToVariableLine(VariableManager.PeekVariableCounter(), 0, VirtualTable.GetOffset(VariableManager.CurrentClass, node.Text)));
            }

            if (note_object_return_type)
            {
                if (node.StaticType.Text == "Int" ||
                    node.StaticType.Text == "Bool" ||
                    node.StaticType.Text == "String")
                    IntermediateCode.AddCodeLine(new ReturnTypeLine(node.StaticType.Text));
            }
        }
        

        public void Visit(ComparisonOperation node)
        {
            BinaryOperationVisit(node);
            if (note_object_return_type)
                IntermediateCode.AddCodeLine(new ReturnTypeLine("Bool"));
        }

        public void Visit(DispatchExplicitNode node)
        {
            string cclass = node.IdType.Text;
            node.Expression.Accept(this);
            DispatchVisit(node, cclass);
        }

        public void Visit(DispatchImplicitNode node)
        {
            string cclass = VariableManager.CurrentClass;
            IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), 0));
            DispatchVisit(node, cclass);
        }

        void DispatchVisit(DispatchNode node, string cclass)
        {
            string method = node.IdMethod.Text;

            if (method == "abort" && (cclass == "Int" || cclass == "String" || cclass == "Bool"))
            {
                IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("Object","abort")));
                return;
            }

            if (method == "type_name")
            {
                if (cclass == "Int" || cclass == "Bool" || cclass == "String")
                {
                    IntermediateCode.AddCodeLine(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), cclass));
                    return;
                }
            }

            //important for define
            if (method == "copy")
            {
                //IntermediateCode.AddCodeLine(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), cclass));
            }

            VariableManager.PushVariableCounter();

            //int t = VariableManager.IncrementVariableCounter();
            int function_address = VariableManager.IncrementVariableCounter();
            //int offset = IntermediateCode.GetMethodOffset(cclass, method);
            int offset = VirtualTable.GetOffset(cclass, method);

            List<int> parameters = new List<int>();
            List<string> parameters_types = VirtualTable.GetParametersTypes(cclass, method);
            for (int i = 0; i < node.Arguments.Count; ++i)
            {
                VariableManager.IncrementVariableCounter();
                VariableManager.PushVariableCounter();
                parameters.Add(VariableManager.VariableCounter);
                node.Arguments[i].Accept(this);

                if (parameters_types[i] == "Object" && (
                    node.Arguments[i].StaticType.Text == "Int" ||
                    node.Arguments[i].StaticType.Text == "Bool" ||
                    node.Arguments[i].StaticType.Text == "String"))
                {
                    IntermediateCode.AddCodeLine(new PushParamLine(VariableManager.PeekVariableCounter()));
                    IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine("_wrapper", node.Arguments[i].StaticType.Text), VariableManager.PeekVariableCounter()));
                    IntermediateCode.AddCodeLine(new PopParamLine(1));
                }

                VariableManager.PopVariableCounter();
            }

            VariableManager.PopVariableCounter();

            if (cclass != "String")
            {
                IntermediateCode.AddCodeLine(new CommentLine("get method: " + cclass + "." + method));
                IntermediateCode.AddCodeLine(new AssignmentMemoryToVariableLine(function_address, VariableManager.PeekVariableCounter(), offset));
            }

            IntermediateCode.AddCodeLine(new PushParamLine(VariableManager.PeekVariableCounter()));
            
            foreach (var p in parameters)
            {
                IntermediateCode.AddCodeLine(new PushParamLine(p));
            }

            if (cclass != "String")
            {
                IntermediateCode.AddCodeLine(new CallAddressLine(function_address, VariableManager.PeekVariableCounter()));
            }
            else
            {
                IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine(cclass, method), VariableManager.PeekVariableCounter()));
            }

            if (note_object_return_type)
            {
                if (node.StaticType.Text == "Int" ||
                    node.StaticType.Text == "Bool" ||
                    node.StaticType.Text == "String")
                    IntermediateCode.AddCodeLine(new ReturnTypeLine(node.StaticType.Text));
            }

            IntermediateCode.AddCodeLine(new PopParamLine(parameters.Count+1));
        }

        public void Visit(EqualNode node)
        {
            BinaryOperationVisit(node);
            if (note_object_return_type)
                IntermediateCode.AddCodeLine(new ReturnTypeLine("Bool"));
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

            if (node.LeftOperand.StaticType.Text == "String" && node.Symbol == "=")
            {
                IntermediateCode.AddCodeLine(new BinaryOperationLine(VariableManager.PeekVariableCounter(), t1, t2, "=:="));
                return;
            }

            IntermediateCode.AddCodeLine(new BinaryOperationLine(VariableManager.PeekVariableCounter(), t1, t2, node.Symbol));
        }

        public void Visit(StringNode node)
        {
            IntermediateCode.AddCodeLine(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), node.Text));
            if (note_object_return_type)
                IntermediateCode.AddCodeLine(new ReturnTypeLine("String"));
        }

        public void Visit(LetNode node)
        {
            VariableManager.PushVariableCounter();

            foreach (var attr in node.Initialization)
            {
                VariableManager.IncrementVariableCounter();
                VariableManager.PushVariable(attr.Formal.Id.Text);
                VariableManager.PushVariableCounter();
                attr.Accept(this);
                //IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), VariableManager.VariableCounter));
                VariableManager.PopVariableCounter();
            }
            VariableManager.IncrementVariableCounter();

            node.ExpressionBody.Accept(this);

            foreach (var attr in node.Initialization)
            {
                VariableManager.PopVariable(attr.Formal.Id.Text);
            }
            VariableManager.PopVariableCounter();

            if (note_object_return_type)
            {
                if (node.StaticType.Text == "Int" ||
                    node.StaticType.Text == "Bool" ||
                    node.StaticType.Text == "String")
                    IntermediateCode.AddCodeLine(new ReturnTypeLine(node.StaticType.Text));
            }
        }

        public void Visit(NewNode node)
        {
            if (node.TypeId.Text == "Int" ||
                node.TypeId.Text == "Bool" ||
                node.TypeId.Text == "String")
            {
                if (node.TypeId.Text == "Int" || node.TypeId.Text == "Bool")
                    IntermediateCode.AddCodeLine(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), 0));
                else
                    IntermediateCode.AddCodeLine(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), ""));

                if (note_object_return_type)
                {
                    IntermediateCode.AddCodeLine(new ReturnTypeLine(node.TypeId.Text));
                }
            }
            else
            {
                New(node.TypeId.Text);
            }


        }

        public void New(string cclass)
        {
            int size = VirtualTable.GetSizeClass(cclass);
            IntermediateCode.AddCodeLine(new AllocateLine(VariableManager.PeekVariableCounter(), size));
            IntermediateCode.AddCodeLine(new PushParamLine(VariableManager.PeekVariableCounter()));
            IntermediateCode.AddCodeLine(new CallLabelLine(new LabelLine(cclass, "constructor")));
            IntermediateCode.AddCodeLine(new PopParamLine(1));
        }

        public void Visit(IsVoidNode node)
        {
            //if special types non void;
            if (node.Operand.StaticType.Text == "Int" ||
               node.Operand.StaticType.Text == "String" ||
               node.Operand.StaticType.Text == "Bool")
                IntermediateCode.AddCodeLine(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), 0));
            else
                UnaryOperationVisit(node);

            if (note_object_return_type)
            {
                IntermediateCode.AddCodeLine(new ReturnTypeLine("Bool"));
            }
        }

        public void Visit(NegNode node)
        {
            UnaryOperationVisit(node);
            if (note_object_return_type)
            {
                IntermediateCode.AddCodeLine(new ReturnTypeLine("Int"));
            }
        }


        public void Visit(NotNode node)
        {
            UnaryOperationVisit(node);
            if (note_object_return_type)
            {
                IntermediateCode.AddCodeLine(new ReturnTypeLine("Bool"));
            }
        }

        void UnaryOperationVisit(UnaryOperationNode node)
        {
            VariableManager.PushVariableCounter();

            VariableManager.IncrementVariableCounter();
            int t1 = VariableManager.VariableCounter;
            VariableManager.PushVariableCounter();
            node.Operand.Accept(this);

            VariableManager.PopVariableCounter();

            IntermediateCode.AddCodeLine(new UnaryOperationLine(VariableManager.PeekVariableCounter(), t1, node.Symbol));
        }

        public void Visit(IfNode node)
        {
            string tag = IntermediateCode.CountLines().ToString();

            node.Condition.Accept(this);

            IntermediateCode.AddCodeLine(new ConditionalJumpLine(VariableManager.PeekVariableCounter(), new LabelLine("_else", tag)));

            node.Body.Accept(this);
            IntermediateCode.AddCodeLine(new GotoJumpLine(new LabelLine("_endif", tag)));

            IntermediateCode.AddCodeLine(new LabelLine("_else", tag));
            node.ElseBody.Accept(this);

            IntermediateCode.AddCodeLine(new LabelLine("_endif", tag));

        }


        public void Visit(WhileNode node)
        {
            string tag = IntermediateCode.CountLines().ToString();

            IntermediateCode.AddCodeLine(new LabelLine("_whilecondition", tag));

            node.Condition.Accept(this);

            IntermediateCode.AddCodeLine(new ConditionalJumpLine(VariableManager.PeekVariableCounter(), new LabelLine("_endwhile", tag)));

            node.Body.Accept(this);

            IntermediateCode.AddCodeLine(new GotoJumpLine(new LabelLine("_whilecondition", tag)));

            IntermediateCode.AddCodeLine(new LabelLine("_endwhile", tag));
        }


        public void Visit(VoidNode node)
        {
            IntermediateCode.AddCodeLine(new AssignmentNullToVariableLine(VariableManager.PeekVariableCounter()));
        }

        public void Visit(SelfNode node)
        {
            IntermediateCode.AddCodeLine(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(),0));
        }
        public void Visit(FormalNode node)
        {
            throw new NotImplementedException();
        }

    }
}
