using System;
using System.Collections.Generic;

namespace bot
{
    public class StateInit
    {
        public readonly Point Finish;
        public readonly Point Bender;
        public readonly Switch[] Switches;
        public readonly int FieldStatus;
        public readonly bool[][] Map;
        public Point[] Stones;

        public StateInit(bool[][] map, Point finish, Point bender,
            Switch[] switches, int fieldStatus, Point[] stones)
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
        public readonly int FieldStatus;
        public readonly Point[] Stones;
        public readonly int usedSwitched;

        public State(Point benderPos, int switches, Point[] stones)
        {
            Path = new List<Point> { benderPos };
            BenderPos = new Point(benderPos);
            FieldStatus = switches;
            usedSwitched = 0;
            Stones = Array.Empty<Point>();//stones;
        }

        public State(State prevState, Point newPosition, Switch[] switches)
        {
            usedSwitched = prevState.usedSwitched;
            BenderPos = newPosition;
            var isChangeFieldStatus = false;
            FieldStatus = prevState.FieldStatus;
            if (FieldStatus != 0)
            {
                var switchWithMagneticField = Array.FindIndex(switches, s => s.location.Equals(newPosition));
                if (switchWithMagneticField != -1)
                {
                    var curentBit = GetBitByIndex(FieldStatus, switchWithMagneticField);
                    FieldStatus = ChangeBit(FieldStatus, switchWithMagneticField, curentBit);
                    usedSwitched++;
                }
            }
            Stones = prevState.Stones; // TODO
            Path = new List<Point>(prevState.Path) { newPosition };
        }
        
        public static int GetBitByIndex(int mask, int index)
        {
            int bitMask = 1 << index;
            bool bitValue = ((mask & bitMask) >> index) == 1;
            return bitValue ? 1 : 0;
        }
        
        public int ChangeBit(int mask, int index, int newValue)
        {
            newValue = newValue == 0 ? 1 : 0;
            int bitMask = 1 << index;
            if (newValue == 0)
            {
                mask &= ~bitMask;
            }
            else
            {
                mask |= bitMask;
            }

            return mask;
        }
    }
}