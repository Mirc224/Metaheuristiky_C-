using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetaHeruistiky_csharp.Algoritmy
{
    class Floyd
    {
        public int NumberOfNodes { get; set; }
        public string[] NodeDesc { get; set; }
        public double[,] Edges { get; set; }
        public double[,] Dist { get; set; }
        public int[,] Next { get; set; }

        public Floyd()
        {

        }

        public double[,] CalculatePath()
        {
            Dist = new double[NumberOfNodes + 1, NumberOfNodes + 1];
            Next = new int[NumberOfNodes + 1, NumberOfNodes + 1];

            for (int i = 1; i <= NumberOfNodes; i++)
            {
                for (int j = 1; j <= NumberOfNodes; j++)
                {
                    Dist[i, j] = double.MaxValue;
                    Next[i, j] = -1;
                }
                Dist[i, i] = 0;
                Next[i, i] = i;
            }
            double actualDistance = -1;

            for (int i = 1; i <= NumberOfNodes; i++)
            {
                for (int j = 1; j <= NumberOfNodes; j++)
                {
                    actualDistance = Edges[i, j];
                    if(actualDistance != -1)
                    {
                        Dist[i, j] = actualDistance;
                        Next[i, j] = j;
                        
                    }
                }
            }

            for (int k = 1; k <= NumberOfNodes; k++)
            {
                for (int i = 1; i <= NumberOfNodes; i++)
                {
                    for (int j = 1; j <= NumberOfNodes; j++)
                    {
                        if(Dist[i, j] > Dist[i, k] + Dist[k, j])
                        {
                            Dist[i, j] = Dist[i, k] + Dist[k, j];
                            Next[i, j] = Next[i, k];
                        }
                    }
                }
            }

            return Dist;
        }

        public string GetPath(int from, int to)
        {
            string line = "";
            if (Next[from, to] == -1)
                return "";
            line += from;
            while(from != to)
            {
                from = Next[from, to];
                line += "-" + from;
            }

            return line;
        }

        public List<string> FormatOutput(int from)
        {
            List<string> outputList = new List<string>();
            for(int i = 1; i <= NumberOfNodes; ++i)
            {
                outputList.Add(GetPath(from, i) + " Vzdialenost: " + Dist[from, i]);
            }

            return outputList;
        }

        public void ReadNodeFile(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            string input = "";
            while ((input = sr.ReadLine()) != null)
            {
                if (input.Length == 0 || (input.Length != 0 && input[0] == '!'))
                    continue;
                this.NumberOfNodes = Int32.Parse(input);
                this.NodeDesc = new string[this.NumberOfNodes + 1];
                int nodeIndex = -1;
                for (int i = 1; i <= this.NumberOfNodes; ++i)
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
                this.Edges = new double[this.NumberOfNodes + 1, this.NumberOfNodes + 1];

                for (int i = 0; i < NumberOfNodes + 1; i++)
                {
                    for (int j = 0; j < NumberOfNodes + 1; j++)
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
                    if (symmetric)
                    {
                        this.Edges[node2, node1] = distance;
                    }
                }
            }
        }
    }
}
