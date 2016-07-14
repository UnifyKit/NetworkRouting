using System;

namespace NetworkRouting
{
    internal class Key
    {
        // the fields which make up the key
        private Coordinate coordinate = new Coordinate();
        private int level;

        // auxiliary data which is derived from the key for use in computation
        private Boundingbox boundingbox;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemEnv"></param>
        public Key(Boundingbox itemEnv)
        {
            ComputeKey(itemEnv);
        }

        /// <summary>
        /// 
        /// </summary>
        public Coordinate Point
        {
            get
            {
                return coordinate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            get
            {
                return level;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boundingbox Envelope
        {
            get
            {
                return boundingbox;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Coordinate Centre
        {
            get
            {
                return new Coordinate((boundingbox.MinX + boundingbox.MaxX) / 2, (boundingbox.MinY + boundingbox.MaxY) / 2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static int ComputeQuadLevel(Boundingbox env)
        {
            double dx = env.Width;
            double dy = env.Height;
            double dMax = dx > dy ? dx : dy;
            int level = DoubleBits.GetExponent(dMax) + 1;
            return level;
        }

        /// <summary>
        /// Return a square envelope containing the argument envelope,
        /// whose extent is a power of two and which is based at a power of 2.
        /// </summary>
        /// <param name="itemEnv"></param>
        public void ComputeKey(Boundingbox itemEnv)
        {
            level = ComputeQuadLevel(itemEnv);
            boundingbox = new Boundingbox();
            ComputeKey(level, itemEnv);
            // MD - would be nice to have a non-iterative form of this algorithm
            while (!boundingbox.Contains(itemEnv))
            {
                level += 1;
                ComputeKey(level, itemEnv);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="itemEnv"></param>
        private void ComputeKey(int level, Boundingbox itemEnv)
        {
            double quadSize = DoubleBits.PowerOf2(level);
            coordinate.Longitude = Math.Floor(itemEnv.MinX / quadSize) * quadSize;
            coordinate.Latitude = Math.Floor(itemEnv.MinY / quadSize) * quadSize;

            boundingbox = new Boundingbox(coordinate.Longitude, coordinate.Longitude + quadSize, coordinate.Latitude, coordinate.Longitude + quadSize);
        }
    }
}
