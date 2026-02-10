using System.Text.Json;
using Domain.Interfaces;
using Domain.Transaction;

namespace IO;

public class TransactionReader : ITransactionReader
{
    public TransactionEntry ReadTransaction(string filePath)
    {
        var json = File.ReadAllText(filePath);

        var transaction = JsonSerializer.Deserialize<TransactionEntry>(json);

        if (transaction == null || transaction.txid == null)
            throw new InvalidDataException("Failed to deserialize TransactionEntry from file.");

        return transaction;
    }
}