using System.Collections.Generic;

namespace algorithms
{
    public class MatrixField
    {
        public int Index { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public HashSet<int> Neighbors { get; set; }
        public FieldState State { get; set; }
        public FieldState Walkable { get; set; }
        public float G { get; set; }
        public float H { get; set; }

        public float F => this.H + this.G;

        public MatrixField ParentField { get; set; }
    }
}