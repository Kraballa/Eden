using Raylib_cs;

namespace Eden
{
    public class Pixel : GraphicsComponent
    {
        public Pixel() : base(false)
        {
        }

        public override int Width => 1;

        public override int Height => 1;

        public override void Render()
        {
            base.Render();
            Raylib.DrawRectangle((int)Entity.Position.X, (int)Entity.Position.Y, (int)Scale.X, (int)Scale.Y, Color.White);
        }
    }
}