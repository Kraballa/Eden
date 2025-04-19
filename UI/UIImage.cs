namespace Eden.UI
{
    /// <summary>
    /// basic image element.
    /// </summary>
    public class UIImage : UIElement
    {
        public MTexture Texture;
        public float Scale;

        public UIImage(MTexture texture, float scale = 1f)
        {
            Texture = texture;
            Scale = scale;
            SizingX = new((int)(texture.Width * scale));
            SizingY = new((int)(texture.Height * scale));
        }

        public override void Render()
        {
            Texture.Draw(Position.ToVector2(), 0, Scale);
            base.Render();
        }
    }
}