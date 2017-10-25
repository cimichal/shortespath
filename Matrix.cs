using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms
{
    public class Matrix
    {
        private int[] wierzcholki;
        private List<MatrixField> listaPowiazanWierzcholkow;

        public Matrix(int[] wierzcholki)
        {
            this.wierzcholki = wierzcholki;
            this.listaPowiazanWierzcholkow = new List<MatrixField>();
        }

        public void GenerateEmptyMatrix()
        {
            var index = 1;
            for (var wiersz = 1; wiersz <= this.wierzcholki.Length; wiersz++)
            {
                for (var kolumna = 1; kolumna <= this.wierzcholki.Length; kolumna++)
                {
                    this.listaPowiazanWierzcholkow.Add(new MatrixField()
                    {
                        Index = index,
                        Neighbors = new HashSet<int>(),
                        PosX = wiersz,
                        PosY = kolumna
                    });
                    index++;
                }
            }
        }

        public void AddEdge(List<Tuple<int, int>> listaPolaczen)
        {
            foreach (var tuple in listaPolaczen)
            {
                var point = this.GetMatrixField(tuple.Item1, null, null);
                point.Neighbors.Add(tuple.Item2);

                var pointItem2 = this.GetMatrixField(tuple.Item2, null, null);
                pointItem2.Neighbors.Add(tuple.Item1);
            }
        }

        public void DisplayMatrix()
        {
            var index = 1;
            var kolumnIndex = 1;
            Console.WriteLine("1 2 3 4 5 6 7 8 9 10");

            for (var wiersz = 1; wiersz <= this.wierzcholki.Length; wiersz++)
            {
                for (var kolumna = 1; kolumna <= this.wierzcholki.Length; kolumna++)
                {
                    var currentPoint = this.GetMatrixField(null, wiersz, kolumna);

                    if (currentPoint.State.Equals(FieldState.Nieodwiedzony))
                    {
                        Console.Write("0 ");
                    }
                    else if (currentPoint.State.Equals(FieldState.Odwiedzony))
                    {
                        Console.Write("1 ");
                    }

                    ++index;
                }
                Console.WriteLine("| {0}\t", kolumnIndex);
                ++kolumnIndex;
            }
        }

        public void AddNeighbors(int indexPunktu)
        {
            var currentPoint = this.GetMatrixField(indexPunktu, null, null);

            // Up
            if (this.PointIsWalkableAt(currentPoint.PosX, currentPoint.PosY - 1))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX, currentPoint.PosY - 1);
            }

            // Down
            if (this.PointIsWalkableAt(currentPoint.PosX, currentPoint.PosY + 1))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX, currentPoint.PosY + 1);
            }

            // Right
            if (this.PointIsWalkableAt(currentPoint.PosX + 1, currentPoint.PosY))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX + 1, currentPoint.PosY);
            }

            // Left
            if (this.PointIsWalkableAt(currentPoint.PosX - 1, currentPoint.PosY))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX - 1, currentPoint.PosY - 1);
            }
            
        }

        private bool PointIsWalkableAt(int neighborPosX, int neighborPosY)
        {
            if (this.PointIsInsideMatrix(neighborPosX, neighborPosY))
            {
                var point = this.GetMatrixField(null, neighborPosX, neighborPosY);

                if (point.State != FieldState.Zablokowany)
                {
                    return true;
                }
            }

            return false;
        }

        private bool PointIsInsideMatrix(int neighborPosX, int neighborPosY)
        {
            return (neighborPosX >= 1 && neighborPosX < this.wierzcholki.Length) &&
                (neighborPosY >= 1 && neighborPosY < this.wierzcholki.Length);
        }

        public HashSet<int> GetAllNeighbors(int index)
        {
            var matrixField = this.GetMatrixField(index, null, null);

            if (matrixField.Neighbors != null)
            {
                return matrixField.Neighbors;
            }

            return null;
        }

        private void AddNewNeighbors(int indexPunktu, int neighborPosX, int neighborPosY)
        {
            var currentPoint = this.GetMatrixField(indexPunktu, null, null);

            var neighborPoint = this.GetMatrixField(null, neighborPosX, neighborPosY);

            if (neighborPoint != null)
            {
                currentPoint.Neighbors.Add(neighborPoint.Index);
            }
        }

        private MatrixField GetMatrixField(int? index, int? posX, int? posY)
        {
            var result = new MatrixField();
            
            if(index != null)
            {
                result = this.listaPowiazanWierzcholkow.Where(item => item.Index.Equals(index)).First();
            }

            if(posX != null && posY != null)
            {
                result = this.listaPowiazanWierzcholkow
                    .Where(item => (item.PosX.Equals(posX) && item.PosY.Equals(posY)))
                    .First();
            }
            
            return result;
        }

    }
}

