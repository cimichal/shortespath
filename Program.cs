using System;
using System.Collections.Generic;

namespace algorithms
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region Init
            var obstacleGenerator = ObstacleGenerator.Instance;
            var wierzcholki = WygenerujWierzcholki(20);
            var indexStart = 1;
            var indexStop = 277;

            var edges = new Dictionary<string, Tuple<int, int>>
            {
                {"A", new Tuple<int, int>(19, 6)},
                {"B", new Tuple<int, int>(18, 15)},
                {"C", new Tuple<int, int>(8, 16)},
                {"D", new Tuple<int, int>(8, 6)}
            };

            var squareIndex = obstacleGenerator.GenerateObstacle(ObstacleType.Square, ObstacleType.Close, edges);

            var lineIndex = obstacleGenerator.GenerateObstacle(ObstacleType.Line, ObstacleType.Open,
                new Dictionary<string, Tuple<int, int>>
                {
                    {"S", new Tuple<int, int>(1, 10)},
                    {"ST", new Tuple<int, int>(12, 10)}
                });

            #endregion

            #region Matrix

            var matrix = new Matrix(wierzcholki)
            {
                IsObstacleOnTheMatrix = false,
                Obstacle = obstacleGenerator.GeneratedObstacles[squareIndex],
                IndexPunktuKoncowego = indexStop,
                IndexPunktuStartowego = indexStart
            };

            matrix.GenerateEmptyMatrix();

            #endregion

            #region BFS

            Console.WriteLine("BFS");

            var bfs = new Bfs(indexStart, indexStop)
            {
                Matrix = matrix
            };

            bfs.ObliczBfs();

            var shortestPathPointsBfs = bfs.NajkrotszaDroga().ToArray();
            
            Console.WriteLine("BFS shortest path: {0,2}", string.Join(", ", shortestPathPointsBfs));

            matrix.DisplayMatrixShortestPath(shortestPathPointsBfs);
            matrix.DisplayMatrix(false);

            #endregion

            #region DFS

            Console.WriteLine("DFS");

            var dfs = new Dfs(indexStart, indexStop)
            {
                Matrix = matrix
            };

            matrix.GenerateEmptyMatrix();

            dfs.ObliczDfs();

            var shortestPathPointsDfs = dfs.NajkrotszaDroga().ToArray();

            matrix.DisplayMatrixShortestPath(shortestPathPointsDfs);
            matrix.DisplayMatrix(false);

            #endregion

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