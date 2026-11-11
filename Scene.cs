using System.Collections;
using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public class Scene : IEnumerable<Entity>, IEnumerable
    {
        public bool Paused;
        public float TimeActive;
        public float RawTimeActive;
        public bool Focused { get; private set; }
        public EntityList Entities { get; private set; }
        public TagLists TagLists { get; private set; }
        public RendererList RendererList { get; private set; }
        public Entity HelperEntity { get; private set; }
        public Tracker Tracker { get; private set; }
        public Renderer ActiveRenderer { get; set; }

        private Dictionary<int, double> actualDepthLookup;

        public event Action OnEndOfFrame;

        public Scene()
        {
            Tracker.Initialize();
            Tracker = new Tracker();
            Entities = new EntityList(this);
            TagLists = new TagLists();
            RendererList = new RendererList(this);

            actualDepthLookup = new Dictionary<int, double>();

            HelperEntity = new Entity();
            Entities.Add(HelperEntity);
        }

        public virtual void Begin()
        {
            Focused = true;
            Entities.UpdateLists();
            TagLists.UpdateLists();
            RendererList.UpdateLists();
            foreach (var entity in Entities)
                entity.SceneBegin(this);
        }

        public virtual void End()
        {
            Focused = false;
            foreach (var entity in Entities)
                entity.SceneEnd(this);

            TextureBank.OnSceneEnd();
        }

        public virtual void BeforeUpdate()
        {
            if (!Paused)
                TimeActive += RaylibExt.GetSafeFrameTime();
            RawTimeActive += RaylibExt.GetSafeFrameTime();

            Entities.UpdateLists();
            TagLists.UpdateLists();
            RendererList.UpdateLists();
        }

        public virtual void Update()
        {
            if (!Paused)
            {
                Entities.Update();
                RendererList.Update();
            }
        }

        public virtual void AfterUpdate()
        {
            var callbacks = OnEndOfFrame;
            OnEndOfFrame = null;
            callbacks?.Invoke();
        }

        public virtual void BeforeRender()
        {
            RendererList.BeforeRender();
        }

        public virtual void Render()
        {
            RendererList.Render();
        }

        public virtual void AfterRender()
        {
            RendererList.AfterRender();
        }

        #region Interval

        /// <summary>
        /// Returns whether the Scene timer has passed the given time interval since the last frame. Ex: given 2.0f, this will return true once every 2 seconds
        /// </summary>
        /// <param name="interval">The time interval to check for</param>
        /// <returns></returns>
        public bool OnInterval(float interval)
        {
            return (int)((TimeActive - RaylibExt.GetSafeFrameTime()) / interval) < (int)(TimeActive / interval);
        }

        /// <summary>
        /// Returns whether the Scene timer has passed the given time interval since the last frame. Ex: given 2.0f, this will return true once every 2 seconds
        /// </summary>
        /// <param name="interval">The time interval to check for</param>
        /// <returns></returns>
        public bool OnInterval(float interval, float offset)
        {
            return Math.Floor((TimeActive - offset - RaylibExt.GetSafeFrameTime()) / interval) < Math.Floor((TimeActive - offset) / interval);
        }

        #endregion


        #region Collisions v Tags

        public bool CollideCheck(Vector2 point, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point))
                    return true;
            return false;
        }

        public Entity CollideFirst(Vector2 point, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point))
                    return list[i];
            return null;
        }

        public void CollideInto(Vector2 point, int tag, List<Entity> hits)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point))
                    hits.Add(list[i]);
        }

        public List<Entity> CollideAll(Vector2 point, int tag)
        {
            List<Entity> list = new List<Entity>();
            CollideInto(point, tag, list);
            return list;
        }

        public void CollideDo(Vector2 point, int tag, Action<Entity> action)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point))
                    action(list[i]);
        }

        #endregion

        #region Collisions v Tracked List Entities

        public bool CollideCheck<T>(Vector2 point) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point))
                    return true;
            return false;
        }

        public T CollideFirst<T>(Vector2 point) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point))
                    return list[i] as T;
            return null;
        }

        public void CollideInto<T>(Vector2 point, List<Entity> hits) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point))
                    hits.Add(list[i]);
        }

        public void CollideInto<T>(Vector2 point, List<T> hits) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point))
                    hits.Add(list[i] as T);
        }

        public List<T> CollideAll<T>(Vector2 point) where T : Entity
        {
            List<T> list = new List<T>();
            CollideInto<T>(point, list);
            return list;
        }

        public void CollideDo<T>(Vector2 point, Action<T> action) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point))
                    action(list[i] as T);
        }

        #endregion

        #region Collisions v Tracked List Components

        public bool CollideCheckByComponent<T>(Vector2 point) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollidePoint(point))
                    return true;
            return false;
        }

        public T CollideFirstByComponent<T>(Vector2 point) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollidePoint(point))
                    return list[i] as T;
            return null;
        }

        public void CollideIntoByComponent<T>(Vector2 point, List<Component> hits) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollidePoint(point))
                    hits.Add(list[i]);
        }

        public void CollideIntoByComponent<T>(Vector2 point, List<T> hits) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollidePoint(point))
                    hits.Add(list[i] as T);
        }

        public List<T> CollideAllByComponent<T>(Vector2 point) where T : Component
        {
            List<T> list = new List<T>();
            CollideIntoByComponent<T>(point, list);
            return list;
        }

        public void CollideDoByComponent<T>(Vector2 point, Action<T> action) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollidePoint(point))
                    action(list[i] as T);
        }

        #endregion


        #region Utils

        internal void SetActualDepth(Entity entity)
        {
            const double theta = .000001f;

            double add = 0;
            if (actualDepthLookup.TryGetValue(entity.depth, out add))
                actualDepthLookup[entity.depth] += theta;
            else
                actualDepthLookup.Add(entity.depth, theta);
            entity.actualDepth = entity.depth - add;

            //Mark lists unsorted
            Entities.MarkUnsorted();
            for (int i = 0; i < BitTag.TotalTags; i++)
                if (entity.TagCheck(1 << i))
                    TagLists.MarkUnsorted(i);
        }

        #endregion

        #region Entity Shortcuts

        /// <summary>
        /// Quick access to entire tag lists of Entities. Result will never be null
        /// </summary>
        /// <param name="tag">The tag list to fetch</param>
        /// <returns></returns>
        public List<Entity> this[BitTag tag]
        {
            get
            {
                return TagLists[tag.ID];
            }
        }

        /// <summary>
        /// Shortcut function for adding an Entity to the Scene's Entities list
        /// </summary>
        /// <param name="entity">The Entity to add</param>
        public void Add(Entity entity)
        {
            Entities.Add(entity);
        }

        /// <summary>
        /// Shortcut function for removing an Entity from the Scene's Entities list
        /// </summary>
        /// <param name="entity">The Entity to remove</param>
        public void Remove(Entity entity)
        {
            Entities.Remove(entity);
        }

        /// <summary>
        /// Shortcut function for adding a set of Entities from the Scene's Entities list
        /// </summary>
        /// <param name="entities">The Entities to add</param>
        public void Add(IEnumerable<Entity> entities)
        {
            Entities.Add(entities);
        }

        /// <summary>
        /// Shortcut function for removing a set of Entities from the Scene's Entities list
        /// </summary>
        /// <param name="entities">The Entities to remove</param>
        public void Remove(IEnumerable<Entity> entities)
        {
            Entities.Remove(entities);
        }

        /// <summary>
        /// Shortcut function for adding a set of Entities from the Scene's Entities list
        /// </summary>
        /// <param name="entities">The Entities to add</param>
        public void Add(params Entity[] entities)
        {
            Entities.Add(entities);
        }

        /// <summary>
        /// Shortcut function for removing a set of Entities from the Scene's Entities list
        /// </summary>
        /// <param name="entities">The Entities to remove</param>
        public void Remove(params Entity[] entities)
        {
            Entities.Remove(entities);
        }

        /// <summary>
        /// Allows you to iterate through all Entities in the Scene
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Entity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        /// <summary>
        /// Allows you to iterate through all Entities in the Scene
        /// </summary>
        /// <returns></returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<Entity> GetEntitiesByTagMask(int mask)
        {
            List<Entity> list = new List<Entity>();
            foreach (var entity in Entities)
                if ((entity.Tag & mask) != 0)
                    list.Add(entity);
            return list;
        }

        public List<Entity> GetEntitiesExcludingTagMask(int mask)
        {
            List<Entity> list = new List<Entity>();
            foreach (var entity in Entities)
                if ((entity.Tag & mask) == 0)
                    list.Add(entity);
            return list;
        }

        #endregion

        #region Renderer Shortcuts

        /// <summary>
        /// Shortcut function to add a Renderer to the Renderer list
        /// </summary>
        /// <param name="renderer">The Renderer to add</param>
        public void Add(Renderer renderer)
        {
            RendererList.Add(renderer);
        }

        /// <summary>
        /// Shortcut function to remove a Renderer from the Renderer list
        /// </summary>
        /// <param name="renderer">The Renderer to remove</param>
        public void Remove(Renderer renderer)
        {
            RendererList.Remove(renderer);
        }

        #endregion
    }
}
