using System;

namespace bot
{

    public enum Direction
    {
        Right = 0,
        Left = 1,
        Down = 2,
        Up = 3
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
                    return new Point(0, -1);
                case Direction.Up:
                    return new Point(0, 1);
                default:
                    throw new ArgumentException("Invalid direction.");
            }
        }
    }
}