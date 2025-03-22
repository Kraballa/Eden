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

        public override bool CollideCheck(Hitbox other)
        {
            return !(other.Left > Right || other.Right < Left || other.Top > Bottom || other.Bottom < Top);
        }

        public override bool CollideCheck(CollisionGrid other)
        {
            return other.CollideCheck(this);
        }

        public override void DebugRender(Color color)
        {
            Raylib.DrawRectangleLines((int)Position.X, (int)Position.Y, (int)Width, (int)Height, color);
        }
    }
}