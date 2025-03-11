using System.IO.Compression;

namespace Eden
{
    public class Component
    {
        Entity Parent;
        public bool Active { get; set; } = true;
        public bool Visible { get; set; } = true;

        public Component()
        {

        }

        public virtual void Added(Entity entity)
        {

        }

        public virtual void Removed(Entity entity)
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Render()
        {

        }

    }
}