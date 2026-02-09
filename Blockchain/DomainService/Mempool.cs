using DataStructures;
using Domain;
using Domain.Interfaces;
using Domain.Transaction;

namespace DomainService;

public class Mempool
{
    private readonly Dictionary<string, TransactionEntry> _dict;
    private readonly DAG<TransactionEntry> _dag;
    private readonly AVL<int, Block> _priorityTree;
    private readonly AVL<int, Block> _evictionTree;
    public Mempool()
    {
        _dict = new Dictionary<string, TransactionEntry>();
        _dag = new DAG<TransactionEntry>();
        _priorityTree = new AVL<int, Block>();
        _evictionTree = new AVL<int, Block>();
    }

    public bool AddTransaction(TransactionEntry transaction)
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

    public List<TransactionEntry> GetTransactionsSortedToCreateBlock()
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
    public void EvictHighestPriorityTransaction(int count)
    {
        throw new NotImplementedException();
    }

    public TransactionEntry GetMaxPriorityTransaction()
    {
        throw new NotImplementedException();
    }

}