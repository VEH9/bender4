using System;
using System.Collections.Generic;

namespace bot
{
    public class StateInit
    {
        public readonly Point finish;
        public readonly Point bender;
        public readonly Switch[] switches;
        public bool[][] Map;
        public Point[] stones;

        public StateInit(int width, int height, string[] mapLine, Point finish, Point bender,
            Switch[] switches)
        {
            this.finish = finish;
            this.bender = bender;
            this.switches = switches;
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
                            lineMap[x] = false; //TODO пока считаем что камни- стена
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
        public Switch[] Switches;
        public Point[] Stones;
        public int HashSum;

        public State(Point benderPos, Switch[] switches, Point[] stones)
        {
            path = new List<Point> { benderPos };
            BenderPos = new Point(benderPos);
            var newSwitches = new Switch[switches.Length];
            Array.Copy(switches, newSwitches, switches.Length);
            Switches = newSwitches;
            var hashCodeSum = BenderPos.GetHashCode();
            foreach (var @switch in newSwitches)
            {
                hashCodeSum ^= @switch.GetHashCode();
            }

            HashSum = hashCodeSum;
            var newStones = new Point[stones.Length];
            Array.Copy(stones, newStones, stones.Length);
            Stones = newStones;
        }

        public State(State prevState, Point newPosition)
        {
            BenderPos = newPosition;
            var newSwitches = new Switch[prevState.Switches.Length];
            for (int i = 0; i < prevState.Switches.Length; i++)
            {
                newSwitches[i] = new Switch(prevState.Switches[i]);
            }
            if (newSwitches.Length != 0)
            {
                var switchWithMagneticField = Array.Find(newSwitches, s => s.location.Equals(newPosition));
                if (switchWithMagneticField != null) 
                    switchWithMagneticField.fieldStatus = switchWithMagneticField.fieldStatus == 1 ? 0 : 1;
            }
            Switches = newSwitches;
            var newStones = new Point[prevState.Stones.Length]; // TODO
            Array.Copy(prevState.Stones, newStones, prevState.Stones.Length);
            Stones = newStones;
            path = new List<Point>(prevState.path) { newPosition };
            var hashCodeSum = BenderPos.GetHashCode();
            foreach (var @switch in newSwitches)
            {
                hashCodeSum ^= @switch.GetHashCode();
            }

            HashSum = hashCodeSum;
        }
    }
}