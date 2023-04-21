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

        public string GetSolution(StateInit init)
        {
            finish = init.Finish;
            map = init.Map;
            dictButtonToField = init.DictButtonToField;
            dictFieldIndex = init.DictFieldIndex;
            var path = FindShortestPath(init.FieldStatus, init.Bender, init.Stones);
            if (path != null)
            {
                var commands = GetCommands(path);
                return commands;
            }
            else
            {
                return "Путь не найден";
            }
        }
        
        private static string GetCommands(List<Point> path)
        {
            var commands = new StringBuilder();

            for (int i = 1; i < path.Count; i++)
            {
                if (path[i].Y > path[i - 1].Y)
                {
                    commands.Append('D');
                }
                else if (path[i].Y < path[i - 1].Y)
                {
                    commands.Append('U');
                }
                else if (path[i].X > path[i - 1].X)
                {
                    commands.Append('R');
                }
                else if (path[i].X < path[i - 1].X)
                {
                    commands.Append('L');
                }
            }

            return commands.ToString();
        }

        private List<Point> FindShortestPath(int fieldStatus, Point start, Point[] stones)
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
                        return neighbor.Path;
                    if (visitedState.ContainsKey(neighbor.BenderPos))
                        visitedState[neighbor.BenderPos].Add(neighbor.FieldStatus);
                    else 
                        visitedState.Add(neighbor.BenderPos, new HashSet<int>{neighbor.FieldStatus});
                }
            }
            
            return null;
        }

        private IEnumerable<State> GetNeighbors(State currentState)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                var state = new State(currentState, currentState.BenderPos + dir.ToPoint(), dictButtonToField, map, finish, dictFieldIndex);
                if (state.Path != null)
                    yield return state;
            }
        }
    }
}   