using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public abstract class GraphicsComponent : Component
    {
        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 Scale = Vector2.One;

        public float Rotation;
        public Color Tint = Color.White;

        public abstract int Width { get; }
        public abstract int Height { get; }

        public GraphicsComponent(bool active) : base(active, true)
        {

        }
    }
}