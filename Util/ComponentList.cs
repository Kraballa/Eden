
using System.Collections;

namespace Eden.Util
{
    public class ComponentList : IEnumerable<Component>, IEnumerable
    {
        public int Count => components.Count;

        private List<Component> components = [];
        private List<Component> toAdd = [];
        private List<Component> toRemove = [];

        private HashSet<Component> current = [];
        private HashSet<Component> adding = [];
        private HashSet<Component> removing = [];

        private bool unsorted;

        private Entity Entity;

        public ComponentList(Entity entity)
        {
            Entity = entity;
        }

        public void UpdateLists()
        {
            if (toAdd.Count > 0)
            {
                for (int i = 0; i < toAdd.Count; i++)
                {
                    var component = toAdd[i];
                    if (!current.Contains(component))
                    {
                        component.Added(Entity);
                        current.Add(component);
                        components.Add(component);
                    }
                }

                unsorted = true;
            }

            if (toRemove.Count > 0)
            {
                for (int i = 0; i < toRemove.Count; i++)
                {
                    var component = toRemove[i];
                    if (components.Contains(component))
                    {
                        current.Remove(component);
                        components.Remove(component);
                        component.Removed(Entity);
                    }
                }

                toRemove.Clear();
                removing.Clear();
            }

            if (unsorted)
            {
                unsorted = false;
                // components.Sort(CompareDepth);
            }

            if (toAdd.Count > 0)
            {
                toAdd.Clear();
                adding.Clear();
            }
        }

        public virtual void Update()
        {
            foreach (Component ent in components)
            {
                if (ent.Active)
                {
                    ent.Update();
                }
            }
        }

        public virtual void Render()
        {
            foreach (Component ent in components)
            {
                if (ent.Visible)
                {
                    ent.Render();
                }
            }
        }

        public void Add(Component component)
        {
            if (!adding.Contains(component) && !current.Contains(component))
            {
                adding.Add(component);
                toAdd.Add(component);
            }
        }

        public void Remove(Component component)
        {
            if (!removing.Contains(component) && current.Contains(component))
            {
                removing.Add(component);
                toRemove.Add(component);
            }
        }

        public void Add(IEnumerable<Component> components)
        {
            foreach (var Component in components)
                Add(Component);
        }

        public void Remove(IEnumerable<Component> components)
        {
            foreach (var Component in components)
                Remove(Component);
        }

        public void Add(params Component[] components)
        {
            for (int i = 0; i < components.Length; i++)
                Add(components[i]);
        }

        public void Remove(params Component[] components)
        {
            for (int i = 0; i < components.Length; i++)
                Remove(components[i]);
        }

        public void With<T>(Action<T> action) where T : Component
        {
            foreach (var e in components)
                if (e is T)
                    action(e as T);
        }

        public IEnumerator<Component> GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return components.GetEnumerator();
        }

        // public static Comparison<Component> CompareDepth = (a, b) => { return Math.Sign(b.Depth - a.Depth); };
    }
}