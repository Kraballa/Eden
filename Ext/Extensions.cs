using System.Numerics;

namespace Eden
{
    public static class Extensions
    {
        public static Point ToPoint(this Vector2 vect)
        {
            return new Point((int)Math.Round(vect.X), (int)Math.Round(vect.Y));
        }
    }
}