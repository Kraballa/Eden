using System.Collections;
using System.Numerics;

namespace Eden
{
    public class Entity : IEnumerable<Component>, IEnumerable
    {
        public Transform2D Transform;

        public bool Active = true;
        public bool Visible = true;
        public bool Collidable = true;

        public Scene Scene { get; private set; }
        public ComponentList Components { get; private set; }

        private int tag;
        internal int depth = 0;
        internal double actualDepth = 0;

        public Entity(Vector2 position)
        {
            Transform = new Transform2D(position);
            Components = new ComponentList(this);
        }

        public Entity()
            : this(Vector2.Zero)
        {

        }

        /// <summary>
        /// Called when the containing Scene Begins
        /// </summary>
        public virtual void SceneBegin(Scene scene)
        {

        }

        /// <summary>
        /// Called when the containing Scene Ends
        /// </summary>
        public virtual void SceneEnd(Scene scene)
        {
            if (Components != null)
                foreach (var c in Components)
                    c.SceneEnd(scene);
        }

        /// <summary>
        /// Called before the frame starts, after Entities are added and removed, on the frame that the Entity was added
        /// Useful if you added two Entities in the same frame, and need them to detect each other before they start Updating
        /// </summary>
        /// <param name="scene"></param>
        public virtual void Awake(Scene scene)
        {
            if (Components != null)
                foreach (var c in Components)
                    c.EntityAwake();
        }

        /// <summary>
        /// Called when this Entity is added to a Scene, which only occurs immediately before each Update. 
        /// Keep in mind, other Entities to be added this frame may be added after this Entity. 
        /// See Awake() for after all Entities are added, but still before the frame Updates.
        /// </summary>
        /// <param name="scene"></param>
        public virtual void Added(Scene scene)
        {
            Scene = scene;
            if (Components != null)
                foreach (var c in Components)
                    c.EntityAdded(scene);
            Scene.SetActualDepth(this);
        }

        /// <summary>
        /// Called when the Entity is removed from a Scene
        /// </summary>
        /// <param name="scene"></param>
        public virtual void Removed(Scene scene)
        {
            if (Components != null)
                foreach (var c in Components)
                    c.EntityRemoved(scene);
            Scene = null;
        }

        /// <summary>
        /// Do game logic here, but do not render here. Not called if the Entity is not Active
        /// </summary>
        public virtual void Update()
        {
            Components.Update();
        }

        /// <summary>
        /// Draw the Entity here. Not called if the Entity is not Visible
        /// </summary>
        public virtual void Render()
        {
            Components.Render();
        }

        /// <summary>
        /// Draw any debug visuals here. Only called if the console is open, but still called even if the Entity is not Visible
        /// </summary>
        public virtual void DebugRender()
        {
            Components.DebugRender();
        }

        public void RemoveSelf()
        {
            if (Scene != null)
                Scene.Entities.Remove(this);
        }

        public int Depth
        {
            get { return depth; }
            set
            {
                if (depth != value)
                {
                    depth = value;
                    if (Scene != null)
                        Scene.SetActualDepth(this);
                }
            }
        }

        public Vector2 Position
        {
            get { return Transform.Position; }
            set { Transform.Position = value; }
        }

        public float X
        {
            get { return Transform.Position.X; }
            set { Transform.Position.X = value; }
        }

        public float Y
        {
            get { return Transform.Position.Y; }
            set { Transform.Position.Y = value; }
        }

        #region Tag

        public int Tag
        {
            get
            {
                return tag;
            }

            set
            {
                if (tag != value)
                {
                    if (Scene != null)
                    {
                        for (int i = 0; i < Eden.BitTag.TotalTags; i++)
                        {
                            int check = 1 << i;
                            bool add = (value & check) != 0;
                            bool has = (Tag & check) != 0;

                            if (has != add)
                            {
                                if (add)
                                    Scene.TagLists[i].Add(this);
                                else
                                    Scene.TagLists[i].Remove(this);
                            }
                        }
                    }

                    tag = value;
                }
            }
        }

        public bool TagFullCheck(int tag)
        {
            return (this.tag & tag) == tag;
        }

        public bool TagCheck(int tag)
        {
            return (this.tag & tag) != 0;
        }

        public void AddTag(int tag)
        {
            Tag |= tag;
        }

        public void RemoveTag(int tag)
        {
            Tag &= ~tag;
        }

        #endregion

        #region Components Shortcuts

        public void Add(Component component)
        {
            Components.Add(component);
        }

        public void Remove(Component component)
        {
            Components.Remove(component);
        }

        public void Add(params Component[] components)
        {
            Components.Add(components);
        }

        public void Remove(params Component[] components)
        {
            Components.Remove(components);
        }

        public T Get<T>() where T : Component
        {
            return Components.Get<T>();
        }

        public IEnumerator<Component> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Misc Utils

        public Entity Closest(params Entity[] entities)
        {
            Entity closest = entities[0];
            float dist = Vector2.DistanceSquared(Position, closest.Position);

            for (int i = 1; i < entities.Length; i++)
            {
                float current = Vector2.DistanceSquared(Position, entities[i].Position);
                if (current < dist)
                {
                    closest = entities[i];
                    dist = current;
                }
            }

            return closest;
        }

        public Entity Closest(BitTag tag)
        {
            var list = Scene[tag];
            Entity closest = null;
            float dist;

            if (list.Count >= 1)
            {
                closest = list[0];
                dist = Vector2.DistanceSquared(Position, closest.Position);

                for (int i = 1; i < list.Count; i++)
                {
                    float current = Vector2.DistanceSquared(Position, list[i].Position);
                    if (current < dist)
                    {
                        closest = list[i];
                        dist = current;
                    }
                }
            }

            return closest;
        }

        public T SceneAs<T>() where T : Scene
        {
            return Scene as T;
        }

        #endregion
    }
}
