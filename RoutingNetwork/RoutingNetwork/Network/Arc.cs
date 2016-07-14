using System.Globalization;

namespace NetworkRouting
{
    public class Arc
    {
        private Node headNode;
        private float cost;

        private bool arcFlagApplied;

        public Arc()
        { }

        public Arc(Node headNode, int cost)
        {
            this.headNode = headNode;
            this.cost = cost;
        }

        public Node HeadNode
        {
            get
            {
                return headNode;
            }
            set
            {
                headNode = value;
            }
        }

        public float Cost
        {
            get
            {
                return cost;
            }
            set
            {
                cost = value;
            }
        }

        public bool ArcFlagApplied
        {
            get
            {
                return arcFlagApplied;
            }
            set
            {
                arcFlagApplied = value;
            }
        }

        public override string ToString()
        {
            string valueInString = string.Empty;

            if (this.cost == 0)
            {
                valueInString = string.Format(CultureInfo.InvariantCulture, "{0}", this.headNode.Id);
            }
            else
            {
                valueInString = string.Format(CultureInfo.InvariantCulture, "{{0}|{1}}", this.headNode.Id, this.cost);
            }

            return valueInString;
        }
    }
}
