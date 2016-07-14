using System;
using System.Globalization;
using System.Text;

namespace NetworkRouting
{
    public class Boundingbox : ICloneable, IComparable, IEquatable<Boundingbox>
    {
        private double minx;
        private double maxx;
        private double miny;
        private double maxy;

        /// <summary>
        /// Method to initialize the Boundingbox. Calling this function will result in <see cref="IsEmpty"/> returning <value>true</value>
        /// </summary>
        public Boundingbox()
            : this(0, -1, 0, -1)
        {
        }

        /// <summary>
        /// Method to initialize the Boundingbox with a <see cref="T:GeoAPI.Geometries.Coordinate"/>. Calling this function will result in an Boundingbox having no extent but a location.
        /// </summary>
        /// <param name="coordinate">The point</param>
        public Boundingbox(Coordinate coordinate)
            : this(coordinate.Longitude, coordinate.Longitude, coordinate.Latitude, coordinate.Latitude)
        {
        }

        /// <summary>
        /// Method to initialize the Boundingbox with two <see cref="T:GeoAPI.Geometries.Coordinate"/>s.
        /// </summary>
        /// <param name="lowerLeft">The first point</param>
        /// <param name="upperRight">The second point</param>
        public Boundingbox(Coordinate lowerLeft, Coordinate upperRight)
            : this(lowerLeft.Longitude, upperRight.Longitude, lowerLeft.Latitude, upperRight.Latitude)
        {
        }

        /// <summary>
        /// Initialize an <c>Boundingbox</c> for a region defined by maximum and minimum values.
        /// </summary>
        /// <param name="minX">The first x-value.</param>
        /// <param name="maxX">The second x-value.</param>
        /// <param name="minY">The first y-value.</param>
        /// <param name="maxY">The second y-value.</param>
        public Boundingbox(double minX, double maxX, double minY, double maxY)
        {
            if (minX < maxX)
            {
                this.minx = minX;
                this.maxx = maxX;
            }
            else
            {
                this.minx = maxX;
                this.maxx = minX;
            }

            if (minY < maxY)
            {
                this.miny = minY;
                this.maxy = maxY;
            }
            else
            {
                this.miny = maxY;
                this.maxy = minY;
            }
        }

        /// <summary>
        /// Gets the maximum x-ordinate of the Boundingbox
        /// </summary>
        public double MaxX
        {
            get { return maxx; }
        }

        /// <summary>
        /// Gets the maximum y-ordinate of the Boundingbox
        /// </summary>
        public double MaxY
        {
            get { return maxy; }
        }

        /// <summary>
        /// Gets the minimum x-ordinate of the Boundingbox
        /// </summary>
        public double MinX
        {
            get { return minx; }
        }

        /// <summary>
        /// Gets the mimimum y-ordinate of the Boundingbox
        /// </summary>
        public double MinY
        {
            get { return miny; }
        }

        /// <summary>
        /// Gets the area of the Boundingbox
        /// </summary>
        public double Area
        {
            get { return Width * Height; }
        }

        /// <summary>
        /// Gets the width of the Boundingbox
        /// </summary>
        public double Width
        {
            get
            {
                if (IsEmpty)
                {
                    return 0;
                }

                return maxx - minx;
            }
        }

        /// <summary>
        /// Gets the height of the Boundingbox
        /// </summary>
        public double Height
        {
            get
            {
                if (IsEmpty)
                {
                    return 0;
                }

                return maxy - miny;
            }
        }

        /// <summary>
        /// Gets the <see cref="Coordinate"/> or the center of the Boundingbox
        /// </summary>
        public Coordinate Centre
        {
            get
            {
                return IsEmpty ? new Coordinate(double.NaN, double.NaN) : new Coordinate((MinX + MaxX) / 2.0, (MinY + MaxY) / 2.0);
            }
        }

        /// <summary>
        /// Returns <c>true</c> if this <c>Boundingbox</c> is a "null" Boundingbox.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this <c>Boundingbox</c> is uninitialized
        /// or is the Boundingbox of the empty point.
        /// </returns>
        public bool IsEmpty
        {
            get
            {
                return maxx < minx;
            }
        }

        /// <summary>
        /// Returns if the point specified by <see paramref="x"/> and <see paramref="y"/> is contained by the Boundingbox.
        /// </summary>
        /// <param name="x">The x-ordinate</param>
        /// <param name="y">The y-ordinate</param>
        /// <returns>True if the point is contained by the envlope</returns>
        public bool Contains(double longitude, double latitude)
        {
            return Covers(longitude, latitude);
        }

        /// <summary>
        /// Returns if the point specified by <see paramref="p"/> is contained by the Boundingbox.
        /// </summary>
        /// <param name="coordinate">The point</param>
        /// <returns>True if the point is contained by the envlope</returns>
        public bool Contains(Coordinate coordinate)
        {
            return Covers(coordinate);
        }

