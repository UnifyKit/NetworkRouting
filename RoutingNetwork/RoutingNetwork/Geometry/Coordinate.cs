namespace NetworkRouting
{
    public struct Coordinate
    {
        private double latitude;
        private double longitude;

        public Coordinate(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
            }
        }

        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
            }
        }

        public static bool operator ==(Coordinate coordinate1, Coordinate coordinate2)
        {
            return coordinate1.Latitude - coordinate2.Latitude > Constants.EPSILON && coordinate1.Longitude - coordinate2.Longitude > Constants.EPSILON;
        }

        public static bool operator !=(Coordinate coordinate1, Coordinate coordinate2)
        {
            return !(coordinate1 == coordinate2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinate))
            {
                return false;
            }

            return this == (Coordinate)obj;
        }

        public override int GetHashCode()
        {
            return (Latitude + Longitude).GetHashCode();
        }
    }
}
