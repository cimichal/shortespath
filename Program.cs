using System;
using System.Collections.Generic;

namespace algorithms
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var wierzcholki = WygenerujWierzcholki(30);
            var indexStart = 1;
            var indexStop = 477;

            var matrixBfs = new Matrix(wierzcholki);
            var matrixDfs = new Matrix(wierzcholki);

            matrixDfs.IndexPunktuKoncowego = indexStop;
            matrixDfs.IndexPunktuStartowego = indexStart;

            matrixBfs.IndexPunktuKoncowego = indexStop;
            matrixBfs.IndexPunktuStartowego = indexStart;

            matrixBfs.GenerateEmptyMatrix();
            matrixDfs.GenerateEmptyMatrix();
            
            var obstacle = new List<int>()
            {
                33, 34, 35, 36, 46, 56, 55, 54, 53, 43
            };

            Console.WriteLine();
            
            var bfsMatrix = new Bfs(indexStart, indexStop)
            {
                Matrix = matrixBfs
            };

            var dfs = new Dfs(indexStart, indexStop)
            {
                Matrix = matrixDfs
            };

            matrixBfs.AddObstacle(obstacle);

            bfsMatrix.ObliczBfs();
            dfs.ObliczDfs();

            matrixBfs.DisplayMatrix(false);
            matrixDfs.DisplayMatrix(false);
            
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