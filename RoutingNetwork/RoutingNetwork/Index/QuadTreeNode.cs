namespace NetworkRouting
{
    internal class QuadTreeNode<T> : NodeBase<T>
    {
        private readonly Boundingbox boundingbox;
        private readonly double centreX, centreY;
        private readonly int level;

        public QuadTreeNode(Boundingbox boudingbox, int level)
        {
            this.boundingbox = boudingbox;
            this.level = level;
            this.centreX = (boudingbox.MinX + boudingbox.MaxX) / 2;
            this.centreY = (boudingbox.MinY + boudingbox.MaxY) / 2;
        }

        public Boundingbox Boundingbox
        {
            get
            {
                return boundingbox;
            }
        }

        public static QuadTreeNode<T> CreateNode(Boundingbox env)
        {
            Key key = new Key(env);
            var node = new QuadTreeNode<T>(key.Envelope, key.Level);

            return node;
        }

        public static QuadTreeNode<T> CreateExpanded(QuadTreeNode<T> node, Boundingbox addBoundingbox)
        {
            Boundingbox expandBoundingbox = addBoundingbox;
            if (node != null)
            {
                expandBoundingbox.ExpandToInclude(node.boundingbox);
            }

            var largerNode = CreateNode(expandBoundingbox);
            if (node != null)
            {
                largerNode.InsertNode(node);
            }

            return largerNode;
        }

        protected override bool IsSearchMatch(Boundingbox searchEnv)
        {
            return boundingbox.Intersects(searchEnv);
        }

        /// <summary> 
        /// Returns the subquad containing the envelope <paramref name="searchEnv"/>.
        /// Creates the subquad if
        /// it does not already exist.
        /// </summary>
        /// <param name="searchEnv">The envelope to search for</param>
        /// <returns>The subquad containing the search envelope.</returns>
        public QuadTreeNode<T> GetNode(Boundingbox searchEnv)
        {
            int subnodeIndex = GetSubnodeIndex(searchEnv, centreX, centreY);
            // if subquadIndex is -1 searchEnv is not contained in a subquad
            if (subnodeIndex != -1)
            {
                // create the quad if it does not exist
                var node = GetSubnode(subnodeIndex);
                // recursively search the found/created quad
                return node.GetNode(searchEnv);
            }

            return this;
        }

        /// <summary>
        /// Returns the smallest <i>existing</i>
        /// node containing the envelope.
        /// </summary>
        /// <param name="searchEnv"></param>
        public NodeBase<T> Find(Boundingbox searchEnv)
        {
            int subnodeIndex = GetSubnodeIndex(searchEnv, centreX, centreY);
            if (subnodeIndex == -1)
            {
                return this;
            }

            if (Subnode[subnodeIndex] != null)
            {
                // query lies in subquad, so search it
                var node = Subnode[subnodeIndex];
                return node.Find(searchEnv);
            }

            // no existing subquad, so return this one anyway
            return this;
        }

        public void InsertNode(QuadTreeNode<T> node)
        {
            int index = GetSubnodeIndex(node.boundingbox, centreX, centreY);
            if (node.level == level - 1)
            {
                Subnode[index] = node;
            }
            else
            {
                // the quad is not a direct child, so make a new child quad to contain it
                // and recursively insert the quad
                var childNode = CreateSubnode(index);
                childNode.InsertNode(node);
                Subnode[index] = childNode;
            }
        }

        /// <summary>
        /// Get the subquad for the index.
        /// If it doesn't exist, create it.
        /// </summary>
        /// <param name="index"></param>
        private QuadTreeNode<T> GetSubnode(int index)
        {
            if (Subnode[index] == null)
            {
                Subnode[index] = CreateSubnode(index);
            }

            return Subnode[index];
        }

        private QuadTreeNode<T> CreateSubnode(int index)
        {
            // create a new subquad in the appropriate quadrant
            double minx = 0.0;
            double maxx = 0.0;
            double miny = 0.0;
            double maxy = 0.0;

            switch (index)
            {
                case 0:
                    minx = boundingbox.MinX;
                    maxx = centreX;
                    miny = boundingbox.MinY;
                    maxy = centreY;
                    break;

                case 1:
                    minx = centreX;
                    maxx = boundingbox.MaxX;
                    miny = boundingbox.MinY;
                    maxy = centreY;
                    break;

                case 2:
                    minx = boundingbox.MinX;
                    maxx = centreX;
                    miny = centreY;
                    maxy = boundingbox.MaxY;
                    break;

                case 3:
                    minx = centreX;
                    maxx = boundingbox.MaxX;
                    miny = centreY;
                    maxy = boundingbox.MaxY;
                    break;

                default:
                    break;
            }
            Boundingbox sqEnv = new Boundingbox(minx, maxx, miny, maxy);
            var node = new QuadTreeNode<T>(sqEnv, level - 1);

            return node;
        }
    }
}
