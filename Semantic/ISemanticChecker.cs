using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes;

namespace Semantic
{
    interface ISemanticChecker
    {
        ProgramNode SemanticCheck(ProgramNode programNode);
    }
}
