using System;
using System.Collections.Generic;
using Cool.AST;
using Cool.Semantics;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;

namespace Cool.CodeGeneration.IntermediateCode
{
    class GenerateTour : IVisitor
    {
        List<CodeLine> IC;
        VirtualTable VirtualTable;
        IScope Scope;
        VariableManager VariableManager;
        bool special_object_return_type = false;
        static int return_type_variable = 1;
        
        public List<CodeLine> GetIntermediateCode(ProgramNode node, IScope scope)
        {
            Scope = scope;

            node = (new OptimizationTour()).Optimize(node, scope);

            IC = new List<CodeLine>();
            VariableManager = new VariableManager();
            VirtualTable = new VirtualTable(scope);

            VariableManager.PushVariableCounter();
            InitCode();
            VariableManager.PopVariableCounter();

            node.Accept(this);

            VariableManager.PushVariableCounter();
            StartFunctionCode();
            VariableManager.PopVariableCounter();

            return IC;
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


        void InitCode()
        {
            int self = VariableManager.PeekVariableCounter();
            (string, string) label;
            List<string> obj = new List<string> { "abort", "type_name", "copy" };

            IC.Add(new CallLabelLine(new LabelLine("start")));

            IC.Add(new LabelLine("Object", "constructor"));
            IC.Add(new ParamLine(self));
            foreach (var f in VirtualTable.Object)
            {
                label = VirtualTable.GetDefinition("Object", f);
                IC.Add(new CommentLine("set method: " + label.Item1 + "." + label.Item2));
                IC.Add(new AssignmentLabelToMemoryLine(self, new LabelLine(label.Item1, label.Item2), VirtualTable.GetOffset("Object", f)));
            }

            IC.Add(new CommentLine("set class name: Object"));
            IC.Add(new AssignmentStringToMemoryLine(0, "Object", 0));
            IC.Add(new CommentLine("set class size: " + VirtualTable.GetSizeClass("Object") + " words"));
            IC.Add(new AssignmentConstantToMemoryLine(0, VirtualTable.GetSizeClass("Object"), 1));
            //IntermediateCode.Add(new CommentLine("set class generation label"));
            //IntermediateCode.Add(new AssignmentLabelToMemoryLine(0, new LabelLine("_class", node.TypeClass.Text), 2));

            IC.Add(new ReturnLine());


            IC.Add(new LabelLine("IO", "constructor"));

            IC.Add(new ParamLine(self));
            IC.Add(new PushParamLine(self));
            IC.Add(new CallLabelLine(new LabelLine("Object", "constructor")));
            IC.Add(new PopParamLine(1));

            foreach (var f in VirtualTable.IO)
            {
                label = VirtualTable.GetDefinition("IO", f);
                IC.Add(new CommentLine("set method: " + label.Item1 + "." + label.Item2));
                IC.Add(new AssignmentLabelToMemoryLine(self, new LabelLine(label.Item1, label.Item2), VirtualTable.GetOffset("IO", f)));
            }

            IC.Add(new CommentLine("set class name: Object"));
            IC.Add(new AssignmentStringToMemoryLine(0, "IO", 0));
            IC.Add(new CommentLine("set class size: " + VirtualTable.GetSizeClass("IO") + " words"));
            IC.Add(new AssignmentConstantToMemoryLine(0, VirtualTable.GetSizeClass("IO"), 1));
            IC.Add(new CommentLine("set class generation label"));
            IC.Add(new AssignmentLabelToMemoryLine(0, new LabelLine("_class", "IO"), 2));

            IC.Add(new ReturnLine());


            IC.Add(new InheritLine("IO", "Object"));
            IC.Add(new InheritLine("Int", "Object"));
            IC.Add(new InheritLine("Bool", "Object"));
            IC.Add(new InheritLine("String", "Object"));

            //Int wrapper for runtime check typing
            IC.Add(new LabelLine("_wrapper", "Int"));
            IC.Add(new ParamLine(self));
            IC.Add(new AllocateLine(self + 1, VirtualTable.GetSizeClass("Int") + 1));
            IC.Add(new PushParamLine(self + 1));
            IC.Add(new CallLabelLine(new LabelLine("Object", "constructor")));
            IC.Add(new PopParamLine(1));
            IC.Add(new AssignmentStringToMemoryLine(self + 1, "Int", 0));
            IC.Add(new AssignmentVariableToMemoryLine(self + 1, self, VirtualTable.GetSizeClass("Int")));
            IC.Add(new AssignmentLabelToMemoryLine(self + 1, new LabelLine("_class", "Int"), 2));
            IC.Add(new ReturnLine(self + 1));

            //Bool wrapper for runtime check typing
            IC.Add(new LabelLine("_wrapper", "Bool"));
            IC.Add(new ParamLine(self));
            IC.Add(new AllocateLine(self + 1, VirtualTable.GetSizeClass("Bool") + 1));
            IC.Add(new PushParamLine(self + 1));
            IC.Add(new CallLabelLine(new LabelLine("Object", "constructor")));
            IC.Add(new PopParamLine(1));
            IC.Add(new AssignmentStringToMemoryLine(self + 1, "Bool", 0));
            IC.Add(new AssignmentVariableToMemoryLine(self + 1, self, VirtualTable.GetSizeClass("Bool")));
            IC.Add(new AssignmentLabelToMemoryLine(self + 1, new LabelLine("_class", "Bool"), 2));
            IC.Add(new ReturnLine(self + 1));

            //String wrapper for runtime check typing
            IC.Add(new LabelLine("_wrapper", "String"));
            IC.Add(new ParamLine(self));
            IC.Add(new AllocateLine(self + 1, VirtualTable.GetSizeClass("String") + 1));
            IC.Add(new PushParamLine(self + 1));
            IC.Add(new CallLabelLine(new LabelLine("Object", "constructor")));
            IC.Add(new PopParamLine(1));
            IC.Add(new AssignmentStringToMemoryLine(self + 1, "String", 0));
            IC.Add(new AssignmentVariableToMemoryLine(self + 1, self, VirtualTable.GetSizeClass("String")));
            IC.Add(new AssignmentLabelToMemoryLine(self + 1, new LabelLine("_class", "String"), 2));
            IC.Add(new ReturnLine(self + 1));


            //abort, typename, copy
            IC.Add(new LabelLine("Object", "abort"));
            IC.Add(new GotoJumpLine(new LabelLine("_abort")));

            IC.Add(new LabelLine("Object", "type_name"));
            IC.Add(new ParamLine(0));
            IC.Add(new AssignmentMemoryToVariableLine(0, 0, 0));
            IC.Add(new ReturnLine(0));


            IC.Add(new LabelLine("Object", "copy"));
            IC.Add(new ParamLine(0));
            IC.Add(new AssignmentMemoryToVariableLine(1, 0, 1));
            IC.Add(new AssignmentConstantToVariableLine(2, 4));
            IC.Add(new BinaryOperationLine(1, 1, 2, "*"));
            IC.Add(new PushParamLine(0));
            IC.Add(new PushParamLine(1));
            IC.Add(new CallLabelLine(new LabelLine("_copy"), 0));
            IC.Add(new PopParamLine(2));

            IC.Add(new ReturnLine(0));


            //io: in_string, out_string, in_int, out_int
            IC.Add(new LabelLine("IO", "out_string"));
            IC.Add(new ParamLine(0));
            IC.Add(new ParamLine(1));
            IC.Add(new PushParamLine(1));
            IC.Add(new CallLabelLine(new LabelLine("_out_string"), 0));
            IC.Add(new PopParamLine(1));
            IC.Add(new ReturnLine(0));

            IC.Add(new LabelLine("IO", "out_int"));
            IC.Add(new ParamLine(0));
            IC.Add(new ParamLine(1));
            IC.Add(new PushParamLine(1));
            IC.Add(new CallLabelLine(new LabelLine("_out_int"), 0));
            IC.Add(new PopParamLine(1));
            IC.Add(new ReturnLine(0));


            IC.Add(new LabelLine("IO", "in_string"));
            IC.Add(new ParamLine(0));
            IC.Add(new CallLabelLine(new LabelLine("_in_string"), 0));
            IC.Add(new ReturnLine(0));


            IC.Add(new LabelLine("IO", "in_int"));
            IC.Add(new ParamLine(0));
            IC.Add(new CallLabelLine(new LabelLine("_in_int"), 0));
            IC.Add(new ReturnLine(0));

            //string: substr, concat, length
            IC.Add(new LabelLine("String", "length"));
            IC.Add(new ParamLine(0));
            IC.Add(new PushParamLine(0));
            IC.Add(new CallLabelLine(new LabelLine("_stringlength"), 0));
            IC.Add(new PopParamLine(1));
            IC.Add(new ReturnLine(0));


            IC.Add(new LabelLine("String", "concat"));
            IC.Add(new ParamLine(0));
            IC.Add(new ParamLine(1));
            IC.Add(new PushParamLine(0));
            IC.Add(new PushParamLine(1));
            IC.Add(new CallLabelLine(new LabelLine("_stringconcat"), 0));
            IC.Add(new PopParamLine(2));
            IC.Add(new ReturnLine(0));


            IC.Add(new LabelLine("String", "substr"));
            IC.Add(new ParamLine(0));
            IC.Add(new ParamLine(1));
            IC.Add(new ParamLine(2));
            IC.Add(new PushParamLine(0));
            IC.Add(new PushParamLine(1));
            IC.Add(new PushParamLine(2));
            IC.Add(new CallLabelLine(new LabelLine("_stringsubstr"), 0));
            IC.Add(new PopParamLine(3));
            IC.Add(new ReturnLine(0));
        }

        void StartFunctionCode()
        {
            IC.Add(new LabelLine("start"));
            New("Main");
            IC.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
            IC.Add(new CallLabelLine(new LabelLine("Main", "main")));
            IC.Add(new PopParamLine(1));
            //IntermediateCode.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
            //IntermediateCode.Add(new CallLabelLine(new LabelLine("Object", "abort")));
            //IntermediateCode.Add(new PopParamLine(1));
        }

        public void Visit(ClassNode node)
        {
            string cclass;
            cclass = VariableManager.CurrentClass = node.TypeClass.Text;
            IC.Add(new InheritLine(node.TypeClass.Text, Scope.GetType(node.TypeClass.Text).Parent.Text));

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

            IC.Add(new LabelLine(VariableManager.CurrentClass, "constructor"));
            IC.Add(new ParamLine(self));

            //calling first the parent constructor method
            if (VariableManager.CurrentClass != "Object")
            {
                IC.Add(new PushParamLine(self));
                LabelLine label = new LabelLine(node.TypeInherit.Text, "constructor");
                IC.Add(new CallLabelLine(label));
                IC.Add(new PopParamLine(1));
            }


            foreach (var method in methods)
            {
                (string, string) label = VirtualTable.GetDefinition(node.TypeClass.Text, method.Id.Text);
                IC.Add(new CommentLine("set method: " + label.Item1 + "." + label.Item2));
                IC.Add(new AssignmentLabelToMemoryLine(self, new LabelLine(label.Item1, label.Item2), VirtualTable.GetOffset(node.TypeClass.Text, method.Id.Text)));
                //IntermediateCode.Add(new AssignmentVariableToMemoryLine(self, VariableManager.VariableCounter, IntermediateCode.GetVirtualTableOffset(node.TypeClass.Text, attr.Formal.Id.Text)));
            }


            foreach (var attr in attributes)
            {
                VariableManager.PushVariableCounter();
                attr.Accept(this);
                VariableManager.PopVariableCounter();
                IC.Add(new CommentLine("set attribute: " + attr.Formal.Id.Text));
                IC.Add(new AssignmentVariableToMemoryLine(self, VariableManager.PeekVariableCounter(), VirtualTable.GetOffset(node.TypeClass.Text, attr.Formal.Id.Text)));
            }
            

            IC.Add(new CommentLine("set class name: " + node.TypeClass.Text));
            IC.Add(new AssignmentStringToMemoryLine(0, node.TypeClass.Text, 0));
            IC.Add(new CommentLine("set class size: " + VirtualTable.GetSizeClass(node.TypeClass.Text) + " words"));
            IC.Add(new AssignmentConstantToMemoryLine(0, VirtualTable.GetSizeClass(node.TypeClass.Text), 1));
            IC.Add(new CommentLine("set class generation label"));
            IC.Add(new AssignmentLabelToMemoryLine(0, new LabelLine("_class", node.TypeClass.Text), 2));

            IC.Add(new ReturnLine(-1));

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
                IC.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
                IC.Add(new CallLabelLine(new LabelLine("_wrapper", node.AssignExp.StaticType.Text), VariableManager.PeekVariableCounter()));
                IC.Add(new PopParamLine(1));
            }
        }

