namespace Eden.UI
{
    public class UIBackdropElement : UIElement
    {
        protected MTexture Frame;
        protected int FrameCellSize = 16;

        public UIBackdropElement(string path, int cellsize)
        {
            Frame = new MTexture(TextureBank.Get(path));
            FrameCellSize = cellsize;
            Padding = [FrameCellSize, FrameCellSize, FrameCellSize, FrameCellSize];
        }

        public override void Render()
        {
            Frame.Draw9Slice(FrameCellSize, FrameCellSize, Position, Width, Height);
            base.Render();
        }
    }
}