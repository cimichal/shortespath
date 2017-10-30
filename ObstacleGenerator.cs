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

                    break;
                default:
                    break;
            }

            return indexOfNewItem;
        }
    }
}