        public void Visit(MethodNode node)
        {
            IC.Add(new LabelLine(VariableManager.CurrentClass, node.Id.Text));

            special_object_return_type = node.TypeReturn.Text == "Object";

            int self = VariableManager.VariableCounter = 0;
            IC.Add(new ParamLine(self));

            //if return type is object, annotation type is needed
            if (special_object_return_type)
                VariableManager.IncrementVariableCounter();

            VariableManager.IncrementVariableCounter();

            foreach (var formal in node.Arguments)
            {
                IC.Add(new ParamLine(VariableManager.VariableCounter));
                VariableManager.PushVariable(formal.Id.Text, formal.Type.Text);
                VariableManager.IncrementVariableCounter();
            }

            VariableManager.PushVariableCounter();
            node.Body.Accept(this);

            if (special_object_return_type)
                ReturnObjectWrapping();

            IC.Add(new ReturnLine(VariableManager.PeekVariableCounter()));


            VariableManager.PopVariableCounter();

            foreach (var formal in node.Arguments)
            {
                VariableManager.PopVariable(formal.Id.Text);
            }

            special_object_return_type = false;
        }

        void ReturnObjectWrapping()
        {
            int t;
            int result = VariableManager.PeekVariableCounter();
            string tag = IC.Count.ToString();

            VariableManager.PushVariableCounter();
            VariableManager.IncrementVariableCounter();
            t = VariableManager.VariableCounter;
            IC.Add(new AssignmentStringToVariableLine(t, "Int"));
            IC.Add(new BinaryOperationLine(t, return_type_variable, t, "="));
            IC.Add(new ConditionalJumpLine(t, new LabelLine("_attempt_bool", tag)));
            IC.Add(new PushParamLine(result));
            IC.Add(new CallLabelLine(new LabelLine("_wrapper", "Int"), result));
            IC.Add(new PopParamLine(1));
            IC.Add(new GotoJumpLine(new LabelLine("_not_more_attempt", tag)));
            VariableManager.PopVariableCounter();

            IC.Add(new LabelLine("_attempt_bool", tag));
            VariableManager.PushVariableCounter();
            VariableManager.IncrementVariableCounter();
            t = VariableManager.VariableCounter;
            IC.Add(new AssignmentStringToVariableLine(t, "Bool"));
            IC.Add(new BinaryOperationLine(t, return_type_variable, t, "="));
            IC.Add(new ConditionalJumpLine(t, new LabelLine("_attempt_string", tag)));
            IC.Add(new PushParamLine(result));
            IC.Add(new CallLabelLine(new LabelLine("_wrapper", "Bool"), result));
            IC.Add(new PopParamLine(1));
            IC.Add(new GotoJumpLine(new LabelLine("_not_more_attempt", tag)));
            VariableManager.PopVariableCounter();

            IC.Add(new LabelLine("_attempt_string", tag));
            VariableManager.PushVariableCounter();
            VariableManager.IncrementVariableCounter();
            t = VariableManager.VariableCounter;
            IC.Add(new AssignmentStringToVariableLine(t, "String"));
            IC.Add(new BinaryOperationLine(t, return_type_variable, t, "="));
            IC.Add(new ConditionalJumpLine(t, new LabelLine("_not_more_attempt", tag)));
            IC.Add(new PushParamLine(result));
            IC.Add(new CallLabelLine(new LabelLine("_wrapper", "String"), result));
            IC.Add(new PopParamLine(1));
            VariableManager.PopVariableCounter();

            IC.Add(new LabelLine("_not_more_attempt", tag));
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

                IC.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
            }
            else
            {
                string tag = IC.Count.ToString();

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

                    IC.Add(new LabelLine("_case", tag + "." + i));
                    IC.Add(new AssignmentStringToVariableLine(VariableManager.VariableCounter, branch_type));
                    IC.Add(new BinaryOperationLine(VariableManager.VariableCounter, expr, VariableManager.VariableCounter, "inherit"));
                    IC.Add(new ConditionalJumpLine(VariableManager.VariableCounter, new LabelLine("_case", tag + "." + (i + 1))));


                    if ((branch_type == "Int" ||
                        branch_type == "Bool" ||
                        branch_type == "String"))
                    {
                        if (static_type == "Object")
                        {

                            IC.Add(new AssignmentMemoryToVariableLine(expr, expr, VirtualTable.GetSizeClass(branch_type)));

                            VariableManager.PushVariableCounter();
                            sorted[i].Expression.Accept(this);
                            VariableManager.PopVariableCounter();

                            IC.Add(new AssignmentVariableToVariableLine(result, VariableManager.PeekVariableCounter()));
                            IC.Add(new GotoJumpLine(new LabelLine("_endcase", tag)));
                        }
                    }
                    else
                    {
                        VariableManager.PushVariableCounter();
                        sorted[i].Expression.Accept(this);
                        VariableManager.PopVariableCounter();

                        IC.Add(new AssignmentVariableToVariableLine(result, VariableManager.PeekVariableCounter()));
                        IC.Add(new GotoJumpLine(new LabelLine("_endcase", tag)));
                    }



                    VariableManager.PopVariableCounter();

                    VariableManager.PopVariable(sorted[i].Formal.Id.Text);
                }

