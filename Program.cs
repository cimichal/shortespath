using System;
using System.Collections.Generic;

namespace algorithms
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var wierzcholki = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var matrix = new Matrix(wierzcholki);

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

            Console.ReadLine();
        }
    }
}