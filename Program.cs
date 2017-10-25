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

            matrix.DisplayMatrix(true);

            matrix.AddEdge(new List<Tuple<int, int>>()
            {
                Tuple.Create(1,12),
                Tuple.Create(2,33),
                Tuple.Create(8,66),
                Tuple.Create(50,3),
                Tuple.Create(2,8),
                Tuple.Create(6,50),
                Tuple.Create(5,5),
                Tuple.Create(69,1),
                Tuple.Create(3,1),
                Tuple.Create(3,20),
                Tuple.Create(3,33),
                Tuple.Create(67,1),
                Tuple.Create(23,3),
                Tuple.Create(3,45),
                Tuple.Create(10,8)
            });

            var obstacle = new List<int>()
            {
                33, 34, 35, 36, 46, 56, 55, 54, 53, 43
            };

            Console.WriteLine();
            Console.WriteLine();

            matrix.DisplayMatrix(false);

            Bfs bfsMatrix = new Bfs(1)
            {
                Matrix = matrix
            };

            matrix.AddObstacle(obstacle);

            bfsMatrix.ObliczBfs();
            matrix.DisplayMatrix(false);
            
            Console.ReadLine();
        }
    }
}