namespace bot
{

    public class Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Point other = (Point)obj;
            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(Point a, Point b)
        {
            if (ReferenceEquals(a, null))
            {
                return ReferenceEquals(b, null);
            }

            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
        
        public static Point operator +(Point a, Point b)
        {
            int x = a.X + b.X;
            int y = a.Y + b.Y;
            return new Point(x, y);
        }
        
        public static Point operator -(Point a, Point b)
        {
            int x = a.X - b.X;
            int y = a.Y - b.Y;
            return new Point(x, y);
        }
    }
}