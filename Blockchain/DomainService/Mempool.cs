
using DataStructures;
using Domain.Interfaces;
using Domain.Transaction;

namespace DomainService;

public class Mempool : IMempool
{
    private readonly Dictionary<string, TransactionEntry> _tDict = new Dictionary<string, TransactionEntry>();
    private readonly DAG _tDag = new DAG();
    private readonly AVL _priorityTree = new AVL();
    private readonly AVL _evictionTree = new AVL();
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