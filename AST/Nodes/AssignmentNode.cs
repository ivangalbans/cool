﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using Grammars;

namespace AST.Nodes
{
    public class AssignmentNode : ExpressionNode
    {
        public string TextID { get; set; }

        public ExpressionNode Expression { get; set; }


        public AssignmentNode(Token id, ExpressionNode expression)
        {
            TextID = id.Text;
            Line = id.Line;
            Column = id.Column;
            Expression = expression;
        }
    }
}