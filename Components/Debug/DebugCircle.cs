using Raylib_cs;

namespace Eden;

public class DebugCircle : Component
{
    public float Radius { get; set; }

    public DebugCircle(float radius) : base(true, true)
    {
        Radius = radius;
    }

    public override void Render()
    {
        base.Render();
        Raylib.DrawCircleV(Entity.Position, Radius, Color.Red);
    }
}