using System.Numerics;
using Raylib_cs;

namespace Eden;

public class CameraFollower : Component
{
    private SingleTagRenderer Renderer;
    private float Speed;

    public CameraFollower(SingleTagRenderer renderer, float speed) : base(true, false)
    {
        Renderer = renderer;
        Speed = speed;
    }

    override public void Update()
    {
        if (Entity == null) return;

        Renderer.CameraApproach(new Vector2(MathF.Round(Entity.Position.X), MathF.Round(Entity.Position.Y)), Speed * RaylibExt.GetSafeFrameTime());
    }
}