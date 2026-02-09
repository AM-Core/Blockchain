namespace DataStructures;

public class AVL<TKey, TValue>
{
    private AVLNode<TKey, TValue>? _root;
    private readonly Comparer<TKey> _comparator = Comparer<TKey>.Default;

    public void InsertOne(TKey k, TValue v)
    {
        if (_root == null)
        {
            _root = new AVLNode<TKey, TValue>(k, v, null);
            return;
        }

        AVLNode<TKey, TValue>? current = _root;
        AVLNode<TKey, TValue>? parent = null;

        while (current != null)
        {
            parent = current;
            int cmp = _comparator.Compare(k, current.Key);
            if (cmp == 0)
            {
                current.Value = v;
                return;
            }

            current = (cmp > 0) ? current.Right : current.Left;
        }

        if (parent == null)
            return;

        current = new AVLNode<TKey, TValue>(k, v, parent);
        if (_comparator.Compare(k, parent.Key) > 0)
            parent.Right = current;
        else
            parent.Left = current;

        UpdateHeight(current);

        AVLNode<TKey, TValue>? z = GetUnBalanceNode(current.Parent);
        if (z == null)
            return;

        AVLNode<TKey, TValue>? y = GetHeight(z.Left) > GetHeight(z.Right) ? z.Left : z.Right;
        if (y == null)
            return;

        AVLNode<TKey, TValue>? x = GetHeight(y.Left) >= GetHeight(y.Right) ? y.Left : y.Right;
        if (x == null)
            return;

        ReStructure(x);
    }

    public void DeleteOne(TKey k, TValue v)
    {
        AVLNode<TKey, TValue>? node = FindNode(k);
        if (node == null || !Equals(v, node.Value))
            return;

        AVLNode<TKey, TValue>? start;

        if (node.Left != null && node.Right != null)
        {
            AVLNode<TKey, TValue> tmp = node.Right;
            while (tmp.Left != null)
                tmp = tmp.Left;

            node.Key = tmp.Key;
            node.Value = tmp.Value;
            node = tmp;
        }

        AVLNode<TKey, TValue>? child = (node.Left != null) ? node.Left : node.Right;
        start = node.Parent;

        if (child != null)
            child.Parent = node.Parent;

        if (node.Parent == null)
        {
            _root = child;
        }
        else if (node == node.Parent.Left)
        {
            node.Parent.Left = child;
        }
        else
        {
            node.Parent.Right = child;
        }

        while (start != null)
        {
            UpdateHeight(start);

            AVLNode<TKey, TValue>? z = GetUnBalanceNode(start);
            if (z != null)
            {
                AVLNode<TKey, TValue>? y = GetHeight(z.Left) > GetHeight(z.Right) ? z.Left : z.Right;
                if (y == null)
                    return;

                AVLNode<TKey, TValue>? x;
                if (GetHeight(y.Left) == GetHeight(y.Right))
                    x = (y == z.Left) ? y.Left : y.Right;
                else
                    x = GetHeight(y.Left) > GetHeight(y.Right) ? y.Left : y.Right;

                if (x != null)
                    ReStructure(x);
            }

            start = start.Parent;
        }
    }

    public List<TValue>? Search(TKey k)
    {
        AVLNode<TKey, TValue>? node = FindNode(k);
        if (node == null)
            return null;

        return new List<TValue> { node.Value };
    }

    private AVLNode<TKey, TValue>? FindNode(TKey k)
    {
        AVLNode<TKey, TValue>? current = _root;
        while (current != null)
        {
            int cmp = _comparator.Compare(k, current.Key);
            if (cmp == 0)
                return current;

            current = (cmp > 0) ? current.Right : current.Left;
        }

        return null;
    }

    private void ReStructure(AVLNode<TKey, TValue> x)
    {
        AVLNode<TKey, TValue>? y = x.Parent;
        AVLNode<TKey, TValue>? z = y?.Parent;
        if (y == null || z == null)
            return;

        if ((y == z.Left) == (x == y.Left))
        {
            Rotate(y);
        }
        else
        {
            Rotate(x);
            Rotate(x);
        }
    }

    private void Rotate(AVLNode<TKey, TValue> x)
    {
        AVLNode<TKey, TValue>? y = x.Parent;
        if (y == null)
            return;

        AVLNode<TKey, TValue>? z = y.Parent;

        if (z == null)
        {
            _root = x;
            x.Parent = null;
        }
        else
        {
            ReLink(z, x, y == z.Left);
        }

        if (x == y.Left)
        {
            ReLink(y, x.Right, true);
            ReLink(x, y, false);
        }
        else
        {
            ReLink(y, x.Left, false);
            ReLink(x, y, true);
        }

        UpdateHeight(y);
        UpdateHeight(x);
    }

    public List<TValue> GetValues()
    {
        var values = new List<TValue>();
        InOrderTraversal(_root, values);
        return values;
    }

    private void InOrderTraversal(AVLNode<TKey, TValue>? node, List<TValue> values)
    {
        if (node == null)   
            return;
        InOrderTraversal(node.Left, values);
        values.Add(node.Value);
        InOrderTraversal(node.Right, values);
    }

    public TValue GetMax()
    {
        if (_root == null)
            return default;

        AVLNode<TKey, TValue>? current = _root;
        while (current?.Right != null)
        {
            current = current.Right;
        }
        return current.Value;
    }
    public TValue GetMin()
    {
        if (_root == null)
            return default;

        AVLNode<TKey, TValue>? current = _root;
        while (current?.Left != null)
        {
            current = current.Left;
        }
        return current.Value;
    }

    private void ReLink(AVLNode<TKey, TValue> parent, AVLNode<TKey, TValue>? child, bool isLeft)
    {
        if (child != null)
            child.Parent = parent;
        if (isLeft)
            parent.Left = child;
        else
            parent.Right = child;
    }

    private int GetHeight(AVLNode<TKey, TValue>? node)
    {
        return node == null ? -1 : node.Height;
    }

    private void UpdateHeight(AVLNode<TKey, TValue>? node)
    {
        while (node != null)
        {
            node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
            node = node.Parent;
        }
    }

    private AVLNode<TKey, TValue>? GetUnBalanceNode(AVLNode<TKey, TValue>? node)
    {
        while (node != null)
        {
            if (Math.Abs(GetHeight(node.Left) - GetHeight(node.Right)) > 1)
                return node;
            node = node.Parent;
        }

        return null;
    }

}

public class AVLNode<TKey, TValue>
{
    public AVLNode<TKey, TValue>? Parent;
    public AVLNode<TKey, TValue>? Left;
    public AVLNode<TKey, TValue>? Right;
    public TKey Key;
    public TValue Value;
    public int Height;

    public AVLNode(TKey k, TValue v, AVLNode<TKey, TValue>? parent)
    {
        Key = k;
        Value = v;
        Parent = parent;
        Height = 0;
    }
}