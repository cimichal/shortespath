using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace algorithms
{
    public class JumpPointSearch : AStar
    {
        public List<MatrixField> SelectNodes { get; set; } = new List<MatrixField>();
        public List<MatrixField> TmpNodes { get; set; } = new List<MatrixField>();

        public override List<MatrixField> FindPath()
        {
            var searchPath = new List<MatrixField>();

            var searchPoint = this.Search(this.Matrix.IndexPunktuStartowego);

            if (searchPoint)
            {
                var point = this.Matrix.GetMatrixField(this.Matrix.IndexPunktuKoncowego, null, null);
                while (point.ParentField != null) // last element doesn't have parent
                {
                    searchPath.Add(point);
                    point = point.ParentField;
                }
                searchPath.Reverse();
            }

            return searchPath;
            
        }

        public override bool Search(int searchPoint)
        {
            var currentPoint = this.Matrix.GetMatrixField(searchPoint, null, null);
            
            var neighboarsId = this.Matrix.GetWalkableNeighbors(searchPoint);

            var neighboars = new List<MatrixField>();
            
            foreach (var i in neighboarsId)
            {
                var neighboar = this.Matrix.GetMatrixField(i, null, null);

                neighboars.Add(neighboar);
                
            }

            // tylko raz sprawszamy pierwszy nody startowy 
            if (currentPoint.Index == this.Matrix.IndexPunktuStartowego)
            {
                this.SelectNodes.Add(this.Matrix.GetMatrixField(this.Matrix.IndexPunktuStartowego, null, null));
                foreach (var item in neighboars)
                {
                    item.ParentField = currentPoint;
                    this.TmpNodes.Add(item);
                }

                this.Search(this.TmpNodes.FirstOrDefault().Index); // go line
            }

            // sparwawdzenie czy ide prosto czy przekatna 

            if (currentPoint.ParentField.PosX == currentPoint.PosX &&
                currentPoint.ParentField.PosY == currentPoint.PosY - 1) // line
            {
                var rightNode = this.Matrix.GetMatrixField(null, currentPoint.PosX, currentPoint.PosY + 1);

                if (rightNode.PosX <= this.Matrix.wierzcholki.Length &&
                    rightNode.PosY <= this.Matrix.wierzcholki.Length)
                {
                    rightNode.ParentField = currentPoint;
                    this.SelectNodes.Add(rightNode);
                    this.Search(rightNode.Index);
                }

            }

            if (currentPoint.ParentField.PosX == currentPoint.PosX - 1 || // diagonal
                currentPoint.ParentField.PosY == currentPoint.PosY - 1)
            {
                var rightDiagonalNode = this.Matrix.GetMatrixField(null, currentPoint.PosX, currentPoint.PosY + 1);
                this.SelectNodes.Add(rightDiagonalNode);
            }

            /*foreach (var nextNode in neighboars)
            {
                if (nextNode.Index == this.Matrix.IndexPunktuKoncowego)
                {
                    // koniec przeszukiwania petli
                    this.SelectNodes.Add(nextNode);
                    nextNode.State = FieldState.Odwiedzony;
                    return true;
                }
                if (nextNode.Index == this.Matrix.IndexPunktuStartowego)
                {
                    this.SelectNodes.Add(nextNode);
                    nextNode.State = FieldState.Odwiedzony;
                    continue;
                }
                // nody odwiedzone sa do ignorowania
                if (this.SelectNodes.Contains(nextNode))
                {
                    continue;
                }

                

                this.SelectNodes.Add(nextNode);
                nextNode.State = FieldState.Odwiedzony;

                if (this.Search(nextNode.Index)) // Note: Recurses back into Search(Node)
                {
                    return true;
                }
            }*/

            return false;
            
        }
    }
}