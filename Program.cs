using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var obstacleGenerator = ObstacleGenerator.Instance;
            var wierzcholki = WygenerujWierzcholki(20);
            var indexStart = 1;
            var indexStop = 277;

            var edges = new Dictionary<string, Tuple<int, int>>(){
                {"A", new Tuple<int, int>(19,6)},
                {"B", new Tuple<int, int>(18,15)},
                {"C", new Tuple<int, int>(8,16)},
                {"D", new Tuple<int, int>(8,6)}
            };

            var squareIndex = obstacleGenerator.GenerateObstacle(ObstacleType.Square, ObstacleType.Open, edges);
            
            var matrixBfs = new Matrix(wierzcholki)
            {
                Obstacle = obstacleGenerator.GeneratedObstacles[squareIndex]
            };

            matrixBfs.GenerateEmptyMatrix();
            
            matrixBfs.IndexPunktuKoncowego = indexStop;
            matrixBfs.IndexPunktuStartowego = indexStart;
                        
            var bfsMatrix = new Bfs(indexStart, indexStop)
            {
                Matrix = matrixBfs
            };

            bfsMatrix.ObliczBfs();
            
            var shortestPathPoints = bfsMatrix.NajkrotszaDroga();

            var pathPoints = shortestPathPoints.ToArray();

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