using System.Collections.Generic;

namespace NetworkRouting
{
    public class ArrayListVisitor<T> : IItemVisitor<T>
    {
        private readonly List<T> items = new List<T>();

        public ArrayListVisitor()
        {
        }

        public void VisitItem(T item)
        {
            items.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<T> Items
        {
            get
            {
                return items;
            }
        }
    }
}
