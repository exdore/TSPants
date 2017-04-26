using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPants
{
    public class Tree
    {
        private List<Edge> MinSpanTree { get; set; }

        public List<Edge> GetMinSpanTree(List<Edge> edges, int verticesCount)
        {
            CruscalAlgorithm(edges, verticesCount);
            return MinSpanTree;
        }

        [Serializable]
        
        public class Node
        {
            public int Number;
            public List<Node> Neighbors;
            public int Level;
        }

        public List<Node> GetNodes()
        {
            DFS(0);
            return NodesList;
        }

        private List<Node> NodesList { get; set; }

        private void DFS(int index)
        {
            if (index < MinSpanTree.Count)
            {
                var node = NodesList[index];
                var edges = MinSpanTree.Where(item => (item.End == index || item.Start == index) && item.Used == false);
                foreach (var edge in edges)
                {
                    edge.Used = true;
                    var newNode = new Node
                    {
                        Level = node.Level + 1,
                        Neighbors = new List<Node>(),
                        Number = (index == edge.Start) ? edge.End : edge.Start
                    };
                    node.Neighbors.Add(newNode);
                    NodesList.Add(newNode);
                }
                index += 1;
                DFS(index);
            }
        }

        private void CruscalAlgorithm(List<Edge> edges, int verticesCount)
        {
            var sortedEdges = edges.OrderBy(item => item.Lenght).ToList();
            var cruscalTree = new List<Edge>();
            var treeId = new List<int>();
            for (int i = 0; i < verticesCount; i++)
            {
                treeId.Add(i);
            }
            foreach (var edge in sortedEdges)
            {
                int start = edge.Start;
                int end = edge.End;
                if (treeId[start] != treeId[end])
                {
                    cruscalTree.Add(edge);
                    var oldId = treeId[end];
                    var newId = treeId[start];
                    for (int i = 0; i < verticesCount; i++)
                    {
                        if (treeId[i] == oldId)
                            treeId[i] = newId;
                    }
                }
            }
            MinSpanTree = cruscalTree.OrderBy(item => item.Start).ToList();
            NodesList = new List<Node>
            {
                new Node
                {
                    Number = 0,
                    Level = 0,
                    Neighbors = new List<Node>()
                }
            };
        }
    }
}
