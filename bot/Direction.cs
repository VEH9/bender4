using System;

namespace bot
{

    public enum Direction
    {
        Left, 
        Up, 
        Down, 
        Right
    }
    
    public static class DirectionExtensions
    {
        public static Point ToPoint(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return new Point(1, 0);
                case Direction.Left:
                    return new Point(-1, 0);
                case Direction.Down:
                    return new Point(0, 1);
                case Direction.Up:
                    return new Point(0, -1);
                default:
                    throw new ArgumentException("Invalid direction.");
            }
        }
    }
}