namespace NetworkRouting
{
    internal class Root<T> : NodeBase<T>
    {
        // the singleton root quad is centred at the origin.
        private static readonly Coordinate Origin = new Coordinate(0.0f, 0.0f);

        public Root()
        {
        }

        /// <summary> 
        /// Insert an item into the quadtree this is the root of.
        /// </summary>
        public void Insert(Boundingbox itemEnv, T item)
        {
            int index = GetSubnodeIndex(itemEnv, Origin.Longitude, Origin.Latitude);
            // if index is -1, itemEnv must cross the X or Y axis.
            if (index == -1)
            {
                Add(item);
                return;
            }
            /*
            * the item must be contained in one quadrant, so insert it into the
            * tree for that quadrant (which may not yet exist)
            */
            var node = Subnode[index];
            /*
            *  If the subquad doesn't exist or this item is not contained in it,
            *  have to expand the tree upward to contain the item.
            */
            if (node == null || !node.Boundingbox.Contains(itemEnv))
            {
                var largerNode = QuadTreeNode<T>.CreateExpanded(node, itemEnv);
                Subnode[index] = largerNode;
            }
            /*
            * At this point we have a subquad which exists and must contain
            * contains the env for the item.  Insert the item into the tree.
            */
            InsertContained(Subnode[index], itemEnv, item);
        }

        /// <summary> 
        /// Insert an item which is known to be contained in the tree rooted at
        /// the given QuadNode root.  Lower levels of the tree will be created
        /// if necessary to hold the item.
        /// </summary>
        private static void InsertContained(QuadTreeNode<T> tree, Boundingbox itemEnv, T item)
        {
            /*
            * Do NOT create a new quad for zero-area envelopes - this would lead
            * to infinite recursion. Instead, use a heuristic of simply returning
            * the smallest existing quad containing the query
            */
            bool isZeroX = IntervalSize.IsZeroWidth(itemEnv.MinX, itemEnv.MaxX);
            bool isZeroY = IntervalSize.IsZeroWidth(itemEnv.MinY, itemEnv.MaxY);
            NodeBase<T> node;
            if (isZeroX || isZeroY)
                node = tree.Find(itemEnv);
            else node = tree.GetNode(itemEnv);
            node.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected override bool IsSearchMatch(Boundingbox searchEnv)
        {
            return true;
        }
    }
}
