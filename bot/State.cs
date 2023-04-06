using System;
using System.Collections.Generic;

namespace bot
{
    public class StateInit
    {
        public readonly Point Finish;
        public readonly Point Bender;
        public readonly Switch[] Switches;
        public readonly int[] FieldStatus;
        public readonly bool[][] Map;
        public Point[] Stones;

        public StateInit(bool[][] map, Point finish, Point bender,
            Switch[] switches, int[] fieldStatus, Point[] stones)
        {
            Finish = finish;
            Bender = bender;
            Switches = switches;
            FieldStatus = fieldStatus;
            Stones = stones;
            Map = map;
        }
    }

    public class State
    {
        public readonly List<Point> Path;
        public readonly Point BenderPos;
        public readonly int[] FieldStatus;
        public readonly Point[] Stones;
        public readonly int HashSum;
        public readonly int usedSwitched;

        private int GetHashSum(bool isChangeFieldStatus, int prevFieldStatusHashSum = 0)
        {
            if (isChangeFieldStatus)
                return FieldStatus.GetHashCode();
            return prevFieldStatusHashSum;
        }
        
        public State(Point benderPos, int[] switches, Point[] stones)
        {
            Path = new List<Point> { benderPos };
            BenderPos = new Point(benderPos);
            FieldStatus = switches;
            usedSwitched = 0;
            Stones = Array.Empty<Point>();//stones;
            HashSum = GetHashSum(true);
        }

        public State(State prevState, Point newPosition, Switch[] switches)
        {
            usedSwitched = prevState.usedSwitched;
            BenderPos = newPosition;
            var newFieldStatus = new int[prevState.FieldStatus.Length];
            for (int i = 0; i < prevState.FieldStatus.Length; i++)
            {
                newFieldStatus[i] = prevState.FieldStatus[i];
            }
            
            var isChangeFieldStatus = false;
            if (newFieldStatus.Length != 0)
            {
                var switchWithMagneticField = Array.FindIndex(switches, s => s.location.Equals(newPosition));
                if (switchWithMagneticField != -1)
                {
                    newFieldStatus[switchWithMagneticField] =
                        prevState.FieldStatus[switchWithMagneticField] == 1 ? 0 : 1;
                    isChangeFieldStatus = true;
                    usedSwitched++;
                }
            }
            FieldStatus = newFieldStatus;
            Stones = prevState.Stones; // TODO
            Path = new List<Point>(prevState.Path) { newPosition };
            HashSum = GetHashSum(isChangeFieldStatus, prevState.HashSum);
        }
    }
}