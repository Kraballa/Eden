
using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public class SingleTagRenderer : Renderer
    {
        public BitTag Tag;
        public Camera2D Camera;
        public Rectangle ScissorRect;
        public bool ClipRender;

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
            
        }

        public override void Render(Scene scene)
        {
            Raylib.BeginMode2D(Camera);
            if (ClipRender)
            {
                Raylib.BeginScissorMode((int)ScissorRect.X, (int)ScissorRect.Y, (int)ScissorRect.Width, (int)ScissorRect.Height);
            }

            foreach (var entity in scene[Tag])
                if (entity.Visible)
                    entity.Render();

            if (ClipRender)
            {
                Raylib.EndScissorMode();
            }
            Raylib.EndMode2D();
        }

        public override void AfterRender(Scene scene)
        {
            
        }
    }
}