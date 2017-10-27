using System;
using System.Collections.Generic;
using static System.Windows.Point;

namespace algorithms
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var wierzcholki = WygenerujWierzcholki(20);

            var indexStart = 1;
            var indexStop = 338;
            
            var points = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(15,7),  // A
                new Tuple<int, int>(15,11), // B
                new Tuple<int, int>(11,11), // C
                new Tuple<int, int>(11,7),  // D
            };

            var obstacleSquare = new Obstacle
            {
                ObstacleType = ObstacleType.Open,
                Edges = new Dictionary<string, Tuple<int,int>>()
                {
                    {"A", points[0]},
                    {"B", points[1]},
                    {"C", points[2]},
                    {"D", points[3]}
                },
                MatrixSize = wierzcholki.Length
            };

            var obstacleLine = new Obstacle
            {

            };

            var matrixBfs = new Matrix(wierzcholki)
            {
                obstacle = obstacle
            };

            matrixBfs.GenerateEmptyMatrix();
            
            matrixBfs.IndexPunktuKoncowego = indexStop;
            matrixBfs.IndexPunktuStartowego = indexStart;
            
            matrixBfs.DisplayMatrix(true);

            Console.WriteLine();

            var bfsMatrix = new Bfs(indexStart, indexStop)
            {
                Matrix = matrixBfs
            };

            bfsMatrix.ObliczBfs();

            matrixBfs.DisplayMatrix(false);

            var shortestPath = bfsMatrix.NajkrotszaDroga();
            var shortestPathPoints = shortestPath(indexStop);

            Console.WriteLine("BFS shortest path: {0,2}", string.Join(", ", shortestPathPoints));

            matrixBfs.DisplayMatrixShortestPath(shortestPathPoints);
            matrixBfs.DisplayMatrix(false);

            Console.ReadLine();
        }

        private static int[] WygenerujWierzcholki(int liczbaWierzcholkow)
        {
            var table = new int[liczbaWierzcholkow];

            for (var j = 0; j < liczbaWierzcholkow; j++)
            {
                table[j] = j;
            }

            return table;
        }
    }
}