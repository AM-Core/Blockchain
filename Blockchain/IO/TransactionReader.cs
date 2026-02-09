using Domain.Interfaces;
using Domain.Transaction;

namespace IO;

public class TransactionReader : ITransactionReader
{
    public TransactionEntry ReadTransaction(string value)
    {
        throw new NotImplementedException();
    }

    public TransactionEntry ReadTransactionFromJson(string json)
    {
        throw new NotImplementedException();
    }

    public TransactionEntry ReadAllTransactions(List<string> values)
    {
        throw new NotImplementedException();
    }
}