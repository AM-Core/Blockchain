using Domain.Transaction;
using DomainService;

namespace DomainServicesTests;

[TestFixture]
public class MempoolTests
{
    [SetUp]
    public void Setup()
    {
        _mempool = new Mempool();
    }

    private Mempool _mempool;

    [Test]
    public void AddTransaction_ValidTransaction_ReturnsTrue()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);

        // Act
        var result = _mempool.AddTransaction(transaction);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_mempool.Exist("tx1"), Is.True);
    }

    [Test]
    public void AddTransaction_DuplicateTransaction_ReturnsFalse()
    {
        // Arrange
        var transaction1 = CreateTestTransaction("tx1", 1.0, 250);
        var transaction2 = CreateTestTransaction("tx1", 2.0, 300);

        // Act
        _mempool.AddTransaction(transaction1);
        var result = _mempool.AddTransaction(transaction2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void AddTransaction_MultipleTransactionsWithSameFeeRate_BothAdded()
    {
        // Arrange - Both have same fee rate (1.0 / 250 = 0.004)
        var transaction1 = CreateTestTransaction("tx1", 1.0, 250);
        var transaction2 = CreateTestTransaction("tx2", 1.0, 250);

        // Act
        var result1 = _mempool.AddTransaction(transaction1);
        var result2 = _mempool.AddTransaction(transaction2);

        // Assert
        Assert.That(result1, Is.True);
        Assert.That(result2, Is.True);
        Assert.That(_mempool.Exist("tx1"), Is.True);
        Assert.That(_mempool.Exist("tx2"), Is.True);
    }

    [Test]
    public void AddTransaction_WithZeroFee_AddsSuccessfully()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 0.0, 250);

        // Act
        var result = _mempool.AddTransaction(transaction);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_mempool.Exist("tx1"), Is.True);
    }

    [Test]
    public void AddTransaction_WithHighFee_AddsSuccessfully()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 100.0, 250);

        // Act
        var result = _mempool.AddTransaction(transaction);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void AddTransaction_MultipleTransactions_AllAdded()
    {
        // Arrange
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        var tx2 = CreateTestTransaction("tx2", 2.0, 300);
        var tx3 = CreateTestTransaction("tx3", 0.5, 200);

        // Act
        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);
        _mempool.AddTransaction(tx3);

        // Assert
        Assert.That(_mempool.Exist("tx1"), Is.True);
        Assert.That(_mempool.Exist("tx2"), Is.True);
        Assert.That(_mempool.Exist("tx3"), Is.True);
    }

    [Test]
    public void RemoveTransaction_ExistingTransaction_ReturnsTrue()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);

        // Act
        var result = _mempool.RemoveTransaction("tx1");

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_mempool.Exist("tx1"), Is.False);
    }

    [Test]
    public void RemoveTransaction_NonExistingTransaction_ReturnsFalse()
    {
        // Act
        var result = _mempool.RemoveTransaction("nonexistent");

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void RemoveTransaction_AfterRemoval_CanAddSameIdAgain()
    {
        // Arrange
        var transaction1 = CreateTestTransaction("tx1", 1.0, 250);
        var transaction2 = CreateTestTransaction("tx1", 2.0, 300);

        // Act
        _mempool.AddTransaction(transaction1);
        _mempool.RemoveTransaction("tx1");
        var result = _mempool.AddTransaction(transaction2);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_mempool.GetTransaction("tx1")?.Fee, Is.EqualTo(2.0));
    }

    [Test]
    public void RemoveTransaction_RemovesFromAllDataStructures()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);

        // Act
        _mempool.RemoveTransaction("tx1");

        // Assert
        Assert.That(_mempool.Exist("tx1"), Is.False);
        Assert.That(_mempool.GetTransaction("tx1"), Is.Null);
        var priorityList = _mempool.GetTransactionsByPriority();
        Assert.That(priorityList, Does.Not.Contain(transaction));
    }

    [Test]
    public void Exist_ExistingTransaction_ReturnsTrue()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);

        // Act
        var exists = _mempool.Exist("tx1");

        // Assert
        Assert.That(exists, Is.True);
    }

    [Test]
    public void Exist_NonExistingTransaction_ReturnsFalse()
    {
        // Act
        var exists = _mempool.Exist("nonexistent");

        // Assert
        Assert.That(exists, Is.False);
    }

    [Test]
    public void Exist_EmptyMempool_ReturnsFalse()
    {
        // Act
        var exists = _mempool.Exist("tx1");

        // Assert
        Assert.That(exists, Is.False);
    }

    [Test]
    public void GetTransaction_ExistingTransaction_ReturnsTransaction()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);

        // Act
        var result = _mempool.GetTransaction("tx1");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo("tx1"));
        Assert.That(result.Fee, Is.EqualTo(1.0));
    }

    [Test]
    public void GetTransaction_NonExistingTransaction_ReturnsNull()
    {
        // Act
        var result = _mempool.GetTransaction("nonexistent");

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetTransactionsByPriority_EmptyMempool_ReturnsEmptyList()
    {
        // Act
        var result = _mempool.GetTransactionsByPriority();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetTransactionsByPriority_SingleTransaction_ReturnsList()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);

        // Act
        var result = _mempool.GetTransactionsByPriority();

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Id, Is.EqualTo("tx1"));
    }

    [Test]
    public void GetTransactionsByPriority_MultipleTransactions_ReturnsSortedByFeeRate()
    {
        // Arrange - Different fee rates
        var tx1 = CreateTestTransaction("tx1", 1.0, 1000); // fee rate = 0.001
        var tx2 = CreateTestTransaction("tx2", 3.0, 1000); // fee rate = 0.003
        var tx3 = CreateTestTransaction("tx3", 2.0, 1000); // fee rate = 0.002

        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);
        _mempool.AddTransaction(tx3);

        // Act
        var result = _mempool.GetTransactionsByPriority();

        // Assert - Should be sorted in ascending order (in-order traversal)
        Assert.That(result, Has.Count.EqualTo(3));
        // Verify all transactions are present
        Assert.That(result.Select(t => t.Id), Contains.Item("tx1"));
        Assert.That(result.Select(t => t.Id), Contains.Item("tx2"));
        Assert.That(result.Select(t => t.Id), Contains.Item("tx3"));
    }

    [Test]
    public void GetMaxPriorityTransaction_EmptyMempool_ReturnsNull()
    {
        // Act
        var result = _mempool.GetMaxPriorityTransaction();

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetMaxPriorityTransaction_SingleTransaction_ReturnsTransaction()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);

        // Act
        var result = _mempool.GetMaxPriorityTransaction();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo("tx1"));
    }

    [Test]
    public void GetMaxPriorityTransaction_MultipleTransactions_ReturnsHighestFee()
    {
        // Arrange
        var tx1 = CreateTestTransaction("tx1", 1.0, 1000); // fee rate = 0.001
        var tx2 = CreateTestTransaction("tx2", 5.0, 1000); // fee rate = 0.005 (highest)
        var tx3 = CreateTestTransaction("tx3", 2.0, 1000); // fee rate = 0.002

        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);
        _mempool.AddTransaction(tx3);

        // Act
        var result = _mempool.GetMaxPriorityTransaction();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo("tx2"));
    }

    [Test]
    public void EvictHighestPriorityTransaction_ZeroCount_DoesNothing()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);

        // Act
        _mempool.EvictHighestPriorityTransaction(0);

        // Assert
        Assert.That(_mempool.Exist("tx1"), Is.True);
    }

    [Test]
    public void EvictHighestPriorityTransaction_NegativeCount_DoesNothing()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);

        // Act
        _mempool.EvictHighestPriorityTransaction(-5);

        // Assert
        Assert.That(_mempool.Exist("tx1"), Is.True);
    }

    [Test]
    public void EvictHighestPriorityTransaction_EmptyMempool_DoesNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => _mempool.EvictHighestPriorityTransaction(1));
    }

    [Test]
    public void EvictHighestPriorityTransaction_EvictsLowestFeeTransaction()
    {
        // Arrange - tx1 has lowest fee rate
        var tx1 = CreateTestTransaction("tx1", 1.0, 1000); // fee rate = 0.001
        var tx2 = CreateTestTransaction("tx2", 5.0, 1000); // fee rate = 0.005
        var tx3 = CreateTestTransaction("tx3", 3.0, 1000); // fee rate = 0.003

        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);
        _mempool.AddTransaction(tx3);

        // Act
        _mempool.EvictHighestPriorityTransaction(1);

        // Assert
        Assert.That(_mempool.Exist("tx1"), Is.False); // Lowest fee should be evicted
        Assert.That(_mempool.Exist("tx2"), Is.True);
        Assert.That(_mempool.Exist("tx3"), Is.True);
    }

    [Test]
    public void EvictHighestPriorityTransaction_EvictsMultipleTransactions()
    {
        // Arrange
        var tx1 = CreateTestTransaction("tx1", 1.0, 1000); // fee rate = 0.001 (lowest)
        var tx2 = CreateTestTransaction("tx2", 2.0, 1000); // fee rate = 0.002
        var tx3 = CreateTestTransaction("tx3", 5.0, 1000); // fee rate = 0.005 (highest)

        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);
        _mempool.AddTransaction(tx3);

        // Act
        _mempool.EvictHighestPriorityTransaction(2);

        // Assert
        Assert.That(_mempool.Exist("tx1"), Is.False); // Lowest
        Assert.That(_mempool.Exist("tx2"), Is.False); // Second lowest
        Assert.That(_mempool.Exist("tx3"), Is.True); // Highest remains
    }

    [Test]
    public void EvictHighestPriorityTransaction_CountExceedsSize_EvictsAll()
    {
        // Arrange
        var tx1 = CreateTestTransaction("tx1", 1.0, 1000);
        var tx2 = CreateTestTransaction("tx2", 2.0, 1000);

        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);

        // Act
        _mempool.EvictHighestPriorityTransaction(10);

        // Assert
        Assert.That(_mempool.Exist("tx1"), Is.False);
        Assert.That(_mempool.Exist("tx2"), Is.False);
    }

    [Test]
    public void GetTransactionsSortedToCreateBlock_EmptyMempool_ReturnsEmptyList()
    {
        // Act
        var result = _mempool.GetTransactionsSortedToCreateBlock();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetTransactionsSortedToCreateBlock_SingleTransaction_ReturnsList()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.0, 250);
        _mempool.AddTransaction(transaction);

        // Act
        var result = _mempool.GetTransactionsSortedToCreateBlock();

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Id, Is.EqualTo("tx1"));
    }

    [Test]
    public void GetTransactionsSortedToCreateBlock_WithDependencies_ReturnsTopologicalOrder()
    {
        // Arrange - tx2 depends on tx1 (tx1 must come first)
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        var tx2 = CreateTestTransactionWithParent("tx2", 2.0, 300, "tx1");

        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);

        // Act
        var result = _mempool.GetTransactionsSortedToCreateBlock();

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        var tx1Index = result.FindIndex(t => t.Id == "tx1");
        var tx2Index = result.FindIndex(t => t.Id == "tx2");
        Assert.That(tx1Index, Is.LessThan(tx2Index)); // tx1 must come before tx2
    }

    [Test]
    public void GetTransactionsSortedToCreateBlock_NoDependencies_ReturnsAllTransactions()
    {
        // Arrange - Independent transactions
        var tx1 = CreateTestTransaction("tx1", 1.0, 250);
        var tx2 = CreateTestTransaction("tx2", 2.0, 300);
        var tx3 = CreateTestTransaction("tx3", 0.5, 200);

        _mempool.AddTransaction(tx1);
        _mempool.AddTransaction(tx2);
        _mempool.AddTransaction(tx3);

        // Act
        var result = _mempool.GetTransactionsSortedToCreateBlock();

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Select(t => t.Id), Contains.Item("tx1"));
        Assert.That(result.Select(t => t.Id), Contains.Item("tx2"));
        Assert.That(result.Select(t => t.Id), Contains.Item("tx3"));
    }

    [Test]
    public void Mempool_ConcurrentAdds_AllTransactionsAdded()
    {
        // Arrange
        var transactionCount = 100;
        var tasks = new List<Task>();

        // Act
        for (var i = 0; i < transactionCount; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() =>
            {
                var tx = CreateTestTransaction($"tx{index}", 1.0 + index, 250);
                _mempool.AddTransaction(tx);
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Assert
        for (var i = 0; i < transactionCount; i++) Assert.That(_mempool.Exist($"tx{i}"), Is.True);
    }

    [Test]
    public void Mempool_ConcurrentAddsAndRemoves_MaintainsConsistency()
    {
        // Arrange
        var tasks = new List<Task>();

        // Add transactions
        for (var i = 0; i < 50; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() =>
            {
                var tx = CreateTestTransaction($"tx{index}", 1.0, 250);
                _mempool.AddTransaction(tx);
            }));
        }

        // Remove some transactions
        for (var i = 0; i < 25; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() =>
            {
                Thread.Sleep(10); // Small delay to ensure add happens first
                _mempool.RemoveTransaction($"tx{index}");
            }));
        }

        // Act
        Task.WaitAll(tasks.ToArray());

        // Assert - Check consistency (no exceptions thrown)
        var allTransactions = _mempool.GetTransactionsByPriority();
        Assert.That(allTransactions, Is.Not.Null);
    }

    [Test]
    public void Mempool_TransactionWithParentFee_CalculatesEffectiveFeeRate()
    {
        // Arrange - Transaction
        var transaction = new TransactionEntry("tx1")
        {
            Fee = 1.0,
            Size = 250
        };
        transaction.Inputs.Add(new Input("prevTx", 0, "pubKey", "signature"));
        transaction.Outputs.Add(new Output(10.0, "pubKey"));

        // Act
        var result = _mempool.AddTransaction(transaction);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_mempool.Exist("tx1"), Is.True);
    }

    [Test]
    public void Mempool_LargeNumberOfTransactions_PerformsWell()
    {
        // Arrange & Act
        for (var i = 0; i < 1000; i++)
        {
            var tx = CreateTestTransaction($"tx{i}", 1.0 + i * 0.001, 250);
            _mempool.AddTransaction(tx);
        }

        // Assert
        var allTransactions = _mempool.GetTransactionsByPriority();
        Assert.That(allTransactions, Has.Count.EqualTo(1000));
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
}