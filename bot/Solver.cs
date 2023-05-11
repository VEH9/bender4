using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bot
{
    public class Solver
    {
        private Point finish;
        private bool[][] map;
        public Dictionary<Point, (Point, int)> dictButtonToField;
        public Dictionary<Point, int> dictFieldIndex;

        public string GetSolution(StateInit init, Countdown timer)
        {
            finish = init.Finish;
            map = init.Map;
            dictButtonToField = init.DictButtonToField;
            dictFieldIndex = init.DictFieldIndex;
            var bestPath = "";
            foreach (var path in FindShortestPath(init.FieldStatus, init.Bender, init.Stones))
            {
                if (path != null)
                {
                    var commands = GetCommands(path);
                    var compressedCommands = CompressString(commands, 1);
                    var p = compressedCommands.Length < commands.Length ? compressedCommands : commands;
                    if (p.Length <= bestPath.Length || bestPath == "")
                    {
                        bestPath = p;
                    }
                    
                    
                    if (timer.IsFinished())
                    {
                        return bestPath;
                    }
                    
                }
            }
            return bestPath;
        }
        
        private static string GetCommands(State path)
        {
            var commands = new StringBuilder();

            while (true)
            {
                if (path.ParentState == null)
                {
                    char[] charArray = commands.ToString().ToCharArray();
                    Array.Reverse(charArray);
                    return new string(charArray);
                }
                var pos = path.BenderPos;
                var prevPos = path.ParentState.BenderPos;

                if (pos.Y > prevPos.Y)
                    commands.Append('D');
                else if (pos.Y < prevPos.Y)
                    commands.Append('U');
                else if (pos.X > prevPos.X)
                    commands.Append('R');
                else if (pos.X < prevPos.X) 
                    commands.Append('L');

                path = path.ParentState;
            }
        }

        private IEnumerable<State> FindShortestPath(int fieldStatus, Point start, Point[] stones)
        {
            var visitedState = new Dictionary<Point, HashSet<int>>();
            var startState = new State(start, fieldStatus, stones);
            visitedState.Add(startState.BenderPos, new HashSet<int>{startState.FieldStatus});
            var queue = new PriorityQueue<State, int>();
            queue.Enqueue(startState, 0);
            while (queue.Count > 0)
            {
                var currentState = queue.Dequeue();
                
                foreach (var neighbor in GetNeighbors(currentState))
                {
                    if (visitedState.ContainsKey(neighbor.BenderPos) &&
                        visitedState[neighbor.BenderPos].Contains(neighbor.FieldStatus))
                    {
                        continue;
                    }

                    queue.Enqueue(neighbor, neighbor.usedSwitched);
                    if (neighbor.BenderPos == finish)
                        yield return neighbor;
                    if (visitedState.ContainsKey(neighbor.BenderPos))
                        visitedState[neighbor.BenderPos].Add(neighbor.FieldStatus);
                    else 
                        visitedState.Add(neighbor.BenderPos, new HashSet<int>{neighbor.FieldStatus});
                }
            }
        }

        private IEnumerable<State> GetNeighbors(State currentState)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                var state = new State(currentState, currentState.BenderPos + dir.ToPoint(), dictButtonToField, map, finish, dictFieldIndex);
                if (state.ParentState != null)
                    yield return state;
            }
        }

        public static string CompressString(string inputString, int deph)
        {
            if (deph == 10)
                return inputString;

            var substringCounts = new HashSet<string>();
            var separator = inputString.IndexOf(';');
            var length = separator == -1 ? inputString.Length : separator + 1;
            var mostFrequentSubstring = "";
            var mostValue = 0;

            for (int i = 0; i < length; i++)
            {
                for (int j = i + 2; j < length; j++)
                {
                    var subLength = j - i;
                    var substring = inputString.Substring(i, subLength);
                    if (substringCounts.Contains(substring))
                        continue;
                    int counter = 0;
                    var k = -substring.Length;
                    while ((k = inputString.IndexOf(substring, k + substring.Length, StringComparison.Ordinal)) > -1)
                        counter++;
                    substringCounts.Add(substring);
                    if (counter == 1)
                        break;
                    var switchValue = (counter - 1) * substring.Length - 1 - counter;
                    if (switchValue >= mostValue)
                    {
                        mostFrequentSubstring = substring;
                        mostValue = switchValue;
                    }
                }
            }


            if (mostFrequentSubstring != "")
            {
                var newString = inputString.Replace(mostFrequentSubstring, $"{deph}") + $";{mostFrequentSubstring}";
                return CompressString(newString, deph + 1);
            }
            return inputString;
        }
    }
}