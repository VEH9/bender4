using System;

namespace bot
{

    public class Sim
    {
        private static bool inspectOnFieldAndStones(bool[][] map, Point nextPos, Point[] stones, Switch[] switches,
            Point pos, Point finish)
        {
            var isWall = map[nextPos.Y][nextPos.X];
            if (!isWall)
                return false;
            if (switches != null)
            {
                var switchWithMagneticField = Array.Find(switches, s => s.magneticField.Equals(nextPos));
                if (switchWithMagneticField != null && switchWithMagneticField.fieldStatus == 1) return false;
            }

            if (stones != null)
            {
                var stone = Array.Find(stones, s => s.Equals(nextPos));
                if (stone != null)
                {
                    var stoneNextPos = stone + (nextPos - pos);
                    if (!map[stoneNextPos.Y][stoneNextPos.X] || stoneNextPos == finish) return false;
                }
            }

            return true;
        }

        public static bool CanVisit(bool[][] map, int[,] distances, Point position, Direction command, Point[] stones,
            Switch[] switches, Point finish)
        {
            var newPos = position + command.ToPoint();
            switch (command)
            {
                case Direction.Right:
                    return position.X < map[0].Length - 1 &&
                           inspectOnFieldAndStones(map, newPos, stones, switches, position, finish) &&
                           distances[newPos.X, newPos.Y] == -1;
                case Direction.Down:
                    return position.Y > 0 &&
                           inspectOnFieldAndStones(map, newPos, stones, switches, position, finish) &&
                           distances[newPos.X, newPos.Y] == -1;
                case Direction.Left:
                    return position.X > 0 &&
                           inspectOnFieldAndStones(map, newPos, stones, switches, position, finish) &&
                           distances[newPos.X, newPos.Y] == -1;
                case Direction.Up:
                    return position.Y < map.Length - 1 &&
                           inspectOnFieldAndStones(map, newPos, stones, switches, position, finish) &&
                           distances[newPos.X, newPos.Y] == -1;
            }

            return false;
        }
    }
}