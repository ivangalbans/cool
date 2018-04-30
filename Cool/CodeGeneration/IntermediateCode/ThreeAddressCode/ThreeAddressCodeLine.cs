using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public abstract class ThreeAddressCodeLine
    {
        
    }

    public abstract class DataLine : ThreeAddressCodeLine
    { }

    public abstract class TypeLine : ThreeAddressCodeLine
    { }

    public abstract class CodeLine : ThreeAddressCodeLine
    { }

}
