using System.Numerics;

namespace Eden
{
    public static class Extensions
    {
        public static Point ToPoint(this Vector2 vect)
        {
            return new Point((int)Math.Round(vect.X), (int)Math.Round(vect.Y));
        }

        public static Vector2 ClampSubdivisions(this Vector2 value, int subdivisions)
        {
            if (value == Vector2.Zero) return Vector2.Zero;

            int totalDirections = 4 * (subdivisions + 1);
            float step = MathF.Tau / totalDirections;
            float angle = MathF.Atan2(value.Y, value.X);
            float snapped = MathF.Round(angle / step) * step;
            float length = value.Length();
            return new Vector2(MathF.Cos(snapped), MathF.Sin(snapped)) * length;
        }
    }
}