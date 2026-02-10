using Domain.Interfaces;
using Domain.Transaction;
using System.Text;
using System.Text.Json;

namespace IO;

public class TransactionReader : ITransactionReader
{
    public TransactionEntry ReadTransaction(string filePath)
    {
        var resolvedPath = ResolveFilePath(filePath);
        
        if (!File.Exists(resolvedPath))
            throw new FileNotFoundException($"Transaction file not found: {resolvedPath}");

        var json = File.ReadAllText(resolvedPath);
        var transaction = JsonSerializer.Deserialize<TransactionEntry>(json);
        
        if (transaction == null || transaction.Id == null)
            throw new InvalidDataException("Failed to deserialize TransactionEntry from file.");
        
        transaction.Size = CalculateSize(json);
        return transaction;
    }

    private string ResolveFilePath(string filePath)
    {
        if (Path.IsPathRooted(filePath))
            return filePath;

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var solutionRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;

        if (solutionRoot == null)
            throw new InvalidOperationException("Could not determine solution root directory");

        return Path.Combine(solutionRoot, filePath);
    }

    private int CalculateSize(string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes.Length;
    }
}