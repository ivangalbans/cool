using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public abstract class AssignmentLine<T> : CodeLine
    {
        public int Left { get; protected set; }
        public T Right { get; protected set; }

    }

    public class AssignmentVariableToMemoryLine : AssignmentLine<int>
    {
        public int Offset { get; }
        public AssignmentVariableToMemoryLine(int left, int right, int offset = 0)
        {
            Left = left;
            Right = right;
            Offset = offset;
        }

        public override string ToString()
        {
            return $"*(t{Left} + {Offset}) = t{Right}";
        }
    }

    public class AssignmentVariableToVariableLine : AssignmentLine<int>
    {
        public AssignmentVariableToVariableLine(int left, int right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"t{Left} = t{Right}";
        }
    }

    public class AssignmentConstantToMemoryLine : AssignmentLine<int>
    {
        public int Offset { get; }
        public AssignmentConstantToMemoryLine(int left, int right, int offset = 0)
        {
            Left = left;
            Right = right;
            Offset = offset;
        }
    }

    public class AssignmentMemoryToVariableLine : AssignmentLine<int>
    {
        public int Offset { get; }

        public AssignmentMemoryToVariableLine(int left, int right, int offset = 0)
        {
            Left = left;
            Right = right;
            Offset = offset;
        }

        public override string ToString()
        {
            return $"t{Left} = *(t{Right} + {Offset})"; ;
        }
    }


    public class AssignmentConstantToVariableLine : AssignmentLine<int>
    {

        public AssignmentConstantToVariableLine(int left, int right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"t{Left} = {Right}";
        }
    }

    public class AssignmentStringToVariable : AssignmentLine<string>
    {

        public AssignmentStringToVariable(int left, string right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"t{Left} = \"{Right}\"";
        }
    }
}