        /// <summary>
        /// Returns if the Boundingbox specified by <see paramref="other"/> is contained by this Boundingbox.
        /// </summary>
        /// <param name="other">The boundingbox to test</param>
        /// <returns>True if the other boundingbox is contained by this envlope</returns>
        public bool Contains(Boundingbox other)
        {
            return Covers(other);
        }

        ///<summary>
        /// Tests if the given point lies in or on the boundingbox.
        ///</summary>
        /// <param name="coordinate">the point which this <c>Boundingbox</c> is being checked for containing</param>
        /// <returns><c>true</c> if the point lies in the interior or on the boundary of this <c>Boundingbox</c>.</returns>
        public bool Covers(Coordinate coordinate)
        {
            return Covers(coordinate.Longitude, coordinate.Latitude);
        }

        ///<summary>
        /// Tests if the <c>Boundingbox other</c> lies wholely inside this <c>Boundingbox</c> (inclusive of the boundary).
        ///</summary>
        /// <param name="other">the <c>Boundingbox</c> to check</param>
        /// <returns>true if this <c>Boundingbox</c> covers the <c>other</c></returns>
        public bool Covers(Boundingbox other)
        {
            if (IsEmpty || other.IsEmpty)
            {
                return false;
            }

            return other.MinX >= minx && other.MaxX <= maxx && other.MinY >= miny && other.MaxY <= maxy;
        }

        ///<summary>
        /// Tests if the given point lies in or on the boundingbox.
        ///</summary>
        /// <param name="x">the x-coordinate of the point which this <c>Boundingbox</c> is being checked for containing</param>
        /// <param name="y">the y-coordinate of the point which this <c>Boundingbox</c> is being checked for containing</param>
        /// <returns> <c>true</c> if <c>(x, y)</c> lies in the interior or on the boundary of this <c>Boundingbox</c>.</returns>
        public bool Covers(double x, double y)
        {
            if (IsEmpty)
            {
                return false;
            }

            return x >= minx && x <= maxx && y >= miny && y <= maxy;
        }

