
using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public class SingleTagRenderer : Renderer
    {
        public BitTag Tag;
        public Camera2D Camera;

        public SingleTagRenderer(BitTag tag)
        {
            Tag = tag;
            Camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
        }

        public SingleTagRenderer CenterCamera()
        {
            Camera.Offset = Engine.ScreenCenter;
            return this;
        }

        public override void BeforeRender(Scene scene)
        {
            Raylib.BeginMode2D(Camera);
        }

        public override void Render(Scene scene)
        {
            foreach (var entity in scene[Tag])
                if (entity.Visible)
                    entity.Render();
        }

        public override void AfterRender(Scene scene)
        {
            Raylib.EndMode2D();
        }
    }
}