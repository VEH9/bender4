using System.Collections.Generic;

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
            string commands = "";

            for (int i = 1; i < path.Count; i++)
            {
                if (path[i].Y > path[i - 1].Y)
                {
                    commands += "D";
                }
                else if (path[i].Y < path[i - 1].Y)
                {
                    commands += "U";
                }
                else if (path[i].X > path[i - 1].X)
                {
                    commands += "R";
                }
                else if (path[i].X < path[i - 1].X)
                {
                    commands += "L";
                }
            }

            return commands;
        }

        private List<Point> FindShortestPath(Switch[] switches, Point start, Point finish, Point[] stones)
        {
            var distances = new int[map[0].Length, map.Length];
            for (int i = 0; i < map[0].Length; i++)
            {
                for (int j = 0; j < map.Length; j++)
                {
                    distances[i, j] = -1;
                }
            }

            distances[start.X, start.Y] = 0;
            var queue = new Queue<Point>();
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                var currentPoint = queue.Dequeue();

                if (currentPoint == finish)
                {
                    return ReconstructPath(distances, start, finish);
                }

                foreach (var neighbor in GetNeighbors(switches, distances, queue, currentPoint, stones))
                {
                    var distance = int.MaxValue;
                    if (distances[neighbor.X, neighbor.Y] != -1)
                    {
                        continue;
                    }
                    
                    distance = 1;
                    if (distances[currentPoint.X, currentPoint.Y] + distance < distances[neighbor.X, neighbor.Y] ||
                        distances[neighbor.X, neighbor.Y] == -1)
                    {
                        distances[neighbor.X, neighbor.Y] = distances[currentPoint.X, currentPoint.Y] + distance;
                        queue.Enqueue(neighbor);
                    }
                }
            }
            
            return null;
        }

        private List<Point> ReconstructPath(int[,] distances, Point start, Point finish)
        {
            var path = new List<Point>();
            path.Add(finish);

            var currentPoint = finish;

            while (currentPoint != start)
            {
                foreach (var neighbor in GetNeighbors(currentPoint, distances))
                {
                    var distance = distances[currentPoint.X, currentPoint.Y];

                    if (distance == distances[neighbor.X, neighbor.Y] + 1)
                    {
                        path.Add(neighbor);
                        currentPoint = neighbor;
                        break;
                    }
                }
            }
            
            path.Reverse();
            return path;
        }

        private List<Point> GetNeighbors(Point cell, int[,] distances)
        {
            var neighbors = new List<Point>();
            
            if (cell.X > 0 && distances[cell.X - 1, cell.Y] != -1)
            {
                neighbors.Add(new Point(cell.X - 1, cell.Y));
            }
    
            if (cell.X < distances.GetLength(0) - 1 && distances[cell.X + 1, cell.Y] != -1)
            {
                neighbors.Add(new Point(cell.X + 1, cell.Y));
            }
    
            if (cell.X > 0 && distances[cell.X, cell.Y - 1] != -1)
            {
                neighbors.Add(new Point(cell.X, cell.Y - 1));
            }
    
            if (cell.Y < distances.GetLength(1) - 1 && distances[cell.X, cell.Y + 1] != -1)
            {
                neighbors.Add(new Point(cell.X, cell.Y + 1));
            }
    
            return neighbors;
        }

        private List<Point> GetNeighbors(Switch[] switches, int[,] distances, Queue<Point> queue,
            Point position, Point[] stones)
        {
            var neighbors = new List<Point>();
            
            if (Sim.CanVisit(map, distances, position, Direction.Left, stones, switches, finish))
            {
                var point = new Point(position.X - 1, position.Y);
                neighbors.Add(point);
                distances[position.X - 1, position.Y] = distances[position.X, position.Y] + 1;
                queue.Enqueue(point);
            }
            
            if (Sim.CanVisit(map, distances, position, Direction.Right, stones, switches, finish))
            {
                var point = new Point(position.X + 1, position.Y);
                neighbors.Add(point);
                distances[position.X + 1, position.Y] = distances[position.X, position.Y] + 1;
                queue.Enqueue(point);
            }

            if (Sim.CanVisit(map, distances, position, Direction.Down, stones, switches, finish))
            {
                var point = new Point(position.X, position.Y - 1);
                neighbors.Add(point);
                distances[position.X, position.Y - 1] = distances[position.X, position.Y] + 1;
                queue.Enqueue(point);
            }

            if (Sim.CanVisit(map, distances, position, Direction.Up, stones, switches, finish))
            {
                var point = new Point(position.X, position.Y + 1);
                neighbors.Add(point);
                distances[position.X, position.Y + 1] = distances[position.X, position.Y] + 1;
                queue.Enqueue(point);
            }

            return neighbors;
        }
    }
}   