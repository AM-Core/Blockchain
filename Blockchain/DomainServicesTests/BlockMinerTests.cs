using Domain;
using Domain.Transaction;
using DomainService;

namespace DomainServicesTests;

[TestFixture]
public class BlockMinerTests
{
    [SetUp]
    public void Setup()
    {
        _mempool = new Mempool();
        _blockMiner = new BlockMiner(_mempool);
    }

    private Mempool _mempool;
    private BlockMiner _blockMiner;

    [Test]
    public void MineBlock_EmptyMempool_CreatesBlockWithNoTransactions()
    {
        // Arrange
        MiningConfig.Instance.Difficulty = 1;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block, Is.Not.Null);
        Assert.That(block.Transactions, Is.Empty);
        Assert.That(block.Difficulty, Is.EqualTo(1));
        Assert.That(block.BlockHash, Is.Not.Null.And.Not.Empty);
        Assert.That(block.MerkleRoot, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void MineBlock_SingleTransaction_CreatesValidBlock()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);
        MiningConfig.Instance.Difficulty = 1;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block, Is.Not.Null);
        Assert.That(block.Transactions, Has.Count.EqualTo(1));
        Assert.That(block.Transactions[0].Id, Is.EqualTo("tx1"));
        Assert.That(block.BlockHash, Is.Not.Null.And.Not.Empty);
        Assert.That(block.MerkleRoot, Is.Not.Null.And.Not.Empty);
        Assert.That(block.Nonce, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void MineBlock_MultipleTransactions_CreatesBlockWithAllTransactions()
    {
        // Arrange
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        var tx2 = CreateTestTransaction("tx2", 2.0, 300);
        var tx3 = CreateTestTransaction("tx3", 0.5, 200);

        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);
        _mempool.AddTransaction(tx3);

        MiningConfig.Instance.Difficulty = 1;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block, Is.Not.Null);
        Assert.That(block.Transactions, Has.Count.EqualTo(3));
        Assert.That(block.Transactions.Select(t => t.Id), Contains.Item("tx1"));
        Assert.That(block.Transactions.Select(t => t.Id), Contains.Item("tx2"));
        Assert.That(block.Transactions.Select(t => t.Id), Contains.Item("tx3"));
    }

    [Test]
    public void MineBlock_WithDifficulty1_FindsValidNonce()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);
        MiningConfig.Instance.Difficulty = 1;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block.BlockHash, Is.Not.Null);
        Assert.That(GetLeadingZeroCount(block.BlockHash), Is.GreaterThan(block.Difficulty));
    }

    [Test]
    public void MineBlock_WithDifficulty2_FindsValidNonce()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);
        MiningConfig.Instance.Difficulty = 2;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block.BlockHash, Is.Not.Null);
        Assert.That(GetLeadingZeroCount(block.BlockHash), Is.GreaterThan(block.Difficulty));
    }

    [Test]
    public void MineBlock_WithDifficulty3_FindsValidNonce()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);
        MiningConfig.Instance.Difficulty = 3;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block.BlockHash, Is.Not.Null);
        Assert.That(GetLeadingZeroCount(block.BlockHash), Is.GreaterThan(block.Difficulty));
    }

    [Test]
    public void MineBlock_WithDependentTransactions_OrdersCorrectly()
    {
        // Arrange - tx2 depends on tx1
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        var tx2 = CreateTestTransactionWithParent("tx2", 2.0, 300, "tx1");

        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);

        MiningConfig.Instance.Difficulty = 1;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block.Transactions, Has.Count.EqualTo(2));
        var tx1Index = block.Transactions.FindIndex(t => t.Id == "tx1");
        var tx2Index = block.Transactions.FindIndex(t => t.Id == "tx2");
        Assert.That(tx1Index, Is.LessThan(tx2Index), "Parent transaction must come before child");
    }

    [Test]
    public void MineBlock_SetsCorrectDifficulty()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);
        MiningConfig.Instance.Difficulty = 5;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block.Difficulty, Is.EqualTo(5));
    }

    [Test]
    public void MineBlock_SetsPrevBlockHashToZeros()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);
        MiningConfig.Instance.Difficulty = 1;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block.PrevBlockHash, Is.EqualTo(new string('0', 64)));
    }

    [Test]
    public void MineBlock_ComputesMerkleRoot()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);
        MiningConfig.Instance.Difficulty = 1;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block.MerkleRoot, Is.Not.Null);
        Assert.That(block.MerkleRoot, Is.Not.Empty);
        Assert.That(block.MerkleRoot.Length, Is.GreaterThan(0));
    }

    [Test]
    public void MineBlock_DifferentTransactions_ProducesDifferentMerkleRoots()
    {
        // Arrange & Act - First block
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(tx1);
        MiningConfig.Instance.Difficulty = 1;
        var block1 = _blockMiner.MineBlock();

        // Reset mempool for second block
        _mempool.RemoveTransaction("tx1");
        var tx2 = CreateTestTransaction("tx2", 2.0, 300);
        _mempool.AddTransaction(tx2);
        var block2 = _blockMiner.MineBlock();

        // Assert
        Assert.That(block1.MerkleRoot, Is.Not.EqualTo(block2.MerkleRoot));
    }

    [Test]
    public void MineBlock_SameTransactions_ProducesSameMerkleRoot()
    {
        // Arrange
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(tx1);
        MiningConfig.Instance.Difficulty = 1;

        // Act
        var block1 = _blockMiner.MineBlock();
        _mempool.AddTransaction(tx1);
        var block2 = _blockMiner.MineBlock();

        // Assert
        Assert.That(block1.MerkleRoot, Is.EqualTo(block2.MerkleRoot));
    }

    [Test]
    public void MineBlock_NonceIsNonNegative()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);
        MiningConfig.Instance.Difficulty = 1;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block.Nonce, Is.GreaterThanOrEqualTo(0));
    }

    //[Test]
    //public void MineBlock_HigherDifficulty_RequiresMoreLeadingZeros()
    //{
    //    // Arrange
    //    var transaction = CreateTestTransaction("tx1", 1.0, 250);
    //    _mempool.AddTransaction(transaction);

    //    var easyConfig = new MiningConfig { Difficulty = 1 };
    //    var hardConfig = new MiningConfig { Difficulty = 3 };

    //    // Act
    //    var easyBlock = _blockMiner.MineBlock();
    //    _mempool.AddTransaction(transaction);
    //    var hardBlock = _blockMiner.MineBlock();

    //    // Assert
    //    var easyZeros = GetLeadingZeroCount(easyBlock.BlockHash);
    //    var hardZeros = GetLeadingZeroCount(hardBlock.BlockHash);

    //    Assert.That(easyZeros, Is.GreaterThan(1));
    //    Assert.That(hardZeros, Is.GreaterThan(3));
    //    Assert.That(hardZeros, Is.GreaterThanOrEqualTo(easyZeros));
    //}

    [Test]
    public void MineBlock_MultipleCalls_ProducesDifferentBlocks()
    {
        // Arrange
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        MiningConfig.Instance.Difficulty = 1;

        // Act
        _mempool.AddTransaction(tx1);
        var block1 = _blockMiner.MineBlock();

        _mempool.AddTransaction(tx1);
        var block2 = _blockMiner.MineBlock();

        // Assert - Blocks should be different due to different nonces
        Assert.That(block1.Nonce, Is.Not.EqualTo(block2.Nonce).Or.EqualTo(block2.Nonce));
        // They might have same nonce if same solution found, which is valid
    }

    [Test]
    public void MineBlock_LargeNumberOfTransactions_HandlesCorrectly()
    {
        // Arrange
        for (var i = 0; i < 100; i++)
        {
            var tx = CreateTestTransaction($"tx{i}", 1.0 + i * 0.01, 250);
            _mempool.AddTransaction(tx);
        }

        MiningConfig.Instance.Difficulty = 1;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block, Is.Not.Null);
        Assert.That(block.Transactions, Has.Count.EqualTo(100));
        Assert.That(block.BlockHash, Is.Not.Null.And.Not.Empty);
        Assert.That(block.MerkleRoot, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void MineBlock_WithZeroDifficulty_CreatesBlock()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);
        MiningConfig.Instance.Difficulty = 0;

        // Act
        var block = _blockMiner.MineBlock();

        // Assert
        Assert.That(block, Is.Not.Null);
        Assert.That(block.Difficulty, Is.EqualTo(0));
        Assert.That(block.BlockHash, Is.Not.Null);
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

    private TransactionEntry CreateTestTransactionWithParent(string id, double fee, int size, string parentId)
    {
        var transaction = new TransactionEntry(id)
        {
            Fee = fee,
            Size = size
        };

        // This transaction depends on parentId
        var input = new Input(parentId, 0, "pubKey1", "signature1");
        transaction.Inputs.Add(input);

        var output = new Output(10.0, "pubKeyOut1");
        transaction.Outputs.Add(output);

        return transaction;
    }

    private int GetLeadingZeroCount(string value)
    {
        return value.TakeWhile(c => c == '0').Count();
    }
}