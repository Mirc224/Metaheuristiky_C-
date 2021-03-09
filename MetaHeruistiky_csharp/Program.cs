using System;
using MetaHeruistiky_csharp.Algoritmy;

namespace MetaHeruistiky_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            /*            Dijkstra dijkstra = new Dijkstra();
                        dijkstra.ReadNodeFile("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\nodes.txt");
                        dijkstra.ReadEdgeFile("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\edges.txt");
                        dijkstra.CalculateShortesPathFromNode(6);
                        var tmpList = dijkstra.FormatOutput();
                        foreach (var item in tmpList)
                        {
                            Console.WriteLine(item);
                        }
                        Console.WriteLine();
                        Floyd floyd = new Floyd();
                        floyd.ReadNodeFile("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\nodes.txt");
                        floyd.ReadEdgeFile("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\edges.txt");
                        floyd.CalculatePath();
                        var floydList = floyd.FormatOutput(6);
                        foreach (var item in tmpList)
                        {
                            Console.WriteLine(item);
                        }*/
            AStar astar = new AStar();
            /*astar.ReadMyNodes("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\my_nodes.txt");
            astar.ReadMyEdges("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\my_edges.txt");
            astar.ReadMyIncidentEdges("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\my_incident_edges.txt");
            astar.CalculateShortestPath(12, 4);*/
            astar.ReadNodes("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\SR_nodes.atr");
            astar.ReadNodeCords("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\SR_nodes.vec");
            astar.ReadEdges("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\SR_edges.atr");
            astar.ReadIncidentEdges("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\SR_edges_incid.txt");
            //astar.CalculateShortestPath(12, 4);
            astar.CalculateShortestPath(7, 20);
            //astar.TestConnectionOfGraph();
            Console.WriteLine("Kontrolujem spojenie");
            astar.FindConnectedComponents();
            //dijkstra.ReadNodeFile(".\\Data\\nodes.txt");
        }
    }
}
