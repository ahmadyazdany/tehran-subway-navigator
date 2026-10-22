using Priority_Queue;
using System.Text;

namespace DsDemo.Data.EventObjects.Graph;

public class Graph<T>
{
    List<Node<T>> nodes = new List<Node<T>>();
    private bool _isDirected;
    private bool _isWeighted;

    public Graph(bool isDirected = false, bool isWeighted = false)
    {
        _isDirected = isDirected;
        _isWeighted = isWeighted;
    }

    #region Readonly Properties - Count, Nodes   

    public int Count { get { return nodes.Count; } }
    public List<Node<T>> Nodes { get { return nodes; } }

    #endregion

    #region Internal class subset

    public class Subset<T>
    {
        public Node<T> Parent { get; set; }
        public int Rank { get; set; }

        public override string ToString()
        {
            return $"Subset with rank {Rank}, parent: {Parent.Value} (index: {Parent.Index})";
        }
    }

    #endregion

    #region Graph: Add Node/Edge, Find, Remove Node/Edge, ToString                 

    public Node<T> AddNode(T value)
    {
        Node<T> node = new Node<T>() { Value = value };
        if (Find(node) != null)
        {
            //duplicate value
            return null;
        }
        else
        {
            Nodes.Add(node);
            UpdateIndices();
            return node;
        }
    }

    public bool AddEdge(Node<T> from, Node<T> to, int weight)
    {
        Node<T> source = Find(from);
        Node<T> destination = Find(to);
        if (source == null || destination == null)
        {
            return false;
        }
        else if (source.Neighbors.Contains(destination))
        {
            return false;
        }
        else
        {
            //for direted graph only below 1st line is required  node1->node2
            from.AddNeighbors(to);
            if (_isWeighted)
            {
                source.Weights.Add(weight);
            }
            //for undireted graph need below line as well
            if (!_isDirected)
            {
                to.AddNeighbors(from);
                if (_isWeighted)
                {
                    to.Weights.Add(weight);
                }
            }
            return true;
        }
    }

    public Node<T> Find(Node<T> Node)
    {
        foreach (Node<T> node in nodes)
        {
            if (node.Value.Equals(Node.Value))
            {
                return node;
            }
        }
        return null;
    }

    public override string ToString()
    {
        StringBuilder nodeString = new StringBuilder();
        for (int i = 0; i < Count; i++)
        {
            nodeString.Append(nodes[i].ToString());
            if (i < Count - 1)
            {
                nodeString.Append("\n");
            }
        }
        return nodeString.ToString();
    }

    #endregion

    #region Graph: Build, Print, Previous node setup 

    internal static Graph<int> BuidGraph()
    {
        #region MyRegion
        Graph<int> graph = new Graph<int>(true, true);
        Node<int> n1 = graph.AddNode(1);
        Node<int> n2 = graph.AddNode(2);
        Node<int> n3 = graph.AddNode(3);
        Node<int> n4 = graph.AddNode(4);
        Node<int> n5 = graph.AddNode(5);
        Node<int> n6 = graph.AddNode(6);
        Node<int> n7 = graph.AddNode(7);
        Node<int> n8 = graph.AddNode(8);

        graph.AddEdge(n1, n2, 3);
        graph.AddEdge(n1, n3, 5);
        graph.AddEdge(n2, n4, 4);
        graph.AddEdge(n3, n4, 12);
        graph.AddEdge(n4, n5, 9);
        graph.AddEdge(n4, n8, 8);
        graph.AddEdge(n5, n6, 4);
        graph.AddEdge(n5, n8, 1);
        graph.AddEdge(n5, n7, 5);
        graph.AddEdge(n6, n7, 6);
        graph.AddEdge(n7, n8, 20);
        #endregion
        return graph;
    }

    internal static string PrintGraph(Graph<T> graph)
    {
        return graph.ToString();
    }

    public List<int> ToDirectPath(List<Edge<int>> path)
    {
        var result = new List<int>();

        result.Add(path[0].From.Value);

        foreach (var edge in path)
            result.Add(edge.To.Value);

        return result;
    }

    #endregion

