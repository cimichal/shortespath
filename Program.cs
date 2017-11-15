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
            #region Diagnostics

            var executionBFS = new Stopwatch();
            var executionDFS = new Stopwatch();
            var executionAstar = new Stopwatch();

            var executionBFS_SP = new Stopwatch();
            var executionDFS_SP = new Stopwatch();

            #endregion

            #region Init & Obsctacles

            var obstacleGenerator = ObstacleGenerator.Instance;
            var wierzcholki = WygenerujWierzcholki(20);

            var indexStart = 1;
            var indexStop = 360;

            var edges = new Dictionary<string, Tuple<int, int>>
            {
                {"A", new Tuple<int, int>(19, 6)},
                {"B", new Tuple<int, int>(19, 16)},
                {"C", new Tuple<int, int>(8, 16)},
                {"D", new Tuple<int, int>(8, 6)}
            };

            var squareIndex = obstacleGenerator.GenerateObstacle(ObstacleType.Square, ObstacleType.Close, edges);

            var lineIndex = obstacleGenerator.GenerateObstacle(ObstacleType.Line, ObstacleType.Open,
                new Dictionary<string, Tuple<int, int>>
                {
                    {"S", new Tuple<int, int>(1, 10)},
                    {"ST", new Tuple<int, int>(30, 10)}
                });

            var openObstacle = obstacleGenerator.GenerateObstacle(ObstacleType.SquareOpen, ObstacleType.Open, edges);

            #endregion

            #region Matrix

            var matrix = new Matrix(wierzcholki)
            {
                IsObstacleOnTheMatrix = false,
                Obstacle = obstacleGenerator.GeneratedObstacles[openObstacle],
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

            Console.WriteLine("A* shortest path: {0,2}", string.Join(", ", nodes.Select(i => i.Index)));

            #endregion

            #region Best search

            Console.WriteLine("Best");
            
            matrix.IsBestSearch = true;
            matrix.GenerateEmptyMatrix();

            var bestSearch = new AStar()
            {
                Matrix = matrix
            };

            bestSearch.FindPath();

            matrix.DisplayMatrix(false);

            #endregion

            #region JumpoPointSearch

            Console.WriteLine("Jump point search");

            matrix.Obstacle = obstacleGenerator.GeneratedObstacles[squareIndex];
            matrix.IsBestSearch = false;
            matrix.IsObstacleOnTheMatrix = true;
            matrix.GenerateEmptyMatrix();

            JumpPointSearch jumpPointSearch = new JumpPointSearch()
            {
                Matrix = matrix,

            };

            jumpPointSearch.FindPath();

            matrix.DisplayMatrix(false);

            #endregion

            #region Measurement

            Console.WriteLine("BFS: {0}, SP: {1}", executionBFS.ElapsedMilliseconds, executionBFS_SP.ElapsedMilliseconds);
            Console.WriteLine("DFS: {0}, SP: {1}", executionDFS.ElapsedMilliseconds, executionDFS_SP.ElapsedMilliseconds);
            Console.WriteLine("A*: {0}, SP: {1}", executionAstar.ElapsedMilliseconds, executionAstar.ElapsedMilliseconds);

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