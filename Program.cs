using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace algorithms
{
    internal class Program
    {
        private static void Main(string[] args)                                 
        {
            var obstacleGenerator = ObstacleGenerator.Instance;
            var wierzcholki = WygenerujWierzcholki(20);

            var indexStart = 21;
            var indexStop = 377;

            var edges = new Dictionary<string, Tuple<int, int>>
            {
                {"A", new Tuple<int, int>(19, 6)},
                {"B", new Tuple<int, int>(19, 16)},
                {"C", new Tuple<int, int>(8, 16)},
                {"D", new Tuple<int, int>(8, 6)}
            };

            /* Obstacles */
            var labirynt = new int[20, 20]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
                {0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0},
                {0,1,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0},
                {0,1,1,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0},
                {0,0,1,0,0,0,0,1,1,1,1,1,1,0,0,1,0,0,0,0},
                {0,0,1,0,1,1,0,1,0,0,0,0,1,0,0,1,0,0,0,0},
                {0,0,1,0,1,0,0,1,0,1,0,0,1,0,0,1,0,0,0,0},
                {0,0,1,1,1,1,1,1,0,1,0,0,1,0,0,1,0,0,0,0},
                {0,0,0,0,0,0,1,0,0,1,1,1,1,0,0,1,0,0,0,0},
                {0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,1,0,0,0,0},
                {0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,1,0,0,0,0},
                {0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0},
                {0,0,1,1,1,1,1,0,0,0,1,0,0,0,0,1,0,0,0,0},
                {0,0,1,0,0,0,1,0,0,0,1,0,1,0,0,1,1,1,0,0},
                {0,0,1,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0},
                {0,0,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            };

            var labiryntIndex = obstacleGenerator.GenerateObstacle(ObstacleType.Labirynt, ObstacleType.Open, labirynt);

            var squareIndex = obstacleGenerator.GenerateObstacle(ObstacleType.Square, ObstacleType.Close, edges);

            var lineIndex = obstacleGenerator.GenerateObstacle(ObstacleType.Line, ObstacleType.Open,
                new Dictionary<string, Tuple<int, int>>
                {
                    {"S1", new Tuple<int, int>(1, 10)},
                    {"ST1", new Tuple<int, int>(19, 10)}
                });

            var threeLineIndex = obstacleGenerator.GenerateObstacle(ObstacleType.Traingle, ObstacleType.Open,
                new Dictionary<string, Tuple<int, int>>
                {
                    {"S1", new Tuple<int, int>(1, 10)},
                    {"ST1", new Tuple<int, int>(16, 10)},
                    {"S2", new Tuple<int, int>(1, 12)},
                    {"ST2", new Tuple<int, int>(15, 12)},
                    {"S3", new Tuple<int, int>(1, 5)},
                    {"ST3", new Tuple<int, int>(19, 5)}
                });

            var openObstacle = obstacleGenerator.GenerateObstacle(ObstacleType.SquareOpen, ObstacleType.Open, edges);
            /* End obstacles */

            var emptyMatrix = new Matrix(wierzcholki)
            {
                IsObstacleOnTheMatrix = false,
                IndexPunktuKoncowego = indexStop,
                IndexPunktuStartowego = indexStart
            };
            ExecuteAlgorithms(emptyMatrix, indexStart, indexStop);

            foreach (var obstacle in obstacleGenerator.GeneratedObstacles)
            {
                Console.WriteLine("Obstacle");

                var matrix = new Matrix(wierzcholki)
                {
                    IsObstacleOnTheMatrix = true,
                    Obstacle = obstacleGenerator.GeneratedObstacles[obstacle.Key],
                    IndexPunktuKoncowego = indexStop,
                    IndexPunktuStartowego = indexStart
                };
                ExecuteAlgorithms(matrix, indexStart, indexStop);

                Console.WriteLine();
                Console.WriteLine();
            }
            

            
            Console.ReadLine();
        }

        public static void ExecuteAlgorithms(Matrix matrix, int indexStart, int indexStop)
        {
            matrix.GenerateEmptyMatrix();


            #region Diagnostics

            var executionBFS = new Stopwatch();
            var executionDFS = new Stopwatch();
            var executionAstar = new Stopwatch();
            var executionBest = new Stopwatch();

            var executionBFS_SP = new Stopwatch();
            var executionDFS_SP = new Stopwatch();

            #endregion

            #region BFS

            Console.WriteLine("BFS");

            var bfs = new Bfs(indexStart, indexStop)
            {
                Matrix = matrix
            };

            executionBFS.Start();
            bfs.ObliczBfs();
            executionBFS.Stop();

            executionBFS_SP.Start();
            var shortestPathPointsBfs = bfs.NajkrotszaDroga().ToArray();
            executionBFS_SP.Stop();

            //Console.WriteLine("BFS shortest path: {0,2}", string.Join(", ", shortestPathPointsBfs));

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

            executionDFS.Start();
            dfs.ObliczDfs();
            executionDFS.Stop();

            executionDFS_SP.Start();
            var shortestPathPointsDfs = dfs.NajkrotszaDroga().ToArray();
            executionDFS_SP.Stop();

            matrix.DisplayMatrixShortestPath(shortestPathPointsDfs);
            matrix.DisplayMatrix(false);

            #endregion

            #region A*

            Console.WriteLine("AFS");

            matrix.GenerateEmptyMatrix();

            var astra = new AStar()
            {
                Matrix = matrix
            };

            executionAstar.Start();
            var nodes = astra.FindPath();
            executionAstar.Stop();

            matrix.DisplayMatrix(false);

            //Console.WriteLine("A* shortest path: {0,2}", string.Join(", ", nodes.Select(i => i.Index)));

            #endregion

            #region Best search

            Console.WriteLine("Best");

            matrix.IsBestSearch = true;
            matrix.GenerateEmptyMatrix();

            var bestSearch = new AStar()
            {
                Matrix = matrix
            };

            executionBest.Start();
            bestSearch.FindPath();
            executionBest.Stop();

            matrix.DisplayMatrix(false);

            #endregion

            #region Measurement

            Console.WriteLine("BFS: {0}, SP: {1}", executionBFS.ElapsedMilliseconds,
                executionBFS_SP.ElapsedMilliseconds);
            Console.WriteLine("DFS: {0}, SP: {1}", executionDFS.ElapsedMilliseconds,
                executionDFS_SP.ElapsedMilliseconds);
            Console.WriteLine("A*: {0}, SP: {1}", executionAstar.ElapsedMilliseconds,
                executionAstar.ElapsedMilliseconds);
            Console.WriteLine("Best: {0}, SP: {1}", executionBest.ElapsedMilliseconds,
                executionBest.ElapsedMilliseconds);

            #endregion

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
