using Domain.Transaction;

namespace Domain;

public class MempoolResult
{
    public MempoolResult(List<TransactionEntry> transactionEntries)
    {
        TransactionEntries = transactionEntries;
    }

    public List<TransactionEntry> TransactionEntries { get; set; }
}