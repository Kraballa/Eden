namespace Eden.UI
{
    public enum SType
    {
        Fit,
        Fixed,
    }

    public enum Layout
    {
        Horizontal,
        Vertical
    }

    public struct Sizing
    {
        public SType Type;
        public int Value;

        public Sizing()
        {

        }

        public Sizing(SType type, int value)
        {
            Type = type;
            Value = value;
        }
    }

}