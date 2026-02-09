using DomainService;
using Domain;
using Domain.Transaction;

namespace DomainServicesTests;

[TestFixture]
public class HashingHandlerTests
{
    private HashingHandler _hashingHandler;

    [SetUp]
    public void Setup()
    {
        _hashingHandler = new HashingHandler();
    }

    #region ComputeHash Tests

    [Test]
    public void ComputeHash_WithValidString_ReturnsHexString()
    {
        // Arrange
        string data = "test data";

        // Act
        string hash = _hashingHandler.ComputeHash(data);

        // Assert
        Assert.That(hash, Is.Not.Null);
        Assert.That(hash, Has.Length.EqualTo(8)); // FNV-1a produces 32-bit hash = 8 hex chars
        Assert.That(hash, Does.Match("^[0-9a-f]{8}$")); // Verify it's lowercase hex
    }

    [Test]
    public void ComputeHash_WithEmptyString_ReturnsValidHash()
    {
        // Arrange
        string data = string.Empty;

        // Act
        string hash = _hashingHandler.ComputeHash(data);

        // Assert
        Assert.That(hash, Is.Not.Null);
        Assert.That(hash, Has.Length.EqualTo(8));
    }

    [Test]
    public void ComputeHash_WithSameInput_ReturnsSameHash()
    {
        // Arrange
        string data = "consistent data";

        // Act
        string hash1 = _hashingHandler.ComputeHash(data);
        string hash2 = _hashingHandler.ComputeHash(data);

        // Assert
        Assert.That(hash1, Is.EqualTo(hash2));
    }

    [Test]
    public void ComputeHash_WithDifferentInputs_ReturnsDifferentHashes()
    {
        // Arrange
        string data1 = "data one";
        string data2 = "data two";

        // Act
        string hash1 = _hashingHandler.ComputeHash(data1);
        string hash2 = _hashingHandler.ComputeHash(data2);

        // Assert
        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }

    [Test]
    public void ComputeHash_WithUnicodeCharacters_ReturnsValidHash()
    {
        // Arrange
        string data = "Hello ?? ??";

        // Act
        string hash = _hashingHandler.ComputeHash(data);

        // Assert
        Assert.That(hash, Is.Not.Null);
        Assert.That(hash, Has.Length.EqualTo(8));
    }

    #endregion

    #region VerifyHash Tests

    [Test]
    public void VerifyHash_WithMatchingHash_ReturnsTrue()
    {
        // Arrange
        string data = "verify this";
        string hash = _hashingHandler.ComputeHash(data);

        // Act
        bool result = _hashingHandler.VerifyHash(data, hash);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void VerifyHash_WithNonMatchingHash_ReturnsFalse()
    {
        // Arrange
        string data = "original data";
        string wrongHash = "12345678";

        // Act
        bool result = _hashingHandler.VerifyHash(data, wrongHash);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void VerifyHash_IsCaseInsensitive()
    {
        // Arrange
        string data = "case test";
        string hash = _hashingHandler.ComputeHash(data);
        string upperHash = hash.ToUpper();

        // Act
        bool result = _hashingHandler.VerifyHash(data, upperHash);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void VerifyHash_WithEmptyData_WorksCorrectly()
    {
        // Arrange
        string data = string.Empty;
        string hash = _hashingHandler.ComputeHash(data);

        // Act
        bool result = _hashingHandler.VerifyHash(data, hash);

        // Assert
        Assert.That(result, Is.True);
    }

    #endregion

    #region ComputeTransactionHash Tests

    [Test]
    public void ComputeTransactionHash_WithValidTransaction_ReturnsHash()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.5, 0.001, 250);

        // Act
        string hash = _hashingHandler.ComputeTransactionHash(transaction);

        // Assert
        Assert.That(hash, Is.Not.Null);
        Assert.That(hash, Has.Length.EqualTo(8));
    }

    [Test]
    public void ComputeTransactionHash_WithSameTransaction_ReturnsSameHash()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.5, 0.001, 250);

        // Act
        string hash1 = _hashingHandler.ComputeTransactionHash(transaction);
        string hash2 = _hashingHandler.ComputeTransactionHash(transaction);

