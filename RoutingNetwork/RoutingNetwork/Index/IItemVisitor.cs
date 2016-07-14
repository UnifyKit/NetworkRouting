namespace NetworkRouting
{
    /// <summary>
    /// A visitor for items in an index.
    /// </summary>
    internal interface IItemVisitor<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void VisitItem(T item);
    }
}
