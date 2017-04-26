using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPants
{
    public class Ant
    {
        private int Alpha { get; set; }
        private int Beta { get; set; }
        private Path CurrentPath { get; set; }

        public class Path
        {
            public List<Edge> Edges { get; set; }
            public double Lenght { get; set; }

            public double CalculateLenght(Data data)
            {
                Lenght = 0;
                for (int i = 0; i < Edges.Count; i++)
                {
                    Lenght += data.Distances[Edges[i].Start, Edges[i].End];
                }
                Lenght += data.Distances[Edges[0].Start, Edges.Last().End];
                return Lenght;
            }
        }

        public Ant()
        {
            var rnd = new Random();
            Alpha = rnd.Next(1, 3);
            Beta = rnd.Next(3, 7);
            CurrentPath = new Path
            {
                Edges = new List<Edge>()
            };
        }

        public Path BuildPath(Edges edges, Data data, int start)
        {
            if (CurrentPath.Edges.Count < data.Cities.Count - 1)
            {
                var availableEdges = new Edges(edges.Where(item => item.Start == start && item.IsVisited == false).ToList());
                availableEdges.CalculateProbabilities(Alpha, Beta);  //take n best edges, normalize, cumulative for them
                var rnd = new Random();
                var probability = rnd.NextDouble();
                var cumulatives = new List<double> {availableEdges.First().ProbabilityValue};
                for (int i = 1; i < availableEdges.Count; i++)
                {
                    cumulatives.Add(cumulatives[i - 1] + availableEdges[i].ProbabilityValue);
                }
                //var selectedEdge = availableEdges[cumulatives.FindIndex(item => item > probability)];
                var selectedEdge = availableEdges.First();
                foreach (var edge in availableEdges)
                {
                    if (selectedEdge.ProbabilityValue < edge.ProbabilityValue)
                        selectedEdge = edge;
                }
                CurrentPath.Edges.Add(selectedEdge);
                edges.Where(item => item.Start == selectedEdge.Start || item.End == selectedEdge.Start).ToList()
                    .ForEach(item => item.IsVisited = true);
                BuildPath(edges, data, selectedEdge.End);
            }
            CurrentPath.CalculateLenght(data);
            return CurrentPath;
        }

        public string PrintPath()
        {
            var str = CurrentPath.Edges.Aggregate("", (current, edge) => current + (edge.Start + " "));
            str += CurrentPath.Edges.Last().End + " " + CurrentPath.Edges.First().Start;
            return str;
        }
    }
}