                IC.Add(new LabelLine("_case", tag + "." + sorted.Count));
                IC.Add(new GotoJumpLine(new LabelLine("_caseselectionexception")));

                IC.Add(new LabelLine("_endcase", tag));
            }
        }

        public void Visit(IntNode node)
        {
            IC.Add(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), node.Value));
            if (special_object_return_type)
                SetReturnType("Int");
        }

        public void Visit(BoolNode node)
        {
            IC.Add(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), node.Value ? 1 : 0));
            if (special_object_return_type)
                SetReturnType("Bool");
        }

        public void Visit(ArithmeticOperation node)
        {
            if(node.Attributes.ContainsKey("integer_constant_value"))
                IC.Add(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), node.Attributes["integer_constant_value"]));
            else
                BinaryOperationVisit(node);
            if (special_object_return_type)
                SetReturnType("Int");
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
                IC.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
                IC.Add(new CallLabelLine(new LabelLine("_wrapper", node.ExpressionRight.StaticType.Text), VariableManager.PeekVariableCounter()));
                IC.Add(new PopParamLine(1));
            }

            if (t != -1)
            {
                //IntermediateCode.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
                IC.Add(new AssignmentVariableToVariableLine(t, VariableManager.PeekVariableCounter()));
            }
            else
            {
                int offset = VirtualTable.GetOffset(VariableManager.CurrentClass, node.ID.Text);
                IC.Add(new AssignmentVariableToMemoryLine(0, VariableManager.PeekVariableCounter(), offset));
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
                IC.Add(new CommentLine("get veriable: " + node.Text));
                IC.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), t));
            }
            else
            {
                IC.Add(new CommentLine("get attribute: " + VariableManager.CurrentClass + "." + node.Text));
                IC.Add(new AssignmentMemoryToVariableLine(VariableManager.PeekVariableCounter(), 0, VirtualTable.GetOffset(VariableManager.CurrentClass, node.Text)));
            }

            if (special_object_return_type)
                SetReturnType(type);
        }
        

        public void Visit(ComparisonOperation node)
        {
            BinaryOperationVisit(node);
            if (special_object_return_type)
            {
                IC.Add(new CommentLine($"set bool as return type"));
                IC.Add(new AssignmentStringToVariableLine(return_type_variable, "Bool"));
            }
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
            IC.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(), 0));
            DispatchVisit(node, cclass);
        }

        void DispatchVisit(DispatchNode node, string cclass)
        {
            string method = node.IdMethod.Text;

            if (method == "abort" && (cclass == "Int" || cclass == "String" || cclass == "Bool"))
            {
                IC.Add(new CallLabelLine(new LabelLine("Object","abort")));
                return;
            }

            if (method == "type_name")
            {
                if (cclass == "Int" || cclass == "Bool" || cclass == "String")
                {
                    IC.Add(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), cclass));
                    return;
                }
            }

            //important for define
            if (method == "copy")
            {
                if (cclass == "Int" || cclass == "Bool" || cclass == "String")
                {
                    IC.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
                    IC.Add(new CallLabelLine(new LabelLine("_wrapper", cclass), VariableManager.PeekVariableCounter()));
                    IC.Add(new PopParamLine(1));
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
                    IC.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
                    IC.Add(new CallLabelLine(new LabelLine("_wrapper", node.Arguments[i].StaticType.Text), VariableManager.PeekVariableCounter()));
                    IC.Add(new PopParamLine(1));
                }

                VariableManager.PopVariableCounter();
            }

            VariableManager.PopVariableCounter();

            if (cclass != "String")
            {
                IC.Add(new CommentLine("get method: " + cclass + "." + method));
                IC.Add(new AssignmentMemoryToVariableLine(function_address, VariableManager.PeekVariableCounter(), offset));
            }

            IC.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
            
            foreach (var p in parameters)
            {
                IC.Add(new PushParamLine(p));
            }

            if (cclass != "String")
            {
                IC.Add(new CallAddressLine(function_address, VariableManager.PeekVariableCounter()));
            }
            else
            {
                IC.Add(new CallLabelLine(new LabelLine(cclass, method), VariableManager.PeekVariableCounter()));
            }

            if (special_object_return_type)
                SetReturnType(node.StaticType.Text);

            IC.Add(new PopParamLine(parameters.Count+1));
        }

        void SetReturnType(string type)
        {
            if (type == "Int" ||
                type == "Bool" ||
                type == "String")
            {
                IC.Add(new CommentLine($"set {type} as return type"));
                IC.Add(new AssignmentStringToVariableLine(return_type_variable, type));
            }
            else
            {
                IC.Add(new CommentLine($"set object as return type"));
                IC.Add(new AssignmentStringToVariableLine(return_type_variable, "Object"));
            }
        }


        public void Visit(EqualNode node)
        {
            BinaryOperationVisit(node);
            if (special_object_return_type)
                SetReturnType("Bool");
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
                IC.Add(new BinaryOperationLine(VariableManager.PeekVariableCounter(), t1, t2, "=:="));
                return;
            }

            IC.Add(new BinaryOperationLine(VariableManager.PeekVariableCounter(), t1, t2, node.Symbol));
        }

        public void Visit(StringNode node)
        {
            IC.Add(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), node.Text));
            if (special_object_return_type)
                SetReturnType("String");
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

            if (special_object_return_type)
                SetReturnType(node.StaticType.Text);
        }

        public void Visit(NewNode node)
        {
            if (node.TypeId.Text == "Int" ||
                node.TypeId.Text == "Bool" ||
                node.TypeId.Text == "String")
            {
                if (node.TypeId.Text == "Int" || node.TypeId.Text == "Bool")
                    IC.Add(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), 0));
                else
                    IC.Add(new AssignmentStringToVariableLine(VariableManager.PeekVariableCounter(), ""));
            }
            else
            {
                New(node.TypeId.Text);
            }

            if (special_object_return_type)
                SetReturnType(node.TypeId.Text);
        }

        public void New(string cclass)
        {
            int size = VirtualTable.GetSizeClass(cclass);
            IC.Add(new AllocateLine(VariableManager.PeekVariableCounter(), size));
            IC.Add(new PushParamLine(VariableManager.PeekVariableCounter()));
            IC.Add(new CallLabelLine(new LabelLine(cclass, "constructor")));
            IC.Add(new PopParamLine(1));
        }

        public void Visit(IsVoidNode node)
        {
            //if special types non void;
            if (node.Operand.StaticType.Text == "Int" ||
               node.Operand.StaticType.Text == "String" ||
               node.Operand.StaticType.Text == "Bool")
                IC.Add(new AssignmentConstantToVariableLine(VariableManager.PeekVariableCounter(), 0));
            else
                UnaryOperationVisit(node);

            if (special_object_return_type)
                SetReturnType("Bool");
        }

        public void Visit(NegNode node)
        {
            UnaryOperationVisit(node);
            if (special_object_return_type)
                SetReturnType("Int");
        }


        public void Visit(NotNode node)
        {
            UnaryOperationVisit(node);
            if (special_object_return_type)
                SetReturnType("Bool");
        }

        void UnaryOperationVisit(UnaryOperationNode node)
        {
            VariableManager.PushVariableCounter();

            VariableManager.IncrementVariableCounter();
            int t1 = VariableManager.VariableCounter;
            VariableManager.PushVariableCounter();
            node.Operand.Accept(this);

            VariableManager.PopVariableCounter();

            IC.Add(new UnaryOperationLine(VariableManager.PeekVariableCounter(), t1, node.Symbol));
        }

        public void Visit(IfNode node)
        {
            string tag = IC.Count.ToString();

            node.Condition.Accept(this);

            IC.Add(new ConditionalJumpLine(VariableManager.PeekVariableCounter(), new LabelLine("_else", tag)));

            node.Body.Accept(this);
            IC.Add(new GotoJumpLine(new LabelLine("_endif", tag)));

            IC.Add(new LabelLine("_else", tag));
            node.ElseBody.Accept(this);

            IC.Add(new LabelLine("_endif", tag));

        }


        public void Visit(WhileNode node)
        {
            string tag = IC.Count.ToString();

            IC.Add(new LabelLine("_whilecondition", tag));

            node.Condition.Accept(this);

            IC.Add(new ConditionalJumpLine(VariableManager.PeekVariableCounter(), new LabelLine("_endwhile", tag)));

            node.Body.Accept(this);

            IC.Add(new GotoJumpLine(new LabelLine("_whilecondition", tag)));

            IC.Add(new LabelLine("_endwhile", tag));
        }


        public void Visit(VoidNode node)
        {
            IC.Add(new AssignmentNullToVariableLine(VariableManager.PeekVariableCounter()));
        }

        public void Visit(SelfNode node)
        {
            IC.Add(new AssignmentVariableToVariableLine(VariableManager.PeekVariableCounter(),0));
        }
        public void Visit(FormalNode node)
        {
            throw new NotImplementedException();
        }

    }
}
