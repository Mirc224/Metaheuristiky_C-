using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace MetaHeruistiky_csharp.Algoritmy
{
    class Dijkstra
    {
        public int NumberOfNodes{ get; set; }
        public string[] NodeDesc { get; set; }
        public double[,] Edges { get; set; }
        public double[] ShortestFromNode { get; set; }
        public List<List<int>> Paths { get; set; }

        public Dijkstra()
        {
            Paths = new List<List<int>>();
        }

        public List<List<int>> CalculateShortesPathFromNode(int startingVertex)
        {
            this.Paths = new List<List<int>>();
            this.ShortestFromNode = new double[this.NumberOfNodes + 1];
            int[] previousVertex = new int[this.NumberOfNodes + 1];
            List<int> visited = new List<int>();
            List<int> unvisited = new List<int>();

            for(int i = 1; i <= this.NumberOfNodes; ++i)
            {
                ShortestFromNode[i] = double.MaxValue;
                previousVertex[i] = -1;
                unvisited.Add(i);
            }
            ShortestFromNode[startingVertex] = 0;

            double actualMinimum = -1;
            int tmpIndex = -1;
            int actualNode = -1;
            double tmpDistance = -1;
            while (unvisited.Count != 0)
            {
                actualMinimum = -1;
                for (int i = 0; i < unvisited.Count; ++i)
                {
                    tmpIndex = unvisited[i];
                    if (ShortestFromNode[tmpIndex] < actualMinimum || actualMinimum == -1)
                    {
                        if (!visited.Contains(tmpIndex))
                        {
                            actualMinimum = ShortestFromNode[tmpIndex];
                            actualNode = tmpIndex;
                        }
                    }
                }
                visited.Add(actualNode);
                unvisited.Remove(actualNode);

                // Preskumanie okolia od aktualneho bodu
                tmpDistance = -1;
                for (int i = 0; i < unvisited.Count; ++i)
                {
                    tmpIndex = unvisited[i];
                    if (this.Edges[actualNode, tmpIndex] != -1)
                    {
                        tmpDistance = ShortestFromNode[actualNode] + Edges[actualNode,tmpIndex];
                        if (tmpDistance < ShortestFromNode[tmpIndex])
                        {
                            ShortestFromNode[tmpIndex] = tmpDistance;
                            previousVertex[tmpIndex] = actualNode;
                        }
                    }
                }
            }

            int actualIndex = -1;
            for (int i = 1; i <= NumberOfNodes; ++i)
            {
                List<int> path = new List<int>();
                this.Paths.Add(path);
                path.Add(i);
                actualIndex = i;
                while (actualIndex != startingVertex)
                {
                    path.Add(previousVertex[actualIndex]);
                    actualIndex = previousVertex[actualIndex];
                }
            }

            return new List<List<int>>(this.Paths);
        }

        public List<string> FormatOutput()
        {
            List<string> formatedOutput = new List<string>();
            string line;
            for (int i = 0; i < Paths.Count; i++)
            {
                line = this.FormatOutputPath(i + 1);
                line += " Vzdialenost: " + this.ShortestFromNode[i+1];
                formatedOutput.Add(line);
            }

            return formatedOutput;
        }

        public string FormatOutputPath(int to)
        {
            string line = "";
            var path = this.Paths[to - 1];
            for (int j = path.Count - 1; j > -1; --j)
            {
                line += path[j] + "-";
            }
            line = line.Substring(0, line.Length - 1);
            return line;
        }

        public void ReadNodeFile(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            string input = "";
            while((input = sr.ReadLine()) != null)
            {
                if (input.Length == 0 || (input.Length != 0 && input[0] == '!'))
                    continue;
                this.NumberOfNodes = Int32.Parse(input);
                this.NodeDesc = new string[this.NumberOfNodes + 1];
                int nodeIndex = -1;
                for(int i = 1; i <= this.NumberOfNodes; ++i)
                {
                    input = sr.ReadLine();
                    string[] split = input.Split(",");
                    nodeIndex = Int32.Parse(split[0]);
                    this.NodeDesc[nodeIndex] = split[1];
                }
            }
        }

        public void ReadEdgeFile(string fileName, bool symmetric = true)
        {
            StreamReader sr = new StreamReader(fileName);
            string input = "";
            int numberOfEdges = 0;
            while ((input = sr.ReadLine()) != null)
            {
                if (input.Length == 0 || (input.Length != 0 && input[0] == '!'))
                    continue;
                numberOfEdges = Int32.Parse(input);
                int node1 = -1;
                int node2 = -1;
                double distance = -1;
                this.Edges = new double[this.NumberOfNodes + 1,this.NumberOfNodes + 1];

                for (int i = 0; i < NumberOfNodes+1; i++)
                {
                    for (int j = 0; j < NumberOfNodes+1; j++)
                    {
                        this.Edges[i, j] = -1;
                    }
                }

                for (int i = 0; i < numberOfEdges; ++i)
                {
                    input = sr.ReadLine();
                    string[] split = input.Split(",");
                    node1 = Int32.Parse(split[0]);
                    node2 = Int32.Parse(split[1]);
                    distance = double.Parse(split[2]);
                    this.Edges[node1, node2] = distance;
                    if(symmetric)
                    {
                        this.Edges[node2, node1] = distance;
                    }
                }
            }
        }
    }
}
