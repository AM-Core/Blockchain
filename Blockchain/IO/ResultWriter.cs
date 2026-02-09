using System.Text.Json;
using System.IO;
using Domain;
using Domain.Interfaces;
using Domain.Transaction;
using DomainService;

namespace IO;

public class ResultWriter : IResultWriter
{
    private readonly Mempool _mempool;
    private readonly string _resultDir = "Result";

    public ResultWriter(Mempool mempool)
    {
        _mempool = mempool;
        Directory.CreateDirectory(_resultDir);
    }

    public string WriteBlock(Block block)
    {
        string json = JsonSerializer.Serialize(block, new JsonSerializerOptions { WriteIndented = true });
        string filePath = Path.Combine(_resultDir, $"block_{block.GetHashCode()}.json");
        File.WriteAllText(filePath, json);

        return filePath;
    }

    public string WriteTransaction(TransactionEntry transactionEntry)
    {
        string json = JsonSerializer.Serialize(transactionEntry, new JsonSerializerOptions { WriteIndented = true });
        string filePath = Path.Combine(_resultDir, $"transaction_{transactionEntry.GetHashCode()}.json");
        File.WriteAllText(filePath, json);

        return filePath;
    }

    public string WriteMempool(bool ascending = false)
    {
        var mempoolResult = new MempoolResult(_mempool.GetAllTransactions(ascending));
        string json = JsonSerializer.Serialize(mempoolResult, new JsonSerializerOptions { WriteIndented = true });
        string filePath = Path.Combine(_resultDir, $"mempool_{(ascending ? "asc" : "desc")}.json");
        File.WriteAllText(filePath, json);

        return filePath;
    }
}