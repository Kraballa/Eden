namespace Eden.UI
{
    public enum SType
    {
        Fit, // fit to children
        Fixed, // fixed sizing
        Grow // grow to parent
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

        public Sizing(int value) : this(SType.Fixed, value)
        {

        }

        public Sizing(SType type, int value)
        {
            Type = type;
            Value = value;
        }
    }

}