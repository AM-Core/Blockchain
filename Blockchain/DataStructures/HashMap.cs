namespace DataStructures;

using System;
using System.Collections.Generic;

public class HashMap<TKey, TValue>
{
    private readonly List<KeyValuePair<TKey, TValue>>[] _buckets;
    private readonly int _capacity;

    public HashMap(int capacity = 16)
    {
        _capacity = capacity;
        _buckets = new List<KeyValuePair<TKey, TValue>>[_capacity];
        for (int i = 0; i < _capacity; i++)
            _buckets[i] = new List<KeyValuePair<TKey, TValue>>();
    }

    private int GetIndex(TKey key)
    {
        return Math.Abs(key!.GetHashCode()) % _capacity;
    }

    public void Put(TKey key, TValue value)
    {
        var index = GetIndex(key);

        for (int i = 0; i < _buckets[index].Count; i++)
        {
            if (EqualityComparer<TKey>.Default.Equals(_buckets[index][i].Key, key))
            {
                _buckets[index][i] = new KeyValuePair<TKey, TValue>(key, value);
                return;
            }
        }

        _buckets[index].Add(new KeyValuePair<TKey, TValue>(key, value));
    }

    public TValue TryGet(TKey key)
    {
        var index = GetIndex(key);

        foreach (var pair in _buckets[index])
        {
            if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
                return pair.Value;
        }

        return default!;
    }

    public bool Remove(TKey key)
    {
        var index = GetIndex(key);

        for (int i = 0; i < _buckets[index].Count; i++)
        {
            if (EqualityComparer<TKey>.Default.Equals(_buckets[index][i].Key, key))
            {
                _buckets[index].RemoveAt(i);
                return true;
            }
        }

        return false;
    }
}