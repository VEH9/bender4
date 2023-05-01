using System;
using System.Collections.Generic;

namespace bot
{
    public class StateInit
    {
        public readonly Point Finish;
        public readonly Point Bender;
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
            DictFieldIndex = dictFieldIndex;
            DictButtonToField = dictButtonToField;
            FieldStatus = fieldStatus;
            Stones = stones;
            Map = map;
        }
    }

    public class State
    {
        public readonly State ParentState;
        public readonly Point BenderPos;
        public readonly int FieldStatus;
        public readonly Point[] Stones;
        public readonly int usedSwitched;

        public State(Point benderPos, int switches, Point[] stones)
        {
            ParentState = null;
            BenderPos = new Point(benderPos);
            FieldStatus = switches;
            usedSwitched = 0;
            Stones = stones;
        }

        public State(State prevState, Point newPosition, Dictionary<Point, (Point, int)> dictButtonToField, bool[][] map, Point finish, Dictionary<Point, int> dictFieldIndex)
        {
            if (!(InspectOnWall(map, newPosition) && InspectOnField(newPosition, prevState.FieldStatus, dictFieldIndex)))
                return;
            var newStones = new Point[prevState.Stones.Length];
            Array.Copy(prevState.Stones, newStones, prevState.Stones.Length);
            usedSwitched = prevState.usedSwitched;
            FieldStatus = prevState.FieldStatus;
            if (prevState.Stones.Length != 0)
            {
                var stoneIndex = Array.FindIndex(newStones, s => s.Equals(newPosition));
                if (stoneIndex != -1)
                {
                    if (!InspectOnStones(map, newPosition, prevState.BenderPos, finish, 
                            newStones, stoneIndex))
                        return;
                
                    var stone = newStones[stoneIndex];
                    stone += (newPosition - prevState.BenderPos);
                    newStones[stoneIndex] = stone;
                    if (dictButtonToField.ContainsKey(stone))
                    {
                        var index = dictButtonToField[stone].Item2;
                        FieldStatus = ChangeBit(FieldStatus, index);
                        usedSwitched++;
                    }
                }
            }
            BenderPos = newPosition;
            Stones = newStones;
            ParentState = prevState;
            if (FieldStatus != 0 && dictButtonToField.ContainsKey(newPosition))
            {
                var index = dictButtonToField[newPosition].Item2;
                FieldStatus = ChangeBit(FieldStatus, index);
                usedSwitched++;
            }
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

        private static bool InspectOnStones(bool[][] map, Point nextPos, Point pos, Point finish, Point[] stones, int stoneIndex)
        {
            var stoneNextPos = stones[stoneIndex] + (nextPos - pos);
            var otherStone = Array.FindIndex(stones, s => s.Equals(stoneNextPos));
            if (!InspectOnWall(map, stoneNextPos) || stoneNextPos == finish || otherStone != -1) return false;
            
            return true;
        }

        private static bool PerformAllChecksForBender(bool[][] map, Point nextPos, int fieldStatus,
            Dictionary<Point, int> dictFieldIndex)
        {
            return InspectOnWall(map, nextPos) && InspectOnField(nextPos, fieldStatus, dictFieldIndex);
        }
    }
}