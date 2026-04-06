namespace Eden
{
    public class SpecComponent<T> : Component where T : Entity
    {
        public SpecComponent(bool active, bool visible) : base(active, visible)
        {
        }

        public T SpecEntity { get { return (T)Entity; } }
    }
}