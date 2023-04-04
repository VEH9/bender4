using System;

namespace bot
{

    public class Sim
    {
        public static bool CanMove(Point nextPos, State state)
        {
            var isWall = state.Map[nextPos.Y][nextPos.X];
            if (isWall)
                return false;
            Switch switchWithMagneticField = Array.Find(state.Switches, s => s.magneticField.Equals(nextPos));
            if (switchWithMagneticField.fieldStatus == 1)
            {
                return false;
            }

            var stone = Array.Find(state.Stones, s => s.Equals(nextPos));
            if (stone != null)
            {
                var stoneNextPos = stone + (nextPos - state.BenderPos);
                if (!state.Map[stoneNextPos.Y][stoneNextPos.X] || stoneNextPos == state.Finish)
                {
                    return false;
                }
            }
            return true;
        }
    }
}