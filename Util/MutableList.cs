using System.Collections;

namespace Eden.Util
{
    public class MutableList<T>
    {
        public int Count => internalList.Count;

        private List<T> internalList = [];
        private HashSet<T> toAdd = [];
        private HashSet<T> toRemove = [];

        public T this[int index]
        {
            get
            {
                return internalList[index];
            }
        }

        public virtual void Update()
        {
            if (toAdd.Count > 0)
            {
                foreach (T ele in toAdd)
                {
                    internalList.Add(ele);
                }
                toAdd.Clear();
            }

            if (toRemove.Count > 0)
            {
                foreach (T ele in toRemove)
                {
                    internalList.Remove(ele);
                }
                toRemove.Clear();
            }
        }

        public virtual void Add(T item)
        {
            toAdd.Add(item);
        }

        public virtual void Remove(T item)
        {
            toRemove.Add(item);
        }
    }
}