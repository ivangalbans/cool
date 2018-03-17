using Cool.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    public interface ICheckSemantics
    {
        ProgramNode CheckSemantic(ProgramNode node, ICollection<SemanticError> errors);
    }
}
