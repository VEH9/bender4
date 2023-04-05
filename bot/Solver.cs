using System.Collections.Generic;
using System.Text;

namespace bot
{
    public class Solver
    {
        public readonly Point finish;
        public readonly bool[][] map;

        public Solver(Point finish, bool[][] map)
        {
            this.finish = finish;
            this.map = map;
        }
        public string GetSolution(StateInit init)
        {
            var path = FindShortestPath(init.switches, init.bender, init.finish, init.stones);
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

        private List<Point> FindShortestPath(Switch[] switches, Point start, Point finish, Point[] stones)
        {
            var visitedPoint = new HashSet<int>();

            var startState = new State(start, switches, stones);
            visitedPoint.Add(startState.HashSum);
            var queue = new Queue<State>();
            queue.Enqueue(startState);
            while (queue.Count > 0)
            {
                var currentState = queue.Dequeue();
                
                foreach (var neighbor in GetNeighbors(currentState, visitedPoint, queue))
                {
                    if (visitedPoint.Contains(neighbor.HashSum))
                    {
                        continue;
                    }
                    
                    if (neighbor.BenderPos == finish)
                    {
                        return neighbor.path;
                    }

                    visitedPoint.Add(neighbor.HashSum);
                }
            }
            
            return null;
        }

        private List<State> GetNeighbors(State currentState, HashSet<int> visitedPoint, Queue<State> queue)
        {
            var neighbors = new List<State>();
            
            if (Sim.CanVisit(map, currentState, Direction.Left, finish))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X - 1, currentState.BenderPos.Y));
                if (!visitedPoint.Contains(newState.HashSum))
                {
                    neighbors.Add(newState);
                    queue.Enqueue(newState);
                }
            }
            
            if (Sim.CanVisit(map, currentState, Direction.Right, finish))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X + 1, currentState.BenderPos.Y));
                if (!visitedPoint.Contains(newState.HashSum))
                {
                    neighbors.Add(newState);
                    queue.Enqueue(newState);
                }
            }

            if (Sim.CanVisit(map, currentState, Direction.Down, finish))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X, currentState.BenderPos.Y - 1));
                if (!visitedPoint.Contains(newState.HashSum))
                {
                    neighbors.Add(newState);
                    queue.Enqueue(newState);
                }
            }

            if (Sim.CanVisit(map, currentState, Direction.Up, finish))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X, currentState.BenderPos.Y + 1));
                if (!visitedPoint.Contains(newState.HashSum))
                {
                    neighbors.Add(newState);
                    queue.Enqueue(newState);
                }
            }

            return neighbors;
        }
    }
}   