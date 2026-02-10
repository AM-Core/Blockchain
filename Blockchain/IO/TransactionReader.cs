using Domain.Interfaces;
using Domain.Transaction;
using System.Text;
using System.Text.Json;

namespace IO;

public class TransactionReader : ITransactionReader
{
    public TransactionEntry ReadTransaction(string filePath)
    {
        // Resolve path relative to project root if not absolute
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
        // If absolute path, return as is
        if (Path.IsPathRooted(filePath))
            return filePath;

        // Navigate from bin/Debug/net8.0 to solution root (4 levels up)
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var solutionRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;

        if (solutionRoot == null)
            throw new InvalidOperationException("Could not determine solution root directory");

        // Combine solution root with relative path
        return Path.Combine(solutionRoot, filePath);
    }

    private int CalculateSize(string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes.Length;
    }
}