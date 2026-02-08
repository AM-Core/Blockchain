
using DataStructures;
using Domain.Interfaces;
using Domain.Transaction;

namespace DomainService;

public class Mempool : IMempool
{
    private readonly Dictionary<string, Transaction> _tDict = new Dictionary<string, Transaction>();
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

    public List<Transaction> GetTransactionsByPriority()
    {
        throw new NotImplementedException();
    }
    public List<Transaction> GetTransactionsByEvictionPriority()
    {
        throw new NotImplementedException();
    }
    public void EvictHighestPriorityTransaction()
    {
        throw new NotImplementedException();
    }

    public Transaction GetMaxPriorityTransaction()
    {
        throw new NotImplementedException();
    }

}