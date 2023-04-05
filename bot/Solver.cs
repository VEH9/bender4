using System.Collections.Generic;
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
            var visitedState = new HashSet<int>();

            var startState = new State(start, fieldStatus, stones);
            visitedState.Add(startState.GetHashSum());
            var queue = new Queue<State>();
            queue.Enqueue(startState);
            while (queue.Count > 0)
            {
                var currentState = queue.Dequeue();
                
                foreach (var neighbor in GetNeighbors(currentState, visitedState, queue))
                {
                    if (visitedState.Contains(neighbor.GetHashSum()))
                    {
                        continue;
                    }
                    
                    if (neighbor.BenderPos == finish)
                    {
                        return neighbor.path;
                    }

                    visitedState.Add(neighbor.GetHashSum());
                }
            }
            
            return null;
        }

        private List<State> GetNeighbors(State currentState, HashSet<int> visitedState, Queue<State> queue)
        {
            var neighbors = new List<State>();
            
            if (Sim.CanVisit(map, currentState, Direction.Left, finish, switchesArr))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X - 1, currentState.BenderPos.Y), switchesArr);
                if (!visitedState.Contains(newState.GetHashSum()))
                {
                    neighbors.Add(newState);
                    queue.Enqueue(newState);
                }
            }
            
            if (Sim.CanVisit(map, currentState, Direction.Right, finish, switchesArr))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X + 1, currentState.BenderPos.Y), switchesArr);
                if (!visitedState.Contains(newState.GetHashSum()))
                {
                    neighbors.Add(newState);
                    queue.Enqueue(newState);
                }
            }

            if (Sim.CanVisit(map, currentState, Direction.Down, finish, switchesArr))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X, currentState.BenderPos.Y - 1), switchesArr);
                if (!visitedState.Contains(newState.GetHashSum()))
                {
                    neighbors.Add(newState);
                    queue.Enqueue(newState);
                }
            }

            if (Sim.CanVisit(map, currentState, Direction.Up, finish, switchesArr))
            {
                var newState = new State(currentState, new Point(currentState.BenderPos.X, currentState.BenderPos.Y + 1), switchesArr);
                if (!visitedState.Contains(newState.GetHashSum()))
                {
                    neighbors.Add(newState);
                    queue.Enqueue(newState);
                }
            }

            return neighbors;
        }
    }
}   