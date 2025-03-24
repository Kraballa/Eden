using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public class Hitbox : Collider
    {
        public override float Width { get; set; }
        public override float Height { get; set; }
        public override float Left => Position.X;
        public override float Right => Position.X + Width;
        public override float Top => Position.Y;
        public override float Bottom => Position.Y + Height;

        public Hitbox(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public override bool Collide(Hitbox other)
        {
            return !(other.Left > Right || other.Right < Left || other.Top > Bottom || other.Bottom < Top);
        }

        public override bool Collide(CollisionGrid other)
        {
            return other.Collide(this);
        }

        public override void DebugRender(Color color)
        {
            Raylib.DrawRectangleLines((int)Position.X, (int)Position.Y, (int)Width, (int)Height, color);
        }

        public override bool Collide(Vector2 point)
        {
            return point.X > Left && point.X < Right && point.Y > Top && point.Y < Bottom;
        }
    }
}