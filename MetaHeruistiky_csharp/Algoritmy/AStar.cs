using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MetaHeruistiky_csharp.Algoritmy.Assets;

namespace MetaHeruistiky_csharp.Algoritmy
{
    class AStar
    {
        public int NumberOfNodes { get; set; }
        public Node[] Nodes { get; set; }
        public double[] Edges { get; set; }
        public ForwardStarNode[] ForwardStar { get; set; }
        public int[] StarIndex { get; set; }
        public int[] PreviousVertex { get; set; }
        public double[] DistanceFromStart { get; set; }
        public double[] EuclidianDistanceToEnd { get; set; }
        public List<int> ReconstructedPath { get; set; } = new List<int>();

        public bool CalculateShortestPath(int from = 12, int to = 5)
        {
            bool[] wasVisited = new bool[NumberOfNodes];
            List<int> unvisitedList = new List<int>();
            List<int> visitedList = new List<int>();
            List<ForwardStarNode> neighbourNodes = new List<ForwardStarNode>();
            ForwardStarNode tmpStarNode = null;
            CalulateEuclidian(to);
            for(int i = 0; i < NumberOfNodes; ++i)
            {
                DistanceFromStart[i] = double.MaxValue;
                PreviousVertex[i] = -1;
            }
            DistanceFromStart[from] = 0;
            unvisitedList.Add(from);

            double actualMinumum;
            int tmpIndex = -1;
            int actualNode = -1;
            double tmpDistance = -1;
            bool vertexReached = false;
            while(unvisitedList.Count != 0)
            {
                actualMinumum = double.MaxValue;
                for (int i = 0; i < unvisitedList.Count; i++)
                {
                    tmpIndex = unvisitedList[i];
                    if(DistanceFromStart[tmpIndex] + EuclidianDistanceToEnd[tmpIndex] < actualMinumum)
                    {
                        actualMinumum = DistanceFromStart[tmpIndex] + EuclidianDistanceToEnd[tmpIndex];
                        actualNode = tmpIndex;
                    }
                }
                if (actualNode == to)
                {
                    vertexReached = true;
                    break;
                }
                    

                wasVisited[actualNode] = true;
                unvisitedList.Remove(actualNode);

                tmpStarNode = ForwardStar[actualNode];
                neighbourNodes.Clear();
                if(tmpStarNode != null)
                {
                    while (true)
                    {
                        neighbourNodes.Add(tmpStarNode);

                        if(tmpStarNode.StartNode == actualNode)
                        {
                            if (tmpStarNode.StartNodeRef == ForwardStar[actualNode])
                                break;
                            tmpStarNode = tmpStarNode.StartNodeRef;
                        }
                        else
                        {
                            if (tmpStarNode.EndNodeRef == ForwardStar[actualNode])
                                break;
                            tmpStarNode = tmpStarNode.EndNodeRef;
                        }
                    }
                }
                
                
                for (int i = 0; i < neighbourNodes.Count; i++)
                {
                    tmpStarNode = neighbourNodes[i];
                    tmpIndex = tmpStarNode.StartNode != actualNode ? tmpStarNode.StartNode : tmpStarNode.EndNode;

                    tmpDistance = DistanceFromStart[actualNode] + tmpStarNode.Distance;
                    if(tmpDistance < DistanceFromStart[tmpIndex])
                    {
                        DistanceFromStart[tmpIndex] = tmpDistance;
                        PreviousVertex[tmpIndex] = actualNode;
                        if(!wasVisited[tmpIndex])
                        {
                            unvisitedList.Add(tmpIndex);
                        }
                    }
                }
            }

            ReconstructedPath.Clear();
            if (vertexReached)
            {
                ReconstructedPath.Add(to);
                tmpIndex = PreviousVertex[to];
                while (tmpIndex != -1)
                {
                    ReconstructedPath.Add(tmpIndex);
                    tmpIndex = PreviousVertex[tmpIndex];
                }
                ReconstructedPath.Reverse();
                Console.WriteLine(String.Join("-", ReconstructedPath.ToArray()));
                Console.WriteLine($"Vzdialenost: {DistanceFromStart[to]}");
            }
            return vertexReached;
        }

