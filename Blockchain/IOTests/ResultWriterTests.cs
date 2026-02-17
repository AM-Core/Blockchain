using Domain;
using Domain.Contracts;
using Domain.Interfaces;
using Domain.Transaction;
using DomainService;
using IO;
using System;
using ConsoleApp.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace IOTests;

[TestFixture]
public class ResultWriterTests
{
    private ServiceProvider _provider;
    private IResultWriter _resultWriter;
    private Mempool _mempool;
    private BlockMiner _blockMiner;
    private MiningConfig _miningConfig;
    private string _testDirectory;
    private string _resultDir;

    [SetUp]
    public void Setup()
    {
        // Build DI container
        _provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();
        
        // Resolve all dependencies from the provider
        _resultWriter = _provider.GetRequiredService<IResultWriter>();
        _mempool = _provider.GetRequiredService<Mempool>();
        _blockMiner = _provider.GetRequiredService<BlockMiner>();
        _miningConfig = _provider.GetRequiredService<MiningConfig>();
        
        _testDirectory = Path.Combine(Path.GetTempPath(), $"ResultWriterTests_{Guid.NewGuid()}");
        _resultDir = Path.Combine(_testDirectory, "Results");
        Directory.CreateDirectory(_resultDir);
        Directory.SetCurrentDirectory(_testDirectory);
    }

    [Test]
    public void WriteBlock_ValidBlock_CreatesJsonFile()
    {
        // Arrange
        var block = CreateTestBlock();
        var blockDto = new BlockDto(block);

        // Act
        var filePath = _resultWriter.WriteBlock(blockDto);

        // Assert
        Assert.That(filePath, Is.Not.Null);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.Contain("Results"));
        Assert.That(filePath, Does.EndWith(".json"));
    }

    [Test]
    public void WriteTransaction_ValidTransaction_CreatesJsonFile()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        var transactionDto = new TransactionDto(transaction.Id);

        // Act
        var filePath = _resultWriter.WriteTransaction(transactionDto);

        // Assert
        Assert.That(filePath, Is.Not.Null);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.Contain("Results"));
        Assert.That(filePath, Does.EndWith(".json"));
    }

    [Test]
    public void WriteMempool_EmptyMempool_CreatesJsonFile()
    {
        // Arrange
        var mempoolDto = new MempoolDto(_mempool.GetAllTransactions());

        // Act
        var filePath = _resultWriter.WriteMempool(mempoolDto, false);

        // Assert
        Assert.That(filePath, Is.Not.Null);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.Contain("Results"));
        Assert.That(filePath, Does.EndWith(".json"));
    }

    [Test]
    public void WriteMempool_WithTransactions_CreatesJsonFile()
    {
        // Arrange
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        var tx2 = CreateTestTransaction("tx2", 2.0, 300);
        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);
        
        var mempoolDto = new MempoolDto(_mempool.GetAllTransactions());

        // Act
        var filePath = _resultWriter.WriteMempool(mempoolDto, false);

        // Assert
        Assert.That(filePath, Is.Not.Null);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.Contain("Results"));
    }

    [Test]
    public void WriteBlock_MultipleCalls_CreatesDifferentFiles()
    {
        // Arrange
        var block1 = CreateTestBlock();
        var block2 = CreateTestBlock();
        var blockDto1 = new BlockDto(block1);
        var blockDto2 = new BlockDto(block2);

        // Act
        var filePath1 = _resultWriter.WriteBlock(blockDto1);
        var filePath2 = _resultWriter.WriteBlock(blockDto2);

        // Assert
        Assert.That(filePath1, Is.EqualTo(filePath2));
        Assert.That(File.Exists(filePath1), Is.True);
        Assert.That(File.Exists(filePath2), Is.True);
    }

    [Test]
    public void WriteMempool_AscendingOrder_CreatesCorrectFile()
    {
        // Arrange
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        var tx2 = CreateTestTransaction("tx2", 2.0, 300);
        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);
        
        var mempoolDto = new MempoolDto(_mempool.GetAllTransactions(true)); // ascending = true

        // Act
        var filePath = _resultWriter.WriteMempool(mempoolDto, true);

        // Assert
        Assert.That(filePath, Is.Not.Null);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.Contain("asc"));
    }

    [Test]
    public void WriteMempool_DescendingOrder_CreatesCorrectFile()
    {
        // Arrange
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        var tx2 = CreateTestTransaction("tx2", 2.0, 300);
        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);
        
        var mempoolDto = new MempoolDto(_mempool.GetAllTransactions(false)); // ascending = false

        // Act
        var filePath = _resultWriter.WriteMempool(mempoolDto, false);

        // Assert
        Assert.That(filePath, Is.Not.Null);
        Assert.That(File.Exists(filePath), Is.True);
        Assert.That(filePath, Does.Contain("desc"));
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

    [TearDown]
    public void TearDown()
    {
        _provider?.Dispose();
        
        try
        {
            if (Directory.Exists(_testDirectory))
                Directory.Delete(_testDirectory, true);
        }
        catch
        {
            // Ignore cleanup errors
        }
    }
}