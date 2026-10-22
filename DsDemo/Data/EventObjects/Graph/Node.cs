using DsDemo.Data.Entities;
using System.Runtime.CompilerServices;
using System.Text;

namespace DsDemo.Data.EventObjects.Graph;

public class Node<T>
{
    public int Index { get; set; }
    public T Value { get; set; }
    public List<Node<T>> Neighbors { get; set; } = new List<Node<T>>();
    public List<int> Weights { get; set; } = new List<int>();

    public bool AddNeighbors(Node<T> neighbor)
    {
        if (Neighbors.Contains(neighbor))
        {
            return false;
        }
        else
        {
            Neighbors.Add(neighbor);
            return true;
        }
    }

    public override string ToString()
    {
        StringBuilder nodeString = new StringBuilder();
        nodeString.Append("[ Node Value - " + Value + " with Neighbors : ");
        for (int i = 0; i < Neighbors.Count; i++)
        {
            nodeString.Append(Neighbors[i].Value + " ");// + edge.ToString()+ " ");                
        }
        nodeString.Append("]");
        return nodeString.ToString();
    }
}
