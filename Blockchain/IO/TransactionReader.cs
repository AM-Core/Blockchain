using Domain.Interfaces;
using Domain.Transaction;

namespace IO;

public class TransactionReader : ITransactionReader
{
    public Transaction ReadTransaction(string value)
    {
        throw new NotImplementedException();
    }

    public Transaction ReadTransactionFromJson(string json)
    {
        throw new NotImplementedException();
    }

    public Transaction ReadAllTransactions(List<string> values)
    {
        throw new NotImplementedException();
    }
}