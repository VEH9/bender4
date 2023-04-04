using System.Collections.Generic;

namespace bot
{
    public class StateInit
    {
        public readonly int width;
        public readonly int height;
        public readonly Point target;
        public readonly Point start;
        public readonly Switch[] switches;
        public bool[][] Map;
        public Point[] stones;

        public StateInit(int width, int height, string[] mapLine, Point target, Point start,
            Switch[] switches)
        {
            this.width = width;
            this.height = height;
            this.target = target;
            this.start = start;
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
        public Point TargetPos;
        public Switch[] Switches;
        public Point[] Stones;

        public State(Point targetPos, Switch[] switches, Point[] stones)
        {
            TargetPos = targetPos;
            Switches = switches;
            Stones = stones;
        }
    }
}