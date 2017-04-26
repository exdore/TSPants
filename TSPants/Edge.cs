using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPants
{
    public class Edge
    {
        public int Start { get; set; }
        public int End { get; set; }
        public double Lenght { get; set; }
        public bool Used { get; set; }
        public double PheromoneLevel { get; set; }
        public double EuristicValue { get; set; }
        public double ProbabilityValue { get; set; }
        public bool IsVisited { get; set; }
    }
}
