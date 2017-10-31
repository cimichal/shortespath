using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms
{
    class ObstacleGenerator
    {
        private static ObstacleGenerator instance;
        private ObstacleGenerator() { }
        public static ObstacleGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObstacleGenerator();
                }
                return instance;
            }
        }


        public Dictionary<int,Obstacle> GeneratedObstacles { get; set; } = new Dictionary<int, Obstacle>();

        public int GenerateObstacle(ObstacleType obstacleType, ObstacleType isOpenObstacle, Dictionary<string, Tuple<int,int>> edges)
        {
            var lastIndexInDictionary = this.GeneratedObstacles.Count.Equals(0) ? 0 : this.GeneratedObstacles.Last().Key;
            var indexOfNewItem = lastIndexInDictionary + 1;
            
            switch (obstacleType)
            {
                case ObstacleType.Open:
                    break;

                case ObstacleType.Close:
                    break;

                case ObstacleType.Square:
                    var newSquare = new Obstacle()
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
                    var newLine = new Obstacle()
                    {
                        Points = this.GenerateLine(edges),
                        ObstacleType = ObstacleType.Line,
                        IsObstacleOpen = isOpenObstacle
                    };

                    this.GeneratedObstacles.Add(indexOfNewItem, newLine);
                    return indexOfNewItem;

                default:
                    break;
            }

            return indexOfNewItem;
        }

        private IEnumerable<Tuple<int, int>> GenerateLine(Dictionary<string, Tuple<int, int>> edges)
        {
            var points = new List<Tuple<int, int>> { };
            
            if (edges["S"].Item1 == edges["ST"].Item1) // horizontally y
            {
                for (int i = edges["S"].Item2; i <= edges["ST"].Item2; i++)
                {
                    points.Add(new Tuple<int, int>(edges["S"].Item1, i));
                }
            }

            if (edges["S"].Item2 == edges["ST"].Item2) // vertically x
            {
                for (int i = edges["S"].Item1; i < edges["ST"].Item1; i++)
                {
                    points.Add(new Tuple<int, int>(i, edges["S"].Item2));
                }
            }
            
            return points;
        }
    }
}
