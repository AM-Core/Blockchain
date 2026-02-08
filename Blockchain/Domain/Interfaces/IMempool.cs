using Domain.Transaction;

namespace Domain.Interfaces;

public interface IMempool
{
    public bool AddTransaction(string transactionId);

    public bool RemoveTransaction(string transactionId);
    public bool Exist(string transactionId);

    public List<TransactionEntry> GetTransactionsByPriority();

    public List<TransactionEntry> GetTransactionsByEvictionPriority();

    public void EvictHighestPriorityTransaction();

    public TransactionEntry GetMaxPriorityTransaction();
}