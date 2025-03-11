using Eden.Util;

namespace Eden
{
    public class Entity
    {
        public Scene? Scene;

        public ComponentList Components;

        public int Depth { get; set; }
        public bool Active { get; set; } = true;
        public bool Visible { get; set; } = true;        

        public Entity()
        {
            Components = new ComponentList(this);
        }

        public void Added(Scene scene)
        {
            Scene = scene;
        }

        public void Removed(Scene scene)
        {
            Scene = null;
        }

        public void Update()
        {

        }

        public void Render()
        {

        }
    }
}