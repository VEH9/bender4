using System;
using System.Collections.Generic;

namespace bot
{

    public class Sim
    {
        private static bool InspectOnField(Point nextPos, int fieldStatus,
            Dictionary<Point, int> dictFieldIndex)
        {
            if (fieldStatus != 0)
            {
                if (dictFieldIndex.ContainsKey(nextPos))
                {
                    if (State.GetBitByIndex(fieldStatus, dictFieldIndex[nextPos]) == 1)
                        return false;
                }
            }

            return true;
        }

        private static bool InspectOnWall(bool[][] map, Point nextPos)
        {
            var isWall = map[nextPos.Y][nextPos.X];
            return isWall;
        }

        private static bool InspectOnStones(bool[][] map, Point nextPos, Point pos, Point finish, Point[] stones)
        {
            var stoneIndex = Array.FindIndex(stones, s => s.Equals(nextPos));
            if (stoneIndex != -1)
            {
                var stoneNextPos = stones[stoneIndex] + (nextPos - pos);
                var otherStone = Array.FindIndex(stones, s => s.Equals(stoneNextPos));
                if (!InspectOnWall(map, stoneNextPos) || stoneNextPos == finish || otherStone != -1) return false;
            }

            return true;
        }

        private static bool PerformAllChecks(bool[][] map, Point nextPos, Point[] stones, int fieldStatus,
            Point pos, Point finish, Dictionary<Point, int> dictFieldIndex)
        {
            return InspectOnWall(map, nextPos) && InspectOnField(nextPos, fieldStatus, dictFieldIndex) &&
                   InspectOnStones(map, nextPos, pos, finish, stones);
        }

        public static bool CanVisit(bool[][] map, State currentState, Direction command,
            Point finish, Dictionary<Point, int> dictFieldIndex)
        {
            var newPos = currentState.BenderPos + command.ToPoint();
            switch (command)
            {
                case Direction.Right:
                    return currentState.BenderPos.X < map[0].Length - 1 && PerformAllChecks(map, newPos,
                        currentState.Stones, currentState.FieldStatus,
                        currentState.BenderPos, finish, dictFieldIndex);
                case Direction.Down:
                    return currentState.BenderPos.Y < map.Length - 1 &&
                           PerformAllChecks(map, newPos, currentState.Stones, currentState.FieldStatus,
                               currentState.BenderPos, finish, dictFieldIndex);
                case Direction.Left:
                    return currentState.BenderPos.X > 0 &&
                           PerformAllChecks(map, newPos, currentState.Stones, currentState.FieldStatus,
                               currentState.BenderPos, finish, dictFieldIndex);
                case Direction.Up:
                    return currentState.BenderPos.Y > 0 &&
                           PerformAllChecks(map, newPos, currentState.Stones, currentState.FieldStatus,
                               currentState.BenderPos, finish, dictFieldIndex);
            }

            return false;
        }
    }
}