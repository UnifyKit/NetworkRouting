using System.Collections.Generic;

namespace NetworkRouting
{
    public interface ISpatialIndex<T>
    {
        /// <summary>
        /// Adds a spatial item with an extent specified by the given <c>Envelope</c> to the index.
        /// </summary>
        void Insert(Boundingbox boundingbox, T item);

        /// <summary> 
        /// Queries the index for all items whose extents intersect the given search <c>Envelope</c> 
        /// Note that some kinds of indexes may also return objects which do not in fact
        /// intersect the query envelope.
        /// </summary>
        /// <param name="boundingbox">The envelope to query for.</param>
        /// <returns>A list of the items found by the query.</returns>
        IList<T> Query(Boundingbox boundingbox);

        ///// <summary>
        ///// Queries the index for all items whose extents intersect the given search <see cref="Boundingbox" />,
        ///// and applies an <see cref="IItemVisitor{T}" /> to them.
        ///// Note that some kinds of indexes may also return objects which do not in fact
        ///// intersect the query envelope.
        ///// </summary>
        ///// <param name="searchEnv">The envelope to query for.</param>
        ///// <param name="visitor">A visitor object to apply to the items found.</param>
        //void Query(Boundingbox searchEnv, IItemVisitor<T> visitor);

        /// <summary> 
        /// Removes a single item from the tree.
        /// </summary>
        /// <param name="boundingbox">The Envelope of the item to remove.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns> <c>true</c> if the item was found.</returns>
        bool Remove(Boundingbox boundingbox, T item);
    }
}