        // Assert
        Assert.That(hash1, Is.EqualTo(hash2));
    }

    [Test]
    public void ComputeTransactionHash_WithDifferentTransactions_ReturnsDifferentHashes()
    {
        // Arrange
        var transaction1 = CreateTestTransaction("tx1", 1.5, 0.001, 250);
        var transaction2 = CreateTestTransaction("tx2", 2.5, 0.002, 300);

        // Act
        string hash1 = _hashingHandler.ComputeTransactionHash(transaction1);
        string hash2 = _hashingHandler.ComputeTransactionHash(transaction2);

        // Assert
        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }

    [Test]
    public void ComputeTransactionHash_WithEmptyInputsAndOutputs_ReturnsHash()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Fee = 0.001,
            Size = 100
        };

        // Act
        string hash = _hashingHandler.ComputeTransactionHash(transaction);

        // Assert
        Assert.That(hash, Is.Not.Null);
        Assert.That(hash, Has.Length.EqualTo(8));
    }

    #endregion

    #region ComputeBlockHash Tests

    [Test]
    public void ComputeBlockHash_WithValidBlock_ReturnsHash()
    {
        // Arrange
        var block = CreateTestBlock(4, 12345);

        // Act
        string hash = _hashingHandler.ComputeBlockHash(block);

        // Assert
        Assert.That(hash, Is.Not.Null);
        Assert.That(hash, Has.Length.EqualTo(8));
    }

    [Test]
    public void ComputeBlockHash_WithSameBlock_ReturnsSameHash()
    {
        // Arrange
        var block = CreateTestBlock(4, 12345);

        // Act
        string hash1 = _hashingHandler.ComputeBlockHash(block);
        string hash2 = _hashingHandler.ComputeBlockHash(block);

        // Assert
        Assert.That(hash1, Is.EqualTo(hash2));
    }

    [Test]
    public void ComputeBlockHash_WithDifferentNonce_ReturnsDifferentHash()
    {
        // Arrange
        var block1 = CreateTestBlock(4, 12345);
        var block2 = CreateTestBlock(4, 67890);

        // Act
        string hash1 = _hashingHandler.ComputeBlockHash(block1);
        string hash2 = _hashingHandler.ComputeBlockHash(block2);

        // Assert
        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }

    [Test]
    public void ComputeBlockHash_WithDifferentDifficulty_ReturnsDifferentHash()
    {
        // Arrange
        var block1 = CreateTestBlock(4, 12345);
        var block2 = CreateTestBlock(5, 12345);

        // Act
        string hash1 = _hashingHandler.ComputeBlockHash(block1);
        string hash2 = _hashingHandler.ComputeBlockHash(block2);

        // Assert
        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }

    #endregion

    #region ComputeMerkleRoot Tests

    [Test]
    public void ComputeMerkleRoot_WithNullTransactions_ReturnsHashOfEmptyString()
    {
        // Act
        string merkleRoot = _hashingHandler.ComputeMerkleRoot(null);

        // Assert
        Assert.That(merkleRoot, Is.Not.Null);
        Assert.That(merkleRoot, Is.EqualTo(_hashingHandler.ComputeHash(string.Empty)));
    }

    [Test]
    public void ComputeMerkleRoot_WithEmptyTransactions_ReturnsHashOfEmptyString()
    {
        // Arrange
        var transactions = new List<TransactionEntry>();

        // Act
        string merkleRoot = _hashingHandler.ComputeMerkleRoot(transactions);

        // Assert
        Assert.That(merkleRoot, Is.Not.Null);
        Assert.That(merkleRoot, Is.EqualTo(_hashingHandler.ComputeHash(string.Empty)));
    }

    [Test]
    public void ComputeMerkleRoot_WithSingleTransaction_ReturnsValidRoot()
    {
        // Arrange
        var transactions = new List<TransactionEntry>
        {
            CreateTestTransaction("tx1", 1.5, 0.001, 250)
        };

        // Act
        string merkleRoot = _hashingHandler.ComputeMerkleRoot(transactions);

        // Assert
        Assert.That(merkleRoot, Is.Not.Null);
        Assert.That(merkleRoot, Has.Length.GreaterThan(0));
    }

    [Test]
    public void ComputeMerkleRoot_WithMultipleTransactions_ReturnsValidRoot()
    {
        // Arrange
        var transactions = new List<TransactionEntry>
        {
            CreateTestTransaction("tx1", 1.5, 0.001, 250),
            CreateTestTransaction("tx2", 2.5, 0.002, 300),
            CreateTestTransaction("tx3", 0.5, 0.0005, 200)
        };

        // Act
        string merkleRoot = _hashingHandler.ComputeMerkleRoot(transactions);

        // Assert
        Assert.That(merkleRoot, Is.Not.Null);
        Assert.That(merkleRoot, Has.Length.GreaterThan(0));
    }

    [Test]
    public void ComputeMerkleRoot_WithSameTransactions_ReturnsSameRoot()
    {
        // Arrange
        var transactions = new List<TransactionEntry>
        {
            CreateTestTransaction("tx1", 1.5, 0.001, 250),
            CreateTestTransaction("tx2", 2.5, 0.002, 300)
        };

        // Act
        string root1 = _hashingHandler.ComputeMerkleRoot(transactions);
        string root2 = _hashingHandler.ComputeMerkleRoot(transactions);

        // Assert
        Assert.That(root1, Is.EqualTo(root2));
    }

    [Test]
    public void ComputeMerkleRoot_WithDifferentTransactionOrder_ReturnsDifferentRoot()
    {
        // Arrange
        var transactions1 = new List<TransactionEntry>
        {
            CreateTestTransaction("tx1", 1.5, 0.001, 250),
            CreateTestTransaction("tx2", 2.5, 0.002, 300)
        };

        var transactions2 = new List<TransactionEntry>
        {
            CreateTestTransaction("tx2", 2.5, 0.002, 300),
            CreateTestTransaction("tx1", 1.5, 0.001, 250)
        };

        // Act
        string root1 = _hashingHandler.ComputeMerkleRoot(transactions1);
        string root2 = _hashingHandler.ComputeMerkleRoot(transactions2);

        // Assert
        Assert.That(root1, Is.Not.EqualTo(root2));
    }

    #endregion

    #region Helper Methods

    private TransactionEntry CreateTestTransaction(string id, double value, double fee, int size)
    {
        var transaction = new TransactionEntry(id)
        {
            Fee = fee,
            Size = size
        };

        var input = new Input("prevTx1", 0, "pubKey1", "signature1");
        transaction.Inputs.Add(input);

        var output = new Output(value, "pubKeyOut1");
        transaction.Outputs.Add(output);

        return transaction;
    }

    private Block CreateTestBlock(int difficulty, int nonce)
    {
        var transactions = new List<TransactionEntry>
        {
            CreateTestTransaction("tx1", 1.5, 0.001, 250)
        };

        var block = new Block(difficulty, transactions)
        {
            Nonce = nonce,
            MerkleRoot = "test_merkle_root"
        };

        return block;
    }

    #endregion
}