using Domain.Transaction;

namespace Domain.Interfaces;

public interface ITransactionReader
{
    TransactionEntry ReadTransaction(string value);
    TransactionEntry ReadTransactionFromJson(string json);
    TransactionEntry ReadAllTransactions(List<string> values);
}