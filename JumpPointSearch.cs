using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace algorithms
{
    public class JumpPointSearch : AStar
    {
        public Stack<int> ListOfItemToCheck { get; set; } = new Stack<int>();
        public List<int> ListOfVisitedItems { get; set; } = new List<int>();

        public override List<MatrixField> FindPath()
        {
            var searchPath = new List<MatrixField>();

            /*var searchPoint = this.Search(this.Matrix.IndexPunktuStartowego);

            if (searchPoint)
            {
                var point = this.Matrix.GetMatrixField(this.Matrix.IndexPunktuKoncowego, null, null);
                while (point.ParentField != null)
                {
                    searchPath.Add(point);
                    point = point.ParentField;
                }
                searchPath.Reverse();
            }*/

            this.AddItemTCheckQueue(this.Matrix.IndexPunktuStartowego);
            this.AddItemToVisitedList(this.Matrix.IndexPunktuStartowego);

            while (this.ListOfItemToCheck.Count > 0)
            {
                var itemToCheck = this.ListOfItemToCheck.Pop();
                var getNeighbors = this.Matrix.GetWalkableNeighbors(itemToCheck, false, true);
            }

            return searchPath;
        }

        private void AddItemTCheckQueue(int indexPunku)
        {
            this.ListOfItemToCheck.Push(indexPunku);
        }

        private void AddItemToVisitedList(int indexPunktu)
        {
            this.ListOfVisitedItems.Add(indexPunktu);
            var point = this.Matrix.GetMatrixField(indexPunktu, null, null);
            point.State = FieldState.Odwiedzony;
            point.JmpState = FieldState.JMP;
        }

        public override bool Search(int searchPoint)
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