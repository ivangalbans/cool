using System;

namespace BottomUpParsing
{
    public class LrItem : IComparable<LrItem>
    {
        //private bool change;
        private int mem;

        public LrItem(int production, int dot)
        {
            ProductionNumber = production;
            DotNumber = dot;
        }

        public int ProductionNumber { get; }
        public int DotNumber { get; }

        public int CompareTo(LrItem obj)
        {
            if (obj.ProductionNumber == ProductionNumber && obj.DotNumber == DotNumber)
                return 0;
            if (obj.ProductionNumber > ProductionNumber)
                return -1;
            return 1;
        }

        public override int GetHashCode()
        {
            //if (!change) return mem;
            mem = ProductionNumber * 311 + DotNumber;
            //change = true;
            return mem;
        }
    }
}