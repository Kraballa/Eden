using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public class ImageComponent : GraphicsComponent
    {
        public Texture2D Texture;
        public Rectangle ClipRect;

        public override int Width => Texture.Width;
        public override int Height => Texture.Height;

        public Vector2 Scale
        {
            get
            {
                return scale * Entity.Transform.Scale;
            }
            set
            {
                scale = value;
            }
        }

        private Vector2 scale = Vector2.One;

        private Rectangle TargetRect;

        public ImageComponent(Texture2D tex, Rectangle clipRect) : this(tex)
        {
            Texture = tex;
            ClipRect = clipRect;

            TargetRect = new Rectangle(0, 0, Width, Height);
        }

        public ImageComponent(Texture2D tex, int x, int y, int width, int height) : this(tex, new Rectangle(x, y, width, height))
        {
        }

        public ImageComponent(Texture2D tex)
        {
            Texture = tex;
            ClipRect = new Rectangle(0, 0, Width, Height);
            TargetRect = ClipRect;
        }

        public override void Render()
        {
            Vector2 scaleCopy = Scale;
            TargetRect.Position = Entity.Position;
            TargetRect.Width = Width * scaleCopy.X;
            TargetRect.Height = Height * scaleCopy.Y;
            Raylib.DrawTexturePro(Texture, ClipRect, TargetRect, Anchor * scaleCopy, Rotation + Entity.Transform.Rotation, Tint);
        }
    }
}