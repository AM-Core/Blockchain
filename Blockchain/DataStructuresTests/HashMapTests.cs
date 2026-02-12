using DataStructures;

namespace DataStructuresTests;

[TestFixture]
public class HashMapTests
{
    [SetUp]
    public void Setup()
    {
        _hashMap = new HashMap<string, int>();
    }

    private HashMap<string, int> _hashMap;

    [Test]
    public void Constructor_DefaultCapacity_CreatesHashMap()
    {
        // Act
        var map = new HashMap<string, string>();

        // Assert
        Assert.That(map, Is.Not.Null);
    }

    [Test]
    public void Constructor_CustomCapacity_CreatesHashMap()
    {
        // Act
        var map = new HashMap<string, string>(32);

        // Assert
        Assert.That(map, Is.Not.Null);
    }

    [Test]
    public void Constructor_SmallCapacity_CreatesHashMap()
    {
        // Act
        var map = new HashMap<string, string>(4);

        // Assert
        Assert.That(map, Is.Not.Null);
    }

    [Test]
    public void Put_SingleItem_StoresSuccessfully()
    {
        // Act
        _hashMap.Put("key1", 100);

        // Assert
        var value = _hashMap.TryGet("key1");
        Assert.That(value, Is.EqualTo(100));
    }

    [Test]
    public void Put_MultipleItems_StoresAll()
    {
        // Act
        _hashMap.Put("key1", 100);
        _hashMap.Put("key2", 200);
        _hashMap.Put("key3", 300);

        // Assert
        Assert.That(_hashMap.TryGet("key1"), Is.EqualTo(100));
        Assert.That(_hashMap.TryGet("key2"), Is.EqualTo(200));
        Assert.That(_hashMap.TryGet("key3"), Is.EqualTo(300));
    }

    [Test]
    public void Put_DuplicateKey_UpdatesValue()
    {
        // Arrange
        _hashMap.Put("key1", 100);

        // Act
        _hashMap.Put("key1", 200);

        // Assert
        var value = _hashMap.TryGet("key1");
        Assert.That(value, Is.EqualTo(200));
    }

    [Test]
    public void Put_DuplicateKey_DoesNotIncreaseCount()
    {
        // Arrange
        _hashMap.Put("key1", 100);
        var initialCount = _hashMap.GetValues().Count;

        // Act
        _hashMap.Put("key1", 200);

        // Assert
        var finalCount = _hashMap.GetValues().Count;
        Assert.That(finalCount, Is.EqualTo(initialCount));
    }

