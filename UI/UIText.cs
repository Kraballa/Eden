using System.Numerics;
using Raylib_cs;

namespace Eden.UI
{
    /// <summary>
    /// basic text label element.
    /// </summary>
    public class UIText : UIElement
    {
        public string Text = "";
        public Vector2 Justify = new(0, 0.5f);
        public int FontSize = 20;
        public float FontSpacing = 2;
        public Color FontColor = Color.Black;
        public bool ExpandRuleDynamic = false; // will adapt its sizing based on the parent. if parent sizing is not fit, grow. fixed otherwise.

        private Vector2 Anchor;

        public UIText(string text = "")
        {
            Text = text;
        }

        protected override void OnBeforeCalcLayout()
        {
            Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), Text, FontSize, FontSpacing);
            Anchor = new Vector2((Width - textSize.X) * Justify.X, (Height - textSize.Y) * Justify.Y);

            SizingX = new(SType.Fixed, (int)textSize.X);
            SizingY = new(SType.Fixed, (int)textSize.Y);

            if (ExpandRuleDynamic && Parent != null)
            {
                if (Parent.SizingX.Type != SType.Fit)
                {
                    SizingX = new(SType.Grow);
                }
                if (Parent.SizingY.Type == SType.Fit)
                {
                    SizingY = new(SType.Grow);
                }
            }
            base.OnBeforeCalcLayout();
        }

        public override void Render()
        {
            Raylib.DrawText(Text, (int)Anchor.X + X, (int)Anchor.Y + Y, FontSize, FontColor);
        }
    }
}