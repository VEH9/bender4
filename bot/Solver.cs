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
        private Switch[] switchesArr;

        public string GetSolution(StateInit init)
        {
            finish = init.finish;
            map = init.Map;
            switchesArr = init.switches;
            var path = FindShortestPath(init.fieldStatus, init.bender, init.finish, init.stones);
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
        
        private string GetCommands(List<Point> path)
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

        private List<Point> FindShortestPath(int[] fieldStatus, Point start, Point finish, Point[] stones)
        {
            //var visitedState = new HashSet<int>();
            var visitedState = new Dictionary<Point, HashSet<int>>();
            var startState = new State(start, fieldStatus, stones);
            //visitedState.Add(startState.HashSum);
            visitedState.Add(startState.BenderPos, new HashSet<int>{startState.HashSum});
            var queue = new PriorityQueue<State, int>();
            queue.Enqueue(startState, 0);
            while (queue.Count > 0)
            {
                var currentState = queue.Dequeue();
                
                foreach (var neighbor in GetNeighbors(currentState))
                {
                    
                    /*if (visitedState.Contains(neighbor.HashSum))
                    {
                        continue;
                    }
                    
                    queue.Enqueue(neighbor);
                    
                    if (neighbor.BenderPos == finish)
                    {
                        return neighbor.path;
                    }

                    visitedState.Add(neighbor.HashSum);
                    */

                    if (visitedState.ContainsKey(neighbor.BenderPos) &&
                        visitedState[neighbor.BenderPos].Contains(neighbor.HashSum))
                    {
                        continue;
                    }

                    queue.Enqueue(neighbor, neighbor.usedSwitched);
                    if (neighbor.BenderPos == finish)
                    {
                        return neighbor.path;
                    }
                    if (visitedState.ContainsKey(neighbor.BenderPos))
                        visitedState[neighbor.BenderPos].Add(neighbor.HashSum);
                    else 
                        visitedState.Add(neighbor.BenderPos, new HashSet<int>{neighbor.HashSum});
                }
            }
            
            return null;
        }

        private List<State> GetNeighbors(State currentState)
        {
            var neighbors = new List<State>();
            
            if (Sim.CanVisit(map, currentState, Direction.Left, finish, switchesArr))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X - 1, currentState.BenderPos.Y), switchesArr);
                neighbors.Add(newState);
            }
            
            if (Sim.CanVisit(map, currentState, Direction.Right, finish, switchesArr))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X + 1, currentState.BenderPos.Y), switchesArr);
                neighbors.Add(newState);
            }

            if (Sim.CanVisit(map, currentState, Direction.Down, finish, switchesArr))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X, currentState.BenderPos.Y - 1), switchesArr);
                neighbors.Add(newState);
            }

            if (Sim.CanVisit(map, currentState, Direction.Up, finish, switchesArr))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X, currentState.BenderPos.Y + 1), switchesArr);
                neighbors.Add(newState);
            }

            return neighbors;
        }
    }
}   