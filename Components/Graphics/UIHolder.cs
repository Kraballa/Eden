using System.Linq.Expressions;
using Eden.UI;

namespace Eden
{
    public class UIHolder : GraphicsComponent
    {
        public UIElement? UI;

        public UIHolder(UIElement? ui = null) : base(true)
        {
            UI = ui;
        }

        public override int Width => Engine.Instance.Width;

        public override int Height => Engine.Instance.Height;

        public override void Render()
        {
            base.Render();
            UI?.Render();
        }
    }
}