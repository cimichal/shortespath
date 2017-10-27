using System;
using System.Collections.Generic;
using System.Linq;
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
            
            var pointsSquare = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(15,7),  // A
                new Tuple<int, int>(15,11), // B
                new Tuple<int, int>(11,11), // C
                new Tuple<int, int>(11,7),  // D
            };

            var points = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(19,6),  // A
                new Tuple<int, int>(18,15), // B
                new Tuple<int, int>(8,16), // C
                new Tuple<int, int>(8,6),  // D
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
                }
            };
            
            var matrixBfs = new Matrix(wierzcholki)
            {
                Obstacle = obstacleSquare
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

            var pathPoints = shortestPathPoints as int[] ?? shortestPathPoints.ToArray();

            Console.WriteLine("BFS shortest path: {0,2}", string.Join(", ", pathPoints));

            matrixBfs.DisplayMatrixShortestPath(pathPoints);
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