using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms
{
    public class Matrix
    {
        private readonly List<MatrixField> listaPowiazanWierzcholkow;
        private readonly int[] wierzcholki;

        public Matrix(int[] wierzcholki)
        {
            this.wierzcholki = wierzcholki;
            this.listaPowiazanWierzcholkow = new List<MatrixField>();
        }

        public int IndexPunktuStartowego { get; set; }
        public int IndexPunktuKoncowego { get; set; }

        public void GenerateEmptyMatrix()
        {
            var index = 1;
            for (var wiersz = 1; wiersz <= this.wierzcholki.Length; wiersz++)
            {
                for (var kolumna = 1; kolumna <= this.wierzcholki.Length; kolumna++)
                {
                    this.listaPowiazanWierzcholkow.Add(new MatrixField
                    {
                        Index = index,
                        Neighbors = new HashSet<int>(),
                        PosX = wiersz,
                        PosY = kolumna,
                        State = FieldState.Nieodwiedzony,
                        Walkable = FieldState.Odblokowany
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
                point.Walkable = FieldState.Odblokowany;

                var pointItem2 = this.GetMatrixField(tuple.Item2, null, null);
                pointItem2.Neighbors.Add(tuple.Item1);
                pointItem2.Walkable = FieldState.Odblokowany;
            }
        }

        public void DisplayMatrix(bool displayIndex)
        {
            var kolumnIndex = 1;

            Console.WriteLine();
            for (int i = 1; i <= this.wierzcholki.Length; i++)
            {
                Console.Write("{0,4}", i);
            }
            Console.WriteLine();
            for (int i = 1; i <= this.wierzcholki.Length; i++)
            {
                Console.Write("----");
            }
            Console.WriteLine();

            for (var wiersz = 1; wiersz <= this.wierzcholki.Length; wiersz++)
            {
                for (var kolumna = 1; kolumna <= this.wierzcholki.Length; kolumna++)
                {
                    var currentPoint = this.GetMatrixField(null, wiersz, kolumna);

                    if (displayIndex)
                    {
                        Console.Write("{0,4}", currentPoint.Index);
                    }
                    else
                    {
                        if (currentPoint.Index.Equals(this.IndexPunktuKoncowego))
                        {
                            Console.Write("{0,2}", "E");
                        }
                        else if (currentPoint.Index.Equals(this.IndexPunktuStartowego))
                        {
                            Console.Write("{0,2}", "S");
                        }
                        else
                        {
                            if (currentPoint.Walkable == FieldState.Zablokowany)
                            {
                                switch (currentPoint.Walkable)
                                {
                                    case FieldState.Zablokowany:
                                        Console.Write("{0,2}", "-");
                                        break;
                                    case FieldState.Odblokowany:
                                        Console.Write("{0,2}", "+");
                                        break;
                                }
                            }
                            else
                            {
                                switch (currentPoint.State)
                                {
                                    case FieldState.Nieodwiedzony:
                                        Console.Write("{0,2}",0);
                                        break;
                                    case FieldState.Odwiedzony:
                                        Console.Write("{0,2}",1);
                                        break;
                                    case FieldState.ShortestPath:
                                        ColoredConsoleWrite(ConsoleColor.Red, "->");
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("| {0}\t", kolumnIndex);
                ++kolumnIndex;
            }

            Console.WriteLine();
        }

        public List<int> GetWalkableNeighbors(int indexPunktu)
        {
            var currentPoint = this.GetMatrixField(indexPunktu, null, null);
            var walkbaleNeighbors = new List<int>();

            // Up
            if (this.PointIsWalkableAt(currentPoint.PosX, currentPoint.PosY - 1))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX, currentPoint.PosY - 1);
                walkbaleNeighbors.Add(this.GetMatrixField(null, currentPoint.PosX, currentPoint.PosY - 1).Index);
            }

            // Down
            if (this.PointIsWalkableAt(currentPoint.PosX, currentPoint.PosY + 1))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX, currentPoint.PosY + 1);
                walkbaleNeighbors.Add(this.GetMatrixField(null, currentPoint.PosX, currentPoint.PosY + 1).Index);
            }

            // Right
            if (this.PointIsWalkableAt(currentPoint.PosX + 1, currentPoint.PosY))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX + 1, currentPoint.PosY);
                walkbaleNeighbors.Add(this.GetMatrixField(null, currentPoint.PosX + 1, currentPoint.PosY).Index);
            }

            // Left
            if (this.PointIsWalkableAt(currentPoint.PosX - 1, currentPoint.PosY))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX - 1, currentPoint.PosY - 1);
                walkbaleNeighbors.Add(this.GetMatrixField(null, currentPoint.PosX - 1, currentPoint.PosY).Index);
            }

            // ↖
            if (this.PointIsWalkableAt(currentPoint.PosX - 1, currentPoint.PosY - 1))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX - 1, currentPoint.PosY - 1);
                walkbaleNeighbors.Add(this.GetMatrixField(null, currentPoint.PosX - 1, currentPoint.PosY - 1).Index);
            }

            // ↗
            if (this.PointIsWalkableAt(currentPoint.PosX + 1, currentPoint.PosY - 1))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX + 1, currentPoint.PosY - 1);
                walkbaleNeighbors.Add(this.GetMatrixField(null, currentPoint.PosX + 1, currentPoint.PosY - 1).Index);
            }

            // ↘
            if (this.PointIsWalkableAt(currentPoint.PosX + 1, currentPoint.PosY + 1))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX + 1, currentPoint.PosY + 1);
                walkbaleNeighbors.Add(this.GetMatrixField(null, currentPoint.PosX + 1, currentPoint.PosY + 1).Index);
            }

            // ↙
            if (this.PointIsWalkableAt(currentPoint.PosX - 1, currentPoint.PosY + 1))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.PosX - 1, currentPoint.PosY + 1);
                walkbaleNeighbors.Add(this.GetMatrixField(null, currentPoint.PosX - 1, currentPoint.PosY + 1).Index);
            }

            return walkbaleNeighbors;
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
            return neighborPosX >= 1 && neighborPosX <= this.wierzcholki.Length && neighborPosY >= 1 &&
                   neighborPosY <= this.wierzcholki.Length;
        }

        public HashSet<int> GetAllNeighbors(int index)
        {
            var matrixField = this.GetMatrixField(index, null, null);

            return matrixField.Neighbors;
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

            if (index != null)
            {
                result = this.listaPowiazanWierzcholkow.First(item => item.Index.Equals(index));
            }

            if (posX != null && posY != null)
            {
                if (this.PointIsInsideMatrix((int)posX, (int)posY))
                {
                    result = this.listaPowiazanWierzcholkow
                        .First(item => item.PosX.Equals(posX) && item.PosY.Equals(posY));
                }
            }

            return result;
        }

        public bool IsItemAsMatrixPoint(int indexPunktuStartowego)
        {
            var point = this.GetMatrixField(indexPunktuStartowego, null, null);

            return this.PointIsInsideMatrix(point.PosX, point.PosY);
        }

        public void SelectVisitedPoint(int indexPunktu)
        {
            var point = this.GetMatrixField(indexPunktu, null, null);

            point.State = FieldState.Odwiedzony;
        }

        public void AddObstacle(List<int> wierzcholkiPrzeszkody)
        {
            foreach (var punkt in wierzcholkiPrzeszkody)
            {
                var point = this.GetMatrixField(punkt, null, null);
                point.Walkable = FieldState.Zablokowany;
            }
        }

        public void DisplayMatrixShortestPath(IEnumerable<int> shortestPathPoints)
        {
            foreach (var shortestPathPoint in shortestPathPoints)
            {
                var point = this.GetMatrixField(shortestPathPoint, null, null);
                point.State = FieldState.ShortestPath;
            }
        }

        public static void ColoredConsoleWrite(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write("{0,2}", text);
            Console.ForegroundColor = originalColor;
        }

    }
}