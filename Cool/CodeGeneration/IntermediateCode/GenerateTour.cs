using System;
using System.Collections.Generic;
using Cool.AST;
using Cool.Semantics;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;

namespace Cool.CodeGeneration.IntermediateCode
{
    class GenerateTour : IVisitor
    {
        List<CodeLine> IntermediateCode;
        VirtualTable VirtualTable;
        IScope Scope;
        VariableManager VariableManager;
        bool note_object_return_type = false;
        
        public List<CodeLine> GetIntermediateCode(ProgramNode node, IScope scope)
        {
            Scope = scope;

            //node = (new OptimizationTour()).Optimize(node, scope);

            IntermediateCode = new List<CodeLine>();
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
                    VirtualTable.DefineAttribute(c.TypeClass.Text, attr.Formal.Id.Text, attr.Formal.Type.Text);
            }

            foreach (var c in sorted)
                c.Accept(this);
        }

        public void Visit(CaseNode node)
        {
            string static_type = node.ExpressionCase.StaticType.Text;

            int result = VariableManager.PeekVariableCounter();
            int expr = VariableManager.IncrementVariableCounter();

            VariableManager.PushVariableCounter();
            node.ExpressionCase.Accept(this);
            VariableManager.PopVariableCounter();

            //VariableManager.IncrementVariableCounter();

            if (static_type == "String" ||
                static_type == "Int" ||
                static_type == "Bool")
            {
                int index = node.Branches.FindIndex((x) => x.Formal.Type.Text == static_type);
                string v = node.Branches[index].Formal.Id.Text;

                VariableManager.PushVariable(v, node.Branches[index].Formal.Type.Text);// v <- expr

                int t = VariableManager.IncrementVariableCounter();
                VariableManager.PushVariableCounter();

                node.Branches[index].Expression.Accept(this);

                VariableManager.PopVariableCounter();

                VariableManager.PopVariable(v);

                IntermediateCode.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
            }
            else
            {
                string tag = IntermediateCode.Count.ToString();

                List<(FormalNode Formal, ExpressionNode Expression)> sorted = new List<(FormalNode Formal, ExpressionNode Expression)>();
                sorted.AddRange(node.Branches);
                sorted.Sort((x, y) => (Scope.GetType(x.Formal.Type.Text) <= Scope.GetType(y.Formal.Type.Text) ? -1 : 1));
                
                for (int i = 0; i < sorted.Count; ++i)
                {
                    //same that expr integer
                    VariableManager.PushVariable(sorted[i].Formal.Id.Text, sorted[i].Formal.Type.Text);

                    string branch_type = sorted[i].Formal.Type.Text;
                    VariableManager.PushVariableCounter();
                    VariableManager.IncrementVariableCounter();

                    IntermediateCode.Add(new LabelLine("_case", tag + "." + i));
                    IntermediateCode.Add(new AssignmentStringToVariableLine(VariableManager.VariableCounter, branch_type));
                    IntermediateCode.Add(new BinaryOperationLine(VariableManager.VariableCounter, expr, VariableManager.VariableCounter, "inherit"));
                    IntermediateCode.Add(new ConditionalJumpLine(VariableManager.VariableCounter, new LabelLine("_case", tag + "." + (i + 1))));


                    if ((branch_type == "Int" ||
                        branch_type == "Bool" ||
                        branch_type == "String"))
                    {
                        if (static_type == "Object")
                        {

                            IntermediateCode.Add(new AssignmentMemoryToVariableLine(expr, expr, VirtualTable.GetSizeClass(branch_type)));

                            VariableManager.PushVariableCounter();
                            sorted[i].Expression.Accept(this);
                            VariableManager.PopVariableCounter();

                            IntermediateCode.Add(new AssignmentVariableToVariableLine(result, VariableManager.PeekVariableCounter()));
                            IntermediateCode.Add(new GotoJumpLine(new LabelLine("_endcase", tag)));
                        }
                    }
                    else
                    {
                        VariableManager.PushVariableCounter();
                        sorted[i].Expression.Accept(this);
                        VariableManager.PopVariableCounter();

                        IntermediateCode.Add(new AssignmentVariableToVariableLine(result, VariableManager.PeekVariableCounter()));
                        IntermediateCode.Add(new GotoJumpLine(new LabelLine("_endcase", tag)));
                    }



                    VariableManager.PopVariableCounter();

                    VariableManager.PopVariable(sorted[i].Formal.Id.Text);
                }

                IntermediateCode.Add(new LabelLine("_case", tag + "." + sorted.Count));
                IntermediateCode.Add(new GotoJumpLine(new LabelLine("_caseselectionexception")));

                IntermediateCode.Add(new LabelLine("_endcase", tag));
            }
        }

