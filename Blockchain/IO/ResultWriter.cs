using System.Text.Json;
using Domain;
using Domain.Interfaces;
using Domain.Transaction;
using DomainService;

namespace IO;

public class ResultWriter : IResultWriter
{
    private readonly Mempool _mempool;
    private readonly string _resultDir;

    public ResultWriter(Mempool mempool)
    {
        _mempool = mempool;
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var solutionRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;

        if (solutionRoot == null)
            throw new InvalidOperationException("Could not determine solution root directory");

        _resultDir = Path.Combine(solutionRoot, "Results");
        Directory.CreateDirectory(_resultDir);
    }

    public string WriteBlock(Block block)
    {
        var json = JsonSerializer.Serialize(block, new JsonSerializerOptions { WriteIndented = true });
        var filePath = Path.Combine(_resultDir, $"block_{block.GetHashCode()}.json");
        File.WriteAllText(filePath, json);

        return filePath;
    }

    public string WriteTransaction(TransactionEntry transactionEntry)
    {
        var json = JsonSerializer.Serialize(transactionEntry, new JsonSerializerOptions { WriteIndented = true });
        var filePath = Path.Combine(_resultDir, $"transaction_{transactionEntry.GetHashCode()}.json");
        File.WriteAllText(filePath, json);

        return filePath;
    }

    public string WriteMempool(bool ascending = false)
    {
        var mempoolResult = new MempoolResult(_mempool.GetAllTransactions(ascending));
        var json = JsonSerializer.Serialize(mempoolResult, new JsonSerializerOptions { WriteIndented = true });
        var filePath = Path.Combine(_resultDir, $"mempool_{(ascending ? "asc" : "desc")}_{DateTime.Now.ToFileTime()}.json");
        File.WriteAllText(filePath, json);

        return filePath;
    }
}