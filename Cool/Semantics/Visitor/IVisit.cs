using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    public interface IVisit
    {
        void Accept(IVisitor visitor, IScope scope, ICollection<string> errors);
    }
}
