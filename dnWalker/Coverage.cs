
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    public readonly struct Coverage
    {
        public Coverage(double edges, double nodes)
        {
            Edges = edges;
            Nodes = nodes;
        }

        public double Edges { get; }
        public double Nodes { get; }
    }
}