    [Test]
    public void Put_NullKey_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _hashMap.Put(null!, 100));
    }

    [Test]
    public void Put_NullValue_StoresNull()
    {
        // Arrange
        var map = new HashMap<string, string>();

        // Act
        map.Put("key1", null!);

        // Assert
        var value = map.TryGet("key1");
        Assert.That(value, Is.Null);
    }

    [Test]
    public void Put_SameHashCodeKeys_StoresAllCorrectly()
    {
        // Arrange - Using custom objects with same hash code
        var map = new HashMap<SameHashObject, int>();
        var obj1 = new SameHashObject("A");
        var obj2 = new SameHashObject("B");

        // Act
        map.Put(obj1, 100);
        map.Put(obj2, 200);

        // Assert
        Assert.That(map.TryGet(obj1), Is.EqualTo(100));
        Assert.That(map.TryGet(obj2), Is.EqualTo(200));
    }

    [Test]
    public void Put_ManyItems_HandlesCollisions()
    {
        // Arrange & Act
        for (var i = 0; i < 100; i++) _hashMap.Put($"key{i}", i);

        // Assert
        for (var i = 0; i < 100; i++) Assert.That(_hashMap.TryGet($"key{i}"), Is.EqualTo(i));
    }

    [Test]
    public void TryGet_ExistingKey_ReturnsValue()
    {
        // Arrange
        _hashMap.Put("key1", 100);

        // Act
        var value = _hashMap.TryGet("key1");

        // Assert
        Assert.That(value, Is.EqualTo(100));
    }

    [Test]
    public void TryGet_NonExistingKey_ReturnsDefault()
    {
        // Act
        var value = _hashMap.TryGet("nonexistent");

        // Assert
        Assert.That(value, Is.EqualTo(default(int)));
    }

    [Test]
    public void TryGet_AfterUpdate_ReturnsNewValue()
    {
        // Arrange
        _hashMap.Put("key1", 100);
        _hashMap.Put("key1", 200);

        // Act
        var value = _hashMap.TryGet("key1");

        // Assert
        Assert.That(value, Is.EqualTo(200));
    }

    [Test]
    public void TryGet_EmptyHashMap_ReturnsDefault()
    {
        // Act
        var value = _hashMap.TryGet("key1");

        // Assert
        Assert.That(value, Is.EqualTo(default(int)));
    }

    [Test]
    public void TryGet_NullKey_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _hashMap.TryGet(null!));
    }

    [Test]
    public void TryGet_WithReferenceType_ReturnsCorrectValue()
    {
        // Arrange
        var map = new HashMap<string, string>();
        map.Put("key1", "value1");

        // Act
        var value = map.TryGet("key1");

        // Assert
        Assert.That(value, Is.EqualTo("value1"));
    }

    [Test]
    public void TryGet_WithReferenceType_NonExisting_ReturnsNull()
    {
        // Arrange
        var map = new HashMap<string, string>();

        // Act
        var value = map.TryGet("nonexistent");

        // Assert
        Assert.That(value, Is.Null);
    }

    [Test]
    public void Remove_ExistingKey_ReturnsTrue()
    {
        // Arrange
        _hashMap.Put("key1", 100);

        // Act
        var result = _hashMap.Remove("key1");

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_hashMap.TryGet("key1"), Is.EqualTo(default(int)));
    }

    [Test]
    public void Remove_NonExistingKey_ReturnsFalse()
    {
        // Act
        var result = _hashMap.Remove("nonexistent");

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Remove_AfterRemoval_CannotRetrieve()
    {
        // Arrange
        _hashMap.Put("key1", 100);

        // Act
        _hashMap.Remove("key1");
        var value = _hashMap.TryGet("key1");

        // Assert
        Assert.That(value, Is.EqualTo(default(int)));
    }

    [Test]
    public void Remove_AfterRemoval_CanAddAgain()
    {
        // Arrange
        _hashMap.Put("key1", 100);
        _hashMap.Remove("key1");

        // Act
        _hashMap.Put("key1", 200);

        // Assert
        Assert.That(_hashMap.TryGet("key1"), Is.EqualTo(200));
    }

    [Test]
    public void Remove_OneOfManyItems_OnlyRemovesSpecifiedItem()
    {
        // Arrange
        _hashMap.Put("key1", 100);
        _hashMap.Put("key2", 200);
        _hashMap.Put("key3", 300);

        // Act
        _hashMap.Remove("key2");

        // Assert
        Assert.That(_hashMap.TryGet("key1"), Is.EqualTo(100));
        Assert.That(_hashMap.TryGet("key2"), Is.EqualTo(default(int)));
        Assert.That(_hashMap.TryGet("key3"), Is.EqualTo(300));
    }

    [Test]
    public void Remove_FromEmptyHashMap_ReturnsFalse()
    {
        // Act
        var result = _hashMap.Remove("key1");

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Remove_NullKey_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _hashMap.Remove(null!));
    }

    [Test]
    public void Remove_SameBucketItems_RemovesCorrectItem()
    {
        // Arrange - Create items that hash to same bucket
        var map = new HashMap<SameHashObject, int>();
        var obj1 = new SameHashObject("A");
        var obj2 = new SameHashObject("B");
        map.Put(obj1, 100);
        map.Put(obj2, 200);

        // Act
        map.Remove(obj1);

        // Assert
        Assert.That(map.TryGet(obj1), Is.EqualTo(default(int)));
        Assert.That(map.TryGet(obj2), Is.EqualTo(200));
    }

    [Test]
    public void GetValues_EmptyHashMap_ReturnsEmptyList()
    {
        // Act
        var values = _hashMap.GetValues();

        // Assert
        Assert.That(values, Is.Empty);
    }

    [Test]
    public void GetValues_SingleItem_ReturnsListWithOneItem()
    {
        // Arrange
        _hashMap.Put("key1", 100);

        // Act
        var values = _hashMap.GetValues();

        // Assert
        Assert.That(values, Has.Count.EqualTo(1));
        Assert.That(values, Contains.Item(100));
    }

    [Test]
    public void GetValues_MultipleItems_ReturnsAllValues()
    {
        // Arrange
        _hashMap.Put("key1", 100);
        _hashMap.Put("key2", 200);
        _hashMap.Put("key3", 300);

        // Act
        var values = _hashMap.GetValues();

        // Assert
        Assert.That(values, Has.Count.EqualTo(3));
        Assert.That(values, Contains.Item(100));
        Assert.That(values, Contains.Item(200));
        Assert.That(values, Contains.Item(300));
    }

    [Test]
    public void GetValues_AfterUpdate_ReturnsUpdatedValue()
    {
        // Arrange
        _hashMap.Put("key1", 100);
        _hashMap.Put("key1", 200);

        // Act
        var values = _hashMap.GetValues();

        // Assert
        Assert.That(values, Has.Count.EqualTo(1));
        Assert.That(values, Contains.Item(200));
        Assert.That(values, Does.Not.Contain(100));
    }

    [Test]
    public void GetValues_AfterRemoval_DoesNotContainRemovedValue()
    {
        // Arrange
        _hashMap.Put("key1", 100);
        _hashMap.Put("key2", 200);
        _hashMap.Remove("key1");

        // Act
        var values = _hashMap.GetValues();

        // Assert
        Assert.That(values, Has.Count.EqualTo(1));
        Assert.That(values, Contains.Item(200));
        Assert.That(values, Does.Not.Contain(100));
    }

    [Test]
    public void GetValues_DuplicateValues_ReturnsAll()
    {
        // Arrange
        _hashMap.Put("key1", 100);
        _hashMap.Put("key2", 100);
        _hashMap.Put("key3", 100);

        // Act
        var values = _hashMap.GetValues();

        // Assert
        Assert.That(values, Has.Count.EqualTo(3));
        Assert.That(values.Count(v => v == 100), Is.EqualTo(3));
    }

    [Test]
    public void GetValues_LargeHashMap_ReturnsAllValues()
    {
        // Arrange
        for (var i = 0; i < 100; i++) _hashMap.Put($"key{i}", i);

        // Act
        var values = _hashMap.GetValues();

        // Assert
        Assert.That(values, Has.Count.EqualTo(100));
        for (var i = 0; i < 100; i++) Assert.That(values, Contains.Item(i));
    }

    [Test]
    public void HashMap_PutGetRemoveSequence_WorksCorrectly()
    {
        // Act & Assert
        _hashMap.Put("key1", 100);
        Assert.That(_hashMap.TryGet("key1"), Is.EqualTo(100));

        _hashMap.Put("key1", 200);
        Assert.That(_hashMap.TryGet("key1"), Is.EqualTo(200));

        _hashMap.Remove("key1");
        Assert.That(_hashMap.TryGet("key1"), Is.EqualTo(default(int)));

        _hashMap.Put("key1", 300);
        Assert.That(_hashMap.TryGet("key1"), Is.EqualTo(300));
    }

    [Test]
    public void HashMap_WithIntegerKeys_WorksCorrectly()
    {
        // Arrange
        var map = new HashMap<int, string>();

        // Act
        map.Put(1, "one");
        map.Put(2, "two");
        map.Put(3, "three");

        // Assert
        Assert.That(map.TryGet(1), Is.EqualTo("one"));
        Assert.That(map.TryGet(2), Is.EqualTo("two"));
        Assert.That(map.TryGet(3), Is.EqualTo("three"));
    }

    [Test]
    public void HashMap_WithCustomObjects_WorksCorrectly()
    {
        // Arrange
        var map = new HashMap<CustomKey, string>();
        var key1 = new CustomKey(1, "A");
        var key2 = new CustomKey(2, "B");

        // Act
        map.Put(key1, "value1");
        map.Put(key2, "value2");

        // Assert
        Assert.That(map.TryGet(key1), Is.EqualTo("value1"));
        Assert.That(map.TryGet(key2), Is.EqualTo("value2"));
    }

    [Test]
    public void HashMap_StressTest_Handles1000Items()
    {
        // Arrange & Act
        for (var i = 0; i < 1000; i++) _hashMap.Put($"key{i}", i);

        // Assert
        for (var i = 0; i < 1000; i++) Assert.That(_hashMap.TryGet($"key{i}"), Is.EqualTo(i));

        var values = _hashMap.GetValues();
        Assert.That(values, Has.Count.EqualTo(1000));
    }

    [Test]
    public void HashMap_SmallCapacity_HandlesCollisionsCorrectly()
    {
        // Arrange
        var map = new HashMap<string, int>(4); // Very small capacity

        // Act
        for (var i = 0; i < 20; i++) map.Put($"key{i}", i);

        // Assert
        for (var i = 0; i < 20; i++) Assert.That(map.TryGet($"key{i}"), Is.EqualTo(i));
    }

    [Test]
    public void HashMap_MixedOperations_MaintainsConsistency()
    {
        // Act
        _hashMap.Put("A", 1);
        _hashMap.Put("B", 2);
        _hashMap.Put("C", 3);

        Assert.That(_hashMap.GetValues().Count, Is.EqualTo(3));

        _hashMap.Remove("B");
        Assert.That(_hashMap.GetValues().Count, Is.EqualTo(2));

        _hashMap.Put("D", 4);
        _hashMap.Put("A", 10); // Update

        // Assert
        Assert.That(_hashMap.TryGet("A"), Is.EqualTo(10));
        Assert.That(_hashMap.TryGet("B"), Is.EqualTo(default(int)));
        Assert.That(_hashMap.TryGet("C"), Is.EqualTo(3));
        Assert.That(_hashMap.TryGet("D"), Is.EqualTo(4));
        Assert.That(_hashMap.GetValues().Count, Is.EqualTo(3));
    }

    private class SameHashObject
    {
        private readonly string _value;

        public SameHashObject(string value)
        {
            _value = value;
        }

        public override int GetHashCode()
        {
            return 42; // Always return same hash code to force collisions
        }

        public override bool Equals(object? obj)
        {
            if (obj is SameHashObject other)
                return _value == other._value;
            return false;
        }
    }

    private class CustomKey
    {
        public CustomKey(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

        public override bool Equals(object? obj)
        {
            if (obj is CustomKey other)
                return Id == other.Id && Name == other.Name;
            return false;
        }
    }
}