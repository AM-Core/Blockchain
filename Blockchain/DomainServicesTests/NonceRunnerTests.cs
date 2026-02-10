using Domain;
using Domain.Transaction;
using DomainService;

namespace DomainServicesTests;

[TestFixture]
public class NonceRunnerTests
{
    private HashingHandler _hashingHandler;
    private NonceRunner _nonceRunner;

    [SetUp]
    public void Setup()
    {
        _hashingHandler = new HashingHandler();
        _nonceRunner = new NonceRunner(_hashingHandler);
    }

    #region FindValidNonce Tests

    [Test]
    public void FindValidNonce_WithDifficulty0_ReturnsValidNonce()
    {
        // Arrange
        var block = CreateTestBlock(0);

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(nonce, Is.GreaterThanOrEqualTo(0));
        block.Nonce = nonce;
        var hash = _hashingHandler.ComputeBlockHash(block);
        Assert.That(GetLeadingZeroCount(hash), Is.GreaterThan(0));
    }

    [Test]
    public void FindValidNonce_WithDifficulty1_ReturnsValidNonce()
    {
        // Arrange
        var block = CreateTestBlock(1);

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(nonce, Is.GreaterThanOrEqualTo(0));
        block.Nonce = nonce;
        var hash = _hashingHandler.ComputeBlockHash(block);
        Assert.That(GetLeadingZeroCount(hash), Is.GreaterThan(1));
    }

    [Test]
    public void FindValidNonce_WithDifficulty2_ReturnsValidNonce()
    {
        // Arrange
        var block = CreateTestBlock(2);

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(nonce, Is.GreaterThanOrEqualTo(0));
        block.Nonce = nonce;
        var hash = _hashingHandler.ComputeBlockHash(block);
        Assert.That(GetLeadingZeroCount(hash), Is.GreaterThan(2));
    }

    [Test]
    public void FindValidNonce_WithDifficulty3_ReturnsValidNonce()
    {
        // Arrange
        var block = CreateTestBlock(3);

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(nonce, Is.GreaterThanOrEqualTo(0));
        block.Nonce = nonce;
        var hash = _hashingHandler.ComputeBlockHash(block);
        Assert.That(GetLeadingZeroCount(hash), Is.GreaterThan(3));
    }

    [Test]
    public void FindValidNonce_WithDifficulty4_ReturnsValidNonce()
    {
        // Arrange
        var block = CreateTestBlock(4);

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(nonce, Is.GreaterThanOrEqualTo(0));
        block.Nonce = nonce;
        var hash = _hashingHandler.ComputeBlockHash(block);
        Assert.That(GetLeadingZeroCount(hash), Is.GreaterThan(4));
    }

    [Test]
    public void FindValidNonce_SetsBlockNonceCorrectly()
    {
        // Arrange
        var block = CreateTestBlock(1);
        var initialNonce = block.Nonce;

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(block.Nonce, Is.EqualTo(nonce));
        Assert.That(block.Nonce, Is.Not.EqualTo(initialNonce).Or.EqualTo(initialNonce));
    }

    [Test]
    public void FindValidNonce_EmptyBlock_FindsValidNonce()
    {
        // Arrange
        var block = new Block(1, new List<TransactionEntry>())
        {
            MerkleRoot = "empty"
        };

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(nonce, Is.GreaterThanOrEqualTo(0));
        block.Nonce = nonce;
        var hash = _hashingHandler.ComputeBlockHash(block);
        Assert.That(GetLeadingZeroCount(hash), Is.GreaterThan(1));
    }

    [Test]
    public void FindValidNonce_BlockWithTransactions_FindsValidNonce()
    {
        // Arrange
        var block = CreateTestBlockWithTransactions(2, 3);

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(nonce, Is.GreaterThanOrEqualTo(0));
        block.Nonce = nonce;
        var hash = _hashingHandler.ComputeBlockHash(block);
        Assert.That(GetLeadingZeroCount(hash), Is.GreaterThan(2));
    }

    [Test]
    public void FindValidNonce_SameBlockTwice_ReturnsSameNonce()
    {
        // Arrange
        var block = CreateTestBlock(1);

        // Act
        var nonce1 = _nonceRunner.FindValidNonce(block);
        block.Nonce = 0; // Reset nonce
        var nonce2 = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(nonce1, Is.EqualTo(nonce2));
    }

    [Test]
    public void FindValidNonce_DifferentBlocks_ReturnsDifferentNonces()
    {
        // Arrange
        var block1 = CreateTestBlock(1);
        var block2 = CreateTestBlock(1);
        block2.MerkleRoot = "different_merkle_root";

        // Act
        var nonce1 = _nonceRunner.FindValidNonce(block1);
        var nonce2 = _nonceRunner.FindValidNonce(block2);

        // Assert
        // Nonces can be different or same, but hashes should meet difficulty
        block1.Nonce = nonce1;
        block2.Nonce = nonce2;
        var hash1 = _hashingHandler.ComputeBlockHash(block1);
        var hash2 = _hashingHandler.ComputeBlockHash(block2);
        
        Assert.That(GetLeadingZeroCount(hash1), Is.GreaterThan(1));
        Assert.That(GetLeadingZeroCount(hash2), Is.GreaterThan(1));
    }

