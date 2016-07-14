using System.Collections.Generic;

namespace NetworkRouting
{
    /// <summary>
    /// A Quadtree is a spatial index structure for efficient range querying
    /// of items bounded by 2D rectangles.<br/>
    /// Geometries can be indexed by using their <see cref="Boundingbox"/>s.<br/>
    /// Any type of object can also be indexed, as long as it has an extent that can be 
    /// represented by an <see cref="Boundingbox"/>.
    /// <para/>
    /// This Quadtree index provides a <b>primary filter</b>
    /// for range rectangle queries.  The various query methods return a list of
    /// all items which <i>may</i> intersect the query rectangle.  Note that
    /// it may thus return items which do <b>not</b> in fact intersect the query rectangle.
    /// A secondary filter is required to test for actual intersection 
    /// between the query rectangle and the envelope of each candidate item. 
    /// The secondary filter may be performed explicitly, 
    /// or it may be provided implicitly by subsequent operations executed on the items 
    /// (for instance, if the index query is followed by computing a spatial predicate 
    /// between the query geometry and tree items, 
    /// the envelope intersection check is performed automatically.
    /// <para/>
    /// This implementation does not require specifying the extent of the inserted
    /// items beforehand.  It will automatically expand to accomodate any extent
    /// of dataset.
    /// <para/>
    /// This data structure is also known as an <c>MX-CIF quadtree</c>
    /// following the terminology usage of Samet and others.
    /// </summary>
    public class QuadtreeIndex
    {
        internal class Quadtree<T> : ISpatialIndex<T>
        {
            private readonly Root<T> root;

            /// <summary>
            /// minExtent is the minimum envelope extent of all items
            /// inserted into the tree so far. It is used as a heuristic value
            /// to construct non-zero envelopes for features with zero X and/or Y extent.
            /// Start with a non-zero extent, in case the first feature inserted has
            /// a zero extent in both directions.  This value may be non-optimal, but
            /// only one feature will be inserted with this value.
            /// </summary>
            private double minExtent = 1.0;

            /// <summary>
            /// Constructs a Quadtree with zero items.
            /// </summary>
            public Quadtree()
            {
                root = new Root<T>();
            }

            /// <summary> 
            /// Returns the number of levels in the tree.
            /// </summary>
            public int Depth
            {
                get
                {
                    if (root != null)
                    {
                        return root.Depth;
                    }

                    return 0;
                }
            }

            /// <summary>
            /// Tests whether the index contains any items.
            /// </summary>
            public bool IsEmpty
            {
                get
                {
                    if (root == null)
                    {
                        return true;
                    }

                    return false;
                }
            }

            /// <summary> 
            /// Returns the number of items in the tree.
            /// </summary>
            public int Count
            {
                get
                {
                    if (root != null)
                    {
                        return root.Count;
                    }

                    return 0;
                }
            }

            /// <summary>
            /// Ensure that the envelope for the inserted item has non-zero extents.
            /// Use the current minExtent to pad the envelope, if necessary.
            /// </summary>
            /// <param name="itemBoundingbox"></param>
            /// <param name="minExtent"></param>
            public static Boundingbox EnsureExtent(Boundingbox itemBoundingbox, double minExtent)
            {
                double minx = itemBoundingbox.MinX;
                double maxx = itemBoundingbox.MaxX;
                double miny = itemBoundingbox.MinY;
                double maxy = itemBoundingbox.MaxY;

                // has a non-zero extent
                if (minx != maxx && miny != maxy)
                {
                    return itemBoundingbox;
                }

                // pad one or both extents
                if (minx == maxx)
                {
                    minx = minx - minExtent / 2.0;
                    maxx = minx + minExtent / 2.0;
                }

                if (miny == maxy)
                {
                    miny = miny - minExtent / 2.0;
                    maxy = miny + minExtent / 2.0;
                }

                return new Boundingbox(minx, maxx, miny, maxy);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="boundingbox"></param>
            /// <param name="item"></param>
            public void Insert(Boundingbox boundingbox, T item)
            {
                CollectStats(boundingbox);
                Boundingbox insertBoundingbox = EnsureExtent(boundingbox, minExtent);
                root.Insert(insertBoundingbox, item);
            }

            /// <summary> 
            /// Removes a single item from the tree.
            /// </summary>
            /// <param name="boundingbox">The Envelope of the item to be removed.</param>
            /// <param name="item">The item to remove.</param>
            /// <returns><c>true</c> if the item was found (and thus removed).</returns>
            public bool Remove(Boundingbox boundingbox, T item)
            {
                Boundingbox posEnv = EnsureExtent(boundingbox, minExtent);
                return root.Remove(posEnv, item);
            }

            /// <summary>
            /// Queries the tree and returns items which may lie in the given search envelope.
            /// </summary>
            /// <remarks>
            /// Precisely, the items that are returned are all items in the tree 
            /// whose envelope <b>may</b> intersect the search Envelope.
            /// Note that some items with non-intersecting envelopes may be returned as well;
            /// the client is responsible for filtering these out.
            /// In most situations there will be many items in the tree which do not
            /// intersect the search envelope and which are not returned - thus
            /// providing improved performance over a simple linear scan.    
            /// </remarks>
            /// <param name="boundingbox">The envelope of the desired query area.</param>
            /// <returns>A List of items which may intersect the search envelope</returns>
            public IList<T> Query(Boundingbox boundingbox)
            {
                /*
                * the items that are matched are the items in quads which
                * overlap the search envelope
                */
                ArrayListVisitor<T> visitor = new ArrayListVisitor<T>();
                Query(boundingbox, visitor);
                return visitor.Items;
            }

            /// <summary>
            /// Queries the tree and visits items which may lie in the given search envelope.
            /// </summary>
            /// <remarks>
            /// Precisely, the items that are visited are all items in the tree 
            /// whose envelope <b>may</b> intersect the search Envelope.
            /// Note that some items with non-intersecting envelopes may be visited as well;
            /// the client is responsible for filtering these out.
            /// In most situations there will be many items in the tree which do not
            /// intersect the search envelope and which are not visited - thus
            /// providing improved performance over a simple linear scan.    
            /// </remarks>
            /// <param name="boundingbox">The envelope of the desired query area.</param>
            /// <param name="visitor">A visitor object which is passed the visited items</param>
            private void Query(Boundingbox boundingbox, IItemVisitor<T> visitor)
            {
                /*
                * the items that are matched are the items in quads which
                * overlap the search envelope
                */
                root.Visit(boundingbox, visitor);
            }

            /// <summary>
            /// Return a list of all items in the Quadtree.
            /// </summary>
            public IList<T> QueryAll()
            {
                IList<T> foundItems = new List<T>();
                root.AddAllItems(ref foundItems);

                return foundItems;
            }

            private void CollectStats(Boundingbox itemBoundingbox)
            {
                double delX = itemBoundingbox.Width;
                if (delX < minExtent && delX > 0.0)
                {
                    minExtent = delX;
                }

                double delY = itemBoundingbox.Height;
                if (delY < minExtent && delY > 0.0)
                {
                    minExtent = delY;
                }
            }
        }
    }
}
