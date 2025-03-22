using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;
using Raylib_cs;

namespace Eden
{
    public class CollisionGrid : Collider
    {
        public override float Width { get => Data.GetLength(0) * CellWidth; set => throw new NotImplementedException(); }
        public override float Height { get => Data.GetLength(1) * CellHeight; set => throw new NotImplementedException(); }

        public bool[,] Data;

        public int CellsX { get; private set; }
        public int CellsY { get; private set; }
        public float CellWidth { get; private set; }
        public float CellHeight { get; private set; }

        public CollisionGrid(int cellX, int cellY, int cellWidth, int cellHeight)
        {
            Data = new bool[cellX, cellY];
            CellsX = cellX;
            CellsY = cellY;
            CellWidth = cellWidth;
            CellHeight = cellHeight;
        }

        public bool this[int x, int y]
        {
            get
            {
                if (x >= 0 && y >= 0 && x < CellsX && y < CellsY)
                    return Data[x, y];
                else
                    return false;
            }

            set
            {
                Data[x, y] = value;
            }
        }

        public override float Left => Position.X;
        public override float Right => Position.X + CellsX * CellWidth;
        public override float Top => Position.Y;
        public override float Bottom => Position.Y + CellsY * CellHeight;

        public override bool CollideCheck(Hitbox other)
        {
            int x = (int)((other.Position.X - Left) / CellWidth);
            int y = (int)((other.Position.Y - Top) / CellHeight);
            int w = (int)((other.Right - Left) / CellWidth) - x + 1;
            int h = (int)((other.Bottom - Top) / CellHeight) - y + 1;
            return CheckRect(x, y, w, h);
        }

        public override bool CollideCheck(CollisionGrid other)
        {
            throw new NotImplementedException();
        }

        public bool CheckRect(int x, int y, int w, int h)
        {
            int posY = 50;
            Raylib.DrawText("collision checks:", 10, 38, 10, Color.White);
            for (int xx = Math.Max(0, x); xx < Math.Min(CellsX, x + w); xx++)
            {
                for (int yy = Math.Max(0, y); yy < Math.Min(CellsY, y + h); yy++)
                {
                    Raylib.DrawText($"pos: {xx}:{yy}", 10, posY, 10, Color.White);
                    posY += 12;
                    if (Data[xx, yy])
                        return true;
                }
            }
            return false;
        }

        public override void DebugRender(Color color)
        {
            for (int x = 0; x < CellsX; x++)
            {
                for (int y = 0; y < CellsY; y++)
                {
                    if (Data[x, y])
                        Raylib.DrawRectangleLines((int)(x * CellWidth + Position.X), (int)(y * CellHeight + Position.Y), (int)CellWidth, (int)CellHeight, color);
                }
            }
        }
    }
}