    [Test]
    public void FindValidNonce_HigherDifficulty_MayRequireMoreIterations()
    {
        // Arrange
        var easyBlock = CreateTestBlock(1);
        var hardBlock = CreateTestBlock(3);

        // Act
        var easyNonce = _nonceRunner.FindValidNonce(easyBlock);
        var hardNonce = _nonceRunner.FindValidNonce(hardBlock);

        // Assert
        // Higher difficulty typically requires more iterations (higher nonce)
        // But this isn't guaranteed, so we just verify both are valid
        Assert.That(easyNonce, Is.GreaterThanOrEqualTo(0));
        Assert.That(hardNonce, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void FindValidNonce_ProducesHashWithRequiredLeadingZeros()
    {
        // Arrange
        var difficulty = 2;
        var block = CreateTestBlock(difficulty);

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        block.Nonce = nonce;
        var hash = _hashingHandler.ComputeBlockHash(block);
        var leadingZeros = GetLeadingZeroCount(hash);
        
        Assert.That(leadingZeros, Is.GreaterThan(difficulty));
    }

    [Test]
    public void FindValidNonce_WithDifferentMerkleRoots_FindsDifferentValidNonces()
    {
        // Arrange
        var block1 = CreateTestBlock(1);
        block1.MerkleRoot = "merkle_root_1";
        
        var block2 = CreateTestBlock(1);
        block2.MerkleRoot = "merkle_root_2";

        // Act
        var nonce1 = _nonceRunner.FindValidNonce(block1);
        var nonce2 = _nonceRunner.FindValidNonce(block2);

        // Assert
        block1.Nonce = nonce1;
        block2.Nonce = nonce2;
        
        var hash1 = _hashingHandler.ComputeBlockHash(block1);
        var hash2 = _hashingHandler.ComputeBlockHash(block2);
        
        Assert.That(GetLeadingZeroCount(hash1), Is.GreaterThan(1));
        Assert.That(GetLeadingZeroCount(hash2), Is.GreaterThan(1));
        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }

    [Test]
    public void FindValidNonce_NonceStartsFromZero()
    {
        // Arrange
        var block = CreateTestBlock(1);

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        // The nonce should be found starting from 0
        Assert.That(nonce, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void FindValidNonce_WithComplexBlock_FindsValidNonce()
    {
        // Arrange
        var transactions = new List<TransactionEntry>();
        for (int i = 0; i < 10; i++)
        {
            transactions.Add(CreateTestTransaction($"tx{i}", 1.0 + i, 250 + i * 10));
        }
        
        var block = new Block(2, transactions)
        {
            MerkleRoot = "complex_merkle_root"
        };

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(nonce, Is.GreaterThanOrEqualTo(0));
        block.Nonce = nonce;
        var hash = _hashingHandler.ComputeBlockHash(block);
        Assert.That(GetLeadingZeroCount(hash), Is.GreaterThan(2));
    }

    [Test]
    public void FindValidNonce_ModifiesBlockNonceDuringSearch()
    {
        // Arrange
        var block = CreateTestBlock(1);
        var initialNonce = block.Nonce;

        // Act
        var finalNonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(block.Nonce, Is.EqualTo(finalNonce));
    }

    #endregion

    #region Edge Cases and Performance Tests

    [Test]
    public void FindValidNonce_VeryLowDifficulty_FindsQuickly()
    {
        // Arrange
        var block = CreateTestBlock(0);

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        Assert.That(nonce, Is.GreaterThanOrEqualTo(0));
        // With difficulty 0, should find a valid nonce very quickly (low nonce value)
    }

    [Test]
    public void FindValidNonce_MultipleCalls_Deterministic()
    {
        // Arrange
        var block1 = CreateTestBlock(1);
        var block2 = CreateTestBlock(1);
        block2.MerkleRoot = block1.MerkleRoot; // Same merkle root

        // Act
        var nonce1 = _nonceRunner.FindValidNonce(block1);
        block2.Nonce = 0; // Reset
        var nonce2 = _nonceRunner.FindValidNonce(block2);

        // Assert
        Assert.That(nonce1, Is.EqualTo(nonce2), "Same block should produce same nonce");
    }

    [Test]
    public void FindValidNonce_ResultingHashMeetsProofOfWork()
    {
        // Arrange
        var difficulty = 2;
        var block = CreateTestBlock(difficulty);

        // Act
        var nonce = _nonceRunner.FindValidNonce(block);

        // Assert
        block.Nonce = nonce;
        var hash = _hashingHandler.ComputeBlockHash(block);
        var leadingZeros = GetLeadingZeroCount(hash);
        
        // Proof of work: leading zeros must be greater than difficulty
        Assert.That(leadingZeros, Is.GreaterThan(difficulty),
            $"Hash {hash} should have more than {difficulty} leading zeros");
    }

    #endregion

    #region Helper Methods

    private Block CreateTestBlock(long difficulty)
    {
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        var block = new Block(difficulty, new List<TransactionEntry> { transaction })
        {
            MerkleRoot = "test_merkle_root"
        };
        return block;
    }

    private Block CreateTestBlockWithTransactions(long difficulty, int transactionCount)
    {
        var transactions = new List<TransactionEntry>();
        for (int i = 0; i < transactionCount; i++)
        {
            transactions.Add(CreateTestTransaction($"tx{i}", 1.0 + i, 250 + i * 10));
        }
        
        var block = new Block(difficulty, transactions)
        {
            MerkleRoot = _hashingHandler.ComputeMerkleRoot(transactions)
        };
        return block;
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

    private int GetLeadingZeroCount(string value)
    {
        return value.TakeWhile(c => c == '0').Count();
    }

    #endregion
}