    #region Graph: GetEdges, GetRoot, Subset, Union, UpdateIndices 
    public List<Edge<T>> GetEdges()
    {
        List<Edge<T>> edges = new List<Edge<T>>();
        foreach (Node<T> from in Nodes)
        {
            for (int i = 0; i < from.Neighbors.Count; i++)
            {
                Edge<T> edge = new Edge<T>()
                {
                    From = from,
                    To = from.Neighbors[i],
                    Weight = i < from.Weights.Count ? from.Weights[i] : 0
                };
                edges.Add(edge);
            }
        }
        return edges;
    }
    //Increase the Index Value
    private void UpdateIndices()
    {
        int i = 0;
        Nodes.ForEach(n => n.Index = i++);
    }

    private Node<T> GetRoot(Subset<T>[] subsets, Node<T> node)
    {
        if (subsets[node.Index].Parent != node)
        {
            subsets[node.Index].Parent = GetRoot(
                subsets,
                subsets[node.Index].Parent);
        }

        return subsets[node.Index].Parent;
    }
    private void Union(Subset<T>[] subsets, Node<T> from, Node<T> to)
    {
        if (subsets[from.Index].Rank > subsets[to.Index].Rank)
        {
            subsets[to.Index].Parent = from;
        }
        else if (subsets[from.Index].Rank < subsets[to.Index].Rank)
        {
            subsets[from.Index].Parent = to;
        }
        else
        {
            subsets[to.Index].Parent = from;
            subsets[from.Index].Rank++;
        }
    }

    #endregion

    #region Graph: MinimumSpanningTreePrim 

    public Edge<T> this[int from, int to]
    {
        get
        {
            Node<T> nodeFrom = Nodes[from];
            Node<T> nodeTo = Nodes[to];
            int i = nodeFrom.Neighbors.IndexOf(nodeTo);
            if (i >= 0)
            {
                Edge<T> edge = new Edge<T>()
                {
                    From = nodeFrom,
                    To = nodeTo,
                    Weight = i < nodeFrom.Weights.Count ? nodeFrom.Weights[i] : 0
                };
                return edge;
            }

            return null;
        }
    }

    private void Fill<Q>(Q[] array, Q value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = value;
        }
    }

    #endregion

    public List<Edge<T>> GetShortestPathDijkstra(Node<T> source, Node<T> target)
    {
        int[] previous = new int[Nodes.Count];
        //Set Every Previous node with initial value -1
        Fill(previous, -1);

        int[] distances = new int[Nodes.Count];
        //Set Every Previous node with initial value -1
        Fill(distances, int.MaxValue);
        //Initially distance will be 0 on starting node
        distances[source.Index] = 0;

        //Create SimplePriorityQueue for dynamicall update the priority of each node on the basis of distance and process accordingly
        SimplePriorityQueue<Node<T>> nodes = new SimplePriorityQueue<Node<T>>();
        for (int i = 0; i < Nodes.Count; i++)
        {
            nodes.Enqueue(Nodes[i], distances[i]);
        }

        while (nodes.Count != 0)
        {
            Node<T> node = nodes.Dequeue();
            for (int i = 0; i < node.Neighbors.Count; i++)
            {
                Node<T> neighbor = node.Neighbors[i];
                int weight = i < node.Weights.Count ? node.Weights[i] : 0;
                int weightTotal = distances[node.Index] + weight;

                if (distances[neighbor.Index] > weightTotal)
                {
                    distances[neighbor.Index] = weightTotal;
                    previous[neighbor.Index] = node.Index;
                    nodes.UpdatePriority(neighbor, distances[neighbor.Index]);
                }
            }
        }

        //Getting all the index
        List<int> indices = new List<int>();
        int index = target.Index;
        while (index >= 0)
        {
            indices.Add(index);
            index = previous[index];
        }

        //Reverse all the index to get the correct order
        indices.Reverse();
        List<Edge<T>> result = new List<Edge<T>>();
        for (int i = 0; i < indices.Count - 1; i++)
        {
            Edge<T> edge = this[indices[i], indices[i + 1]];
            result.Add(edge);
        }
        //return list of Edge
        return result;
    }
}
