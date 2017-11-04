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
            this.Obstacle = new Obstacle();
        }

        public Obstacle Obstacle { private get; set; }
        public bool IsObstacleOnTheMatrix { get; set; }
        public int IndexPunktuStartowego { get; set; }
        public int IndexPunktuKoncowego { get; set; }

        public void GenerateEmptyMatrix()
        {
            this.listaPowiazanWierzcholkow.Clear();

            var index = 1;
            for (var wiersz = 1; wiersz <= this.wierzcholki.Length; wiersz++)
            {
                for (var kolumna = 1; kolumna <= this.wierzcholki.Length; kolumna++)
                {
                    var point = new MatrixField
                    {
                        Index = index,
                        Neighbors = new HashSet<int>(),
                        PosX = wiersz,
                        PosY = kolumna,
                        State = FieldState.Nieodwiedzony,
                        Walkable = FieldState.Odblokowany
                    };

                    this.listaPowiazanWierzcholkow.Add(point);

                    index++;
                }
            }

            for (var wiersz = 1; wiersz <= this.wierzcholki.Length; wiersz++)
            {
                for (var kolumna = 1; kolumna <= this.wierzcholki.Length; kolumna++)
                {
                    var point = this.GetMatrixField(null, wiersz, kolumna);

                    if (this.PointIsInsideObstacle(kolumna, wiersz))
                    {
                        point.Walkable = FieldState.Zablokowany;
                    }
                }
            }
        }

        public void DisplayMatrix(bool displayIndex)
        {
            var kolumnIndex = 1;

            Console.WriteLine();
            for (var i = 1; i <= this.wierzcholki.Length; i++)
            {
                Console.Write("{0,2} ", i);
            }
            Console.WriteLine();
            for (var i = 1; i <= this.wierzcholki.Length; i++)
            {
                Console.Write("---");
            }
            Console.WriteLine();

            for (var wiersz = 1; wiersz <= this.wierzcholki.Length; wiersz++)
            {
                for (var kolumna = 1; kolumna <= this.wierzcholki.Length; kolumna++)
                {
                    var currentPoint = this.GetMatrixField(null, wiersz, kolumna);

                    if (displayIndex)
                    {
                        Console.Write(" {0}:({1},{2})", currentPoint.Index, currentPoint.PosX,
                            currentPoint.PosY);
                    }
                    else
                    {
                        if (currentPoint.Index.Equals(this.IndexPunktuKoncowego))
                        {
                            ColoredConsoleWrite(ConsoleColor.Blue, "E");
                        }
                        else if (currentPoint.Index.Equals(this.IndexPunktuStartowego))
                        {
                            ColoredConsoleWrite(ConsoleColor.Yellow, "S");
                        }
                        else
                        {
                            if (currentPoint.Walkable == FieldState.Zablokowany)
                            {
                                switch (currentPoint.Walkable)
                                {
                                    case FieldState.Zablokowany:
                                        ColoredConsoleWrite(ConsoleColor.DarkBlue, "-");
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
                                        Console.Write("{0,2}", 0);
                                        break;
                                    case FieldState.Odwiedzony:
                                        ColoredConsoleWrite(ConsoleColor.DarkYellow, "1");
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

        public List<MatrixField> GetWalkableNeighbors(int indexPunktu, bool isAStartAlgorithm)
        {
            var walkableNodes = new List<MatrixField>();
            var currentPoint = this.GetMatrixField(indexPunktu, null, null);
            var walkableNeighbors = this.GetWalkableNeighbors(indexPunktu);

            this.InitGhForMatrixFields(walkableNeighbors);
            
            foreach (var walkableNeighbor in walkableNeighbors)
            {
                var walkableNeighborPoint = this.GetMatrixField(walkableNeighbor, null, null);

                if (walkableNeighborPoint.State == FieldState.Odwiedzony)
                {
                    continue;
                }

                if (walkableNeighborPoint.Walkable == FieldState.Zablokowany)
                {
                    continue;
                }

                if (indexPunktu == this.IndexPunktuStartowego)
                {
                    walkableNeighborPoint.ParentField = currentPoint;
                    walkableNodes.Add(walkableNeighborPoint);
                }

                if (walkableNeighborPoint.State == FieldState.Odwiedzony)
                {
                    walkableNeighborPoint.ParentField = currentPoint;
                    walkableNeighborPoint.State = FieldState.Nieodwiedzony;
                    walkableNodes.Add(walkableNeighborPoint);
                }
                else
                {
                    var traversalCost = this.GetTravelsalCostFromParent(walkableNeighbor);
                    var tempG = currentPoint.G + traversalCost;
                    if (tempG < walkableNeighborPoint.G)
                    {
                        walkableNeighborPoint.ParentField = currentPoint;
                        walkableNodes.Add(walkableNeighborPoint);
                    }
                }
            }

            return walkableNodes;
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

        public void DisplayMatrixShortestPath(IEnumerable<int> shortestPathPoints)
        {
            foreach (var shortestPathPoint in shortestPathPoints)
            {
                var point = this.GetMatrixField(shortestPathPoint, null, null);
                point.State = FieldState.ShortestPath;
            }
        }

        public void SelectShortestPath(IEnumerable<int> shortestPath)
        {
            foreach (var pointShort in shortestPath)
            {
                var point = this.GetMatrixField(pointShort, null, null);
                point.State = FieldState.ShortestPath;
            }
        }

        private void InitGhForMatrixFields(List<int> walkableNeighbors)
        {
            foreach (var walkableNeighbor in walkableNeighbors)
            {
                var point = this.GetMatrixField(walkableNeighbor, null, null);
                point.G = this.GetTrabelsalCostFromStartPoint(point.Index);
                point.H = this.GetTrabelsalCostToEndPoint(point.Index);
            }
        }

        private double GetTravelsalCostFromParent(int walkableNeighbor)
        {
            var currentPoint = this.GetMatrixField(walkableNeighbor, null, null);
            var parentPoint = this.GetMatrixField(currentPoint.ParentField.Index, null, null);

            if (walkableNeighbor == this.IndexPunktuStartowego)
            {
                return 0;
            }

            var distance = GetDistance(currentPoint.PosX, parentPoint.PosX, currentPoint.PosY, parentPoint.PosY);

            return distance;
        }

        private float GetTrabelsalCostToEndPoint(int point)
        {
            var currentPoint = this.GetMatrixField(point, null, null);
            var endPoint = this.GetMatrixField(this.IndexPunktuKoncowego, null, null);

            var distance = GetDistance(currentPoint.PosX, currentPoint.PosY, endPoint.PosX, endPoint.PosY);

            return distance;
        }

        private float GetTrabelsalCostFromStartPoint(int point)
        {
            var currentPoint = this.GetMatrixField(point, null, null);
            var startPoint = this.GetMatrixField(this.IndexPunktuStartowego, null, null);

            var distance = GetDistance(startPoint.PosX, startPoint.PosY, currentPoint.PosX, currentPoint.PosY);

            return distance;
        }

        private bool PointIsWalkableAt(int neighborPosX, int neighborPosY)
        {
            if (this.PointIsInsideMatrix(neighborPosX, neighborPosY) &&
                !this.PointIsInsideObstacle(neighborPosY, neighborPosX))
            {
                var point = this.GetMatrixField(null, neighborPosX, neighborPosY);

                if (point.State != FieldState.Zablokowany)
                {
                    return true;
                }
            }

            return false;
        }

        private bool PointIsInsideObstacle(int neighborPosY, int neighborPosX)
        {
            if (this.IsObstacleOnTheMatrix.Equals(true))
            {
                return this.Obstacle.CheckIfPointIsInsideObstacle(neighborPosX, neighborPosY);
            }

            return false;
        }

        private bool PointIsInsideMatrix(int neighborPosX, int neighborPosY)
        {
            return neighborPosX >= 1 && neighborPosX <= this.wierzcholki.Length && neighborPosY >= 1 &&
                   neighborPosY <= this.wierzcholki.Length;
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

        public MatrixField GetMatrixField(int? index, int? posX, int? posY)
        {
            var result = new MatrixField();

            if (index != null)
            {
                result = this.listaPowiazanWierzcholkow.FirstOrDefault(item => item.Index.Equals(index));
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

        private static void ColoredConsoleWrite(ConsoleColor color, string text)
        {
            var originalColor = Console.BackgroundColor;
            Console.BackgroundColor = color;
            Console.Write("{0,2}", text);
            Console.BackgroundColor = originalColor;
        }

        private static float GetDistance(double x1, double y1, double x2, double y2)
        {
            var result = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            return (float)result;
        }

        public void CalculateF(List<MatrixField> neighboars)
        {
            this.CalculateG(neighboars);
            this.CalculateH(neighboars);
        }

        private void CalculateG(List<MatrixField> neighboars)
        {
            foreach (var matrixField in neighboars)
            {
                var cost = this.GetTrabelsalCostFromStartPoint(matrixField.Index);
                matrixField.G = cost;
            }
        }

        private void CalculateH(List<MatrixField> neighboars)
        {
            foreach (var matrixField in neighboars)
            {
                var cost = this.GetTrabelsalCostToEndPoint(matrixField.Index);
                matrixField.H = cost;
            }
        }
    }
}