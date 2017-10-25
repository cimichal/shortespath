using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms
{
    class MatrixField
    {
        public int Index { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public HashSet<int> Neighbors { get; set; }
        public FieldState State { get; set; }
        public FieldState Walkable { get; set; } 
    }
}
