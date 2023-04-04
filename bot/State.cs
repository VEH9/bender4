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
        public Point BenderPos;
        public Switch[] Switches;
        public Point[] Stones;
        public readonly bool[][] Map;
        public readonly Point Finish;

        public State(Point benderPos, Switch[] switches, Point[] stones, bool[][] map, Point finish)
        {
            BenderPos = benderPos;
            Switches = switches;
            Stones = stones;
            Map = map;
            Finish = finish;
        }
    }
}