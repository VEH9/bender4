using System;
using System.Collections.Generic;

namespace bot
{
    public class StateInit
    {
        public readonly Point Finish;
        public readonly Point Bender;
        //public readonly Switch[] Switches;
        public readonly int FieldStatus;
        public readonly Dictionary<Point, (Point, int)> DictButtonToField;
        public readonly Dictionary<Point, int> DictFieldIndex;
        public readonly bool[][] Map;
        public Point[] Stones;

        public StateInit(bool[][] map, Point finish, Point bender,
            Dictionary<Point, (Point, int)> dictButtonToField ,Dictionary<Point, int> dictFieldIndex, int fieldStatus, Point[] stones)
        {
            Finish = finish;
            Bender = bender;
            //Switches = switches;
            DictFieldIndex = dictFieldIndex;
            DictButtonToField = dictButtonToField;
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
            Stones = stones;
        }

        public State(State prevState, Point newPosition, Dictionary<Point, (Point, int)> dictButtonToField)
        {
            usedSwitched = prevState.usedSwitched;
            BenderPos = newPosition;
            FieldStatus = prevState.FieldStatus;
            if (FieldStatus != 0)
            {
                if (dictButtonToField.ContainsKey(newPosition))
                {
                    var index = dictButtonToField[newPosition].Item2;
                    var curentBit = GetBitByIndex(FieldStatus, index);
                    FieldStatus = ChangeBit(FieldStatus, index);
                    usedSwitched++;
                }
            }
            
            var newStones = (Point[])prevState.Stones.Clone();
            if (prevState.Stones.Length != 0)
            {
                var stoneIndex = Array.FindIndex(newStones, s => s.Equals(newPosition));
                if (stoneIndex != -1)
                {
                    var stone = newStones[stoneIndex];
                    stone = stone + (newPosition - prevState.BenderPos);
                    newStones[stoneIndex] = stone;
                    if (dictButtonToField.ContainsKey(stone))
                    {
                        var index = dictButtonToField[stone].Item2;
                        FieldStatus = ChangeBit(FieldStatus, index);
                        usedSwitched++;
                    }
                }
            }

            Stones = newStones;
            Path = new List<Point>(prevState.Path) { newPosition };
        }
        
        public static int GetBitByIndex(int mask, int index)
        {
            int bitMask = 1 << index;
            bool bitValue = ((mask & bitMask) >> index) == 1;
            return bitValue ? 1 : 0;
        }
        
        public int ChangeBit(int mask, int index)
        {
            var newValue = GetBitByIndex(mask, index);
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