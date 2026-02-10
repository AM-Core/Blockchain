using DataStructures;
using Domain.Transaction;

namespace DomainService;

public class Mempool
{
    private readonly DAG<TransactionEntry> _dag;
    private readonly AVL<string, TransactionEntry> _evictionTree;
    private readonly object _lock = new();
    private readonly HashMap<string, TransactionEntry> _map;
    private readonly FeeRateCalculator _FeeRateCalculator;
    private readonly AVL<string, TransactionEntry> _priorityTree;

    public Mempool()
    {
        _map = new HashMap<string, TransactionEntry>();
        _dag = new DAG<TransactionEntry>();
        _priorityTree = new AVL<string, TransactionEntry>();
        _evictionTree = new AVL<string, TransactionEntry>();
        _FeeRateCalculator = new FeeRateCalculator();
    }

    public bool AddTransaction(TransactionEntry transaction)
    {
        lock (_lock)
        {
            if (IsValid(transaction))
            {
                if (Exist(transaction.Id))
                    return false;

                _map.Put(transaction.Id, transaction);
                _dag.AddNode(transaction);
                AddDependencies(transaction);

                _FeeRateCalculator.CalculateFee(transaction, _map);
                var feeRate = transaction.Size > 0 ? (int)(transaction.Fee / transaction.Size * 100000) : 0;
                var priorityKey = $"{feeRate:D10}_{transaction.Size}_{transaction.Id}";
                _priorityTree.InsertOne(priorityKey, transaction);
                _evictionTree.InsertOne(priorityKey, transaction);
                return true;
            }
            else
            {
                throw new InvalidValueException("Invalid Value for Outputs !");
            }
        }
            
    }

    public bool RemoveTransaction(string transactionId)
    {
        lock (_lock)
        {
            var transaction = _map.TryGet(transactionId);
            if (transaction == null)
                return false;

            var dependenciesToRemove = _dag.GetDependencies(transaction);

            foreach (var dependentTx in dependenciesToRemove)
            {
                if (dependentTx.Id == transactionId)
                    continue;

                RemoveTransactionInternal(dependentTx);
            }

            RemoveTransactionInternal(transaction);

            return true;
        }
    }
    private void RemoveTransactionInternal(TransactionEntry transaction)
    {
        _FeeRateCalculator.CalculateFee(transaction, _map);
        var feeRate = transaction.Size > 0 ? (int)(transaction.Fee / transaction.Size * 100000) : 0;
        var priorityKey = $"{feeRate:D10}_{transaction.Id}";
        var evictionKey = $"{feeRate:D10}_{transaction.Id}";

        _map.Remove(transaction.Id);
        _dag.RemoveNode(transaction);
        _priorityTree.DeleteOne(priorityKey, transaction);
        _evictionTree.DeleteOne(evictionKey, transaction);
    }
    public bool Exist(string transactionId)
    {
        return _map.TryGet(transactionId) != null;
    }

    public List<TransactionEntry> GetTransactionsSortedToCreateBlock()
    {
        try
        {
            return _dag.TopologicalSort();
        }
        catch (InvalidOperationException)
        {
            return new List<TransactionEntry>();
        }
    }

    public List<TransactionEntry> GetTransactionsByPriority()
    {
        return _priorityTree.GetValues();
    }

    public List<TransactionEntry> GetTransactionsByEvictionPriority()
    {
        return _evictionTree.GetValues();
    }

    public List<TransactionEntry> GetAllTransactions(bool ascending = false)
    {
        var transactions = _map.GetValues();
        return ascending
            ? transactions.OrderBy
                (tx => tx.Fee).ToList()
            : transactions.OrderByDescending(tx => tx.Fee).ToList();
    }

    public void EvictHighestPriorityTransaction(int count)
    {
        if (count <= 0)
            return;

        for (var i = 0; i < count; i++)
        {
            var transaction = _evictionTree.GetMin();
            if (transaction == null)
                break;

            RemoveTransaction(transaction.Id);
        }
    }

    public TransactionEntry? GetMaxPriorityTransaction()
    {
        return _priorityTree.GetMax();
    }

    public TransactionEntry? GetTransaction(string transactionId)
    {
        return _map.TryGet(transactionId);
    }

    private void AddDependencies(TransactionEntry transaction)
    {
        foreach (var input in transaction.Inputs)
        {
            if (input.PrevId == null)
                continue;
            
            var parentTx = _map.TryGet(input.PrevId);
            if (parentTx != null)
                try
                {
                    _dag.AddEdge(parentTx, transaction);
                }
                catch (InvalidOperationException)
                {
                    _dag.RemoveNode(transaction);
                    _map.Remove(transaction.Id);
                    return;
                }
        }
    }

    private bool IsValid(TransactionEntry transaction)
    {
        foreach (var output in transaction.Outputs)
        {
            if (output.Value == Double.PositiveInfinity || output.Value < 0)
            {
                return false;
            }
            
        }

        return true;
    }
}