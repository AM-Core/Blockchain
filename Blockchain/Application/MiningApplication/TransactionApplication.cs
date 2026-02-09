using Domain.Interfaces;
using Domain.Transaction;
using DomainService;

namespace Application.MiningApplication;

public class TransactionApplication
{
    public void AddTransactionToMempool(string filePath,
        ITransactionReader transactionReader, Mempool mempool)
    {
        TransactionEntry transactionEntry = transactionReader.ReadTransaction(filePath);
        mempool.AddTransaction(transactionEntry);
    }
}
