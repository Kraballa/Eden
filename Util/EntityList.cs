
using System.Collections;

namespace Eden.Util
{
    public class EntityList : IEnumerable<Entity>, IEnumerable
    {
        public int Count => entities.Count;

        private Scene Scene;

        private List<Entity> entities = [];
        private List<Entity> toAdd = [];
        private List<Entity> toRemove = [];

        private HashSet<Entity> current = [];
        private HashSet<Entity> adding = [];
        private HashSet<Entity> removing = [];

        private bool unsorted;

        public EntityList(Scene scene){
            Scene = scene;
        }

        public void UpdateLists()
        {
            if (toAdd.Count > 0)
            {
                for (int i = 0; i < toAdd.Count; i++)
                {
                    var entity = toAdd[i];
                    if (!current.Contains(entity))
                    {
                        entity.Added(Scene);
                        current.Add(entity);
                        entities.Add(entity);
                    }
                }

                unsorted = true;
            }

            if (toRemove.Count > 0)
            {
                for (int i = 0; i < toRemove.Count; i++)
                {
                    var entity = toRemove[i];
                    if (entities.Contains(entity))
                    {
                        entity.Removed(Scene);
                        current.Remove(entity);
                        entities.Remove(entity);
                    }
                }

                toRemove.Clear();
                removing.Clear();
            }

            if (unsorted)
            {
                unsorted = false;
                entities.Sort(CompareDepth);
            }

            if (toAdd.Count > 0)
            {
                toAdd.Clear();
                adding.Clear();
            }
        }

        public virtual void Update()
        {
            foreach(Entity ent in entities){
                if(ent.Active){
                    ent.Update();
                }
            }
        }

        public virtual void Render()
        {
            foreach(Entity ent in entities){
                if(ent.Visible){
                    ent.Render();
                }
            }
        }

        public void Add(Entity entity)
        {
            if (!adding.Contains(entity) && !current.Contains(entity))
            {
                adding.Add(entity);
                toAdd.Add(entity);
            }
        }

        public void Remove(Entity entity)
        {
            if (!removing.Contains(entity) && current.Contains(entity))
            {
                removing.Add(entity);
                toRemove.Add(entity);
            }
        }

        public void Add(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
                Add(entity);
        }

        public void Remove(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
                Remove(entity);
        }

        public void Add(params Entity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
                Add(entities[i]);
        }

        public void Remove(params Entity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
                Remove(entities[i]);
        }

        public void With<T>(Action<T> action) where T : Entity
        {
            foreach (var e in entities)
                if (e is T)
                    action(e as T);
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        public static Comparison<Entity> CompareDepth = (a, b) => { return Math.Sign(b.Depth - a.Depth); };
    }
}