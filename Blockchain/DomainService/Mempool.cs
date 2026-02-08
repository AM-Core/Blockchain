
using DataStructures;
using Domain.Transaction;

namespace DomainService;

public class Mempool
{
    private readonly Dictionary<string, Transaction> _tDict = new Dictionary<string, Transaction>();
    private readonly DAG<Transaction> _tDag = new DAG<Transaction>();
    private readonly AVL _priorityTree = new AVL();
    private readonly AVL _evictionTree = new AVL();
    public bool AddTransaction(string transactionId)
    {
        //need to be complete
        return false;
    }

    public bool RemoveTransaction(string transactionId)
    {
        //need to be complete
        return false;
    }

    public bool Exist(string transactionId)
    {
        //need to be complete
        return false;
    }

    public List<Transaction> GetTransactionsByPriority()
    {
        return new List<Transaction>();
    }
    public List<Transaction> GetTransactionsByEvictionPriority()
    {
        return new List<Transaction>();
    }
    public void EvictHighestPriorityTransaction()
    {
        
    }

    public Transaction GetMaxPriorityTransaction()
    {
        return new Transaction("0");
    }

}