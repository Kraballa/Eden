namespace Eden.UI
{
    /// <summary>
    /// very basic shorthand spacing element.
    /// </summary>
    public class UISpacing : UIElement
    {
        public UISpacing(bool horiz = false, bool vert = false)
        {
            if (horiz)
            {
                SizingX = new(SType.Grow);
            }
            if (vert)
            {
                SizingY = new(SType.Grow);
            }
        }
    }
}