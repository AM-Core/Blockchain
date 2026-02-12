using DataStructures;

namespace DataStructuresTests;

[TestFixture]
public class DAGTests
{
    [SetUp]
    public void Setup()
    {
        _dag = new DAG<string>();
    }

    private DAG<string> _dag;

    [Test]
    public void AddNode_SingleNode_AddsSuccessfully()
    {
        // Act
        _dag.AddNode("A");

        // Assert
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Contains.Item("A"));
        Assert.That(sorted, Has.Count.EqualTo(1));
    }

    [Test]
    public void AddNode_MultipleNodes_AddsAll()
    {
        // Act
        _dag.AddNode("A");
        _dag.AddNode("B");
        _dag.AddNode("C");

        // Assert
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Has.Count.EqualTo(3));
        Assert.That(sorted, Contains.Item("A"));
        Assert.That(sorted, Contains.Item("B"));
        Assert.That(sorted, Contains.Item("C"));
    }

    [Test]
    public void AddNode_DuplicateNode_DoesNotDuplicate()
    {
        // Act
        _dag.AddNode("A");
        _dag.AddNode("A");

        // Assert
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Has.Count.EqualTo(1));
    }

    [Test]
    public void AddNode_EmptyDAG_TopologicalSortReturnsEmpty()
    {
        // Act
        var sorted = _dag.TopologicalSort();

        // Assert
        Assert.That(sorted, Is.Empty);
    }

    [Test]
    public void AddEdge_ValidEdge_AddsSuccessfully()
    {
        // Act
        _dag.AddEdge("A", "B");

        // Assert
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Has.Count.EqualTo(2));
        var indexA = sorted.IndexOf("A");
        var indexB = sorted.IndexOf("B");
        Assert.That(indexA, Is.LessThan(indexB), "A should come before B");
    }

    [Test]
    public void AddEdge_MultipleEdges_CreatesCorrectOrder()
    {
        // Arrange & Act
        // A -> B -> C
        _dag.AddEdge("A", "B");
        _dag.AddEdge("B", "C");

        // Assert
        var sorted = _dag.TopologicalSort();
        var indexA = sorted.IndexOf("A");
        var indexB = sorted.IndexOf("B");
        var indexC = sorted.IndexOf("C");

        Assert.That(indexA, Is.LessThan(indexB));
        Assert.That(indexB, Is.LessThan(indexC));
    }

    [Test]
    public void AddEdge_DiamondPattern_MaintainsCorrectOrder()
    {
        // Arrange & Act
        //     A
        //    / \
        //   B   C
        //    \ /
        //     D
        _dag.AddEdge("A", "B");
        _dag.AddEdge("A", "C");
        _dag.AddEdge("B", "D");
        _dag.AddEdge("C", "D");

        // Assert
        var sorted = _dag.TopologicalSort();
        var indexA = sorted.IndexOf("A");
        var indexB = sorted.IndexOf("B");
        var indexC = sorted.IndexOf("C");
        var indexD = sorted.IndexOf("D");

        Assert.That(indexA, Is.LessThan(indexB));
        Assert.That(indexA, Is.LessThan(indexC));
        Assert.That(indexB, Is.LessThan(indexD));
        Assert.That(indexC, Is.LessThan(indexD));
    }

    [Test]
    public void AddEdge_CreatingCycle_ThrowsInvalidOperationException()
    {
        // Arrange
        _dag.AddEdge("A", "B");
        _dag.AddEdge("B", "C");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _dag.AddEdge("C", "A"));
    }

    [Test]
    public void AddEdge_DirectCycle_ThrowsInvalidOperationException()
    {
        // Arrange
        _dag.AddEdge("A", "B");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _dag.AddEdge("B", "A"));
    }

    [Test]
    public void AddEdge_SelfLoop_ThrowsInvalidOperationException()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _dag.AddEdge("A", "A"));
    }

    [Test]
    public void AddEdge_AfterCycleAttempt_GraphRemainsValid()
    {
        // Arrange
        _dag.AddEdge("A", "B");
        _dag.AddEdge("B", "C");

        try
        {
            _dag.AddEdge("C", "A"); // This should fail
        }
        catch (InvalidOperationException)
        {
            // Expected
        }

        // Assert - Graph should still be valid
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Has.Count.EqualTo(3));
        Assert.That(_dag.HasCycle(), Is.False);
    }

    [Test]
    public void AddEdge_ComplexGraph_MaintainsTopologicalOrder()
    {
        // Arrange & Act
        // A -> B -> D
        // A -> C -> D
        // C -> E
        _dag.AddEdge("A", "B");
        _dag.AddEdge("A", "C");
        _dag.AddEdge("B", "D");
        _dag.AddEdge("C", "D");
        _dag.AddEdge("C", "E");

        // Assert
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Has.Count.EqualTo(5));

        var indexA = sorted.IndexOf("A");
        var indexB = sorted.IndexOf("B");
        var indexC = sorted.IndexOf("C");
        var indexD = sorted.IndexOf("D");
        var indexE = sorted.IndexOf("E");

        Assert.That(indexA, Is.LessThan(indexB));
        Assert.That(indexA, Is.LessThan(indexC));
        Assert.That(indexB, Is.LessThan(indexD));
        Assert.That(indexC, Is.LessThan(indexD));
        Assert.That(indexC, Is.LessThan(indexE));
    }

    [Test]
    public void RemoveNode_ExistingNode_ReturnsTrue()
    {
        // Arrange
        _dag.AddNode("A");

        // Act
        var result = _dag.RemoveNode("A");

        // Assert
        Assert.That(result, Is.True);
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Does.Not.Contain("A"));
    }

    [Test]
    public void RemoveNode_NonExistingNode_ReturnsFalse()
    {
        // Act
        var result = _dag.RemoveNode("A");

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void RemoveNode_NodeWithEdges_RemovesNodeAndEdges()
    {
        // Arrange
        _dag.AddEdge("A", "B");
        _dag.AddEdge("B", "C");

        // Act
        var result = _dag.RemoveNode("B");

        // Assert
        Assert.That(result, Is.True);
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Has.Count.EqualTo(2));
        Assert.That(sorted, Contains.Item("A"));
        Assert.That(sorted, Contains.Item("C"));
        Assert.That(sorted, Does.Not.Contain("B"));
    }

    [Test]
    public void RemoveNode_MiddleNodeInChain_RemainingNodesStillValid()
    {
        // Arrange
        _dag.AddEdge("A", "B");
        _dag.AddEdge("B", "C");
        _dag.AddEdge("C", "D");

        // Act
        _dag.RemoveNode("B");

        // Assert
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Has.Count.EqualTo(3));
        Assert.That(_dag.HasCycle(), Is.False);
    }

    [Test]
    public void RemoveNode_NodeWithMultipleIncomingEdges_RemovesCorrectly()
    {
        // Arrange
        //   A -> C
        //   B -> C
        _dag.AddEdge("A", "C");
        _dag.AddEdge("B", "C");

        // Act
        _dag.RemoveNode("C");

        // Assert
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Has.Count.EqualTo(2));
        Assert.That(sorted, Contains.Item("A"));
        Assert.That(sorted, Contains.Item("B"));
        Assert.That(sorted, Does.Not.Contain("C"));
    }

    [Test]
    public void RemoveNode_AllNodes_ResultsInEmptyGraph()
    {
        // Arrange
        _dag.AddNode("A");
        _dag.AddNode("B");
        _dag.AddNode("C");

        // Act
        _dag.RemoveNode("A");
        _dag.RemoveNode("B");
        _dag.RemoveNode("C");

        // Assert
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Is.Empty);
    }

    [Test]
    public void HasCycle_EmptyGraph_ReturnsFalse()
    {
        // Act
        var hasCycle = _dag.HasCycle();

        // Assert
        Assert.That(hasCycle, Is.False);
    }

    [Test]
    public void HasCycle_SingleNode_ReturnsFalse()
    {
        // Arrange
        _dag.AddNode("A");

        // Act
        var hasCycle = _dag.HasCycle();

        // Assert
        Assert.That(hasCycle, Is.False);
    }

    [Test]
    public void HasCycle_LinearGraph_ReturnsFalse()
    {
        // Arrange
        _dag.AddEdge("A", "B");
        _dag.AddEdge("B", "C");

        // Act
        var hasCycle = _dag.HasCycle();

        // Assert
        Assert.That(hasCycle, Is.False);
    }

    [Test]
    public void HasCycle_DiamondPattern_ReturnsFalse()
    {
        // Arrange
        _dag.AddEdge("A", "B");
        _dag.AddEdge("A", "C");
        _dag.AddEdge("B", "D");
        _dag.AddEdge("C", "D");

        // Act
        var hasCycle = _dag.HasCycle();

        // Assert
        Assert.That(hasCycle, Is.False);
    }

    [Test]
    public void TopologicalSort_EmptyGraph_ReturnsEmptyList()
    {
        // Act
        var sorted = _dag.TopologicalSort();

        // Assert
        Assert.That(sorted, Is.Empty);
    }

    [Test]
    public void TopologicalSort_SingleNode_ReturnsSingleItem()
    {
        // Arrange
        _dag.AddNode("A");

        // Act
        var sorted = _dag.TopologicalSort();

        // Assert
        Assert.That(sorted, Has.Count.EqualTo(1));
        Assert.That(sorted[0], Is.EqualTo("A"));
    }

    [Test]
    public void TopologicalSort_LinearChain_ReturnsCorrectOrder()
    {
        // Arrange
        _dag.AddEdge("A", "B");
        _dag.AddEdge("B", "C");
        _dag.AddEdge("C", "D");

        // Act
        var sorted = _dag.TopologicalSort();

        // Assert
        Assert.That(sorted, Is.EqualTo(new[] { "A", "B", "C", "D" }));
    }

    [Test]
    public void TopologicalSort_IndependentNodes_ReturnsAllNodes()
    {
        // Arrange
        _dag.AddNode("A");
        _dag.AddNode("B");
        _dag.AddNode("C");

        // Act
        var sorted = _dag.TopologicalSort();

        // Assert
        Assert.That(sorted, Has.Count.EqualTo(3));
        Assert.That(sorted, Contains.Item("A"));
        Assert.That(sorted, Contains.Item("B"));
        Assert.That(sorted, Contains.Item("C"));
    }

    [Test]
    public void TopologicalSort_ComplexDAG_MaintainsDependencies()
    {
        // Arrange
        // A -> C
        // B -> C
        // C -> D
        _dag.AddEdge("A", "C");
        _dag.AddEdge("B", "C");
        _dag.AddEdge("C", "D");

        // Act
        var sorted = _dag.TopologicalSort();

        // Assert
        var indexA = sorted.IndexOf("A");
        var indexB = sorted.IndexOf("B");
        var indexC = sorted.IndexOf("C");
        var indexD = sorted.IndexOf("D");

        Assert.That(indexA, Is.LessThan(indexC));
        Assert.That(indexB, Is.LessThan(indexC));
        Assert.That(indexC, Is.LessThan(indexD));
    }

    [Test]
    public void GetDependencies_NodeWithNoDependencies_ReturnsOnlyNode()
    {
        // Arrange
        _dag.AddNode("A");

        // Act
        var dependencies = _dag.GetDependencies("A");

        // Assert
        Assert.That(dependencies, Has.Count.EqualTo(1));
        Assert.That(dependencies[0], Is.EqualTo("A"));
    }

    [Test]
    public void GetDependencies_NodeWithSingleDependency_ReturnsBoth()
    {
        // Arrange
        _dag.AddEdge("A", "B");

        // Act
        var dependencies = _dag.GetDependencies("A");

        // Assert
        Assert.That(dependencies, Has.Count.EqualTo(2));
        Assert.That(dependencies, Contains.Item("A"));
        Assert.That(dependencies, Contains.Item("B"));
    }

    [Test]
    public void GetDependencies_LinearChain_ReturnsAllDependencies()
    {
        // Arrange
        _dag.AddEdge("A", "B");
        _dag.AddEdge("B", "C");
        _dag.AddEdge("C", "D");

        // Act
        var dependencies = _dag.GetDependencies("A");

        // Assert
        Assert.That(dependencies, Has.Count.EqualTo(4));
        Assert.That(dependencies, Contains.Item("A"));
        Assert.That(dependencies, Contains.Item("B"));
        Assert.That(dependencies, Contains.Item("C"));
        Assert.That(dependencies, Contains.Item("D"));
    }

    [Test]
    public void GetDependencies_DiamondPattern_ReturnsAllDependencies()
    {
        // Arrange
        _dag.AddEdge("A", "B");
        _dag.AddEdge("A", "C");
        _dag.AddEdge("B", "D");
        _dag.AddEdge("C", "D");

        // Act
        var dependencies = _dag.GetDependencies("A");

        // Assert
        Assert.That(dependencies, Has.Count.EqualTo(4));
        Assert.That(dependencies, Contains.Item("A"));
        Assert.That(dependencies, Contains.Item("B"));
        Assert.That(dependencies, Contains.Item("C"));
        Assert.That(dependencies, Contains.Item("D"));
    }

    [Test]
    public void GetDependencies_LeafNode_ReturnsOnlyItself()
    {
        // Arrange
        _dag.AddEdge("A", "B");
        _dag.AddEdge("B", "C");

        // Act
        var dependencies = _dag.GetDependencies("C");

        // Assert
        Assert.That(dependencies, Has.Count.EqualTo(1));
        Assert.That(dependencies[0], Is.EqualTo("C"));
    }

    [Test]
    public void DAG_LargeGraph_HandlesCorrectly()
    {
        // Arrange
        for (var i = 0; i < 100; i++) _dag.AddNode($"Node{i}");

        // Create some edges
        for (var i = 0; i < 50; i++) _dag.AddEdge($"Node{i}", $"Node{i + 50}");

        // Act
        var sorted = _dag.TopologicalSort();

        // Assert
        Assert.That(sorted, Has.Count.EqualTo(100));
        Assert.That(_dag.HasCycle(), Is.False);
    }

    [Test]
    public void DAG_AddRemoveSequence_MaintainsConsistency()
    {
        // Arrange & Act
        _dag.AddEdge("A", "B");
        _dag.AddEdge("B", "C");
        _dag.RemoveNode("B");
        _dag.AddNode("D");
        _dag.AddEdge("A", "D");

        // Assert
        var sorted = _dag.TopologicalSort();
        Assert.That(sorted, Has.Count.EqualTo(3));
        Assert.That(_dag.HasCycle(), Is.False);
    }

    [Test]
    public void DAG_WithIntegerType_WorksCorrectly()
    {
        // Arrange
        var intDag = new DAG<int>();
        intDag.AddEdge(1, 2);
        intDag.AddEdge(2, 3);

        // Act
        var sorted = intDag.TopologicalSort();

        // Assert
        Assert.That(sorted, Is.EqualTo(new[] { 1, 2, 3 }));
    }

    [Test]
    public void DAG_WithCustomObjectType_WorksCorrectly()
    {
        // Arrange
        var obj1 = new TestObject { Id = 1 };
        var obj2 = new TestObject { Id = 2 };
        var obj3 = new TestObject { Id = 3 };

        var objDag = new DAG<TestObject>();
        objDag.AddEdge(obj1, obj2);
        objDag.AddEdge(obj2, obj3);

        // Act
        var sorted = objDag.TopologicalSort();

        // Assert
        Assert.That(sorted, Has.Count.EqualTo(3));
        Assert.That(sorted[0], Is.SameAs(obj1));
        Assert.That(sorted[1], Is.SameAs(obj2));
        Assert.That(sorted[2], Is.SameAs(obj3));
    }

    private class TestObject
    {
        public int Id { get; set; }
    }
}