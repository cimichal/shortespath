﻿using System.Collections.Generic;
using System.Linq;

namespace algorithms
{
    public class AStar
    {
        public Matrix Matrix { get; set; }
        public List<MatrixField> SelectNodes { get; set; } = new List<MatrixField>();

        public virtual List<MatrixField> FindPath()
        {
                var searchPath = new List<MatrixField>();

                var searchPoint = this.Search(this.Matrix.IndexPunktuStartowego);

                if (searchPoint)
                {
                    var point = this.Matrix.GetMatrixField(this.Matrix.IndexPunktuKoncowego, null, null);
                    while (point.ParentField != null) // last element doesn't have parent
                    {
                        if (point.Index == 1)
                        {
                            var test = 1;
                        }
                        searchPath.Add(point);
                        point = point.ParentField;
                    }
                    searchPath.Reverse();
                }

                return searchPath;
        }

        public virtual bool Search(int searchPoint)
        {
            var neighboarsId = this.Matrix.GetWalkableNeighbors(searchPoint, true).Select(x => x.Index);

            var neighboars = new List<MatrixField>();

            foreach (var i in neighboarsId)
            {
                var neighboar = this.Matrix.GetMatrixField(i, null, null);

                if (searchPoint == this.Matrix.IndexPunktuStartowego)
                {
                    this.SelectNodes.Add(this.Matrix.GetMatrixField(searchPoint, null, null));
                }

                if (!this.SelectNodes.Contains(neighboar))
                {
                    neighboars.Add(neighboar);
                }
            }

            this.Matrix.CalculateF(neighboars);

            // smallest F is first 
            neighboars.Sort((node1, node2) => node1.F.CompareTo(node2.F));

            foreach (var nextNode in neighboars)
            {
                if (nextNode.Index == this.Matrix.IndexPunktuKoncowego)
                {
                    this.SelectNodes.Add(nextNode);
                    nextNode.State = FieldState.Odwiedzony;
                    return true;
                }

                if (nextNode.State == FieldState.Odwiedzony)
                {
                    continue;
                }

                this.SelectNodes.Add(nextNode);
                nextNode.State = FieldState.Odwiedzony;

                if (this.Search(nextNode.Index)) // Note: Recurses back into Search(Node)
                {
                    return true;
                }
            }
            return false;
        }
    }
}