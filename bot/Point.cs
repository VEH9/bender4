namespace bot
{

    public struct Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
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
        
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }
    }
}