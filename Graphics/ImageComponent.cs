using Raylib_cs;

namespace Eden.Graphics
{
    public class ImageComponent : GraphicsComponent
    {
        public Texture2D Texture;
        public Rectangle ClipRect;

        public override int Width => Texture.Width;
        public override int Height => Texture.Height;

        public ImageComponent(string path, Rectangle clipRect)
        {
            Texture = Raylib.LoadTexture(path);
            ClipRect = clipRect;
        }

        public ImageComponent(string path, int x, int y, int width, int height) : this(path, new Rectangle(x, y, width, height))
        {
        }

        public override void Render()
        {
            Raylib.DrawTextureRec(Texture, ClipRect, Position, Tint);
        }
    }
}