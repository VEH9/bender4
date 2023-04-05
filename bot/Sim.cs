﻿using System;
using System.Collections.Generic;

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
            if (switches.Length != 0)
            {
                var switchWithMagneticField = Array.Find(switches, s => s.magneticField.Equals(nextPos));
                if (switchWithMagneticField != null && switchWithMagneticField.fieldStatus == 1) return false;
            }

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

        public static bool CanVisit(bool[][] map, State currentState, Direction command,
            Point finish)
        {
            var newPos = currentState.BenderPos + command.ToPoint();
            switch (command)
            {
                case Direction.Right:
                    return currentState.BenderPos.X < map[0].Length - 1 &&
                           inspectOnFieldAndStones(map, newPos, currentState.Stones, currentState.Switches,
                               currentState.BenderPos, finish);
                case Direction.Down:
                    return currentState.BenderPos.Y > 0 &&
                           inspectOnFieldAndStones(map, newPos, currentState.Stones, currentState.Switches,
                               currentState.BenderPos, finish);
                case Direction.Left:
                    return currentState.BenderPos.X > 0 &&
                           inspectOnFieldAndStones(map, newPos, currentState.Stones, currentState.Switches,
                               currentState.BenderPos, finish);
                case Direction.Up:
                    return currentState.BenderPos.Y < map.Length - 1 &&
                           inspectOnFieldAndStones(map, newPos, currentState.Stones, currentState.Switches,
                               currentState.BenderPos, finish);
            }

            return false;
        }
    }
}