        public void FindConnectedComponents()
        {
            bool[] visited = new bool[NumberOfNodes];
            int componentCount = 0;
            Queue<int> front = new Queue<int>();
            Queue<int> tmpFront = null;
            int tmpIndex = -1;
            for(int i = 0; i < NumberOfNodes; ++i)
            {
                if(!visited[i])
                {
                    visited[i] = true;
                    front = GetNeighboursIndexes(i);
                    while(front.Count != 0)
                    {
                        tmpIndex = front.Dequeue();
                        if(!visited[tmpIndex])
                        {
                            visited[tmpIndex] = true;
                            tmpFront = GetNeighboursIndexes(tmpIndex);
                            while (tmpFront.Count != 0)
                                front.Enqueue(tmpFront.Dequeue());
                        }
                    }
                    ++componentCount;
                }
            }
            Console.WriteLine(componentCount);
        }

        private Queue<int> GetNeighboursIndexes(int vertex)
        {
            Queue<int> neighbourNodes = new Queue<int>();
            ForwardStarNode tmpStarNode = null;
            tmpStarNode = ForwardStar[vertex];
            if (tmpStarNode != null)
            {
                while (true)
                {
                    if (tmpStarNode.StartNode != vertex)
                        neighbourNodes.Enqueue(tmpStarNode.StartNode);
                    else
                        neighbourNodes.Enqueue(tmpStarNode.EndNode);

                    if (tmpStarNode.StartNode == vertex)
                    {
                        if (tmpStarNode.StartNodeRef == ForwardStar[vertex])
                            break;
                        tmpStarNode = tmpStarNode.StartNodeRef;
                    }
                    else
                    {
                        if (tmpStarNode.EndNodeRef == ForwardStar[vertex])
                            break;
                        tmpStarNode = tmpStarNode.EndNodeRef;
                    }
                }
            }
            return neighbourNodes;
        }

        public void CheckIfGraphIsConnected()
        {
            int startVertex = 0;
            for(int i = 0; i < NumberOfNodes; ++i)
            {
                if (startVertex == i)
                    continue;
                Console.WriteLine("Testujem " + i);
                if(!CalculateShortestPath(startVertex, i))
                {
                    Console.WriteLine("Graph is not connected!");
                    break;
                }
            }
        }

        private void CalculateDistanceFromStart(int from)
        {
            for (int i = 0; i < NumberOfNodes; i++)
            {
                DistanceFromStart[i] = double.MaxValue;
            }
            int indexToStar = StarIndex[from];

            ForwardStarNode tmpNode = null;
            for(int i = 0; i + indexToStar < ForwardStar.GetLength(0); ++i)
            {
                tmpNode = ForwardStar[indexToStar + i];
                if (tmpNode.StartNode != from)
                    break;
                DistanceFromStart[tmpNode.EndNode] = tmpNode.Distance;
            }
        }

        private void CalulateEuclidian(int to)
        {
            //EuclidianDistanceToEnd = new double[] { 9, 7, 8, 8, 0, 6, 3, 6, 4, 4, 3, 6, 10};
            double fromX1;
            double endX2 = Nodes[to].XCord;
            double fromY1;
            double endY2 = Nodes[to].YCord;
            double x;
            double y;
            for (int i = 0; i < NumberOfNodes; ++i)
            {
                fromX1 = Nodes[i].XCord;
                fromY1 = Nodes[i].YCord;
                x = (fromX1 - endX2);
                y = (fromY1 - endY2);
                EuclidianDistanceToEnd[i] = Math.Sqrt(x * x + y * y); ;
            }
        }

