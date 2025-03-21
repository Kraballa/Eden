using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public class MImage : GraphicsComponent
    {
        public MTexture Texture;

        public override int Width => Texture.Width;
        public override int Height => Texture.Height;


        public MImage(MTexture texture) : base(false)
        {
            Texture = texture;
        }

        public override void Render()
        {
            Texture.Draw(Position + Entity.Position, Origin, Scale, Rotation, Tint);
        }

        public MImage SetOrigin(float x, float y)
        {
            Origin.X = x;
            Origin.Y = y;
            return this;
        }

        public MImage CenterOrigin()
        {
            Origin.X = Width / 2f;
            Origin.Y = Height / 2f;
            return this;
        }

        public MImage JustifyOrigin(Vector2 at)
        {
            Origin.X = Width * at.X;
            Origin.Y = Height * at.Y;
            return this;
        }

        public MImage JustifyOrigin(float x, float y)
        {
            Origin.X = Width * x;
            Origin.Y = Height * y;
            return this;
        }
    }
}