        /// <summary>
        /// Computes the distance between this and another
        /// <c>Boundingbox</c>.
        /// The distance between overlapping Boundingboxs is 0.  Otherwise, the
        /// distance is the Euclidean distance between the closest points.
        /// </summary>
        /// <returns>The distance between this and another <c>Boundingbox</c>.</returns>
        public double Distance(Boundingbox boundingbox)
        {
            if (Intersects(boundingbox))
            {
                return 0;
            }

            double dx = 0.0;
            if (maxx < boundingbox.MinX)
            {
                dx = boundingbox.MinX - maxx;
            }
            else if (minx > boundingbox.MaxX)
            {
                dx = minx - boundingbox.MaxX;
            }

            double dy = 0.0;
            if (maxy < boundingbox.MinY)
            {
                dy = boundingbox.MinY - maxy;
            }
            else if (miny > boundingbox.MaxY)
            {
                dy = miny - boundingbox.MaxY;
            }

            // if either is zero, the boundingboxs overlap either vertically or horizontally
            if (dx == 0.0)
            {
                return dy;
            }

            if (dy == 0.0)
            {
                return dx;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Expands this boundingbox by a given distance in all directions.
        /// Both positive and negative distances are supported.
        /// </summary>
        /// <param name="distance">The distance to expand the boundingbox.</param>
        public void ExpandBy(double distance)
        {
            ExpandBy(distance, distance);
        }

        /// <summary>
        /// Expands this boundingbox by a given distance in all directions.
        /// Both positive and negative distances are supported.
        /// </summary>
        /// <param name="deltaX">The distance to expand the boundingbox along the the X axis.</param>
        /// <param name="deltaY">The distance to expand the boundingbox along the the Y axis.</param>
        public void ExpandBy(double deltaX, double deltaY)
        {
            if (IsEmpty)
            {
                return;
            }

            minx -= deltaX;
            maxx += deltaX;
            miny -= deltaY;
            maxy += deltaY;

            if (minx > maxx || miny > maxy)
            {
                SetToNull();
            }
        }

        /// <summary>
        /// Enlarges this <code>Boundingbox</code> so that it contains
        /// the given <see cref="Coordinate"/>.
        /// Has no effect if the point is already on or within the boundingbox.
        /// </summary>
        /// <param name="p">The Coordinate.</param>
        public void ExpandToInclude(Coordinate coordinate)
        {
            ExpandToInclude(coordinate.Longitude, coordinate.Latitude);
        }

        /// <summary>
        /// Enlarges this <c>Boundingbox</c> so that it contains
        /// the given <see cref="Coordinate"/>.
        /// </summary>
        /// <remarks>Has no effect if the point is already on or within the boundingbox.</remarks>
        /// <param name="x">The value to lower the minimum x to or to raise the maximum x to.</param>
        /// <param name="y">The value to lower the minimum y to or to raise the maximum y to.</param>
        public void ExpandToInclude(double x, double y)
        {
            if (IsEmpty)
            {
                minx = x;
                maxx = x;
                miny = y;
                maxy = y;
            }
            else
            {
                if (x < minx)
                {
                    minx = x;
                }
                if (x > maxx)
                {
                    maxx = x;
                }
                if (y < miny)
                {
                    miny = y;
                }
                if (y > maxy)
                {
                    maxy = y;
                }
            }
        }

        /// <summary>
        /// Enlarges this <c>Boundingbox</c> so that it contains
        /// the <c>other</c> Boundingbox.
        /// Has no effect if <c>other</c> is wholly on or
        /// within the boundingbox.
        /// </summary>
        /// <param name="other">the <c>Boundingbox</c> to expand to include.</param>
        public void ExpandToInclude(Boundingbox other)
        {
            if (other.IsEmpty)
            {
                return;
            }

            if (IsEmpty)
            {
                minx = other.MinX;
                maxx = other.MaxX;
                miny = other.MinY;
                maxy = other.MaxY;
            }
            else
            {
                if (other.MinX < minx)
                {
                    minx = other.MinX;
                }
                if (other.MaxX > maxx)
                {
                    maxx = other.MaxX;
                }
                if (other.MinY < miny)
                {
                    miny = other.MinY;
                }
                if (other.MaxY > maxy)
                {
                    maxy = other.MaxY;
                }
            }
        }

        /// <summary>
        /// Check if the point <c>p</c> overlaps (lies inside) the region of this <c>Boundingbox</c>.
        /// </summary>
        /// <param name="coordinate"> the <c>Coordinate</c> to be tested.</param>
        /// <returns><c>true</c> if the point overlaps this <c>Boundingbox</c>.</returns>
        public bool Intersects(Coordinate coordinate)
        {
            return Intersects(coordinate.Longitude, coordinate.Latitude);
        }

        /// <summary>
        /// Check if the point <c>(x, y)</c> overlaps (lies inside) the region of this <c>Boundingbox</c>.
        /// </summary>
        /// <param name="x"> the x-ordinate of the point.</param>
        /// <param name="y"> the y-ordinate of the point.</param>
        /// <returns><c>true</c> if the point overlaps this <c>Boundingbox</c>.</returns>
        public bool Intersects(double x, double y)
        {
            return ((x >= (minx < maxx ? minx : maxx)) && (x <= (minx > maxx ? minx : maxx)))
                && ((y >= (MinY < MaxY ? MinY : MaxY)) && (y <= (MinY > MaxY ? MinY : MaxY)));
        }

        /// <summary>
        /// Check if the region defined by <c>other</c>
        /// overlaps (intersects) the region of this <c>Boundingbox</c>.
        /// </summary>
        /// <param name="other"> the <c>Boundingbox</c> which this <c>Boundingbox</c> is
        /// being checked for overlapping.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <c>Boundingbox</c>s overlap.
        /// </returns>
        public bool Intersects(Boundingbox other)
        {
            if (IsEmpty || other.IsEmpty)
            {
                return false;
            }

            return !(other.MinX > maxx || other.MaxX < minx || other.MinY > maxy || other.MaxY < miny);
        }

        /// <summary>
        /// Computes the intersection of two <see cref="Boundingbox"/>s.
        /// </summary>
        /// <param name="env">The boundingbox to intersect with</param>
        /// <returns>
        /// A new boundingbox representing the intersection of the boundingboxs (this will be
        /// the null boundingbox if either argument is null, or they do not intersect
        /// </returns>
        public Boundingbox Intersection(Boundingbox boundingbox)
        {
            if (IsEmpty || boundingbox.IsEmpty || !(this).Intersects(boundingbox))
            {
                return new Boundingbox();
            }

            return new Boundingbox(Math.Max(MinX, boundingbox.MinX), Math.Min(MaxX, boundingbox.MaxX), Math.Max(MinY, boundingbox.MinY), Math.Min(MaxY, boundingbox.MaxY));
        }

        /// <summary>
        /// Translates this boundingbox by given amounts in the X and Y direction.
        /// </summary>
        /// <param name="offsetX">The amount to translate along the X axis.</param>
        /// <param name="offsetY">The amount to translate along the Y axis.</param>
        public void Translate(double offsetX, double offsetY)
        {
            if (!IsEmpty)
            {
                minx += offsetX;
                maxx += offsetX;
                miny += offsetY;
                maxy += offsetY;
            }
        }

        /// <summary>
        /// Zoom the box.
        /// Possible values are e.g. 50 (to zoom in a 50%) or -50 (to zoom out a 50%).
        /// </summary>
        /// <param name="percentage">
        /// Negative do boundingbox smaller.
        /// Positive do boundingbox bigger.
        /// </param>
        /// <example>
        ///  perCent = -50 compact the boundingbox a 50% (make it smaller).
        ///  perCent = 200 enlarge boundingbox by 2.
        /// </example>
        public void Zoom(double percentage)
        {
            double w = (Width * percentage / 100);
            double h = (Height * percentage / 100);

            this.SetCentre(w, h);
        }

        /// <summary>
        /// Use Intersects instead. In the future, Overlaps may be
        /// changed to be a true overlap check; that is, whether the intersection is
        /// two-dimensional.
        /// </summary>
        public bool Overlaps(Boundingbox other)
        {
            return this.Intersects(other);
        }

        /// <summary>
        /// Use Intersects instead. In the future, Overlaps may be
        /// changed to be a true overlap check; that is, whether the intersection is
        /// two-dimensional.
        /// </summary>
        public bool Overlaps(Coordinate coordinate)
        {
            return this.Intersects(coordinate);
        }

        /// <summary>
        /// Use Intersects instead. In the future, Overlaps may be
        /// changed to be a true overlap check; that is, whether the intersection is
        /// two-dimensional.
        /// </summary>
        public bool Overlaps(double x, double y)
        {
            return this.Intersects(x, y);
        }

        public void SetCentre(Coordinate centre)
        {
            this.SetCentre(centre, Width, Height);
        }

        public void SetCentre(double width, double height)
        {
            this.SetCentre(Centre, width, height);
        }

        public void SetCentre(Coordinate centre, double width, double height)
        {
            minx = centre.Longitude - (width / 2);
            maxx = centre.Longitude + (width / 2);
            miny = centre.Latitude - (height / 2);
            maxy = centre.Latitude + (height / 2);
        }

        /// <summary>
        /// Calculates the union of the current box and the given coordinate.
        /// </summary>
        public Boundingbox Union(Coordinate coordinate)
        {
            Boundingbox boundingbox = Clone() as Boundingbox;
            boundingbox.ExpandToInclude(coordinate);

            return boundingbox;
        }

        public Boundingbox Union(Boundingbox box)
        {
            if (box.IsEmpty)
            {
                return this;
            }

            if (IsEmpty)
            {
                return box;
            }

            return new Boundingbox(Math.Min(minx, box.MinX), Math.Max(maxx, box.MaxX), Math.Min(miny, box.MinY), Math.Max(maxy, box.MaxY));
        }

        public object Clone()
        {
            if (IsEmpty)
            {
                return new Boundingbox();
            }

            return new Boundingbox(minx, maxx, miny, maxy);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Boundingbox))
            {
                return -1;
            }

            Boundingbox boundingbox = obj as Boundingbox;
            if (IsEmpty)
            {
                if (boundingbox.IsEmpty)
                {
                    return 0;
                }

                return -1;
            }
            else
            {
                if (boundingbox.IsEmpty)
                {
                    return 1;
                }
            }

            if (MinX < boundingbox.MinX) return -1;
            if (MinX > boundingbox.MinX) return 1;
            if (MinY < boundingbox.MinY) return -1;
            if (MinY > boundingbox.MinY) return 1;
            if (MaxX < boundingbox.MaxX) return -1;
            if (MaxX > boundingbox.MaxX) return 1;
            if (MaxY < boundingbox.MaxY) return -1;
            if (MaxY > boundingbox.MaxY) return 1;

            return 0;
        }