        public void ReadMyNodes(string fileName)
        {
            List<Node> tmpList = new List<Node>(20);
            StreamReader sr = new StreamReader(fileName);
            string input = "";
            int nodeID = 0;
            string[] split = new string[2];
            while ((input = sr.ReadLine()) != null)
            {
                split = input.Split(" ");
                nodeID = Int32.Parse(split[0]);
                tmpList.Add(new Node { ID = nodeID });
            }
            Nodes = tmpList.ToArray();
            NumberOfNodes = Nodes.GetLength(0);
            Edges = new double[NumberOfNodes];
            StarIndex = new int[NumberOfNodes];
            PreviousVertex = new int[NumberOfNodes];
            DistanceFromStart = new double[NumberOfNodes];
            EuclidianDistanceToEnd = new double[NumberOfNodes];
        }

        public void ReadMyEdges(string fileName)
        {
            string input = "";
            int maxEdge = 0;

            var tmpList = new List<Tuple<int, double>>();
            Tuple<int, double> tmpTuple = null;

            using (var reader = new StreamReader(fileName))
            {
                string[] splitResult = new string[2];
                while ((input = reader.ReadLine()) != null)
                {
                    splitResult = input.Split(" ");
                    tmpTuple = new Tuple<int, double>(Int32.Parse(splitResult[0]), double.Parse(splitResult[1].Replace('.', ',')));
                    if (tmpTuple.Item1 > maxEdge)
                        maxEdge = tmpTuple.Item1;
                    tmpList.Add(tmpTuple);
                }
            }
            Edges = new double[maxEdge + 1];
            foreach (var tuple in tmpList)
            {
                Edges[tuple.Item1] = tuple.Item2;
            }
        }

        public void ReadMyIncidentEdges(string fileName)
        {
            string input = "";
            double distance = 0;
            int startIndex = -1;
            int endIndex = -1;
            int edgeId = -1;

            ForwardStarNode tmpStartNode = null;
            ForwardStarNode newNode = null;
            ForwardStarNode tmpNode = null;

            ForwardStar = new ForwardStarNode[NumberOfNodes];
            using (var reader = new StreamReader(fileName))
            {
                string[] splitResult = new string[3];
                while ((input = reader.ReadLine()) != null)
                {
                    splitResult = input.Split(" ");
                    edgeId = Int32.Parse(splitResult[0]);
                    startIndex = Int32.Parse(splitResult[1]);
                    endIndex = Int32.Parse(splitResult[2]);
                    distance = Edges[edgeId];

                    newNode = new ForwardStarNode { ID = edgeId, StartNode = startIndex, EndNode = endIndex, Distance = distance };
                    tmpStartNode = ForwardStar[startIndex];

                    if (tmpStartNode == null)
                    {
                        ForwardStar[startIndex] = newNode;
                        newNode.StartNodeRef = newNode;
                    }
                    else
                    {
                        tmpNode = null;
                        if (tmpStartNode.StartNode == startIndex)
                            tmpNode = tmpStartNode.StartNodeRef;
                        else
                            tmpNode = tmpStartNode.EndNodeRef;

                        newNode.StartNodeRef = tmpStartNode;
                        bool found = false;
                        while (true)
                        {
                            if (tmpNode.StartNode == startIndex)
                            {
                                if (tmpNode.StartNodeRef == tmpStartNode)
                                {
                                    tmpNode.StartNodeRef = newNode;
                                    found = true;
                                    break;
                                }
                                tmpNode = tmpNode.StartNodeRef;
                            }
                            else
                            {
                                if (tmpNode.EndNodeRef == tmpStartNode)
                                {
                                    tmpNode.EndNodeRef = newNode;
                                    found = true;
                                    break;
                                }
                                tmpNode = tmpNode.EndNodeRef;
                            }
                        }
                        if (found)
                        {

                        }
                    }


                    tmpStartNode = ForwardStar[endIndex];
                    if (tmpStartNode == null)
                    {
                        ForwardStar[endIndex] = newNode;
                        newNode.EndNodeRef = newNode;
                    }
                    else
                    {
                        tmpNode = null;
                        if (tmpStartNode.StartNode == endIndex)
                            tmpNode = tmpStartNode.StartNodeRef;
                        else
                            tmpNode = tmpStartNode.EndNodeRef;

                        newNode.EndNodeRef = tmpStartNode;
                        while (true)
                        {
                            if (tmpNode.StartNode == endIndex)
                            {
                                if (tmpNode.StartNodeRef == tmpStartNode)
                                {
                                    tmpNode.StartNodeRef = newNode;
                                    break;
                                }
                                tmpNode = tmpNode.StartNodeRef;
                            }
                            else
                            {
                                if (tmpNode.EndNodeRef == tmpStartNode)
                                {
                                    tmpNode.EndNodeRef = newNode;
                                    break;
                                }
                                tmpNode = tmpNode.EndNodeRef;
                            }
                        }
                    }
                }
            }
            /*startIndex = 3;
            tmpStartNode = ForwardStar[startIndex];
            tmpNode = tmpStartNode;
            while (true)
            {
                Console.WriteLine(tmpNode.ID);
                if (tmpNode.StartNode == startIndex)
                {
                    tmpNode = tmpNode.StartNodeRef;
                }
                else
                {
                    tmpNode = tmpNode.EndNodeRef;
                }
                if (tmpNode == tmpStartNode)
                    break;
            }*/
        }

