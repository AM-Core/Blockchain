using Domain.Interfaces;
using DomainService;

namespace Application.MiningApplication;

public class TransactionApplication
{
    public void AddTransactionToMempool(string filePath,
        ITransactionReader transactionReader, Mempool mempool, IResultWriter resultWriter)
    {
        var transactionEntry = transactionReader.ReadTransaction(filePath);
        mempool.AddTransaction(transactionEntry);
        resultWriter.WriteMempool();
    }
}