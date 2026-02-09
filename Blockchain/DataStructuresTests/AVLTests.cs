using DataStructures;
using NUnit.Framework;

namespace DataStructuresTests;

[TestFixture]
public class AVLTests
{
    private AVL<int, string> _avl;

    [SetUp]
    public void Setup()
    {
        _avl = new AVL<int, string>();
    }

    #region InsertOne Tests

    [Test]
    public void InsertOne_IntoEmptyTree_InsertsSuccessfully()
    {
        // Act
        _avl.InsertOne(10, "ten");
        var result = _avl.Search(10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.EqualTo("ten"));
    }

    [Test]
    public void InsertOne_MultipleElements_InsertsSuccessfully()
    {
        // Act
        _avl.InsertOne(10, "ten");
        _avl.InsertOne(20, "twenty");
        _avl.InsertOne(30, "thirty");

        // Assert
        Assert.That(_avl.Search(10)?[0], Is.EqualTo("ten"));
        Assert.That(_avl.Search(20)?[0], Is.EqualTo("twenty"));
        Assert.That(_avl.Search(30)?[0], Is.EqualTo("thirty"));
    }

    [Test]
    public void InsertOne_DuplicateKey_UpdatesValue()
    {
        // Arrange
        _avl.InsertOne(10, "ten");

        // Act
        _avl.InsertOne(10, "updated_ten");
        var result = _avl.Search(10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result[0], Is.EqualTo("updated_ten"));
    }

