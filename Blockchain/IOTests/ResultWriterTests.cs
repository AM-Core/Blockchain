using Domain;
using Domain.Contracts;
using Domain.Transaction;
using DomainService;
using IO;

namespace IOTests;

[TestFixture]
public class ResultWriterTests
{
    [SetUp]
    public void Setup()
    {
        _mempool = new Mempool();
        _writer = new ResultWriter();
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
        catch
        {
        }
    }

    private ResultWriter _writer;
    private Mempool _mempool;
    private string _testDirectory;
    private string _resultDir;

    [Test]
    public void WriteBlock_ValidBlock_CreatesJsonFile()
    {
        var block = CreateTestBlock();
        var blockDto = new BlockDto(block); // Create BlockDto from Block
        var filePath = _writer.WriteBlock(blockDto);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.Contain("Result"));
        Assert.That(filePath, Does.EndWith(".json"));
    }


    [Test]
    public void WriteTransaction_ValidTransaction_CreatesJsonFile()
    {
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        var transactionDto = new TransactionDto(transaction.Id); // Create TransactionDto
        var filePath = _writer.WriteTransaction(transactionDto);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.Contain("Result"));
    }


    [Test]
    public void WriteMempool_EmptyMempool_CreatesJsonFile()
    {
        var mempoolDto = new MempoolDto(_mempool.GetAllTransactions()); // Create an empty MempoolDto
        var filePath = _writer.WriteMempool(mempoolDto);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.Contain("Result"));
    }


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