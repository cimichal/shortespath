using System;
using System.Collections.Generic;
using System.Windows;

namespace algorithms
{
    public class Obstacle
    {
        public ObstacleType ObstacleType { get; set; }
        public Dictionary<string, Tuple<int, int>> Edges { get; set; }
        public IEnumerable<Tuple<int, int>> Points { get; set; }
        public ObstacleType IsObstacleOpen { get; set; }

        public bool CheckIfPointIsInsideObstacle(int posX, int posY)
        {
            if (IsObstacleOpen == ObstacleType.Open)
            {
                foreach (var point in Points)
                {
                    if(point.Item1 == posX && point.Item2 == posY)
                    {
                        return true;
                    }
                }
                return false;
            }

            var recWidth = this.Edges["B"].Item2 - this.Edges["A"].Item2;
            var recHeihgt = this.Edges["B"].Item1 - this.Edges["C"].Item1;

            foreach (var tuple in this.Edges)
            {
                if (tuple.Value.Item2 == posX && tuple.Value.Item1 == posY)
                {
                    return true;
                }

                var rectangle = new Rect
                {
                    Location = new Point(this.Edges["D"].Item2, this.Edges["D"].Item1),
                    Size = new Size
                    {
                        Width = recWidth,
                        Height = recHeihgt
                    }
                };

                var recContaingPoint = rectangle.Contains(posY, posX);

                return recContaingPoint;
            }

            return false;
        }
    }
}