using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public abstract class Collider
    {
        Vector2 Offset = new();
        public abstract float Width { get; set; }
        public abstract float Height { get; set; }
        public abstract float Left { get; }
        public abstract float Right { get; }
        public abstract float Top { get; }
        public abstract float Bottom { get; }

        public Entity? Entity { get; private set; }

        /// <summary>
        /// Standalone Colliders are legal.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                if (Entity == null)
                {
                    return Offset;
                }
                return Entity.Position + Offset;
            }
            set
            {
                if (Entity == null)
                {
                    Offset = value;
                }
                else
                {
                    Offset = value - Entity.Position;
                }
            }
        }

        /// <summary>
        /// A collider that optionally may be attached to an entity.
        /// </summary>
        public Collider()
        {

        }

        public void Added(Entity ent)
        {
            Entity = ent;
        }

        public void Removed()
        {
            Entity = null;
        }

        public Collider JustifyOrigin(float x, float y)
        {
            Offset = new Vector2(-Width * x, -Height * y);
            return this;
        }

        public bool CollideCheck(Collider collider)
        {
            if (collider is Hitbox)
            {
                return CollideCheck(collider as Hitbox);
            }
            if (collider is CollisionGrid)
            {
                return CollideCheck(collider as CollisionGrid);
            }
            throw new NotImplementedException("error, collision against collider type not implemented");
        }

        public abstract bool CollideCheck(Hitbox other);
        public abstract bool CollideCheck(CollisionGrid other);
        public abstract void DebugRender(Color color);
    }
}