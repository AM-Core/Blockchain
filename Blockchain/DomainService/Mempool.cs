using DataStructures;
using Domain.Interfaces;
using Domain.Transaction;

namespace DomainService;

public class Mempool : IMempool
{
    private readonly Dictionary<string, TransactionEntry> _tDict;
    private readonly DAG _tDag;
    private readonly AVL _priorityTree;
    private readonly AVL _evictionTree;
    public Mempool()
    {
        _tDict = new Dictionary<string, TransactionEntry>();
        _tDag = new DAG();
        _priorityTree = new AVL();
        _evictionTree = new AVL();
    }

    public bool AddTransaction(string transactionId)
    {
        throw new NotImplementedException();
    }

    public bool RemoveTransaction(string transactionId)
    {
        throw new NotImplementedException();
    }

    public bool Exist(string transactionId)
    {
        throw new NotImplementedException();
    }

    public List<TransactionEntry> GetTransactionsByPriority()
    {
        throw new NotImplementedException();
    }
    public List<TransactionEntry> GetTransactionsByEvictionPriority()
    {
        throw new NotImplementedException();
    }
    public void EvictHighestPriorityTransaction()
    {
        throw new NotImplementedException();
    }

    public TransactionEntry GetMaxPriorityTransaction()
    {
        throw new NotImplementedException();
    }

}