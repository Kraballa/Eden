
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

        private Vector2 TrueCameraTarget;

        public SingleTagRenderer(BitTag tag)
        {
            Tag = tag;
            Camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
            TrueCameraTarget = Camera.Target;
        }

        public SingleTagRenderer CenterCamera()
        {
            Camera.Offset = Engine.ScreenCenter;
            return this;
        }

        public void CameraApproach(Vector2 target, float speed)
        {
            TrueCameraTarget = Vector2.Lerp(TrueCameraTarget, target, speed * Raylib.GetFrameTime());
        }

        public override void BeforeRender(Scene scene)
        {
            Camera.Target = new Vector2(MathF.Round(TrueCameraTarget.X * Camera.Zoom), MathF.Round(TrueCameraTarget.Y * Camera.Zoom)) / Camera.Zoom;
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

            if (Engine.Instance.Debug)
            {
                foreach (var entity in scene[Tag])
                    if (entity.Visible)
                        entity.DebugRender();
            }

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