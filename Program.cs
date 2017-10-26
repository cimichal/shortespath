using System;
using System.Collections.Generic;

namespace algorithms
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var wierzcholki = WygenerujWierzcholki(20);

            var indexStart = 1;
            var indexStop = 338;

            var matrixBfs = new Matrix(wierzcholki);
            var matrixDfs = new Matrix(wierzcholki);

            matrixDfs.IndexPunktuKoncowego = indexStop;
            matrixDfs.IndexPunktuStartowego = indexStart;

            matrixBfs.IndexPunktuKoncowego = indexStop;
            matrixBfs.IndexPunktuStartowego = indexStart;

            matrixBfs.GenerateEmptyMatrix();
            matrixDfs.GenerateEmptyMatrix();
            
            var square = new List<int>()
            {
                167, 168, 169, 170, 171, 172, 192, 212, 232,
                231, 230, 229, 228, 227,
                207, 187
            };

            var line = new List<int>()
            {
                
            };

            var openSquare = new List<int>()
            {

            };

            matrixDfs.DisplayMatrix(true);

            Console.WriteLine();
            
            var bfsMatrix = new Bfs(indexStart, indexStop)
            {
                Matrix = matrixBfs
            };

            var dfs = new Dfs(indexStart, indexStop)
            {
                Matrix = matrixDfs
            };

            matrixBfs.AddObstacle(square);

            bfsMatrix.ObliczBfs();
            dfs.ObliczDfs();

            matrixBfs.DisplayMatrix(false);
            matrixDfs.DisplayMatrix(false);

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