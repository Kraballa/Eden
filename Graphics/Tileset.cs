using System.Xml;

namespace Eden
{
    public class Tileset
    {
        private static Dictionary<string, Tileset> CachedTilesets = new();

        public MTexture Texture;
        public int TileWidth;
        public int TileHeight;

        public MTexture[] Tiles;

        public static Tileset FromTiledTileset(string path)
        {
            if (CachedTilesets.ContainsKey(path))
            {
                return CachedTilesets[path];
            }

            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNode tilesetNode = document.DocumentElement.SelectSingleNode("/tileset");
            foreach (XmlAttribute attr in tilesetNode.Attributes)
            {
                Console.WriteLine($"{attr.Name}: {attr.Value}");
            }
            int tileheight = int.Parse(tilesetNode.Attributes["tileheight"].Value);
            int tilewidth = int.Parse(tilesetNode.Attributes["tilewidth"].Value);
            XmlNode imageNode = tilesetNode["image"];
            string imageSource = imageNode.Attributes["source"].Value;
            imageSource = Path.Join(Path.GetDirectoryName(path), imageSource);
            MTexture texture = new MTexture(TextureBank.Get(imageSource));

            Tileset tileset = new Tileset(texture, tilewidth, tileheight);
            CachedTilesets[path] = tileset;
            return tileset;
        }

        public Tileset(MTexture texture, int tileWidth, int tileHeight)
        {
            Texture = texture;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            int numX = texture.Width / tileWidth;
            int numY = texture.Height / tileHeight;

            int numTiles = numX + numY * numX;

            Tiles = new MTexture[numTiles];
            for (int i = 0; i < numTiles; i++)
            {
                Tiles[i] = new MTexture(Texture, (i % numX) * TileWidth, (i / numX) * TileHeight, TileWidth, TileHeight);
            }
        }

        public MTexture this[int tileId]
        {
            get
            {
                return Tiles[tileId];
            }
        }
    }
}