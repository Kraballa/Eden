using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public abstract class Collider
    {
        public Vector2 Offset {get; set;}
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

        public bool Collide(Collider collider)
        {
            if (collider is Hitbox)
            {
                return Collide(collider as Hitbox);
            }
            if (collider is CollisionGrid)
            {
                return Collide(collider as CollisionGrid);
            }
            throw new NotImplementedException("error, collision against collider type not implemented");
        }

        public bool Collide(Entity ent)
        {
            if (!ent.Collidable)
                return false;
            return Collide(ent.Collider);
        }

        public abstract bool Collide(Vector2 point);
        public abstract bool Collide(Hitbox other);
        public abstract bool Collide(CollisionGrid other);
        public abstract void DebugRender(Color color);
    }
}