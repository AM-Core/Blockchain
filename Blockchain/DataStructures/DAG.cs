namespace DataStructures;

public class DAG<T>
{
    private readonly Dictionary<T, HashSet<T>> _adj = new();
    private readonly HashSet<T> _nodes = new();

    public void AddNode(T node)
    {
        _nodes.Add(node);
        if (!_adj.ContainsKey(node))
            _adj[node] = new HashSet<T>();
    }

    public void AddEdge(T from, T to)
    {
        AddNode(from);
        AddNode(to);
        _adj[from].Add(to);
        if (HasCycle())
        {
            _adj[from].Remove(to);
            throw new InvalidOperationException("Adding this edge creates a cycle.");
        }
    }

    public bool RemoveNode(T node)
    {
        if (!_nodes.Contains(node))
            return false;

        _nodes.Remove(node);
        _adj.Remove(node);

        foreach (var u in _adj.Keys)
            _adj[u].Remove(node);

        return true;
    }

    public bool HasCycle()
    {
        var indeg = new Dictionary<T, int>();
        foreach (var n in _nodes) indeg[n] = 0;

        foreach (var u in _adj.Keys)
        foreach (var v in _adj[u])
            indeg[v]++;

        var q = new Queue<T>();
        foreach (var n in _nodes)
            if (indeg[n] == 0)
                q.Enqueue(n);

        int visited = 0;
        while (q.Count > 0)
        {
            var u = q.Dequeue();
            visited++;
            foreach (var v in _adj[u])
            {
                indeg[v]--;
                if (indeg[v] == 0)
                    q.Enqueue(v);
            }
        }
        return visited != _nodes.Count;
    }

    public List<T> TopologicalSort()
    {
        var indeg = new Dictionary<T, int>();
        foreach (var n in _nodes) indeg[n] = 0;

        foreach (var u in _adj.Keys)
        foreach (var v in _adj[u])
            indeg[v]++;

        var q = new Queue<T>();
        foreach (var n in _nodes)
            if (indeg[n] == 0)
                q.Enqueue(n);

        var order = new List<T>();
        while (q.Count > 0)
        {
            var u = q.Dequeue();
            order.Add(u);
            foreach (var v in _adj[u])
            {
                indeg[v]--;
                if (indeg[v] == 0)
                    q.Enqueue(v);
            }
        }

        if (order.Count != _nodes.Count)
            throw new InvalidOperationException("Graph has a cycle.");

        return order;
    }
}