        public bool Equals(Boundingbox other)
        {
            if (IsEmpty)
            {
                return other.IsEmpty;
            }

            return maxx == other.MaxX && maxy == other.MaxY && minx == other.MinX && miny == other.MinY;
        }

        public override bool Equals(object obj)
        {
            if (obj is Boundingbox)
            {
                return Equals((Boundingbox)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            var result = 17;

            result = 37 * result + GetHashCode(minx);
            result = 37 * result + GetHashCode(maxx);
            result = 37 * result + GetHashCode(miny);
            result = 37 * result + GetHashCode(maxy);

            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("BoudingBox[");
            if (IsEmpty)
            {
                sb.Append("Null]");
            }
            else
            {
                sb.AppendFormat(NumberFormatInfo.InvariantInfo, "{0:R} : {1:R}, ", minx, maxx);
                sb.AppendFormat(NumberFormatInfo.InvariantInfo, "{0:R} : {1:R}]", miny, maxy);
            }

            return sb.ToString();
        }

        private void SetToNull()
        {
            minx = 0;
            maxx = -1;
            miny = 0;
            maxy = -1;
        }

        private static int GetHashCode(double value)
        {
            long f = BitConverter.DoubleToInt64Bits(value);

            return (int)(f ^ (f >> 32));
        }
    }
}
