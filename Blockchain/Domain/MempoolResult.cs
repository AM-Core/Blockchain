using Domain.Transaction;

namespace Domain;

public class MempoolResult
{
    public List<TransactionEntry> TransactionEntries { get; set; }
     public MempoolResult(List<TransactionEntry> transactionEntries)
    {
        TransactionEntries = transactionEntries;
    }
}