using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPants
{
    public class Edges : List<Edge>
    {
        public Edges()
        {
            new List<Edge>();
        }

        public Edges(List<Edge> edges)
        {
            AddRange(edges);
        }

        public void GenerateEdges(Data data)
        {
            for (int i = 0; i < data.Cities.Count; i++)
            {
                for (int j = 0; j < data.Cities.Count; j++)
                {
                    if (i != j)
                        Add(new Edge
                        {
                            Start = i,
                            End = j,
                            Lenght = data.Distances[i, j],
                            Used = false
                        });
                }
            }
        }

        private void InitializePheromones()
        {
            foreach (var edge in this)
            {
                edge.PheromoneLevel = 1.0 / Count;
            }
        }

        public void UpdatePheromones(Ant.Path path)
        {
            this.Where(item => path.Edges.Contains(item)).ToList()
                .ForEach(item => item.PheromoneLevel += 1.0 / path.Lenght);
        }

        private void InitializeEuristcs()
        {
            foreach (var edge in this)
            {
                edge.EuristicValue = 1.0 / edge.Lenght;
            }
        }

        public void Initialize()
        {
            InitializePheromones();
            InitializeEuristcs();
        }

        public void CalculateProbabilities(int alpha, int beta)
        {
            var sum = this.Sum(edge => edge.EuristicValue * alpha + edge.PheromoneLevel * beta);
            foreach (var edge in this)
            {
                edge.ProbabilityValue = (edge.EuristicValue * alpha + edge.PheromoneLevel * beta) / sum;
            }
        }
    }
}
