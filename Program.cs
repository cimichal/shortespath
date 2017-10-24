using System;
using System.Collections.Generic;

namespace algorithms
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region graf

            var wierzcholki = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var paryWierzcholkow = new[]
            {
                Tuple.Create(1, 2),
                Tuple.Create(1, 3),
                Tuple.Create(2, 4),
                Tuple.Create(3, 5),
                Tuple.Create(3, 6),
                Tuple.Create(4, 7),
                Tuple.Create(5, 7),
                Tuple.Create(5, 8),
                Tuple.Create(5, 6),
                Tuple.Create(8, 9),
                Tuple.Create(9, 10),
                Tuple.Create(8, 10)
            };

            var graf = new Graph<int>(wierzcholki, paryWierzcholkow);
            var punktStartowy = 1;

            var dfs = new Dfs<int>(graf, punktStartowy);
            //var bfs = new Bfs<int>(graf, punktStartowy);

            Console.WriteLine(dfs.ObliczDfs<int>());
            //Console.WriteLine(bfs.ObliczBfs<int>());

            //var najkrotszaDroga = bfs.ObliczNajkrotszaDroge();

            //Console.WriteLine("Najkrotsza droga: {0}", string.Join(", ", najkrotszaDroga(4)));

            #endregion

            #region matrix

            var wierzcholkiMatrix = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var matrix = new Matrix(wierzcholkiMatrix);
            matrix.GenerateEmptyMatrix();

            matrix.DisplayMatrix();

            matrix.AddEdge(new List<Tuple<int, int>>()
            {
                Tuple.Create(1,2),
                Tuple.Create(2,9),
                Tuple.Create(8,1),
                Tuple.Create(2,3),
                Tuple.Create(2,8),
                Tuple.Create(6,7),
                Tuple.Create(5,5),
                Tuple.Create(3,1),
                Tuple.Create(3,1),
                Tuple.Create(3,7),
                Tuple.Create(3,7),
                Tuple.Create(5,1),
                Tuple.Create(3,3),
                Tuple.Create(3,6),
                Tuple.Create(10,8)
            });

            Console.WriteLine();
            Console.WriteLine();

            matrix.DisplayMatrix();

            Bfs bfsMatrix = new Bfs(1)
            {
                Matrix = matrix
            };

            bfsMatrix.ObliczBfs();
            #endregion

            Console.ReadLine();
        }
    }
}