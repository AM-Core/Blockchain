using Application.MiningApplication;
using Application.QueryHandler;
using Application.QueryHandler.Command;
using Domain;
using Domain.Interfaces;
using Domain.Transaction;
using DomainService;
using Moq;

namespace ApplicationTests;

[TestFixture]
public class ApplicationHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _mockResultWriter = new Mock<IResultWriter>();
        _mockTransactionReader = new Mock<ITransactionReader>();
        _mockQueryParser = new Mock<IQueryParser>();
        _miningConfig = new MiningConfig();

        // Use real instances instead of mocks
        _mempool = new Mempool(_miningConfig);
        _blockMiner = new BlockMiner(_mempool);

        _handler = new ApplicationHandler(
            _mockResultWriter.Object,
            _mockTransactionReader.Object,
            _mockQueryParser.Object,
            _mempool,
            _blockMiner,
            _miningConfig
        );
    }

    private Mock<IResultWriter> _mockResultWriter;
    private Mock<ITransactionReader> _mockTransactionReader;
    private Mock<IQueryParser> _mockQueryParser;
    private Mempool _mempool;
    private BlockMiner _blockMiner;
    private ApplicationHandler _handler;
    private MiningConfig _miningConfig;

    [Test]
    public void Constructor_WithValidDependencies_CreatesInstance()
    {
        // Assert
        Assert.That(_handler, Is.Not.Null);
    }

    [Test]
    public void Handle_SetDifficultyCommand_ParsesAndExecutes()
    {
        // Arrange
        var query = "SetDifficulty 5";
        var command = new Command(CommandType.SETDIFFICULTY, "5");
        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);

        // Act
        _handler.Handle(query);

        // Assert
        _mockQueryParser.Verify(p => p.Parse(query), Times.Once);
    }

    [Test]
    public void Handle_SetDifficultyWithZero_SetsToZero()
    {
        // Arrange
        var query = "SetDifficulty 0";
        var command = new Command(CommandType.SETDIFFICULTY, "0");
        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);

        // Act
        _handler.Handle(query);

        // Assert
        _mockQueryParser.Verify(p => p.Parse(query), Times.Once);
    }

    [Test]
    public void Handle_SetDifficultyWithLargeNumber_SetsCorrectly()
    {
        // Arrange
        var query = "SetDifficulty 100";
        var command = new Command(CommandType.SETDIFFICULTY, "100");
        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);

        // Act
        _handler.Handle(query);

        // Assert
        _mockQueryParser.Verify(p => p.Parse(query), Times.Once);
    }

    [Test]
    public void Handle_SetDifficultyWithInvalidNumber_ThrowsFormatException()
    {
        // Arrange
        var query = "SetDifficulty abc";
        var command = new Command(CommandType.SETDIFFICULTY, "abc");
        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);

        // Act & Assert
        Assert.Throws<FormatException>(() => _handler.Handle(query));
    }

    [Test]
    public void Handle_AddTransactionToMempoolCommand_ParsesAndExecutes()
    {
        // Arrange
        var query = "AddTransactionToMempool tx.json";
        var command = new Command(CommandType.ADDTRANSACTIONTOMEMPOOL, "tx.json");
        var transaction = CreateTestTransaction("tx1", 1.0, 250);

        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);
        _mockTransactionReader.Setup(r => r.ReadTransaction("tx.json")).Returns(transaction);
        _mockResultWriter.Setup(w => w.WriteMempool(It.IsAny<bool>())).Returns("path");

        // Act
        _handler.Handle(query);

        // Assert
        _mockQueryParser.Verify(p => p.Parse(query), Times.Once);
        _mockTransactionReader.Verify(r => r.ReadTransaction("tx.json"), Times.Once);
        Assert.That(_mempool.Exist("tx1"), Is.True);
    }

    [Test]
    public void Handle_AddTransactionWithFilePath_ReadsFromCorrectPath()
    {
        // Arrange
        var filePath = "./transactions/tx123.json";
        var query = $"AddTransactionToMempool {filePath}";
        var command = new Command(CommandType.ADDTRANSACTIONTOMEMPOOL, filePath);
        var transaction = CreateTestTransaction("tx123", 2.5, 300);

        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);
        _mockTransactionReader.Setup(r => r.ReadTransaction(filePath)).Returns(transaction);
        _mockResultWriter.Setup(w => w.WriteMempool(It.IsAny<bool>())).Returns("path");

        // Act
        _handler.Handle(query);

        // Assert
        _mockTransactionReader.Verify(r => r.ReadTransaction(filePath), Times.Once);
        Assert.That(_mempool.Exist("tx123"), Is.True);
    }

    [Test]
    public void Handle_AddTransactionToMempool_WritesResultToFile()
    {
        // Arrange
        var query = "AddTransactionToMempool tx.json";
        var command = new Command(CommandType.ADDTRANSACTIONTOMEMPOOL, "tx.json");
        var transaction = CreateTestTransaction("tx1", 1.0, 250);

        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);
        _mockTransactionReader.Setup(r => r.ReadTransaction("tx.json")).Returns(transaction);
        _mockResultWriter.Setup(w => w.WriteMempool(It.IsAny<bool>())).Returns("result.json");

        // Act
        _handler.Handle(query);

        // Assert
        _mockResultWriter.Verify(w => w.WriteMempool(It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public void Handle_EvictMempoolCommand_ParsesAndExecutes()
    {
        // Arrange
        var query = "EvictMempool 10";
        var command = new Command(CommandType.EVICTMEMPOOL, "10");

        // Add transactions to evict
        for (var i = 0; i < 15; i++) _mempool.AddTransaction(CreateTestTransaction($"tx{i}", 1.0 + i, 250));

        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);
        _mockResultWriter.Setup(w => w.WriteMempool(It.IsAny<bool>())).Returns("path");

        // Act
        _handler.Handle(query);

        // Assert
        _mockQueryParser.Verify(p => p.Parse(query), Times.Once);
        _mockResultWriter.Verify(w => w.WriteMempool(It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public void Handle_EvictMempoolWithZero_DoesNotEvict()
    {
        // Arrange
        var query = "EvictMempool 0";
        var command = new Command(CommandType.EVICTMEMPOOL, "0");
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);

        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);
        _mockResultWriter.Setup(w => w.WriteMempool(It.IsAny<bool>())).Returns("path");

        // Act
        _handler.Handle(query);

        // Assert
        Assert.That(_mempool.Exist("tx1"), Is.True);
        _mockResultWriter.Verify(w => w.WriteMempool(It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public void Handle_EvictMempoolWithInvalidNumber_ThrowsFormatException()
    {
        // Arrange
        var query = "EvictMempool invalid";
        var command = new Command(CommandType.EVICTMEMPOOL, "invalid");
        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);

        // Act & Assert
        Assert.Throws<FormatException>(() => _handler.Handle(query));
    }

    [Test]
    public void Handle_MineBlockCommand_ParsesAndExecutes()
    {
        // Arrange
        var query = "MineBlock";
        var command = new Command(CommandType.MINEBLOCK, "");

        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);
        _mockResultWriter.Setup(w => w.WriteBlock(It.IsAny<Block>())).Returns("block.json");

        // Act
        _handler.Handle(query);

        // Assert
        _mockQueryParser.Verify(p => p.Parse(query), Times.Once);
        _mockResultWriter.Verify(w => w.WriteBlock(It.IsAny<Block>()), Times.Once);
    }

    [Test]
    public void Handle_MineBlock_WritesBlockToFile()
    {
        // Arrange
        var query = "MineBlock";
        var command = new Command(CommandType.MINEBLOCK, "");

        // Add a transaction to the mempool
        _mempool.AddTransaction(CreateTestTransaction("tx1", 1.0, 250));

        _mockQueryParser.Setup(p => p.Parse(query)).Returns(command);
        _mockResultWriter.Setup(w => w.WriteBlock(It.IsAny<Block>())).Returns("block_path.json");

        // Act
        _handler.Handle(query);

        // Assert
        _mockResultWriter.Verify(w => w.WriteBlock(It.IsAny<Block>()), Times.Once);
    }

    [Test]
    public void Handle_AddMultipleTransactions_AddsAll()
    {
        // Arrange
        var command1 = new Command(CommandType.ADDTRANSACTIONTOMEMPOOL, "tx1.json");
        var command2 = new Command(CommandType.ADDTRANSACTIONTOMEMPOOL, "tx2.json");
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        var tx2 = CreateTestTransaction("tx2", 2.0, 300);

        _mockQueryParser.Setup(p => p.Parse("AddTransactionToMempool tx1.json")).Returns(command1);
        _mockQueryParser.Setup(p => p.Parse("AddTransactionToMempool tx2.json")).Returns(command2);
        _mockTransactionReader.Setup(r => r.ReadTransaction("tx1.json")).Returns(tx1);
        _mockTransactionReader.Setup(r => r.ReadTransaction("tx2.json")).Returns(tx2);
        _mockResultWriter.Setup(w => w.WriteMempool(It.IsAny<bool>())).Returns("path");

        // Act
        _handler.Handle("AddTransactionToMempool tx1.json");
        _handler.Handle("AddTransactionToMempool tx2.json");

        // Assert
        _mockTransactionReader.Verify(r => r.ReadTransaction("tx1.json"), Times.Once);
        _mockTransactionReader.Verify(r => r.ReadTransaction("tx2.json"), Times.Once);
        Assert.That(_mempool.Exist("tx1"), Is.True);
        Assert.That(_mempool.Exist("tx2"), Is.True);
    }

    [Test]
    public void Handle_CompleteWorkflow_AddSetDifficultyMine()
    {
        // Arrange
        var addCmd = new Command(CommandType.ADDTRANSACTIONTOMEMPOOL, "tx1.json");
        var setDiffCmd = new Command(CommandType.SETDIFFICULTY, "3");
        var mineCmd = new Command(CommandType.MINEBLOCK, "");

        var tx1 = CreateTestTransaction("tx1", 1.0, 250);

        _mockQueryParser.Setup(p => p.Parse("AddTransactionToMempool tx1.json")).Returns(addCmd);
        _mockQueryParser.Setup(p => p.Parse("SetDifficulty 3")).Returns(setDiffCmd);
        _mockQueryParser.Setup(p => p.Parse("MineBlock")).Returns(mineCmd);

        _mockTransactionReader.Setup(r => r.ReadTransaction("tx1.json")).Returns(tx1);
        _mockResultWriter.Setup(w => w.WriteMempool(It.IsAny<bool>())).Returns("mempool.json");
        _mockResultWriter.Setup(w => w.WriteBlock(It.IsAny<Block>())).Returns("block.json");

        // Act
        _handler.Handle("AddTransactionToMempool tx1.json");
        _handler.Handle("SetDifficulty 3");
        _handler.Handle("MineBlock");

        // Assert
        _mockTransactionReader.Verify(r => r.ReadTransaction(It.IsAny<string>()), Times.Once);
        _mockResultWriter.Verify(w => w.WriteBlock(It.IsAny<Block>()), Times.Once);
        Assert.That(_mempool.Exist("tx1"), Is.True);
    }

    private TransactionEntry CreateTestTransaction(string id, double fee, int size)
    {
        var transaction = new TransactionEntry(id)
        {
            Fee = fee,
            Size = size
        };

        var input = new Input("prevTx1", 0, "pubKey1", "signature1");
        transaction.Inputs.Add(input);

        var output = new Output(10.0, "pubKeyOut1");
        transaction.Outputs.Add(output);

        return transaction;
    }

    private Block CreateTestBlock(long difficulty, int nonce)
    {
        var transactions = new List<TransactionEntry>
        {
            CreateTestTransaction("tx1", 1.0, 250)
        };

        var block = new Block(difficulty, transactions)
        {
            Nonce = nonce,
            MerkleRoot = "test_merkle_root",
            BlockHash = "test_block_hash"
        };

        return block;
    }
}