using DataStructures;
using Domain;
using Domain.Interfaces;
using Domain.Transaction;

namespace DomainService;

public class Mempool : IMempool
{
    private readonly Dictionary<string, TransactionEntry> _dict;
    private readonly DAG _dag;
    private readonly AVL<int, Block> _priorityTree;
    private readonly AVL<int, Block> _evictionTree;
    public Mempool()
    {
        _dict = new Dictionary<string, TransactionEntry>();
        _dag = new DAG();
        _priorityTree = new AVL<int, Block>();
        _evictionTree = new AVL<int, Block>();
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