        void InitCode()
        {
            int self = VariableManager.PeekVariableCounter();
            (string, string) label;
            List<string> obj = new List<string> { "abort", "type_name", "copy" };

            IntermediateCode.Add(new CallLabelLine(new LabelLine("start")));

            IntermediateCode.Add(new LabelLine("Object", "constructor"));
            IntermediateCode.Add(new ParamLine(self));
            foreach (var f in VirtualTable.Object)
            {
                label = VirtualTable.GetDefinition("Object", f);
                IntermediateCode.Add(new CommentLine("set method: " + label.Item1 + "." + label.Item2));
                IntermediateCode.Add(new AssignmentLabelToMemoryLine(self, new LabelLine(label.Item1, label.Item2), VirtualTable.GetOffset("Object", f)));
            }

            IntermediateCode.Add(new CommentLine("set class name: Object"));
            IntermediateCode.Add(new AssignmentStringToMemoryLine(0, "Object", 0));
            IntermediateCode.Add(new CommentLine("set class size: " + VirtualTable.GetSizeClass("Object") + " words"));
            IntermediateCode.Add(new AssignmentConstantToMemoryLine(0, VirtualTable.GetSizeClass("Object"), 1));
            //IntermediateCode.Add(new CommentLine("set class generation label"));
            //IntermediateCode.Add(new AssignmentLabelToMemoryLine(0, new LabelLine("_class", node.TypeClass.Text), 2));

            IntermediateCode.Add(new ReturnLine());



            IntermediateCode.Add(new LabelLine("IO", "constructor"));

            IntermediateCode.Add(new ParamLine(self));
            IntermediateCode.Add(new PushParamLine(self));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("Object", "constructor")));
            IntermediateCode.Add(new PopParamLine(1));

            foreach (var f in VirtualTable.IO)
            {
                label = VirtualTable.GetDefinition("IO", f);
                IntermediateCode.Add(new CommentLine("set method: " + label.Item1 + "." + label.Item2));
                IntermediateCode.Add(new AssignmentLabelToMemoryLine(self, new LabelLine(label.Item1, label.Item2), VirtualTable.GetOffset("IO", f)));
            }

            IntermediateCode.Add(new CommentLine("set class name: Object"));
            IntermediateCode.Add(new AssignmentStringToMemoryLine(0, "IO", 0));
            IntermediateCode.Add(new CommentLine("set class size: " + VirtualTable.GetSizeClass("IO") + " words"));
            IntermediateCode.Add(new AssignmentConstantToMemoryLine(0, VirtualTable.GetSizeClass("IO"), 1));
            IntermediateCode.Add(new CommentLine("set class generation label"));
            IntermediateCode.Add(new AssignmentLabelToMemoryLine(0, new LabelLine("_class", "IO"), 2));

            IntermediateCode.Add(new ReturnLine());


            IntermediateCode.Add(new InheritLine("IO", "Object"));
            IntermediateCode.Add(new InheritLine("Int", "Object"));
            IntermediateCode.Add(new InheritLine("Bool", "Object"));
            IntermediateCode.Add(new InheritLine("String", "Object"));

            //Int wrapper for runtime check typing
            IntermediateCode.Add(new LabelLine("_wrapper", "Int"));
            IntermediateCode.Add(new ParamLine(self));
            IntermediateCode.Add(new AllocateLine(self + 1, VirtualTable.GetSizeClass("Int") + 1));
            IntermediateCode.Add(new PushParamLine(self + 1));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("Object", "constructor")));
            IntermediateCode.Add(new PopParamLine(1));
            IntermediateCode.Add(new AssignmentStringToMemoryLine(self + 1, "Int", 0));
            IntermediateCode.Add(new AssignmentVariableToMemoryLine(self + 1, self, VirtualTable.GetSizeClass("Int")));
            IntermediateCode.Add(new AssignmentLabelToMemoryLine(self + 1, new LabelLine("_class", "Int"), 2));
            IntermediateCode.Add(new ReturnLine(self + 1));

            //Bool wrapper for runtime check typing
            IntermediateCode.Add(new LabelLine("_wrapper", "Bool"));
            IntermediateCode.Add(new ParamLine(self));
            IntermediateCode.Add(new AllocateLine(self + 1, VirtualTable.GetSizeClass("Bool") + 1));
            IntermediateCode.Add(new PushParamLine(self + 1));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("Object", "constructor")));
            IntermediateCode.Add(new PopParamLine(1));
            IntermediateCode.Add(new AssignmentStringToMemoryLine(self + 1, "Bool", 0));
            IntermediateCode.Add(new AssignmentVariableToMemoryLine(self + 1, self, VirtualTable.GetSizeClass("Bool")));
            IntermediateCode.Add(new AssignmentLabelToMemoryLine(self + 1, new LabelLine("_class", "Bool"), 2));
            IntermediateCode.Add(new ReturnLine(self + 1));

            //String wrapper for runtime check typing
            IntermediateCode.Add(new LabelLine("_wrapper", "String"));
            IntermediateCode.Add(new ParamLine(self));
            IntermediateCode.Add(new AllocateLine(self + 1, VirtualTable.GetSizeClass("String") + 1));
            IntermediateCode.Add(new PushParamLine(self + 1));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("Object", "constructor")));
            IntermediateCode.Add(new PopParamLine(1));
            IntermediateCode.Add(new AssignmentStringToMemoryLine(self + 1, "String", 0));
            IntermediateCode.Add(new AssignmentVariableToMemoryLine(self + 1, self, VirtualTable.GetSizeClass("String")));
            IntermediateCode.Add(new AssignmentLabelToMemoryLine(self + 1, new LabelLine("_class", "String"), 2));
            IntermediateCode.Add(new ReturnLine(self + 1));


            //abort, typename, copy
            IntermediateCode.Add(new LabelLine("Object", "abort"));
            IntermediateCode.Add(new GotoJumpLine(new LabelLine("_abort")));

            IntermediateCode.Add(new LabelLine("Object", "type_name"));
            IntermediateCode.Add(new ParamLine(0));
            IntermediateCode.Add(new AssignmentMemoryToVariableLine(0, 0, 0));
            IntermediateCode.Add(new ReturnLine(0));


            IntermediateCode.Add(new LabelLine("Object", "copy"));
            IntermediateCode.Add(new ParamLine(0));
            IntermediateCode.Add(new AssignmentMemoryToVariableLine(1, 0, 1));
            IntermediateCode.Add(new AssignmentConstantToVariableLine(2, 4));
            IntermediateCode.Add(new BinaryOperationLine(1, 1, 2, "*"));
            IntermediateCode.Add(new PushParamLine(0));
            IntermediateCode.Add(new PushParamLine(1));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("_copy"), 0));
            IntermediateCode.Add(new PopParamLine(2));

            IntermediateCode.Add(new ReturnLine(0));


            //io: in_string, out_string, in_int, out_int
            IntermediateCode.Add(new LabelLine("IO", "out_string"));
            IntermediateCode.Add(new ParamLine(0));
            IntermediateCode.Add(new ParamLine(1));
            IntermediateCode.Add(new PushParamLine(1));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("_out_string"), 0));
            IntermediateCode.Add(new PopParamLine(1));
            IntermediateCode.Add(new ReturnLine(0));

            IntermediateCode.Add(new LabelLine("IO", "out_int"));
            IntermediateCode.Add(new ParamLine(0));
            IntermediateCode.Add(new ParamLine(1));
            IntermediateCode.Add(new PushParamLine(1));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("_out_int"), 0));
            IntermediateCode.Add(new PopParamLine(1));
            IntermediateCode.Add(new ReturnLine(0));


            IntermediateCode.Add(new LabelLine("IO", "in_string"));
            IntermediateCode.Add(new ParamLine(0));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("_in_string"), 0));
            IntermediateCode.Add(new ReturnLine(0));


            IntermediateCode.Add(new LabelLine("IO", "in_int"));
            IntermediateCode.Add(new ParamLine(0));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("_in_int"), 0));
            IntermediateCode.Add(new ReturnLine(0));

            //string: substr, concat, length
            IntermediateCode.Add(new LabelLine("String", "length"));
            IntermediateCode.Add(new ParamLine(0));
            IntermediateCode.Add(new PushParamLine(0));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("_stringlength"), 0));
            IntermediateCode.Add(new PopParamLine(1));
            IntermediateCode.Add(new ReturnLine(0));


            IntermediateCode.Add(new LabelLine("String", "concat"));
            IntermediateCode.Add(new ParamLine(0));
            IntermediateCode.Add(new ParamLine(1));
            IntermediateCode.Add(new PushParamLine(0));
            IntermediateCode.Add(new PushParamLine(1));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("_stringconcat"), 0));
            IntermediateCode.Add(new PopParamLine(2));
            IntermediateCode.Add(new ReturnLine(0));


            IntermediateCode.Add(new LabelLine("String", "substr"));
            IntermediateCode.Add(new ParamLine(0));
            IntermediateCode.Add(new ParamLine(1));
            IntermediateCode.Add(new ParamLine(2));
            IntermediateCode.Add(new PushParamLine(0));
            IntermediateCode.Add(new PushParamLine(1));
            IntermediateCode.Add(new PushParamLine(2));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("_stringsubstr"), 0));
            IntermediateCode.Add(new PopParamLine(3));
            IntermediateCode.Add(new ReturnLine(0));
        }

        void StartFunctionCode()
        {
            IntermediateCode.Add(new LabelLine("start"));
            New("Main");
            IntermediateCode.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
            IntermediateCode.Add(new CallLabelLine(new LabelLine("Main", "main")));
            IntermediateCode.Add(new PopParamLine(1));
            //IntermediateCode.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
            //IntermediateCode.Add(new CallLabelLine(new LabelLine("Object", "abort")));
            //IntermediateCode.Add(new PopParamLine(1));
        }

        public void Visit(ClassNode node)
        {
            string cclass;
            cclass = VariableManager.CurrentClass = node.TypeClass.Text;
            IntermediateCode.Add(new InheritLine(node.TypeClass.Text, Scope.GetType(node.TypeClass.Text).Parent.Text));

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

            IntermediateCode.Add(new LabelLine(VariableManager.CurrentClass, "constructor"));
            IntermediateCode.Add(new ParamLine(self));

            //calling first the parent constructor method
            if (VariableManager.CurrentClass != "Object")
            {
                IntermediateCode.Add(new PushParamLine(self));
                LabelLine label = new LabelLine(node.TypeInherit.Text, "constructor");
                IntermediateCode.Add(new CallLabelLine(label));
                IntermediateCode.Add(new PopParamLine(1));
            }


            foreach (var method in methods)
            {
                (string, string) label = VirtualTable.GetDefinition(node.TypeClass.Text, method.Id.Text);
                IntermediateCode.Add(new CommentLine("set method: " + label.Item1 + "." + label.Item2));
                IntermediateCode.Add(new AssignmentLabelToMemoryLine(self, new LabelLine(label.Item1, label.Item2), VirtualTable.GetOffset(node.TypeClass.Text, method.Id.Text)));
                //IntermediateCode.Add(new AssignmentVariableToMemoryLine(self, VariableManager.VariableCounter, IntermediateCode.GetVirtualTableOffset(node.TypeClass.Text, attr.Formal.Id.Text)));
            }


            foreach (var attr in attributes)
            {
                VariableManager.PushVariableCounter();
                attr.Accept(this);
                VariableManager.PopVariableCounter();
                IntermediateCode.Add(new CommentLine("set attribute: " + attr.Formal.Id.Text));
                IntermediateCode.Add(new AssignmentVariableToMemoryLine(self, VariableManager.PeekVariableCounter(), VirtualTable.GetOffset(node.TypeClass.Text, attr.Formal.Id.Text)));
            }
            

            IntermediateCode.Add(new CommentLine("set class name: " + node.TypeClass.Text));
            IntermediateCode.Add(new AssignmentStringToMemoryLine(0, node.TypeClass.Text, 0));
            IntermediateCode.Add(new CommentLine("set class size: " + VirtualTable.GetSizeClass(node.TypeClass.Text) + " words"));
            IntermediateCode.Add(new AssignmentConstantToMemoryLine(0, VirtualTable.GetSizeClass(node.TypeClass.Text), 1));
            IntermediateCode.Add(new CommentLine("set class generation label"));
            IntermediateCode.Add(new AssignmentLabelToMemoryLine(0, new LabelLine("_class", node.TypeClass.Text), 2));

            IntermediateCode.Add(new ReturnLine(-1));

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
                IntermediateCode.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
                IntermediateCode.Add(new CallLabelLine(new LabelLine("_wrapper", node.AssignExp.StaticType.Text), VariableManager.PeekVariableCounter()));
                IntermediateCode.Add(new PopParamLine(1));
            }
        }

        public void Visit(MethodNode node)
        {
            IntermediateCode.Add(new LabelLine(VariableManager.CurrentClass, node.Id.Text));

            if (node.TypeReturn.Text == "Object")
                note_object_return_type = true;

            int self = VariableManager.VariableCounter = 0;
            IntermediateCode.Add(new ParamLine(self));

            VariableManager.IncrementVariableCounter();

            foreach (var formal in node.Arguments)
            {
                IntermediateCode.Add(new ParamLine(VariableManager.VariableCounter));
                VariableManager.PushVariable(formal.Id.Text, formal.Type.Text);
                VariableManager.IncrementVariableCounter();
            }

            VariableManager.PushVariableCounter();
            node.Body.Accept(this);

            if (!note_object_return_type)
                IntermediateCode.Add(new ReturnLine(VariableManager.PeekVariableCounter()));
            else
                IntermediateCode.Add(new SpecialObjectReturn(VariableManager.PeekVariableCounter()));


            VariableManager.PopVariableCounter();

            foreach (var formal in node.Arguments)
            {
                VariableManager.PopVariable(formal.Id.Text);
            }

            note_object_return_type = false;
        }

        public void Visit(IntNode node)
        {
            IntermediateCode.Add(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), node.Value));
            if (note_object_return_type)
                IntermediateCode.Add(new ReturnTypeLine("Int"));
        }

        public void Visit(BoolNode node)
        {
            IntermediateCode.Add(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), node.Value ? 1 : 0));
            if (note_object_return_type)
                IntermediateCode.Add(new ReturnTypeLine("Bool"));
        }

        public void Visit(ArithmeticOperation node)
        {
            BinaryOperationVisit(node);
            if (note_object_return_type)
                IntermediateCode.Add(new ReturnTypeLine("Int"));
        }

        public void Visit(AssignmentNode node)
        {

            node.ExpressionRight.Accept(this);
            var (t, type) = VariableManager.GetVariable(node.ID.Text);

            if (type == "")
                type = VirtualTable.GetAttributeType(VariableManager.CurrentClass, node.ID.Text);


            if ((node.ExpressionRight.StaticType.Text == "Int" ||
                node.ExpressionRight.StaticType.Text == "Bool" ||
                node.ExpressionRight.StaticType.Text == "String") &&
                type == "Object")
                //node.StaticType.Text == "Object")
            {
                IntermediateCode.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
                IntermediateCode.Add(new CallLabelLine(new LabelLine("_wrapper", node.ExpressionRight.StaticType.Text), VariableManager.PeekVariableCounter()));
                IntermediateCode.Add(new PopParamLine(1));
            }

            if (t != -1)
            {
                //IntermediateCode.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
                IntermediateCode.Add(new AssignmentVariableToVariableLine(t, VariableManager.PeekVariableCounter()));
            }
            else
            {
                int offset = VirtualTable.GetOffset(VariableManager.CurrentClass, node.ID.Text);
                IntermediateCode.Add(new AssignmentVariableToMemoryLine(0, VariableManager.PeekVariableCounter(), offset));
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
            var (t, type) = VariableManager.GetVariable(node.Text);
            if (t != -1)
            {
                IntermediateCode.Add(new CommentLine("get veriable: " + node.Text));
                IntermediateCode.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
            }
            else
            {
                IntermediateCode.Add(new CommentLine("get attribute: " + VariableManager.CurrentClass + "." + node.Text));
                IntermediateCode.Add(new AssignmentMemoryToVariableLine(VariableManager.PeekVariableCounter(), 0, VirtualTable.GetOffset(VariableManager.CurrentClass, node.Text)));
            }

            if (note_object_return_type)
            {
                //if (node.StaticType.Text == "Int" ||
                //    node.StaticType.Text == "Bool" ||
                //    node.StaticType.Text == "String")
                if (type == "Int" ||
                    type == "Bool" ||
                    type == "String")
                    IntermediateCode.Add(new ReturnTypeLine(node.StaticType.Text));
            }
        }
        

        public void Visit(ComparisonOperation node)
        {
            BinaryOperationVisit(node);
            if (note_object_return_type)
                IntermediateCode.Add(new ReturnTypeLine("Bool"));
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
            IntermediateCode.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), 0));
            DispatchVisit(node, cclass);
        }

        void DispatchVisit(DispatchNode node, string cclass)
        {
            string method = node.IdMethod.Text;

            if (method == "abort" && (cclass == "Int" || cclass == "String" || cclass == "Bool"))
            {
                IntermediateCode.Add(new CallLabelLine(new LabelLine("Object","abort")));
                return;
            }

            if (method == "type_name")
            {
                if (cclass == "Int" || cclass == "Bool" || cclass == "String")
                {
                    IntermediateCode.Add(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), cclass));
                    return;
                }
            }

            //important for define
            if (method == "copy")
            {
                if (cclass == "Int" || cclass == "Bool" || cclass == "String")
                {
                    IntermediateCode.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
                    IntermediateCode.Add(new CallLabelLine(new LabelLine("_wrapper", cclass), VariableManager.PeekVariableCounter()));
                    IntermediateCode.Add(new PopParamLine(1));
                    return;
                }
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
                    IntermediateCode.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
                    IntermediateCode.Add(new CallLabelLine(new LabelLine("_wrapper", node.Arguments[i].StaticType.Text), VariableManager.PeekVariableCounter()));
                    IntermediateCode.Add(new PopParamLine(1));
                }

                VariableManager.PopVariableCounter();
            }

            VariableManager.PopVariableCounter();

            if (cclass != "String")
            {
                IntermediateCode.Add(new CommentLine("get method: " + cclass + "." + method));
                IntermediateCode.Add(new AssignmentMemoryToVariableLine(function_address, VariableManager.PeekVariableCounter(), offset));
            }

            IntermediateCode.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
            
            foreach (var p in parameters)
            {
                IntermediateCode.Add(new PushParamLine(p));
            }

            if (cclass != "String")
            {
                IntermediateCode.Add(new CallAddressLine(function_address, VariableManager.PeekVariableCounter()));
            }
            else
            {
                IntermediateCode.Add(new CallLabelLine(new LabelLine(cclass, method), VariableManager.PeekVariableCounter()));
            }

            if (note_object_return_type)
            {
                if (node.StaticType.Text == "Int" ||
                    node.StaticType.Text == "Bool" ||
                    node.StaticType.Text == "String")
                    IntermediateCode.Add(new ReturnTypeLine(node.StaticType.Text));
            }

            IntermediateCode.Add(new PopParamLine(parameters.Count+1));
        }

        public void Visit(EqualNode node)
        {
            BinaryOperationVisit(node);
            if (note_object_return_type)
                IntermediateCode.Add(new ReturnTypeLine("Bool"));
        }

        void BinaryOperationVisit(BinaryOperationNode node)
        {
            VariableManager.PushVariableCounter();

            int t1 = VariableManager.IncrementVariableCounter();
            VariableManager.PushVariableCounter();
            node.LeftOperand.Accept(this);
            VariableManager.PopVariableCounter();

            int t2 = VariableManager.IncrementVariableCounter();
            VariableManager.PushVariableCounter();
            node.RightOperand.Accept(this);
            VariableManager.PopVariableCounter();

            VariableManager.PopVariableCounter();

            if (node.LeftOperand.StaticType.Text == "String" && node.Symbol == "=")
            {
                IntermediateCode.Add(new BinaryOperationLine(VariableManager.PeekVariableCounter(), t1, t2, "=:="));
                return;
            }

            IntermediateCode.Add(new BinaryOperationLine(VariableManager.PeekVariableCounter(), t1, t2, node.Symbol));
        }

        public void Visit(StringNode node)
        {
            IntermediateCode.Add(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), node.Text));
            if (note_object_return_type)
                IntermediateCode.Add(new ReturnTypeLine("String"));
        }

        public void Visit(LetNode node)
        {
            VariableManager.PushVariableCounter();

            foreach (var attr in node.Initialization)
            {
                VariableManager.IncrementVariableCounter();
                VariableManager.PushVariable(attr.Formal.Id.Text, attr.Formal.Type.Text);
                VariableManager.PushVariableCounter();
                attr.Accept(this);
                //IntermediateCode.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), VariableManager.VariableCounter));
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
                    IntermediateCode.Add(new ReturnTypeLine(node.StaticType.Text));
            }
        }

        public void Visit(NewNode node)
        {
            if (node.TypeId.Text == "Int" ||
                node.TypeId.Text == "Bool" ||
                node.TypeId.Text == "String")
            {
                if (node.TypeId.Text == "Int" || node.TypeId.Text == "Bool")
                    IntermediateCode.Add(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), 0));
                else
                    IntermediateCode.Add(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), ""));

                if (note_object_return_type)
                {
                    IntermediateCode.Add(new ReturnTypeLine(node.TypeId.Text));
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
            IntermediateCode.Add(new AllocateLine(VariableManager.PeekVariableCounter(), size));
            IntermediateCode.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
            IntermediateCode.Add(new CallLabelLine(new LabelLine(cclass, "constructor")));
            IntermediateCode.Add(new PopParamLine(1));
        }

        public void Visit(IsVoidNode node)
        {
            //if special types non void;
            if (node.Operand.StaticType.Text == "Int" ||
               node.Operand.StaticType.Text == "String" ||
               node.Operand.StaticType.Text == "Bool")
                IntermediateCode.Add(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), 0));
            else
                UnaryOperationVisit(node);

            if (note_object_return_type)
            {
                IntermediateCode.Add(new ReturnTypeLine("Bool"));
            }
        }

        public void Visit(NegNode node)
        {
            UnaryOperationVisit(node);
            if (note_object_return_type)
            {
                IntermediateCode.Add(new ReturnTypeLine("Int"));
            }
        }


        public void Visit(NotNode node)
        {
            UnaryOperationVisit(node);
            if (note_object_return_type)
            {
                IntermediateCode.Add(new ReturnTypeLine("Bool"));
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

            IntermediateCode.Add(new UnaryOperationLine(VariableManager.PeekVariableCounter(), t1, node.Symbol));
        }

        public void Visit(IfNode node)
        {
            string tag = IntermediateCode.Count.ToString();

            node.Condition.Accept(this);

            IntermediateCode.Add(new ConditionalJumpLine(VariableManager.PeekVariableCounter(), new LabelLine("_else", tag)));

            node.Body.Accept(this);
            IntermediateCode.Add(new GotoJumpLine(new LabelLine("_endif", tag)));

            IntermediateCode.Add(new LabelLine("_else", tag));
            node.ElseBody.Accept(this);

            IntermediateCode.Add(new LabelLine("_endif", tag));

        }


        public void Visit(WhileNode node)
        {
            string tag = IntermediateCode.Count.ToString();

            IntermediateCode.Add(new LabelLine("_whilecondition", tag));

            node.Condition.Accept(this);

            IntermediateCode.Add(new ConditionalJumpLine(VariableManager.PeekVariableCounter(), new LabelLine("_endwhile", tag)));

            node.Body.Accept(this);

            IntermediateCode.Add(new GotoJumpLine(new LabelLine("_whilecondition", tag)));

            IntermediateCode.Add(new LabelLine("_endwhile", tag));
        }


        public void Visit(VoidNode node)
        {
            IntermediateCode.Add(new AssignmentNullToVariableLine(VariableManager.PeekVariableCounter()));
        }

        public void Visit(SelfNode node)
        {
            IntermediateCode.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(),0));
        }
        public void Visit(FormalNode node)
        {
            throw new NotImplementedException();
        }

    }
}
