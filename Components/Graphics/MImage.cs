using System.Numerics;

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
            Scale.X = -1;
        }

        public override void Render()
        {
            Texture.Draw(Position + Entity.Position, Origin, Scale * Entity.Transform.Scale, Rotation, Tint);
        }

        public MImage SetOrigin(float x, float y)
        {
            Origin.X = x;
            Origin.Y = y;
            return this;
        }

        public MImage CenterOrigin()
        {
            Origin.X = Texture.ClipRect.Width / 2f;
            Origin.Y = Texture.ClipRect.Height / 2f;
            return this;
        }

        public MImage JustifyOrigin(Vector2 at)
        {
            Origin.X = Texture.ClipRect.Width * at.X;
            Origin.Y = Texture.ClipRect.Height * at.Y;
            return this;
        }

        public MImage JustifyOrigin(float x, float y)
        {
            Origin.X = Texture.ClipRect.Width * x;
            Origin.Y = Texture.ClipRect.Height * y;
            return this;
        }
    }
}