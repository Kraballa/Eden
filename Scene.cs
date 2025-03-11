using System.Collections;
using Eden.Util;

namespace Eden
{
    public class Scene : IEnumerable<Entity>, IEnumerable
    {
        public EntityList Entities { get; private set; }

        public Scene()
        {
            Entities = new EntityList(this);
        }

        public virtual void BeforeUpdate()
        {
            Entities.UpdateLists();
        }

        public virtual void Update()
        {
            Entities.Update();
        }

        public virtual void Draw()
        {
            Entities.Render();
        }

        public void Add(Entity entity)
        {
            Entities.Add(entity);
        }

        public void Remove(Entity entity)
        {
            Entities.Remove(entity);
        }

        public void Add(IEnumerable<Entity> entities)
        {
            Entities.Add(entities);
        }

        public void Remove(IEnumerable<Entity> entities)
        {
            Entities.Remove(entities);
        }

        public void Add(params Entity[] entities)
        {
            Entities.Add(entities);
        }

        public void Remove(params Entity[] entities)
        {
            Entities.Remove(entities);
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Entities.GetEnumerator();
        }
    }
}