using Domain.Interfaces;
using Domain.Transaction;
using System.Text;
using System.Text.Json;

namespace IO;

public class TransactionReader : ITransactionReader
{
    public TransactionEntry ReadTransaction(string filePath)
    {
        
        try
        {
            var json = File.ReadAllText(filePath);
            var transaction = JsonSerializer.Deserialize<TransactionEntry>(json);
            if (transaction == null || transaction.Id == null)
                throw new InvalidDataException("Failed to deserialize TransactionEntry from file.");
            transaction.Size = CalculateSize(json);
            return transaction;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
        
    }
    private int CalculateSize(string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes.Length;
    }
}