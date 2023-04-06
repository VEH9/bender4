using System;
using System.Collections.Generic;

namespace bot
{
    public class StateInit
    {
        public readonly Point finish;
        public readonly Point bender;
        public readonly Switch[] switches;
        public readonly int[] fieldStatus;
        public bool[][] Map;
        public Point[] stones;

        public StateInit(int width, int height, string[] mapLine, Point finish, Point bender,
            Switch[] switches, int[] fieldStatus)
        {
            this.finish = finish;
            this.bender = bender;
            this.switches = switches;
            this.fieldStatus = fieldStatus;
            var map = new bool[height][];
            var stoneList = new List<Point>();
            for (var y = 0; y < mapLine.Length; y++)
            {
                var lineMap = new bool[mapLine.Length];
                
                for (int x = 0; x < mapLine[y].Length; x++)
                {
                    switch (mapLine[y][x])
                    {
                        case '.':
                            lineMap[x] = true;
                            break;
                        case '+':
                            lineMap[x] = false; //TODO пока считаем что камни- стены
                            stoneList.Add(new Point(x,y));
                            break;
                        case '#':
                            lineMap[x] = false;
                            break;
                    }
                }

                map[y] = lineMap;
            }

            stones = stoneList.ToArray();
            Map = map;
        }
    }

    public class State
    {
        public List<Point> path;
        public Point BenderPos;
        public int[] fieldStatus;
        public Point[] Stones;
        public readonly int HashSum;
        public int usedSwitched;

        private int GetHashSum(bool isChangeFieldStatus, int prevFieldStatusHashSum = 0)
        {
            if (isChangeFieldStatus)
                return fieldStatus.GetHashCode();
            return prevFieldStatusHashSum;
        }
        
        public State(Point benderPos, int[] switches, Point[] stones)
        {
            path = new List<Point> { benderPos };
            BenderPos = new Point(benderPos);
            fieldStatus = switches;
            
            usedSwitched = 0;

            Stones = Array.Empty<Point>();//stones;
            HashSum = GetHashSum(true);
        }

        public State(State prevState, Point newPosition, Switch[] switches)
        {
            usedSwitched = prevState.usedSwitched;
            BenderPos = newPosition;
            var newFieldStatus = new int[prevState.fieldStatus.Length];
            for (int i = 0; i < prevState.fieldStatus.Length; i++)
            {
                newFieldStatus[i] = prevState.fieldStatus[i];
            }
            
            var isChangeFieldStatus = false;
            if (newFieldStatus.Length != 0)
            {
                var switchWithMagneticField = Array.FindIndex(switches, s => s.location.Equals(newPosition));
                if (switchWithMagneticField != -1)
                {
                    newFieldStatus[switchWithMagneticField] =
                        prevState.fieldStatus[switchWithMagneticField] == 1 ? 0 : 1;
                    isChangeFieldStatus = true;
                    usedSwitched++;
                }
            }
            fieldStatus = newFieldStatus;
            Stones = prevState.Stones; // TODO
            path = new List<Point>(prevState.path) { newPosition };
            HashSum = GetHashSum(isChangeFieldStatus, prevState.HashSum);
        }
    }
}