using Raylib_cs;

namespace Eden
{
    public enum ResourceScope
    {
        Engine,
        Scene
    }

    public static class TextureBank
    {
        private static Dictionary<string, Texture2D> globalTextures = new();
        private static Dictionary<string, Texture2D> sceneTextures = new();

        public static Texture2D Get(string path, ResourceScope scope = ResourceScope.Engine)
        {
            if (scope == ResourceScope.Engine)
            {
                if (sceneTextures.TryGetValue(path, out Texture2D outTex))
                {
                    return outTex;
                }
                Texture2D tex = Raylib.LoadTexture(path);
                if (tex.Width == 0)
                {
                    throw new Exception($"error, couldn't load image at path '{path}'");
                }
                globalTextures.Add(path, tex);
                return tex;
            }
            else
            {
                if (sceneTextures.TryGetValue(path, out Texture2D outTex))
                {
                    return outTex;
                }
                Texture2D tex = Raylib.LoadTexture(path);
                if (tex.Width == 0)
                {
                    throw new Exception($"error, couldn't load image at path '{path}'");
                }
                sceneTextures.Add(path, tex);
                return tex;
            }
        }

        public static void OnSceneEnd()
        {
            foreach (var kv in sceneTextures)
            {
                Raylib.UnloadTexture(kv.Value);
            }
            sceneTextures.Clear();
        }
    }
}