        public void ReadNodes(string fileName)
        {
            List<Node> tmpList = new List<Node>(50000);
            StreamReader sr = new StreamReader(fileName);
            string input = "";
            int nodeID = 0;
            while ((input = sr.ReadLine()) != null)
            {
                nodeID = Int32.Parse(input);
                tmpList.Add(new Node { ID = nodeID });
            }
            Nodes = tmpList.ToArray();
            NumberOfNodes = Nodes.GetLength(0);
            PreviousVertex = new int[NumberOfNodes]; 
            StarIndex = new int[NumberOfNodes];
            PreviousVertex = new int[NumberOfNodes];
            DistanceFromStart = new double[NumberOfNodes];
            EuclidianDistanceToEnd = new double[NumberOfNodes];
        }

        public void ReadNodeCords(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            string input = "";
            int nodeId = -1;
            string[] splitResult = new string[2];
            Node tmpNode = null;
            while ((input = sr.ReadLine()) != null)
            {
                splitResult = input.Split(" ");
                nodeId = Int32.Parse(splitResult[0]);
                input = sr.ReadLine();
                tmpNode = Nodes[nodeId];
                splitResult = input.Split("  ");
                tmpNode.XCord = double.Parse(splitResult[0].Replace('.', ','));
                tmpNode.YCord = double.Parse(splitResult[1].Replace('.', ','));
            }
        }

        public void ReadEdges(string fileName)
        {
            string input = "";
            int maxEdge = 0;

            var tmpList = new List<Tuple<int, double>>();
            Tuple<int, double> tmpTuple = null;
            using (var reader = new StreamReader(fileName))
            {
                string[] splitResult = new string[2];
                while ((input = reader.ReadLine()) != null)
                {
                    splitResult = input.Split(" ");
                    tmpTuple = new Tuple<int, double>(Int32.Parse(splitResult[0]), double.Parse(splitResult[1].Replace('.', ',')));
                    if (tmpTuple.Item1 > maxEdge)
                        maxEdge = tmpTuple.Item1;
                    tmpList.Add(tmpTuple);
                }
            }
            Edges = new double[maxEdge + 1];
            foreach (var tuple in tmpList)
            {
                Edges[tuple.Item1] = tuple.Item2;
            }
        }

