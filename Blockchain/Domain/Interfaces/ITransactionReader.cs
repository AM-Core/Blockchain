using Domain.Transaction;

namespace Domain.Interfaces;

public interface ITransactionReader
{
    TransactionEntry ReadTransaction(string filePath);
}