using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms
{
    internal class ObstacleGenerator
    {
        private static ObstacleGenerator instance;

        private ObstacleGenerator()
        {
        }

        public static ObstacleGenerator Instance => instance ?? (instance = new ObstacleGenerator());

        public Dictionary<int, Obstacle> GeneratedObstacles { get; set; } = new Dictionary<int, Obstacle>();

        public int GenerateObstacle(ObstacleType obstacleType, ObstacleType isOpenObstacle,
            Dictionary<string, Tuple<int, int>> edges)
        {
            var lastIndexInDictionary = this.GeneratedObstacles.Count.Equals(0)
                ? 0
                : this.GeneratedObstacles.Last().Key;
            var indexOfNewItem = lastIndexInDictionary + 1;

            switch (obstacleType)
            {
                case ObstacleType.Open:
                    break;

                case ObstacleType.Close:
                    break;

                case ObstacleType.Square:
                    var newSquare = new Obstacle
                    {
                        Edges = edges,
                        IsObstacleOpen = isOpenObstacle,
                        ObstacleType = ObstacleType.Square
                    };

                    this.GeneratedObstacles.Add(indexOfNewItem, newSquare);
                    return indexOfNewItem;

                case ObstacleType.Traingle:
                    break;

                case ObstacleType.Line:
                    var newLine = new Obstacle
                    {
                        Points = this.GenerateLine(edges),
                        ObstacleType = ObstacleType.Line,
                        IsObstacleOpen = isOpenObstacle
                    };

                    this.GeneratedObstacles.Add(indexOfNewItem, newLine);
                    return indexOfNewItem;

                case ObstacleType.SquareOpen:
                    var newSquareOpen = new Obstacle
                    {
                        Points = this.GenerateOpenSquare(edges),
                        IsObstacleOpen = isOpenObstacle,
                        ObstacleType = ObstacleType.SquareOpen
                    };

                    this.GeneratedObstacles.Add(indexOfNewItem, newSquareOpen);
                    return indexOfNewItem;

                default:
                    break;
            }

            return indexOfNewItem;
        }

        private IEnumerable<Tuple<int, int>> GenerateOpenSquare(Dictionary<string, Tuple<int, int>> edges)
        {
            if (edges == null)
            {
                Console.WriteLine("Please add edges.");
            }

            var points = new List<Tuple<int, int>>();

            // Top DC & Bottom AB - poziom
            if (edges != null && edges["D"].Item1 == edges["C"].Item1) // horizontally y
            {
                for (var i = edges["D"].Item2; i <= edges["C"].Item2; i++)
                {
                    points.Add(new Tuple<int, int>(edges["D"].Item1, i));
                }
            }

            if (edges != null && edges["A"].Item1 == edges["B"].Item1) // horizontally y
            {
                for (var i = edges["A"].Item2; i <= edges["B"].Item2; i++)
                {
                    points.Add(new Tuple<int, int>(edges["A"].Item1, i));
                }
            }


            // Right BC - pion
            if (edges != null && edges["C"].Item2 == edges["B"].Item2) // vertically x
            {
                for (var i = edges["C"].Item1; i < edges["B"].Item1; i++)
                {
                    points.Add(new Tuple<int, int>(i, edges["C"].Item2));
                }
            }

            return points;
        }

        private IEnumerable<Tuple<int, int>> GenerateLine(Dictionary<string, Tuple<int, int>> edges)
        {
            if (edges == null)
            {
                Console.WriteLine("Please add edges.");
            }

            var points = new List<Tuple<int, int>>();

            if (edges != null && edges["S"].Item1 == edges["ST"].Item1) // horizontally y
            {
                for (var i = edges["S"].Item2; i <= edges["ST"].Item2; i++)
                {
                    points.Add(new Tuple<int, int>(edges["S"].Item1, i));
                }
            }

            if (edges != null && edges["S"].Item2 == edges["ST"].Item2) // vertically x
            {
                for (var i = edges["S"].Item1; i < edges["ST"].Item1; i++)
                {
                    points.Add(new Tuple<int, int>(i, edges["S"].Item2));
                }
            }

            return points;
        }
    }
}