using System.Collections;
using System.Numerics;
using Eden.Renderers;
using Eden.Util;
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
                TimeActive += Raylib.GetFrameTime();
            RawTimeActive += Raylib.GetFrameTime();

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
            if (OnEndOfFrame != null)
            {
                OnEndOfFrame();
                OnEndOfFrame = null;
            }
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
            return (int)((TimeActive - Raylib.GetFrameTime()) / interval) < (int)(TimeActive / interval);
        }

        /// <summary>
        /// Returns whether the Scene timer has passed the given time interval since the last frame. Ex: given 2.0f, this will return true once every 2 seconds
        /// </summary>
        /// <param name="interval">The time interval to check for</param>
        /// <returns></returns>
        public bool OnInterval(float interval, float offset)
        {
            return Math.Floor((TimeActive - offset - Raylib.GetFrameTime()) / interval) < Math.Floor((TimeActive - offset) / interval);
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
