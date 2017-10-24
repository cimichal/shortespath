using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms
{
    public class Matrix
    {
        public Matrix(int[] wierzcholki)
        {
            this.Wierzcholki = wierzcholki;
            this.ListaPowiazan = new List<(int, int, int, HashSet<int>, FieldState)>();
            this.TablicaPowiazan = new int[wierzcholki.Length,wierzcholki.Length];
        }

        private int[] Wierzcholki { get; set; }
        private List<(int index, int pozX, int pozY, HashSet<int> sasiedzi, FieldState Stan)> ListaPowiazan  { get; set; }
        private int[,] TablicaPowiazan { get; set; }

        public void GenerateEmptyMatrix()
        {
            var index = 1;
            for (var wiersz = 1; wiersz <= this.Wierzcholki.Length; wiersz++)
            {
                for (var kolumna = 1; kolumna <= this.Wierzcholki.Length; kolumna++)
                {
                    this.ListaPowiazan.Add(new ValueTuple<int, int, int, HashSet<int>, FieldState>
                    {
                        Item1 = index,
                        Item2 = wiersz, 
                        Item3 = kolumna,
                        Item4 = new HashSet<int>(),
                        Item5 = FieldState.Nieodwiedzony
                    });
                    index++;
                }
            }
        }

        public void AddEdge(List<Tuple<int,int>> listaPolaczen)
        {
            foreach (var tuple in listaPolaczen)
            {
                var wierzcholek = this.ListaPowiazan.FirstOrDefault(item => item.pozX.Equals(tuple.Item1) && item.pozY.Equals(tuple.Item2));
                this.ListaPowiazan[wierzcholek.index] = new ValueTuple<int, int, int, HashSet<int>, FieldState>
                {
                    Item1 = wierzcholek.index,
                    Item2 = wierzcholek.pozX,
                    Item3 = wierzcholek.pozY,
                    Item4 = wierzcholek.sasiedzi,
                    Item5 = FieldState.Odwiedzony
                };
            }
        }

        public void DisplayMatrix()
        {
            var index = 1;
            var kolumnIndex = 1;
            Console.WriteLine("1 2 3 4 5 6 7 8 9 10");

            for (var wiersz = 1; wiersz <= this.Wierzcholki.Length; wiersz++)
            {
                for (var kolumna = 1; kolumna <= this.Wierzcholki.Length; kolumna++)
                {
                    var aktulanyWierzcholek =
                        this.ListaPowiazan.FirstOrDefault(item => item.pozX.Equals(wiersz) && item.pozY.Equals(kolumna));

                    if (aktulanyWierzcholek.Stan.Equals(FieldState.Nieodwiedzony))
                    {
                        Console.Write("0 ");
                    }
                    else if (aktulanyWierzcholek.Stan.Equals(FieldState.Odwiedzony))
                    {
                        Console.Write("1 ");
                    }

                    ++index;
                }
                Console.WriteLine("| {0}\t", kolumnIndex);
                ++kolumnIndex;
            }
        }

        public IEnumerable<int> AddNeighbors(int indexPunktu)
        {
            var s0 = false;
            var s1 = false;
            var s2 = false;
            var s3 = false;

            var currentPoint = this.ListaPowiazan.FirstOrDefault(p => p.index.Equals(indexPunktu));

            // Up
            if (this.PointIsWalkableAt(currentPoint.pozX, currentPoint.pozY - 1))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.pozX, currentPoint.pozY - 1);
                s0 = true;
            }
            
            // Down
            if (this.PointIsWalkableAt(currentPoint.pozX, currentPoint.pozY + 1))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.pozX, currentPoint.pozY + 1);
                s1 = true;
            }
            
            // Right
            if (this.PointIsWalkableAt(currentPoint.pozX + 1, currentPoint.pozY))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.pozX + 1, currentPoint.pozY);
                s2 = true;
            }
            
            // Left
            if (this.PointIsWalkableAt(currentPoint.pozX - 1, currentPoint.pozY))
            {
                this.AddNewNeighbors(indexPunktu, currentPoint.pozX - 1, currentPoint.pozY - 1);
                s3 = true;
            }

            return this.ListaPowiazan.FirstOrDefault(p => p.index.Equals(indexPunktu)).sasiedzi.ToList();
        }

        private bool PointIsWalkableAt(int neighborPosX, int neighborPosY)
        {
            if (this.PointIsInsideMatrix(neighborPosX, neighborPosY))
            {
                var point = this.ListaPowiazan.First(p =>
                    p.pozX.Equals(neighborPosX) && p.pozY.Equals(neighborPosY));

                if (point.Stan != FieldState.Zablokowany)
                {
                    return true;
                }
            }

            return false;
        }

        private bool PointIsInsideMatrix(int neighborPosX, int neighborPosY)
        {
            return (neighborPosX >= 1 && neighborPosX < this.Wierzcholki.Length) &&
                (neighborPosY >= 1 && neighborPosY < this.Wierzcholki.Length);
        }

        private void AddNewNeighbors(int indexPunktu, int neighborPosX, int neighborPosY)
        {
            var currentPoint = this.ListaPowiazan.FirstOrDefault(p => p.index.Equals(indexPunktu));
            var neighborPoint =
                this.ListaPowiazan.FirstOrDefault(p => p.pozX.Equals(neighborPosX) && p.pozY.Equals(neighborPosY));

            var neighborsList = currentPoint.sasiedzi;
            neighborsList.Add(neighborPoint.index);

            this.UpdateAdjanceList(indexPunktu, new ValueTuple<int, int, int, HashSet<int>, FieldState>()
            {
                Item1 = indexPunktu,
                Item2 = currentPoint.pozX,
                Item3 = currentPoint.pozY,
                Item4 = neighborsList,
                Item5 = FieldState.Nieodwiedzony
            });
        }

        private void UpdateAdjanceList(int index, (int, int, int, HashSet<int>, FieldState) newTuple)
        {
            this.ListaPowiazan[index] = newTuple;
        }

    }

    public class MatrixFiled<T>
    {
        public FieldState State { get; set; }
        public int IndexPoziom { get; set; }
        public int IndexPion { get; set; }
        public IEnumerable<T> ListaSasiadow { get; set; }
    }

    public enum FieldState
    {
        Nieodwiedzony,
        Odwiedzony,
        Zablokowany
    }
}