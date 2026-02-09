using System.IO;
using System.Text.Json;
using Domain;
using Domain.Transaction;
using DomainService;
using IO;

namespace IOTests;

[TestFixture]
public class ResultWriterTests
{
    private ResultWriter _writer;
    private Mempool _mempool;
    private string _testDirectory;
    private string _resultDir;

    [SetUp]
    public void Setup()
    {
        _mempool = new Mempool();
        _writer = new ResultWriter(_mempool);
        _testDirectory = Path.Combine(Path.GetTempPath(), $"ResultWriterTests_{Guid.NewGuid()}");
        _resultDir = Path.Combine(_testDirectory, "result");
        Directory.CreateDirectory(_resultDir);
        Directory.SetCurrentDirectory(_testDirectory);
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            if (Directory.Exists(_testDirectory))
                Directory.Delete(_testDirectory, true);
        }
        catch { }
    }

    #region WriteBlock Tests

    [Test]
    public void WriteBlock_ValidBlock_CreatesJsonFile()
    {
        var block = CreateTestBlock();
        string filePath = _writer.WriteBlock(block);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.StartWith("Result"));
        Assert.That(filePath, Does.EndWith(".json"));
    }

    [Test]
    public void WriteBlock_WritesCorrectContent()
    {
        var block = CreateTestBlock();
        string filePath = _writer.WriteBlock(block);
        string json = File.ReadAllText(filePath);
        var deserialized = JsonSerializer.Deserialize<Block>(json);
        Assert.That(deserialized, Is.Not.Null);
        Assert.That(deserialized.Difficulty, Is.EqualTo(block.Difficulty));
    }

    #endregion

    #region WriteTransaction Tests

    [Test]
    public void WriteTransaction_ValidTransaction_CreatesJsonFile()
    {
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        string filePath = _writer.WriteTransaction(transaction);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.StartWith("Result"));
    }

    [Test]
    public void WriteTransaction_WritesCorrectContent()
    {
        var transaction = CreateTestTransaction("tx1", 1.5, 300);
        string filePath = _writer.WriteTransaction(transaction);
        string json = File.ReadAllText(filePath);
        var deserialized = JsonSerializer.Deserialize<TransactionEntry>(json);
        Assert.That(deserialized.Id, Is.EqualTo("tx1"));
    }

    #endregion

    #region WriteMempool Tests

    [Test]
    public void WriteMempool_EmptyMempool_CreatesJsonFile()
    {
        string filePath = _writer.WriteMempool();
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.StartWith("Result"));
    }

    [Test]
    public void WriteMempool_WithTransactions_WritesAll()
    {
        _mempool.AddTransaction(CreateTestTransaction("tx1", 1.0, 250));
        _mempool.AddTransaction(CreateTestTransaction("tx2", 2.0, 300));
        string filePath = _writer.WriteMempool();
        string json = File.ReadAllText(filePath);
        var deserialized = JsonSerializer.Deserialize<MempoolResult>(json);
        Assert.That(deserialized.TransactionEntries.Count, Is.EqualTo(2));
    }

    #endregion

    private Block CreateTestBlock()
    {
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        return new Block(4, new List<TransactionEntry> { tx1 })
        {
            MerkleRoot = "testRoot",
            Nonce = 12345
        };
    }

    private TransactionEntry CreateTestTransaction(string id, double fee, int size)
    {
        var transaction = new TransactionEntry(id) { Fee = fee, Size = size };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(10.0, "pubKeyOut1"));
        return transaction;
    }
}
