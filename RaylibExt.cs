
namespace Raylib_cs;

public static class RaylibExt
{
    public static float MaxFrameTime = 1f / 30f;

    /// <summary>
    /// Returns the raylib frametime but capped at a maximum value to prevent glitches.
    /// </summary>
    public static float GetSafeFrameTime()
    {
        float frameTime = Raylib.GetFrameTime();
        return Math.Min(frameTime, MaxFrameTime);
    }
}