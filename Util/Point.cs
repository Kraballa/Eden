using System.Numerics;
using System.Runtime.CompilerServices;

namespace Eden
{
    public partial struct Point : IEquatable<Point>
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(int value) : this(value, value)
        {

        }


        public static Point Zero
        {

            get => default;
        }

        public static Point One
        {
            get => new Point(1);
        }

        public static Point UnitX
        {
            get => new Point(1, 0);
        }

        public static Point UnitY
        {
            get => new Point(0, 1);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator +(Point left, Point right)
        {
            return new Point(
                left.X + right.X,
                left.Y + right.Y
            );
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator /(Point left, Point right)
        {
            return new Point(
                left.X / right.X,
                left.Y / right.Y
            );
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point left, Point right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator *(Point left, Point right)
        {
            return new Point(
                left.X * right.X,
                left.Y * right.Y
            );
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator *(Point left, int right)
        {
            return left * new Point(right);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator *(int left, Point right)
        {
            return right * left;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator -(Point left, Point right)
        {
            return new Point(
                left.X - right.X,
                left.Y - right.Y
            );
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator -(Point value)
        {
            return Zero - value;
        }

        public bool Equals(Point other)
        {
            return this == other;
        }

        public override readonly string ToString()
        {
            return $"{X}:{Y}";
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Point && Equals((Point)obj);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
    }
}