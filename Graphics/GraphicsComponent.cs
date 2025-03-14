using System.Numerics;
using Raylib_cs;

namespace Eden.Graphics
{
    public abstract class GraphicsComponent : Component
    {
        public Vector2 Offset = new();
        public Color Tint = Color.White;
        public Vector2 Position => Entity.Position + Offset;

        public abstract int Width { get; }
        public abstract int Height { get; }

        public GraphicsComponent() : base(true, true)
        {

        }

        public void Justify(float x, float y)
        {
            Offset = new Vector2(-Width * x, -Height * y);
        }
    }
}