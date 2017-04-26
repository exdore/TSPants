using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TSPants
{
    public class Solver
    {
        
        private int iterationsCount = 10;
        public Ant.Path Run(Data data)
        {
            var edges = new Edges();
            edges.GenerateEdges(data);
            var antsCount = data.Cities.Count / 2;
            edges.Initialize();
            var tree = new Tree();
            var minSpanTree = tree.GetMinSpanTree(edges, data.Cities.Count);
            var nodes = tree.GetNodes();
            Write(nodes);
            var currentIteration = 0;
            var best = new Ant.Path
            {
                Lenght = int.MaxValue
            };
            while (currentIteration < iterationsCount)
            {
                var ants = new List<Ant>();
                for (int i = 0; i < antsCount; i++)
                {
                    ants.Add(new Ant());
                }
                var pathes = new List<Ant.Path>();
                foreach (var ant in ants)
                {
                    pathes.Add(ant.BuildPath(edges, data, new Random().Next(0, data.Cities.Count))); //check that all starts are different
                    edges.ForEach(item => item.IsVisited = false);
                }
                foreach (var path in pathes)
                {
                    edges.UpdatePheromones(path);
                }
                pathes = pathes.OrderBy(item => item.Lenght).ToList();
                best = pathes.First().Lenght < best.Lenght ? pathes.First() : best;
                currentIteration++;
            }
            return best;
        }

        public Data Initialize(string filePath, GroupBox groupBox1)
        {
            var data = new Data();
            data.ReadCitiesCoordinates(filePath);
            data.CalculateDistances(groupBox1);
            return data;
        }

        private void Write(List<Tree.Node> nodes )
        {
            XmlSerializer xml = new XmlSerializer(typeof(Tree.Node));
            StreamWriter stw = new StreamWriter(@"D:\nodes.xml");
            xml.Serialize(stw, nodes.First());
            stw.Close();
        }
    }
}
