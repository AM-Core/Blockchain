namespace Domain.Interfaces;

public interface IMempool
{
    public bool AddTransaction(string transactionId);

    public bool RemoveTransaction(string transactionId);
    public bool Exist(string transactionId);

    public List<Transaction.Transaction> GetTransactionsByPriority();

    public List<Transaction.Transaction> GetTransactionsByEvictionPriority();

    public void EvictHighestPriorityTransaction();

    public Transaction.Transaction GetMaxPriorityTransaction();
}