using System;
using MetaHeruistiky_csharp.Algoritmy;

namespace MetaHeruistiky_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Dijkstra dijkstra = new Dijkstra();
            dijkstra.ReadNodeFile("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\nodes.txt");
            dijkstra.ReadEdgeFile("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\edges.txt");
            dijkstra.CalculateShortesPathFromNode(6);
            var tmpList = dijkstra.FormatOutput();
            foreach (var item in tmpList)
            {
                Console.WriteLine(item);
            }

            Floyd floyd = new Floyd();
            floyd.ReadNodeFile("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\nodes.txt");
            floyd.ReadEdgeFile("C:\\Users\\miros\\source\\repos\\MetaHeruistiky_csharp\\MetaHeruistiky_csharp\\Data\\edges.txt");
            floyd.CalculatePath();
            var floydList = floyd.FormatOutput(6);
            foreach (var item in tmpList)
            {
                Console.WriteLine(item);
            }
            //dijkstra.ReadNodeFile(".\\Data\\nodes.txt");
        }
    }
}
