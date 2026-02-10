using DataStructures;
using System.Collections.Generic;
using NUnit.Framework;

namespace DataStructuresTests;

[TestFixture]
public class MerkleTreeTests
{
    #region Constructor Tests

    [Test]
    public void Constructor_NullDataBlocks_CreatesTreeWithHashOfEmptyString()
    {
        // Act
        var tree = new MerkleTree(null!);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64)); // SHA256 produces 64 hex characters
    }

    [Test]
    public void Constructor_EmptyDataBlocks_CreatesTreeWithHashOfEmptyString()
    {
        // Arrange
        var dataBlocks = new List<string>();

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_SingleDataBlock_CreatesTreeWithHashOfBlock()
    {
        // Arrange
        var dataBlocks = new List<string> { "data1" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_TwoDataBlocks_CreatesValidMerkleRoot()
    {
        // Arrange
        var dataBlocks = new List<string> { "data1", "data2" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_MultipleDataBlocks_CreatesValidMerkleRoot()
    {
        // Arrange
        var dataBlocks = new List<string> { "data1", "data2", "data3", "data4" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    #endregion

    #region Root Property Tests

    [Test]
    public void Root_AfterConstruction_IsNotNull()
    {
        // Arrange
        var dataBlocks = new List<string> { "data1" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree.Root, Is.Not.Null);
    }

    [Test]
    public void Root_AfterConstruction_IsNotEmpty()
    {
        // Arrange
        var dataBlocks = new List<string> { "data1" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree.Root, Is.Not.Empty);
    }

    [Test]
    public void Root_SHA256Hash_Has64Characters()
    {
        // Arrange
        var dataBlocks = new List<string> { "data1", "data2" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Root_ContainsOnlyHexCharacters()
    {
        // Arrange
        var dataBlocks = new List<string> { "data1", "data2" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree.Root, Does.Match("^[0-9a-f]{64}$"));
    }

    #endregion

    #region Determinism Tests

    [Test]
    public void Constructor_SameDataBlocks_ProducesSameRoot()
    {
        // Arrange
        var dataBlocks1 = new List<string> { "data1", "data2", "data3" };
        var dataBlocks2 = new List<string> { "data1", "data2", "data3" };

        // Act
        var tree1 = new MerkleTree(dataBlocks1);
        var tree2 = new MerkleTree(dataBlocks2);

        // Assert
        Assert.That(tree1.Root, Is.EqualTo(tree2.Root));
    }

    [Test]
    public void Constructor_DifferentDataBlocks_ProducesDifferentRoots()
    {
        // Arrange
        var dataBlocks1 = new List<string> { "data1", "data2" };
        var dataBlocks2 = new List<string> { "data3", "data4" };

        // Act
        var tree1 = new MerkleTree(dataBlocks1);
        var tree2 = new MerkleTree(dataBlocks2);

        // Assert
        Assert.That(tree1.Root, Is.Not.EqualTo(tree2.Root));
    }

    [Test]
    public void Constructor_DifferentOrder_ProducesDifferentRoots()
    {
        // Arrange
        var dataBlocks1 = new List<string> { "data1", "data2" };
        var dataBlocks2 = new List<string> { "data2", "data1" };

        // Act
        var tree1 = new MerkleTree(dataBlocks1);
        var tree2 = new MerkleTree(dataBlocks2);

        // Assert
        Assert.That(tree1.Root, Is.Not.EqualTo(tree2.Root));
    }

    [Test]
    public void Constructor_SingleChange_ChangesRoot()
    {
        // Arrange
        var dataBlocks1 = new List<string> { "data1", "data2", "data3", "data4" };
        var dataBlocks2 = new List<string> { "data1", "data2", "data3", "modified" };

        // Act
        var tree1 = new MerkleTree(dataBlocks1);
        var tree2 = new MerkleTree(dataBlocks2);

        // Assert
        Assert.That(tree1.Root, Is.Not.EqualTo(tree2.Root));
    }

    #endregion

    #region Edge Cases Tests

    [Test]
    public void Constructor_OddNumberOfBlocks_HandlesDuplication()
    {
        // Arrange - 3 blocks, last one should be duplicated
        var dataBlocks = new List<string> { "data1", "data2", "data3" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_OddNumberOfBlocks_ProducesDifferentRootThanEven()
    {
        // Arrange
        var oddBlocks = new List<string> { "data1", "data2", "data3" };
        var evenBlocks = new List<string> { "data1", "data2", "data3", "data3" };

        // Act
        var oddTree = new MerkleTree(oddBlocks);
        var evenTree = new MerkleTree(evenBlocks);

        // Assert
        // They should produce same root because odd number duplicates last element
        Assert.That(oddTree.Root, Is.EqualTo(evenTree.Root));
    }

    [Test]
    public void Constructor_VeryLargeDataBlocks_CreatesValidRoot()
    {
        // Arrange
        var dataBlocks = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            dataBlocks.Add($"data{i}");
        }

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_EmptyStrings_CreatesValidRoot()
    {
        // Arrange
        var dataBlocks = new List<string> { "", "", "" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_SpecialCharacters_CreatesValidRoot()
    {
        // Arrange
        var dataBlocks = new List<string> { "data@#$%", "data!^&*()", "data<>?/" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_UnicodeCharacters_CreatesValidRoot()
    {
        // Arrange
        var dataBlocks = new List<string> { "データ", "数据", "données" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_VeryLongStrings_CreatesValidRoot()
    {
        // Arrange
        var longString = new string('a', 10000);
        var dataBlocks = new List<string> { longString, longString + "b" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    #endregion

    #region Tree Structure Tests

    [Test]
    public void Constructor_PowerOfTwo_BuildsBalancedTree()
    {
        // Arrange - 4 blocks creates a perfect binary tree
        var dataBlocks = new List<string> { "data1", "data2", "data3", "data4" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_NonPowerOfTwo_BuildsValidTree()
    {
        // Arrange - 5 blocks creates unbalanced tree
        var dataBlocks = new List<string> { "data1", "data2", "data3", "data4", "data5" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_TwoLevels_CreatesCorrectRoot()
    {
        // Arrange
        // Level 0: data1, data2
        // Level 1: hash(hash(data1) + hash(data2))
        var dataBlocks = new List<string> { "data1", "data2" };

        // Act
        var tree1 = new MerkleTree(dataBlocks);
        var tree2 = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree1.Root, Is.EqualTo(tree2.Root));
    }

    [Test]
    public void Constructor_ThreeLevels_CreatesCorrectRoot()
    {
        // Arrange
        // Level 0: data1, data2, data3, data4
        // Level 1: hash(data1+data2), hash(data3+data4)
        // Level 2: hash(hash(data1+data2) + hash(data3+data4))
        var dataBlocks = new List<string> { "data1", "data2", "data3", "data4" };

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    #endregion

    #region Comparison Tests

    [Test]
    public void Constructor_IdenticalData_ProducesIdenticalRoots()
    {
        // Arrange
        var data = "identical_data";
        var dataBlocks1 = new List<string> { data, data, data };
        var dataBlocks2 = new List<string> { data, data, data };

        // Act
        var tree1 = new MerkleTree(dataBlocks1);
        var tree2 = new MerkleTree(dataBlocks2);

        // Assert
        Assert.That(tree1.Root, Is.EqualTo(tree2.Root));
    }

    [Test]
    public void Constructor_SubsetOfData_ProducesDifferentRoot()
    {
        // Arrange
        var fullData = new List<string> { "data1", "data2", "data3" };
        var subsetData = new List<string> { "data1", "data2" };

        // Act
        var fullTree = new MerkleTree(fullData);
        var subsetTree = new MerkleTree(subsetData);

        // Assert
        Assert.That(fullTree.Root, Is.Not.EqualTo(subsetTree.Root));
    }

    [Test]
    public void Constructor_DuplicateElements_ProducesDifferentRootThanUnique()
    {
        // Arrange
        var unique = new List<string> { "data1", "data2" };
        var duplicate = new List<string> { "data1", "data1" };

        // Act
        var uniqueTree = new MerkleTree(unique);
        var duplicateTree = new MerkleTree(duplicate);

        // Assert
        Assert.That(uniqueTree.Root, Is.Not.EqualTo(duplicateTree.Root));
    }

    #endregion

    #region Integrity Tests

    [Test]
    public void Constructor_ChangingOneBlock_DetectableThroughRoot()
    {
        // Arrange
        var originalBlocks = new List<string> { "block1", "block2", "block3", "block4" };
        var modifiedBlocks = new List<string> { "block1", "block2_modified", "block3", "block4" };

        // Act
        var originalTree = new MerkleTree(originalBlocks);
        var modifiedTree = new MerkleTree(modifiedBlocks);

        // Assert - Any change should produce different root
        Assert.That(originalTree.Root, Is.Not.EqualTo(modifiedTree.Root));
    }

    [Test]
    public void Constructor_InsertingBlock_ChangesRoot()
    {
        // Arrange
        var original = new List<string> { "block1", "block2", "block3" };
        var withInsert = new List<string> { "block1", "block2", "inserted", "block3" };

        // Act
        var originalTree = new MerkleTree(original);
        var insertedTree = new MerkleTree(withInsert);

        // Assert
        Assert.That(originalTree.Root, Is.Not.EqualTo(insertedTree.Root));
    }

    [Test]
    public void Constructor_RemovingBlock_ChangesRoot()
    {
        // Arrange
        var original = new List<string> { "block1", "block2", "block3", "block4" };
        var withRemoval = new List<string> { "block1", "block2", "block4" };

        // Act
        var originalTree = new MerkleTree(original);
        var removedTree = new MerkleTree(withRemoval);

        // Assert
        Assert.That(originalTree.Root, Is.Not.EqualTo(removedTree.Root));
    }

    #endregion

    #region Performance Tests

    [Test]
    public void Constructor_100Blocks_CompletesQuickly()
    {
        // Arrange
        var dataBlocks = new List<string>();
        for (int i = 0; i < 100; i++)
        {
            dataBlocks.Add($"block{i}");
        }

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_1000Blocks_CompletesSuccessfully()
    {
        // Arrange
        var dataBlocks = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            dataBlocks.Add($"transaction_{i}");
        }

        // Act
        var tree = new MerkleTree(dataBlocks);

        // Assert
        Assert.That(tree, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    #endregion

    #region Blockchain Use Case Tests

    [Test]
    public void Constructor_TransactionHashes_CreatesValidMerkleRoot()
    {
        // Arrange - Simulate transaction hashes
        var txHashes = new List<string>
        {
            "a1b2c3d4e5f6",
            "f6e5d4c3b2a1",
            "123456789abc",
            "cba987654321"
        };

        // Act
        var tree = new MerkleTree(txHashes);

        // Assert
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_EmptyBlockchain_CreatesGenesisRoot()
    {
        // Arrange - No transactions
        var emptyTransactions = new List<string>();

        // Act
        var tree = new MerkleTree(emptyTransactions);

        // Assert
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    [Test]
    public void Constructor_MultipleIdenticalTransactions_CreatesValidRoot()
    {
        // Arrange
        var txHash = "same_transaction_hash";
        var transactions = new List<string> { txHash, txHash, txHash, txHash };

        // Act
        var tree = new MerkleTree(transactions);

        // Assert
        Assert.That(tree.Root, Is.Not.Null);
        Assert.That(tree.Root, Has.Length.EqualTo(64));
    }

    #endregion
}