    [Test]
    public void InsertOne_AscendingOrder_MaintainsBalance()
    {
        // Act - Insert in ascending order (worst case for unbalanced BST)
        for (int i = 1; i <= 10; i++)
        {
            _avl.InsertOne(i, $"value_{i}");
        }

        // Assert - All elements should be searchable
        for (int i = 1; i <= 10; i++)
        {
            var result = _avl.Search(i);
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0], Is.EqualTo($"value_{i}"));
        }
    }

    [Test]
    public void InsertOne_DescendingOrder_MaintainsBalance()
    {
        // Act - Insert in descending order
        for (int i = 10; i >= 1; i--)
        {
            _avl.InsertOne(i, $"value_{i}");
        }

        // Assert - All elements should be searchable
        for (int i = 1; i <= 10; i++)
        {
            var result = _avl.Search(i);
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0], Is.EqualTo($"value_{i}"));
        }
    }

    [Test]
    public void InsertOne_RandomOrder_MaintainsBalance()
    {
        // Arrange
        int[] keys = { 50, 25, 75, 10, 30, 60, 80, 5, 15, 27, 55 };

        // Act
        foreach (var key in keys)
        {
            _avl.InsertOne(key, $"value_{key}");
        }

        // Assert
        foreach (var key in keys)
        {
            var result = _avl.Search(key);
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0], Is.EqualTo($"value_{key}"));
        }
    }

    [Test]
    public void InsertOne_WithStringKeys_WorksCorrectly()
    {
        // Arrange
        var avlString = new AVL<string, int>();

        // Act
        avlString.InsertOne("apple", 1);
        avlString.InsertOne("banana", 2);
        avlString.InsertOne("cherry", 3);

        // Assert
        Assert.That(avlString.Search("apple")?[0], Is.EqualTo(1));
        Assert.That(avlString.Search("banana")?[0], Is.EqualTo(2));
        Assert.That(avlString.Search("cherry")?[0], Is.EqualTo(3));
    }

    #endregion

    #region Search Tests

    [Test]
    public void Search_ExistingKey_ReturnsValue()
    {
        // Arrange
        _avl.InsertOne(10, "ten");

        // Act
        var result = _avl.Search(10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.EqualTo("ten"));
    }

    [Test]
    public void Search_NonExistingKey_ReturnsNull()
    {
        // Arrange
        _avl.InsertOne(10, "ten");

        // Act
        var result = _avl.Search(999);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Search_EmptyTree_ReturnsNull()
    {
        // Act
        var result = _avl.Search(10);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Search_AfterMultipleInserts_FindsAllElements()
    {
        // Arrange
        for (int i = 1; i <= 100; i++)
        {
            _avl.InsertOne(i, $"value_{i}");
        }

        // Act & Assert
        for (int i = 1; i <= 100; i++)
        {
            var result = _avl.Search(i);
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0], Is.EqualTo($"value_{i}"));
        }
    }

    [Test]
    public void Search_AfterUpdate_ReturnsUpdatedValue()
    {
        // Arrange
        _avl.InsertOne(10, "ten");
        _avl.InsertOne(10, "updated_ten");

        // Act
        var result = _avl.Search(10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result[0], Is.EqualTo("updated_ten"));
    }

    #endregion

    #region DeleteOne Tests

    [Test]
    public void DeleteOne_ExistingLeafNode_RemovesSuccessfully()
    {
        // Arrange
        _avl.InsertOne(10, "ten");
        _avl.InsertOne(5, "five");
        _avl.InsertOne(15, "fifteen");

        // Act
        _avl.DeleteOne(5, "five");
        var result = _avl.Search(5);

        // Assert
        Assert.That(result, Is.Null);
        Assert.That(_avl.Search(10), Is.Not.Null);
        Assert.That(_avl.Search(15), Is.Not.Null);
    }

    [Test]
    public void DeleteOne_NonExistingKey_DoesNothing()
    {
        // Arrange
        _avl.InsertOne(10, "ten");

        // Act
        _avl.DeleteOne(999, "nonexistent");
        var result = _avl.Search(10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result[0], Is.EqualTo("ten"));
    }

    [Test]
    public void DeleteOne_WrongValue_DoesNotDelete()
    {
        // Arrange
        _avl.InsertOne(10, "ten");

        // Act
        _avl.DeleteOne(10, "wrong_value");
        var result = _avl.Search(10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result[0], Is.EqualTo("ten"));
    }

    [Test]
    public void DeleteOne_NodeWithOneChild_RemovesSuccessfully()
    {
        // Arrange
        _avl.InsertOne(10, "ten");
        _avl.InsertOne(5, "five");
        _avl.InsertOne(3, "three");

        // Act
        _avl.DeleteOne(5, "five");

        // Assert
        Assert.That(_avl.Search(5), Is.Null);
        Assert.That(_avl.Search(10), Is.Not.Null);
        Assert.That(_avl.Search(3), Is.Not.Null);
    }

    [Test]
    public void DeleteOne_NodeWithTwoChildren_RemovesSuccessfully()
    {
        // Arrange
        _avl.InsertOne(10, "ten");
        _avl.InsertOne(5, "five");
        _avl.InsertOne(15, "fifteen");
        _avl.InsertOne(3, "three");
        _avl.InsertOne(7, "seven");

        // Act
        _avl.DeleteOne(5, "five");

        // Assert
        Assert.That(_avl.Search(5), Is.Null);
        Assert.That(_avl.Search(10), Is.Not.Null);
        Assert.That(_avl.Search(15), Is.Not.Null);
        Assert.That(_avl.Search(3), Is.Not.Null);
        Assert.That(_avl.Search(7), Is.Not.Null);
    }

    [Test]
    public void DeleteOne_RootNode_RemovesSuccessfully()
    {
        // Arrange
        _avl.InsertOne(10, "ten");

        // Act
        _avl.DeleteOne(10, "ten");
        var result = _avl.Search(10);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void DeleteOne_MultipleDeletes_MaintainsTreeIntegrity()
    {
        // Arrange
        for (int i = 1; i <= 10; i++)
        {
            _avl.InsertOne(i, $"value_{i}");
        }

        // Act - Delete odd numbers
        for (int i = 1; i <= 10; i += 2)
        {
            _avl.DeleteOne(i, $"value_{i}");
        }

        // Assert - Even numbers should still exist
        for (int i = 2; i <= 10; i += 2)
        {
            var result = _avl.Search(i);
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0], Is.EqualTo($"value_{i}"));
        }

        // Assert - Odd numbers should be deleted
        for (int i = 1; i <= 10; i += 2)
        {
            Assert.That(_avl.Search(i), Is.Null);
        }
    }

    [Test]
    public void DeleteOne_AllElements_EmptiesTree()
    {
        // Arrange
        _avl.InsertOne(10, "ten");
        _avl.InsertOne(5, "five");
        _avl.InsertOne(15, "fifteen");

        // Act
        _avl.DeleteOne(10, "ten");
        _avl.DeleteOne(5, "five");
        _avl.DeleteOne(15, "fifteen");

        // Assert
        Assert.That(_avl.Search(10), Is.Null);
        Assert.That(_avl.Search(5), Is.Null);
        Assert.That(_avl.Search(15), Is.Null);
    }

    #endregion

    #region Balance Tests

    [Test]
    public void AVL_LeftLeftCase_PerformsRightRotation()
    {
        // Act - This triggers LL case
        _avl.InsertOne(30, "thirty");
        _avl.InsertOne(20, "twenty");
        _avl.InsertOne(10, "ten");

        // Assert - Tree should remain balanced
        Assert.That(_avl.Search(30), Is.Not.Null);
        Assert.That(_avl.Search(20), Is.Not.Null);
        Assert.That(_avl.Search(10), Is.Not.Null);
    }

    [Test]
    public void AVL_RightRightCase_PerformsLeftRotation()
    {
        // Act - This triggers RR case
        _avl.InsertOne(10, "ten");
        _avl.InsertOne(20, "twenty");
        _avl.InsertOne(30, "thirty");

        // Assert - Tree should remain balanced
        Assert.That(_avl.Search(10), Is.Not.Null);
        Assert.That(_avl.Search(20), Is.Not.Null);
        Assert.That(_avl.Search(30), Is.Not.Null);
    }

    [Test]
    public void AVL_LeftRightCase_PerformsDoubleRotation()
    {
        // Act - This triggers LR case
        _avl.InsertOne(30, "thirty");
        _avl.InsertOne(10, "ten");
        _avl.InsertOne(20, "twenty");

        // Assert - Tree should remain balanced
        Assert.That(_avl.Search(30), Is.Not.Null);
        Assert.That(_avl.Search(10), Is.Not.Null);
        Assert.That(_avl.Search(20), Is.Not.Null);
    }

    [Test]
    public void AVL_RightLeftCase_PerformsDoubleRotation()
    {
        // Act - This triggers RL case
        _avl.InsertOne(10, "ten");
        _avl.InsertOne(30, "thirty");
        _avl.InsertOne(20, "twenty");

        // Assert - Tree should remain balanced
        Assert.That(_avl.Search(10), Is.Not.Null);
        Assert.That(_avl.Search(30), Is.Not.Null);
        Assert.That(_avl.Search(20), Is.Not.Null);
    }

    #endregion

    #region Edge Cases

    [Test]
    public void AVL_InsertAndDeleteSameElement_TreeBecomesEmpty()
    {
        // Act
        _avl.InsertOne(10, "ten");
        _avl.DeleteOne(10, "ten");

        // Assert
        Assert.That(_avl.Search(10), Is.Null);
    }

    [Test]
    public void AVL_LargeDataSet_MaintainsPerformance()
    {
        // Arrange & Act
        for (int i = 1; i <= 1000; i++)
        {
            _avl.InsertOne(i, $"value_{i}");
        }

        // Assert - Random searches should all succeed
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int key = random.Next(1, 1001);
            var result = _avl.Search(key);
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0], Is.EqualTo($"value_{key}"));
        }
    }

    [Test]
    public void AVL_InsertDeleteInsert_WorksCorrectly()
    {
        // Act
        _avl.InsertOne(10, "ten");
        _avl.DeleteOne(10, "ten");
        _avl.InsertOne(10, "new_ten");

        // Assert
        var result = _avl.Search(10);
        Assert.That(result, Is.Not.Null);
        Assert.That(result[0], Is.EqualTo("new_ten"));
    }

    [Test]
    public void AVL_NegativeKeys_WorksCorrectly()
    {
        // Act
        _avl.InsertOne(-10, "minus_ten");
        _avl.InsertOne(-5, "minus_five");
        _avl.InsertOne(0, "zero");
        _avl.InsertOne(5, "five");

        // Assert
        Assert.That(_avl.Search(-10)?[0], Is.EqualTo("minus_ten"));
        Assert.That(_avl.Search(-5)?[0], Is.EqualTo("minus_five"));
        Assert.That(_avl.Search(0)?[0], Is.EqualTo("zero"));
        Assert.That(_avl.Search(5)?[0], Is.EqualTo("five"));
    }

    #endregion

    #region Integration Tests

    [Test]
    public void AVL_ComplexScenario_MaintainsCorrectness()
    {
        // Arrange & Act - Complex mix of operations
        _avl.InsertOne(50, "fifty");
        _avl.InsertOne(25, "twenty-five");
        _avl.InsertOne(75, "seventy-five");
        _avl.InsertOne(10, "ten");
        _avl.InsertOne(30, "thirty");
        _avl.InsertOne(60, "sixty");
        _avl.InsertOne(80, "eighty");

        _avl.DeleteOne(10, "ten");
        _avl.InsertOne(15, "fifteen");
        _avl.DeleteOne(75, "seventy-five");
        _avl.InsertOne(75, "new_seventy-five");

        // Assert
        Assert.That(_avl.Search(10), Is.Null);
        Assert.That(_avl.Search(15)?[0], Is.EqualTo("fifteen"));
        Assert.That(_avl.Search(25)?[0], Is.EqualTo("twenty-five"));
        Assert.That(_avl.Search(30)?[0], Is.EqualTo("thirty"));
        Assert.That(_avl.Search(50)?[0], Is.EqualTo("fifty"));
        Assert.That(_avl.Search(60)?[0], Is.EqualTo("sixty"));
        Assert.That(_avl.Search(75)?[0], Is.EqualTo("new_seventy-five"));
        Assert.That(_avl.Search(80)?[0], Is.EqualTo("eighty"));
    }

    [Test]
    public void AVL_StressTest_InsertsAndDeletesManyElements()
    {
        // Arrange
        int count = 500;

        // Act - Insert
        for (int i = 0; i < count; i++)
        {
            _avl.InsertOne(i, $"value_{i}");
        }

        // Act - Delete half
        for (int i = 0; i < count; i += 2)
        {
            _avl.DeleteOne(i, $"value_{i}");
        }

        // Assert - Remaining elements should exist
        for (int i = 1; i < count; i += 2)
        {
            var result = _avl.Search(i);
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0], Is.EqualTo($"value_{i}"));
        }

        // Assert - Deleted elements should not exist
        for (int i = 0; i < count; i += 2)
        {
            Assert.That(_avl.Search(i), Is.Null);
        }
    }

    #endregion
}