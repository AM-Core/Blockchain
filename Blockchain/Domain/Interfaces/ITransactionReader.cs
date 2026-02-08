namespace Domain.Interfaces;

public interface ITransactionReader
{
    Transaction.Transaction ReadTransaction(string value);
    Transaction.Transaction ReadTransactionFromJson(string json);
    Transaction.Transaction ReadAllTransactions(List<string> values);
}