        public void ReadIncidentEdges(string fileName)
        {
            string input = "";
            double distance = 0;
            int startIndex = -1;
            int endIndex = -1;
            int edgeId = -1;
     

            ForwardStarNode tmpStartNode = null;
            ForwardStarNode newNode = null;
            ForwardStarNode tmpNode = null;

            ForwardStar = new ForwardStarNode[NumberOfNodes];
            int iter = 0;
            using (var reader = new StreamReader(fileName))
            {
                string[] splitResult = new string[3];
                while ((input = reader.ReadLine()) != null)
                {
                    splitResult = input.Split("  ");
                    edgeId = Int32.Parse(splitResult[0]);
                    startIndex = Int32.Parse(splitResult[1]);
                    endIndex = Int32.Parse(splitResult[2]);
                    distance = Edges[edgeId];
                    if (startIndex == endIndex)
                        continue;

                    newNode = new ForwardStarNode { ID = edgeId, StartNode = startIndex, EndNode = endIndex, Distance = distance };
                    tmpStartNode = ForwardStar[startIndex];

                    if(tmpStartNode == null)
                    {
                        ForwardStar[startIndex] = newNode;
                        newNode.StartNodeRef = newNode;
                    }
                    else
                    {
                        tmpNode = null;
                        if (tmpStartNode.StartNode == startIndex)
                            tmpNode = tmpStartNode.StartNodeRef;
                        else
                            tmpNode = tmpStartNode.EndNodeRef;

                        newNode.StartNodeRef = tmpStartNode;
                        bool found = false;
                        while (true)
                        {
                            if (tmpNode.StartNode == startIndex)
                            {
                                if (tmpNode.StartNodeRef == tmpStartNode)
                                {
                                    tmpNode.StartNodeRef = newNode;
                                    found = true;
                                    break;
                                }
                                tmpNode = tmpNode.StartNodeRef;
                            }
                            else
                            {
                                if (tmpNode.EndNodeRef == tmpStartNode)
                                {
                                    tmpNode.EndNodeRef = newNode;
                                    found = true;
                                    break;
                                }
                                tmpNode = tmpNode.EndNodeRef;
                            }
                        }
                        if(found)
                        {

                        }
                    }


                    tmpStartNode = ForwardStar[endIndex];
                    if (tmpStartNode == null)
                    { 
                        ForwardStar[endIndex] = newNode;
                        newNode.EndNodeRef = newNode;
                    }
                    else
                    {
                        tmpNode = null;
                        if (tmpStartNode.StartNode == endIndex)
                            tmpNode = tmpStartNode.StartNodeRef;
                        else
                            tmpNode = tmpStartNode.EndNodeRef;

                        newNode.EndNodeRef = tmpStartNode;
                        while (true)
                        {
                            if (tmpNode.StartNode == endIndex)
                            {
                                if (tmpNode.StartNodeRef == tmpStartNode)
                                {
                                    tmpNode.StartNodeRef = newNode;
                                    break;
                                }
                                tmpNode = tmpNode.StartNodeRef;
                            }
                            else
                            {
                                if (tmpNode.EndNodeRef == tmpStartNode)
                                {
                                    tmpNode.EndNodeRef = newNode;
                                    break;
                                }
                                tmpNode = tmpNode.EndNodeRef;
                            }
                        }
                    }
/*                    Console.WriteLine(++iter);

                    Console.WriteLine(edgeId);*/
                }
            }
            //Console.WriteLine("done");
            startIndex = 0;
            tmpStartNode = ForwardStar[startIndex];
            tmpNode = tmpStartNode;
            while (true)
            {
                //Console.WriteLine(tmpNode.ID);
                if(tmpNode.StartNode == startIndex)
                {
                    tmpNode = tmpNode.StartNodeRef;
                }
                else
                {
                    tmpNode = tmpNode.EndNodeRef;
                }
                if (tmpNode == tmpStartNode)
                    break;
            }
        }

        public void TestConnectionOfGraph()
        {
            for(int i = 0; i < ForwardStar.GetLength(0); ++i)
            {
                if(ForwardStar[0] == null)
                    Console.WriteLine($"Vrchol {i} nie je incidentny so ziadnou hranou.");
            }
        }
    }
}


/*splitResult = input.Split("  ");
edgeId = Int32.Parse(splitResult[0]);
startIndex = Int32.Parse(splitResult[1]);
endIndex = Int32.Parse(splitResult[2]);
distance = Edges[edgeId];
tmpEdgeList.Add(new ForwardStarNode { ID = edgeId, StartNode = startIndex, EndNode = endIndex, Distance = distance });
if (previousVertex != startIndex)
{
    if (previousVertex != -1)
    {
        StarIndex[previousVertex] = previousEdgeCount;
        previousEdgeCount = edgeCount;
    }
    previousVertex = startIndex;
}
++edgeCount;*/