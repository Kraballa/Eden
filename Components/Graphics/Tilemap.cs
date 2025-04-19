using System.Data;
using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public class Tilemap : GraphicsComponent
    {
        public override int Width => Data.GetLength(0) * Tileset.TileWidth;
        public override int Height => Data.GetLength(1) * Tileset.TileHeight;

        public Tileset Tileset { get; private set; }
        public int[,] Data;

        public Tilemap(int[,] data, Tileset tileset) : base(false)
        {
            Tileset = tileset;
            Data = data;
        }

        public override void Render()
        {
            base.Render();

            bool useClipping = Scene.ActiveRenderer is SingleTagRenderer;

            int minX = 0;
            int minY = 0;
            int width = Data.GetLength(0) - 1;
            int height = Data.GetLength(1) - 1;

            if (useClipping)
            {
                SingleTagRenderer renderer = Scene.ActiveRenderer as SingleTagRenderer;
                Rectangle clipRect = renderer.Camera.GetClipRect();
                minX = Math.Max(minX, (int)Math.Floor(clipRect.X / Tileset.TileWidth));
                minY = Math.Max(minY, (int)Math.Floor(clipRect.Y / Tileset.TileHeight));
                width = Math.Min(width - minX + 1, (int)Math.Ceiling(clipRect.Width / Tileset.TileWidth) + 1);
                height = Math.Min(height - minY + 1, (int)Math.Ceiling(clipRect.Height / Tileset.TileHeight) + 1);
            }

            for (int x = minX; x < minX + width; x++)
            {
                for (int y = minY; y < minY + height; y++)
                {
                    if (Data[x, y] >= 0)
                        Tileset[Data[x, y]].Draw(Position + new Vector2(x * Tileset.TileWidth, y * Tileset.TileHeight));
                }
            }
        }
    }
}