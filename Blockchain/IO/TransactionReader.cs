using System.IO;
using System.Text.Json;
using Domain.Interfaces;
using Domain.Transaction;

namespace IO;

public class TransactionReader : ITransactionReader
{
    public TransactionEntry ReadTransaction(string filePath)
    {
        string json = File.ReadAllText(filePath);

        var transaction = JsonSerializer.Deserialize<TransactionEntry>(json);

        if (transaction == null || transaction.Id == null)
            throw new InvalidDataException("Failed to deserialize TransactionEntry from file.");

        return transaction;
    }
}