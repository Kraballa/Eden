using System.Numerics;
using Raylib_cs;

namespace Eden.Graphics
{
    public class ImageComponent : GraphicsComponent
    {
        public Texture2D Texture;
        public Rectangle ClipRect;

        public override int Width => Texture.Width;
        public override int Height => Texture.Height;

        public Vector2 Scale
        {
            get => scale;
            set
            {
                scale = value;
                TargetRect.Width = Width * value.X;
                TargetRect.Height = Height * value.Y;
            }
        }

        private Vector2 scale = Vector2.One;

        private Rectangle TargetRect;

        public ImageComponent(string path, Rectangle clipRect) : this(path)
        {
            Texture = Raylib.LoadTexture(path);
            ClipRect = clipRect;

            TargetRect = new Rectangle(0, 0, Width, Height);
        }

        public ImageComponent(string path, int x, int y, int width, int height) : this(path, new Rectangle(x, y, width, height))
        {
        }

        public ImageComponent(string path)
        {
            Texture = Raylib.LoadTexture(path);
            ClipRect = new Rectangle(0, 0, Width, Height);
            TargetRect = ClipRect;
        }

        public override void Render()
        {
            TargetRect.Position = Entity.Position;
            Raylib.DrawTexturePro(Texture, ClipRect, TargetRect, Anchor * scale, Rotation, Tint);
        }
    }
}