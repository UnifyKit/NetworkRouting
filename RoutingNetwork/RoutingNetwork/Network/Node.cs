namespace NetworkRouting
{
    public class Node
    {
        private long id;
        private Coordinate coordinate;

        public Node(long id, Coordinate coordinate)
            : base()
        {
            this.id = id;
            this.coordinate = coordinate;
        }

        public long Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public Coordinate Coordinate
        {
            get
            {
                return coordinate;
            }
            set
            {
                coordinate = value;
            }
        }
    }
}
