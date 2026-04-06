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
        public Vector2 Justify = new(0, 0);
        public float FontSpacing = 2;
        public Color FontColor = Color.Black;
        public Font Font;

        protected Vector2 Anchor;

        public UIText(string text = "")
        {
            Font = Raylib.GetFontDefault();
            Text = text;
            SizingX = new(SType.Grow);
        }

        public UIText(Font font, string text = "")
        {
            Font = font;
            Text = text;
        }

        protected override void OnBeforeCalcLayout()
        {
            Vector2 textSize = Raylib.MeasureTextEx(Font, Text, Font.BaseSize, FontSpacing);
            Anchor = new Vector2((Width - textSize.X) * Justify.X, (Height - textSize.Y) * Justify.Y);

            SizingY = new(SType.Fixed, (int)textSize.Y);

            base.OnBeforeCalcLayout();
        }

        public override void Render()
        {
            base.Render();
            Raylib.DrawTextEx(Font, Text, new Vector2((int)Anchor.X + X, (int)Anchor.Y + Y), Font.BaseSize, FontSpacing, FontColor);
        }
    }
}