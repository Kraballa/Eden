using System.Dynamic;
using System.Numerics;

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
            for (int x = 0; x < Data.GetLength(0); x++)
            {
                for (int y = 0; y < Data.GetLength(1); y++)
                {
                    if (Data[x, y] > 0)
                        Tileset[Data[x, y]].Draw(Position + new Vector2(x * Tileset.TileWidth, y * Tileset.TileHeight));
                }
            }
        }
    }
}