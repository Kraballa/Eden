
using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public static class CameraExt
    {
        public static Rectangle GetClipRect(this Camera2D cam)
        {
            float width = Engine.Instance.Width / cam.Zoom;
            float height = Engine.Instance.Height / cam.Zoom;

            Vector2 pos = -cam.Offset / cam.Zoom + cam.Target;

            return new Rectangle(pos.X, pos.Y, width, height);
        }
    }
}