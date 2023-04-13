using System;

namespace bot
{

    public class Sim
    {
        private static bool InspectOnField(Point nextPos, int fieldStatus,
            Switch[] switches)
        {
            if (fieldStatus != 0)
            {
                var switchWithMagneticField = Array.FindIndex(switches, s => s.magneticField.Equals(nextPos));
                if (switchWithMagneticField != -1 && State.GetBitByIndex(fieldStatus, switchWithMagneticField) == 1)
                    return false;
            }

            return true;
        }

        private static bool InspectOnWall(bool[][] map, Point nextPos)
        {
            var isWall = map[nextPos.Y][nextPos.X];
            if (!isWall)
                return false;
            return true;
        }

        private static bool InspectOnStones(bool[][] map, Point nextPos, Point pos, Point finish, Point[] stones)
        {
            if (stones.Length != 0)
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

        private static bool PerformAllChecks(bool[][] map, Point nextPos, Point[] stones, int fieldStatus,
            Point pos, Point finish, Switch[] switches)
        {
            return InspectOnWall(map, nextPos) && InspectOnField(nextPos, fieldStatus, switches) &&
                   InspectOnStones(map, nextPos, pos, finish, stones);
        }

        public static bool CanVisit(bool[][] map, State currentState, Direction command,
            Point finish, Switch[] switches)
        {
            var newPos = currentState.BenderPos + command.ToPoint();
            switch (command)
            {
                case Direction.Right:
                    return currentState.BenderPos.X < map[0].Length - 1 && PerformAllChecks(map, newPos,
                        currentState.Stones, currentState.FieldStatus,
                        currentState.BenderPos, finish, switches);
                case Direction.Down:
                    return currentState.BenderPos.Y > 0 &&
                           PerformAllChecks(map, newPos, currentState.Stones, currentState.FieldStatus,
                               currentState.BenderPos, finish, switches);
                case Direction.Left:
                    return currentState.BenderPos.X > 0 &&
                           PerformAllChecks(map, newPos, currentState.Stones, currentState.FieldStatus,
                               currentState.BenderPos, finish, switches);
                case Direction.Up:
                    return currentState.BenderPos.Y < map.Length - 1 &&
                           PerformAllChecks(map, newPos, currentState.Stones, currentState.FieldStatus,
                               currentState.BenderPos, finish, switches);
            }

            return false;
        }
    }
}