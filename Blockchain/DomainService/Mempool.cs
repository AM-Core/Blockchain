using DataStructures;
using Domain;
using Domain.Interfaces;
using Domain.Transaction;

namespace DomainService;

public class Mempool
{
    private readonly HashMap<string, TransactionEntry> _map;
    private readonly DAG<TransactionEntry> _dag;
    private readonly AVL<string, TransactionEntry> _priorityTree;
    private readonly AVL<string, TransactionEntry> _evictionTree;
    private readonly object _lock = new object();
    public Mempool()
    {
        _map = new HashMap<string, TransactionEntry>();
        _dag = new DAG<TransactionEntry>();
        _priorityTree = new AVL<string, TransactionEntry>();
        _evictionTree = new AVL<string, TransactionEntry>();
    }

    public bool AddTransaction(TransactionEntry transaction)
    {
        lock (_lock)
        {
            if (Exist(transaction.Id))
                return false;

            _map.Put(transaction.Id, transaction);
            _dag.AddNode(transaction);
            AddDependencies(transaction);
            double effectiveFee = transaction.Fee + transaction.ParentFee;
            int effectiveSize = transaction.Size + transaction.ParentSize;
            int feeRate = effectiveSize > 0 ? (int)((effectiveFee / effectiveSize) * 1_000_000) : 0;
            string priorityKey = $"{feeRate:D10}_{transaction.Id}";
            string evictionKey = $"{-feeRate:D10}_{transaction.Id}";
            _priorityTree.InsertOne(priorityKey, transaction);
            _evictionTree.InsertOne(evictionKey, transaction);
            return true;
        }
        
    }

    public bool RemoveTransaction(string transactionId)
    {
        lock (_lock)
        {
            var transaction = _map.TryGet(transactionId);
            if (transaction == null)
                return false;

            double effectiveFee = transaction.Fee + transaction.ParentFee;
            int effectiveSize = transaction.Size + transaction.ParentSize;
            int feeRate = effectiveSize > 0 ? (int)((effectiveFee / effectiveSize) * 1_000_000) : 0;
            string priorityKey = $"{feeRate:D10}_{transaction.Id}";
            string evictionKey = $"{-feeRate:D10}_{transaction.Id}";

            _map.Remove(transactionId);
            _dag.RemoveNode(transaction);
            _priorityTree.DeleteOne(priorityKey, transaction);
            _evictionTree.DeleteOne(evictionKey, transaction);

            return true;
        }
        
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
        return ascending ? transactions.OrderBy(tx => tx.Fee).ToList() : transactions.OrderByDescending(tx => tx.Fee).ToList();
    }

    public void EvictHighestPriorityTransaction(int count)
    {
        if (count <= 0)
            return;

        for (int i = 0; i < count; i++)
        {
            var transaction = _evictionTree.GetMin();
            if (transaction == null)
                break;

            RemoveTransaction(transaction.Id);
        }
    }

    private void AddDependencies(TransactionEntry transaction)
    {
        foreach (var input in transaction.Inputs)
        {
            var parentTx = _map.TryGet(input.PrevId);
            if (parentTx != null)
            {
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
    }
    
    public TransactionEntry? GetMaxPriorityTransaction()
    {
        return _priorityTree.GetMax();
    }
    public TransactionEntry? GetTransaction(string transactionId)
    {
        return _map.TryGet(transactionId);
    }
}