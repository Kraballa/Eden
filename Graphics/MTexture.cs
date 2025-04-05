using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public class MTexture
    {
        public int Width => (int)ClipRect.Width;
        public int Height => (int)ClipRect.Height;

        public Texture2D Texture;
        public Rectangle ClipRect;
        public Vector2 DrawOffset = Vector2.Zero;
        public MTexture? Parent;

        private Rectangle TargetRect;

        public static MTexture FromFile(string path)
        {
            Texture2D tex = TextureBank.Get(path);
            return new MTexture(tex);
        }

        public MTexture(Texture2D texture) : this(texture, new Rectangle(0, 0, texture.Width, texture.Height))
        {
        }


        public MTexture(Texture2D texture, int x, int y, int width, int height) : this(texture, new Rectangle(x, y, width, height))
        {

        }

        public MTexture(Texture2D texture, Rectangle rect)
        {
            Texture = texture;
            ClipRect = rect;
            TargetRect = rect;
        }

        public MTexture(MTexture parent, int x, int y, int width, int height) : this(parent.Texture, x, y, width, height)
        {
            Parent = parent;
        }

        public MTexture(MTexture parent, Rectangle clipRect) : this(parent.Texture, clipRect)
        {
            
        }

        public void SetClipRect(int x, int y, int width, int height)
        {
            ClipRect = new Rectangle(x, y, width, height);
        }

        #region basic draw

        public void Draw(Vector2 pos)
        {
            Raylib.DrawTextureRec(Texture, ClipRect, pos - DrawOffset, Color.White);
        }

        public void Draw(Vector2 pos, Color color)
        {
            Raylib.DrawTextureRec(Texture, ClipRect, pos - DrawOffset, color);
        }

        public void Draw(Vector2 pos, float rotation, float scale)
        {
            TargetRect.Position = pos - DrawOffset;
            TargetRect.Width = Width * scale;
            TargetRect.Height = Height * scale;
            Raylib.DrawTexturePro(Texture, ClipRect, TargetRect, Vector2.Zero, rotation, Color.White);
        }

        public void Draw(Vector2 pos, float rotation, float scale, Color tint)
        {
            TargetRect.Position = pos - DrawOffset;
            TargetRect.Width = Width * scale;
            TargetRect.Height = Height * scale;
            Raylib.DrawTexturePro(Texture, ClipRect, TargetRect, Vector2.Zero, rotation, tint);
        }

        public void Draw(Vector2 pos, Vector2 origin, Vector2 scale, float rotation, Color color)
        {
            TargetRect.Position = pos - DrawOffset;
            TargetRect.Width = Math.Abs(ClipRect.Width) * scale.X;
            TargetRect.Height = Math.Abs(ClipRect.Height) * scale.Y;
            ClipRect.Width = Math.Sign(scale.X) * Math.Abs(ClipRect.Width);
            ClipRect.Height = Math.Sign(scale.Y) * Math.Abs(ClipRect.Height);
            Raylib.DrawTexturePro(Texture, ClipRect, TargetRect, origin - DrawOffset, rotation, color);
        }

        #endregion
    }
}