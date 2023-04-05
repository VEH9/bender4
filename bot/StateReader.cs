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
            return new State(init.bender, init.fieldStatus, init.stones);
        }

        // ReSharper disable once InconsistentNaming
        public static StateInit ReadInit(this ConsoleReader Console)
        {
            // Copy paste here the code for initialization input data (or delete if no initialization data in this game)
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int width = int.Parse(inputs[0]);
            int height = int.Parse(inputs[1]);
            var mapLine = new string[height];
            for (int i = 0; i < height; i++)
            {
                string line = Console.ReadLine();
                mapLine[i] = line;
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

            var switches = new List<Switch>();
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
                int initialState = int.Parse(inputs[4]); // 1 if blocking, 0 otherwise
                switches.Add(new Switch(switchPos, blockPos));
                fieldStatus.Add(initialState);
            }
            return new StateInit(width, height, mapLine, targetPos, startPos, switches.ToArray(), fieldStatus.ToArray());
        }
    }
}