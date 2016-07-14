using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkRouting
{
    public class Way
    {
        private Collection<Arc> arcs;

        public Way()
        {
            this.arcs = new Collection<Arc>();
        }

        public Collection<Arc> Arcs
        {
            get
            {
                return arcs;
            }
        }

        public override string ToString()
        {
            StringBuilder valueString = new StringBuilder();
            for (int i = 0; i < Arcs.Count; i++)
            {
                valueString.Append(arcs[i]);
            }

            return valueString.ToString();
        }
    }
}
