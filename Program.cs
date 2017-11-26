using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace algorithms
{
    internal class Program
    {
        public static readonly int IndexStart = 21;
        public static readonly int IndexStop = 377;
        private static readonly ObstacleGenerator ObstacleGenerator = ObstacleGenerator.Instance;
        private static int[] macierz;

        private static void Main(string[] args)
        {
            macierz = WygenerujWierzcholki(20);
            var obstacles = new Dictionary<string, int>();

            InitObstacle(obstacles);

            /* Glowna petla programu */
            ProgramDescription("desc");
            ProgramDescription("selectOption");

            var key = Console.ReadKey();
            while (key.Key != ConsoleKey.Escape)
            {
                switch (key.Key)
                {
                    case ConsoleKey.Q:
                        Console.Clear();
                        break;

                    case ConsoleKey.A:
                        var emptyMatrix = new Matrix(macierz)
                        {
                            IsObstacleOnTheMatrix = false,
                            IndexPunktuKoncowego = IndexStop,
                            IndexPunktuStartowego = IndexStart
                        };

                        Console.Clear();
                        Console.WriteLine("Pusta plansza z punktem poczatkowym oraz koncowym.");
                        ExecuteAlgorithms(emptyMatrix);
                        Console.WriteLine("Pusta plansza - koniec.\n");
                        break;

                    case ConsoleKey.S:
                        Console.Clear();
                        ExecuteSpecificAlgortihm(obstacles["S"]);
                        break;

                    case ConsoleKey.D:
                        Console.Clear();
                        ExecuteSpecificAlgortihm(obstacles["D"]);
                        break;

                    case ConsoleKey.F:
                        Console.Clear();
                        ExecuteSpecificAlgortihm(obstacles["F"]);
                        break;

                    case ConsoleKey.G:
                        Console.Clear();
                        ExecuteSpecificAlgortihm(obstacles["G"]);
                        break;

                    case ConsoleKey.H:
                        Console.Clear();
                        ExecuteSpecificAlgortihm(obstacles["H"]);
                        break;

                    case ConsoleKey.M:
                        ProgramDescription("desc");
                        ProgramDescription("selectOption");
                        break;
                        
                    default:
                        ProgramDescription("desc");
                        ProgramDescription("selectOption");
                        break;
                }

                Console.WriteLine("W celu powortu do wyboru planszy uzyj : M");
                key = Console.ReadKey();
            }
            
        }

        private static void ExecuteSpecificAlgortihm(int obstacleIndex)
        {
            Console.WriteLine($"{ObstacleGenerator.GeneratedObstacles[obstacleIndex].ObstacleDescription}");

            var matrix = new Matrix(macierz)
            {
                IsObstacleOnTheMatrix = true,
                Obstacle = ObstacleGenerator.GeneratedObstacles[obstacleIndex],
                IndexPunktuKoncowego = IndexStop,
                IndexPunktuStartowego = IndexStart
            };
            ExecuteAlgorithms(matrix);
            Console.WriteLine("Koniec.\n");
        }

        private static void ProgramDescription(string option)
        {
            var description = "";

            switch (option)
            {
                case "desc":
                    Console.Clear();
                    description = "W celu wyboru planszy do analizy algorytmow nalezy wybrac " +
                                  "jedna z przedstawionych ponizej opcji.\n" +
                                  "W celu wyszczyszczenia consoli, nalezy uzyc klawiszy Q\n" +
                                  "Statystki dla wybranej planszy z dana przeszkoda zostana wyswietone na koncu dzialania algorytmow.\n" +
                                  "Plansza sklada sie z 20 wierszy oraz 20 kolum. Punkt startowy oraz koncowy nie ma mozliwosci edycji.\n";
                    break;

                case "selectOption":
                    description = "A - pusta plansz.\n" +
                                  "S - plansza z kwadratem\n" +
                                  "D - plansza z jedna linia\n" +
                                  "F - plansza z trzema liniami\n" +
                                  "G - plansza z otwarta przeszkoda\n" +
                                  "H - labirynt\n\n" +
                                  "Wybierz odpowiednia plansze.\n";
                    break;

                default:
                    description = "Ctr + C";
                    break;
            }

            Console.WriteLine($"{description}");
        }

        public static void ExecuteAlgorithms(Matrix matrix)
        {
            matrix.GenerateEmptyMatrix();

            #region Diagnostics

            var executionBfs = new Stopwatch();
            var executionDfs = new Stopwatch();
            var executionAstar = new Stopwatch();
            var executionBest = new Stopwatch();

            var executionBfsSp = new Stopwatch();
            var executionDfsSp = new Stopwatch();

            #endregion

            #region BFS

            Console.WriteLine("BFS");

            var bfs = new Bfs(IndexStart, IndexStop)
            {
                Matrix = matrix
            };

            executionBfs.Start();
            bfs.ObliczBfs();
            executionBfs.Stop();

            executionBfsSp.Start();
            var shortestPathPointsBfs = bfs.NajkrotszaDroga().ToArray();
            executionBfsSp.Stop();

            matrix.DisplayMatrixShortestPath(shortestPathPointsBfs);
            matrix.DisplayMatrix(false);

            #endregion

            #region DFS

            Console.WriteLine("DFS");

            var dfs = new Dfs(IndexStart, IndexStop)
            {
                Matrix = matrix
            };

            matrix.GenerateEmptyMatrix();

            executionDfs.Start();
            dfs.ObliczDfs();
            executionDfs.Stop();

            executionDfsSp.Start();
            var shortestPathPointsDfs = dfs.NajkrotszaDroga().ToArray();
            executionDfsSp.Stop();

            matrix.DisplayMatrixShortestPath(shortestPathPointsDfs);
            matrix.DisplayMatrix(false);

            #endregion

            #region A*

            Console.WriteLine("AFS");

            matrix.GenerateEmptyMatrix();

            var astra = new AStar
            {
                Matrix = matrix
            };

            executionAstar.Start();
            var nodes = astra.FindPath();
            executionAstar.Stop();

            matrix.DisplayMatrix(false);

            #endregion

            #region Best search

            Console.WriteLine("Best");

            matrix.IsBestSearch = true;
            matrix.GenerateEmptyMatrix();

            var bestSearch = new AStar
            {
                Matrix = matrix
            };

            executionBest.Start();
            bestSearch.FindPath();
            executionBest.Stop();

            matrix.DisplayMatrix(false);

            #endregion

            #region Measurement

            Console.WriteLine("Performance");

            Console.WriteLine("BFS: {0} ms, Najkrotsza droga: {1}", executionBfs.ElapsedMilliseconds,
                executionBfsSp.ElapsedMilliseconds);
            Console.WriteLine("DFS: {0} ms, Najkrotsza droga: {1}", executionDfs.ElapsedMilliseconds,
                executionDfsSp.ElapsedMilliseconds);
            Console.WriteLine("A*: {0} ms", executionAstar.ElapsedMilliseconds);
            Console.WriteLine("Best: {0} ms", executionBest.ElapsedMilliseconds);

            Console.WriteLine();

            #endregion
        }

        private static void InitObstacle(Dictionary<string, int> obstacles)
        {
            if (obstacles == null)
            {
                throw new ArgumentNullException(nameof(obstacles));
            }

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
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                {0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0},
                {0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            var squareIndex = ObstacleGenerator.GenerateObstacle(ObstacleType.Square, ObstacleType.Close, edges,
                "Przeszkoda pierwsza - kwadrat");

            var lineIndex = ObstacleGenerator.GenerateObstacle(ObstacleType.Line, ObstacleType.Open,
                new Dictionary<string, Tuple<int, int>>
                {
                    {"S1", new Tuple<int, int>(1, 10)},
                    {"ST1", new Tuple<int, int>(19, 10)}
                }, "Przeszkoda druga - linia");

            var threeLineIndex = ObstacleGenerator.GenerateObstacle(ObstacleType.Traingle, ObstacleType.Open,
                new Dictionary<string, Tuple<int, int>>
                {
                    {"S1", new Tuple<int, int>(1, 10)},
                    {"ST1", new Tuple<int, int>(16, 10)},
                    {"S2", new Tuple<int, int>(1, 12)},
                    {"ST2", new Tuple<int, int>(15, 12)},
                    {"S3", new Tuple<int, int>(1, 5)},
                    {"ST3", new Tuple<int, int>(19, 5)}
                }, "Przeszkoda trzecia - trzy linie ");

            var openObstacle = ObstacleGenerator.GenerateObstacle(ObstacleType.SquareOpen, ObstacleType.Open, edges,
                "Przeszkoda czwarta - otwarty kwadrat");

            var labiryntIndex = ObstacleGenerator.GenerateObstacle(ObstacleType.Labirynt, ObstacleType.Open, labirynt,
                "Labirynt z jednym wejsciem i jednym wyjsciem");

            obstacles.Add("S", squareIndex);
            obstacles.Add("D", lineIndex);
            obstacles.Add("F", threeLineIndex);
            obstacles.Add("G", openObstacle);
            obstacles.Add("H", labiryntIndex);
            /* End obstacles */
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