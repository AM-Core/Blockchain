using Domain.Transaction;

namespace Domain.Interfaces;

public interface ITransactionReader
{
    TransactionEntry ReadTransaction(string filePath);
    TransactionEntry ReadTransactionFromJson(string json);
    TransactionEntry ReadAllTransactions(List<string> values);
}