using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classical_genetic
{
    public class IndexAndDistance: IComparable<IndexAndDistance>
    {
       
            public int idx;  // index of a training item
            public double dist;  // distance to unknown
            public int CompareTo(IndexAndDistance other)
            {
                if (this.dist < other.dist) return -1;
                else if (this.dist > other.dist) return +1;
                else return 0;
            }
        
    }
}
