namespace DataStructures;

public class DAG<TKey>
{
    private readonly Dictionary<TKey, HashSet<TKey>> _adj = new();
    private readonly HashSet<TKey> _nodes = new();

    public void AddNode(TKey node)
    {
        _nodes.Add(node);
        if (!_adj.ContainsKey(node))
            _adj[node] = new HashSet<TKey>();
    }

    public void AddEdge(TKey from, TKey to)
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

    public List<TKey> GetDependencies(TKey node)
    {
        var dependencyList = new List<TKey>();
        var queue = new Queue<TKey>();
        TKey newNode;
        queue.Enqueue(node);


        while (queue.Count != 0)
        {
            newNode = queue.Dequeue();
            if (!dependencyList.Contains(newNode)) dependencyList.Add(newNode);

            foreach (var n in _adj[newNode])
                queue.Enqueue(n);
        }

        return dependencyList;
    }

    public bool RemoveNode(TKey node)
    {
        if (!_nodes.Contains(node))
            return false;

        _nodes.Remove(node);
        _adj.Remove(node);

        foreach (var u in _adj.Keys)
            _adj[u].Remove(node);

        return true;
    }

    public List<TKey> TopologicalSort()
    {
        var indeg = new Dictionary<TKey, int>();
        foreach (var n in _nodes) indeg[n] = 0;

        foreach (var u in _adj.Keys)
        foreach (var v in _adj[u])
            indeg[v]++;

        var q = new Queue<TKey>();
        foreach (var n in _nodes)
            if (indeg[n] == 0)
                q.Enqueue(n);

        var order = new List<TKey>();
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

    public bool HasCycle()
    {
        var indeg = new Dictionary<TKey, int>();
        foreach (var n in _nodes) indeg[n] = 0;

        foreach (var u in _adj.Keys)
        foreach (var v in _adj[u])
            indeg[v]++;

        var q = new Queue<TKey>();
        foreach (var n in _nodes)
            if (indeg[n] == 0)
                q.Enqueue(n);

        var visited = 0;
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
}