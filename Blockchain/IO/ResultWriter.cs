using System.Text.Json;
using Domain.Contracts;
using Domain.Interfaces;
using System.IO;
namespace IO;

public class ResultWriter : IResultWriter
{
    private readonly string _resultDir;

    public ResultWriter()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _resultDir = Path.Combine(baseDirectory, "Results");
        Directory.CreateDirectory(_resultDir);
    }

    public string WriteBlock(BlockDto block)
    {
        var json = JsonSerializer.Serialize(block, new JsonSerializerOptions { WriteIndented = true });
        var filePath = Path.Combine(_resultDir, $"block_{block.Header.BlockHash}.json");
        File.WriteAllText(filePath, json);

        return filePath;
    }

    public string WriteTransaction(TransactionDto transaction)
    {
        var json = JsonSerializer.Serialize(transaction, new JsonSerializerOptions { WriteIndented = true });
        var filePath = Path.Combine(_resultDir, $"transaction_{transaction.TxId}.json");
        File.WriteAllText(filePath, json);

        return filePath;
    }

    public string WriteMempool(MempoolDto mempool, bool ascending = false)
    {
        var json = JsonSerializer.Serialize(mempool, new JsonSerializerOptions { WriteIndented = true });
        var filePath = Path.Combine(_resultDir,
            $"mempool_{(ascending ? "asc" : "desc")}_{DateTime.Now.ToFileTime()}.json");
        File.WriteAllText(filePath, json);

        return filePath;
    }
}