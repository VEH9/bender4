using System;
using System.Collections.Generic;

namespace bot
{
    public static class StateReader
    {
        public static State ReadState(this ConsoleReader reader)
        {
            var init = reader.ReadInit();
            return reader.ReadState(init);
        }
        
        // ReSharper disable once InconsistentNaming
        public static State ReadState(this ConsoleReader Console, StateInit init)
        {
            // Copy paste here the code for input turn data
            return new State(init.Bender, init.FieldStatus, init.Stones);
        }

        // ReSharper disable once InconsistentNaming
        public static StateInit ReadInit(this ConsoleReader Console)
        {
            // Copy paste here the code for initialization input data (or delete if no initialization data in this game)
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int width = int.Parse(inputs[0]);
            int height = int.Parse(inputs[1]);
            
            var map = new bool[height][];
            var stoneList = new List<Point>();
            for (int y = 0; y < height; y++)
            {
                string line = Console.ReadLine();
                var lineToBool = new bool[width];
                for (int x = 0; x < line.Length; x++)
                {
                    switch (line[x])
                    {
                        case '.':
                            lineToBool[x] = true;
                            break;
                        case '+':
                            lineToBool[x] = true; // TODO если играем с камнями, то не проходим 27 тест валидатора
                            stoneList.Add(new Point(x, y));
                            break;
                        case '#':
                            lineToBool[x] = false;
                            break;
                    }

                    map[y] = lineToBool;
                }
            }
                
            inputs = Console.ReadLine().Split(' ');
            int startX = int.Parse(inputs[0]);
            int startY = int.Parse(inputs[1]);
            var startPos = new Point(startX, startY);
            
            inputs = Console.ReadLine().Split(' ');
            int targetX = int.Parse(inputs[0]);
            int targetY = int.Parse(inputs[1]);
            var targetPos = new Point(targetX, targetY);
            
            int switchCount = int.Parse(Console.ReadLine());
            var dictButtonToField = new Dictionary<Point, (Point, int)>();
            var dictFieldIndex = new Dictionary<Point, int>();
           // var switches = new List<Switch>();
            var fieldStatus = new List<int>();
            for (int i = 0; i < switchCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int switchX = int.Parse(inputs[0]);
                int switchY = int.Parse(inputs[1]);
                var switchPos = new Point(switchX, switchY);
                
                int blockX = int.Parse(inputs[2]);
                int blockY = int.Parse(inputs[3]);
                var blockPos = new Point(blockX, blockY);
                
                int initialState = int.Parse(inputs[4]); 
               // switches.Add(new Switch(switchPos, blockPos));
                dictButtonToField.Add(switchPos, (blockPos, i));
                dictFieldIndex.Add(blockPos, i);
                fieldStatus.Add(initialState);
            }
            
            int mask = 0;
            mask <<= 1;
            mask |= 1;
            for (int i = fieldStatus.Count-1; i >= 0; i--)
            {
                mask <<= 1;
                mask |= fieldStatus[i];
            }
            return new StateInit(map, targetPos, startPos, dictButtonToField, dictFieldIndex, mask, stoneList.ToArray());
        }
    }
}