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
            Alpha = 1;
            Beta = 5;
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
                availableEdges.CalculateProbabilities(Alpha, Beta);  
                var chosenEdges = availableEdges.OrderByDescending(item => item.ProbabilityValue).Take(3).ToList();
                var sumProb = chosenEdges.Sum(item => item.ProbabilityValue);
                chosenEdges.ForEach(item => item.ProbabilityValue /= sumProb);
                var rnd = new Random();
                var probability = rnd.NextDouble();
                var cumulatives = new List<double> { chosenEdges.First().ProbabilityValue };
                for (int i = 1; i < chosenEdges.Count; i++)
                {
                    cumulatives.Add(cumulatives[i - 1] + chosenEdges[i].ProbabilityValue);
                }
                //var selectedEdge = chosenEdges[cumulatives.FindIndex(item => item > probability)];
                var selectedEdge = chosenEdges.First();
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
