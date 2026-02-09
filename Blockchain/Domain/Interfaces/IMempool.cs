using Domain.Transaction;

namespace Domain.Interfaces;

public interface IMempool
{
    public bool AddTransaction(TransactionEntry transaction);

    public bool RemoveTransaction(string transactionId);
    public bool Exist(string transactionId);

    public List<TransactionEntry> GetTransactionsSortedToCreateBlock();
    
    public List<TransactionEntry> GetTransactionsByPriority();

    public List<TransactionEntry> GetTransactionsByEvictionPriority();

    public void EvictHighestPriorityTransaction(int count);

    public TransactionEntry GetMaxPriorityTransaction();
}