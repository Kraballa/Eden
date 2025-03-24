using System.Numerics;

namespace Eden
{
    public static class Collide
    {
        #region Entity vs Entity

        public static bool Check(Entity a, Entity b)
        {
            if (a.Collider == null || b.Collider == null)
                return false;
            else
                return a != b && b.Collidable && a.Collider.Collide(b);
        }

        public static bool Check(Entity a, Entity b, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            bool ret = Check(a, b);
            a.Position = old;
            return ret;
        }

        public static bool Check(Entity a, Collider b)
        {
            if (a.Collider == null)
                return false;
            else
                return b.Entity.Collidable && a.Collider.Collide(b);
        }

        #endregion

        #region Entity vs Entity Enumerable

        #region Check

        public static bool Check(Entity a, IEnumerable<Entity> b)
        {
            foreach (var e in b)
                if (Check(a, e))
                    return true;

            return false;
        }

        public static bool Check(Entity a, IEnumerable<Entity> b, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            bool ret = Check(a, b);
            a.Position = old;
            return ret;
        }

        #endregion

        #region First

        public static Entity First(Entity a, IEnumerable<Entity> b)
        {
            foreach (var e in b)
                if (Check(a, e))
                    return e;

            return null;
        }

        public static Entity First(Entity a, IEnumerable<Entity> b, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            Entity ret = First(a, b);
            a.Position = old;
            return ret;
        }

        #endregion

        #region All

        public static List<Entity> All(Entity a, IEnumerable<Entity> b, List<Entity> into)
        {
            foreach (var e in b)
                if (Check(a, e))
                    into.Add(e);

            return into;
        }

        public static List<Entity> All(Entity a, IEnumerable<Entity> b, List<Entity> into, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            List<Entity> ret = All(a, b, into);
            a.Position = old;
            return ret;
        }

        public static List<Entity> All(Entity a, IEnumerable<Entity> b)
        {
            return All(a, b, new List<Entity>());
        }

        public static List<Entity> All(Entity a, IEnumerable<Entity> b, Vector2 at)
        {
            return All(a, b, new List<Entity>(), at);
        }

        #endregion

        #endregion

        #region Entity vs Point

        public static bool CheckPoint(Entity a, Vector2 point)
        {
            if (a.Collider == null)
                return false;
            else
                return a.Collider.Collide(point);
        }

        public static bool CheckPoint(Entity a, Vector2 point, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            bool ret = CheckPoint(a, point);
            a.Position = old;
            return ret;
        }

        #endregion
}

}