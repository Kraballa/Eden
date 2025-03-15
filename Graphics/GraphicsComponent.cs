using System.Numerics;
using Raylib_cs;

namespace Eden.Graphics
{
    public abstract class GraphicsComponent : Component
    {
        public Vector2 Anchor = new();
        public Color Tint = Color.White;
        public Vector2 Position => Entity.Position + Anchor;
        public float Rotation;

        public abstract int Width { get; }
        public abstract int Height { get; }

        public GraphicsComponent() : base(true, true)
        {

        }

        /// <summary>
        /// Set anchor using justified positions. 0.5, 0.5 means the anchor is in the center.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void JustifyAnchor(float x, float y)
        {
            Anchor = new Vector2(-Width * x, -Height * y);
        }
    }
}