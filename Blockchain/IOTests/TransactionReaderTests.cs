using Application.MiningApplication.Dispatching;
using Application.QueryHandler;
using ConsoleApp.Bootstrap;
using Domain;
using Domain.Interfaces;
using Domain.Transaction;
using DomainService;
using IO;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Text.Json;

namespace IOTests;

[TestFixture]
public class TransactionReaderTests
{
    private ServiceProvider _provider;
    private ITransactionReader _reader;
    private string _testDirectory;

    [SetUp]
    public void Setup()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), $"TransactionReaderTests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDirectory);

        // Build DI container
        _provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();
        
        // Resolve the transaction reader from the container
        _reader = _provider.GetRequiredService<ITransactionReader>();
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
        }
    }

    [Test]
    public void ReadTransaction_ValidJsonFile_ReturnsTransaction()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 1.5, 300);
        var filePath = Path.Combine(_testDirectory, "test_transaction.json");
        WriteTransactionToFile(transaction, filePath);

        // Act
        var result = _reader.ReadTransaction(filePath);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo("tx1"));
        Assert.That(result.Fee, Is.EqualTo(1.5));
    }

    [Test]
    public void ReadTransaction_WithInputsAndOutputs_ReturnsCompleteTransaction()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 2.0, 250);
        var filePath = Path.Combine(_testDirectory, "transaction_with_io.json");
        WriteTransactionToFile(transaction, filePath);

        // Act
        var result = _reader.ReadTransaction(filePath);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Inputs.Count, Is.EqualTo(1));
        Assert.That(result.Inputs[0].PrevId, Is.EqualTo("prevTx1"));
        Assert.That(result.Outputs.Count, Is.EqualTo(1));
        Assert.That(result.Outputs[0].Value, Is.EqualTo(10.0));
    }

    [Test]
    public void ReadTransaction_WithZeroFee_ReturnsCorrectly()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx1", 0.0, 250);
        var filePath = Path.Combine(_testDirectory, "zero_fee_transaction.json");
        WriteTransactionToFile(transaction, filePath);

        // Act
        var result = _reader.ReadTransaction(filePath);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Fee, Is.EqualTo(0.0));
    }

    [Test]
    public void ReadTransaction_FileNotFound_ThrowsFileNotFoundException()
    {
        // Arrange
        var nonExistentPath = Path.Combine(_testDirectory, "nonexistent.json");

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => _reader.ReadTransaction(nonExistentPath));
    }

    [Test]
    public void ReadTransaction_InvalidJson_ThrowsJsonException()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "invalid.json");
        File.WriteAllText(filePath, "{ invalid json content }");

        // Act & Assert
        Assert.Throws<JsonException>(() => _reader.ReadTransaction(filePath));
    }

    [Test]
    public void ReadTransaction_EmptyFile_ThrowsInvalidDataException()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "empty.json");
        File.WriteAllText(filePath, "null");

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _reader.ReadTransaction(filePath));
    }

    [Test]
    public void ReadTransaction_EmptyJsonObject_ThrowsInvalidDataException()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "empty_object.json");
        File.WriteAllText(filePath, "{}");

        // Act & Assert
        var ex = Assert.Throws<InvalidDataException>(() => _reader.ReadTransaction(filePath));
        Assert.That(ex.Message, Does.Contain("Failed to deserialize"));
    }

    [Test]
    public void ReadTransaction_MultipleTransactionsFromDifferentFiles_ReturnsCorrectData()
    {
        // Arrange
        var transaction1 = CreateTestTransaction("tx1", 1.0, 250);
        var transaction2 = CreateTestTransaction("tx2", 2.0, 300);
        var transaction3 = CreateTestTransaction("tx3", 0.5, 200);

        var filePath1 = Path.Combine(_testDirectory, "transaction1.json");
        var filePath2 = Path.Combine(_testDirectory, "transaction2.json");
        var filePath3 = Path.Combine(_testDirectory, "transaction3.json");

        WriteTransactionToFile(transaction1, filePath1);
        WriteTransactionToFile(transaction2, filePath2);
        WriteTransactionToFile(transaction3, filePath3);

        // Act
        var result1 = _reader.ReadTransaction(filePath1);
        var result2 = _reader.ReadTransaction(filePath2);
        var result3 = _reader.ReadTransaction(filePath3);

        // Assert
        Assert.That(result1.Id, Is.EqualTo("tx1"));
        Assert.That(result2.Id, Is.EqualTo("tx2"));
        Assert.That(result3.Id, Is.EqualTo("tx3"));
    }

    [Test]
    public void ReadTransaction_WithMultipleInputs_ReturnsAllInputs()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Fee = 1.0,
            Size = 250
        };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Inputs.Add(new Input("prevTx2", 1, "pubKey2", "sig2"));
        transaction.Inputs.Add(new Input("prevTx3", 0, "pubKey3", "sig3"));
        transaction.Outputs.Add(new Output(10.0, "pubKeyOut"));

        var filePath = Path.Combine(_testDirectory, "multiple_inputs.json");
        WriteTransactionToFile(transaction, filePath);

        // Act
        var result = _reader.ReadTransaction(filePath);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Inputs.Count, Is.EqualTo(3));
        Assert.That(result.Inputs[0].PrevId, Is.EqualTo("prevTx1"));
        Assert.That(result.Inputs[1].PrevId, Is.EqualTo("prevTx2"));
        Assert.That(result.Inputs[2].PrevId, Is.EqualTo("prevTx3"));
    }

    [Test]
    public void ReadTransaction_WithMultipleOutputs_ReturnsAllOutputs()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Fee = 1.0,
            Size = 250
        };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(5.0, "pubKeyOut1"));
        transaction.Outputs.Add(new Output(3.0, "pubKeyOut2"));
        transaction.Outputs.Add(new Output(2.0, "pubKeyOut3"));

        var filePath = Path.Combine(_testDirectory, "multiple_outputs.json");
        WriteTransactionToFile(transaction, filePath);

        // Act
        var result = _reader.ReadTransaction(filePath);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Outputs.Count, Is.EqualTo(3));
        Assert.That(result.Outputs[0].Value, Is.EqualTo(5.0));
        Assert.That(result.Outputs[1].Value, Is.EqualTo(3.0));
        Assert.That(result.Outputs[2].Value, Is.EqualTo(2.0));
    }

    [Test]
    public void ReadTransaction_LargeTransaction_ReadsSuccessfully()
    {
        // Arrange
        var transaction = new TransactionEntry("large_tx")
        {
            Fee = 10.0,
            Size = 5000
        };

        for (var i = 0; i < 50; i++) 
            transaction.Inputs.Add(new Input($"prevTx{i}", i, $"pubKey{i}", $"sig{i}"));

        for (var i = 0; i < 50; i++) 
            transaction.Outputs.Add(new Output(i * 0.1, $"pubKeyOut{i}"));

        var filePath = Path.Combine(_testDirectory, "large_transaction.json");
        WriteTransactionToFile(transaction, filePath);

        // Act
        var result = _reader.ReadTransaction(filePath);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Inputs.Count, Is.EqualTo(50));
        Assert.That(result.Outputs.Count, Is.EqualTo(50));
    }

    [Test]
    public void ReadTransaction_RoundTrip_PreservesAllData()
    {
        // Arrange
        var originalTransaction = CreateTestTransaction("tx1", 2.5, 350);
        var filePath = Path.Combine(_testDirectory, "roundtrip.json");
        WriteTransactionToFile(originalTransaction, filePath);

        // Act
        var readTransaction = _reader.ReadTransaction(filePath);

        // Assert
        Assert.That(readTransaction.Id, Is.EqualTo(originalTransaction.Id));
        Assert.That(readTransaction.Fee, Is.EqualTo(originalTransaction.Fee));
        Assert.That(readTransaction.Inputs.Count, Is.EqualTo(originalTransaction.Inputs.Count));
        Assert.That(readTransaction.Outputs.Count, Is.EqualTo(originalTransaction.Outputs.Count));
    }

    [Test]
    public void ReadTransaction_WithSpecialCharactersInId_ReturnsCorrectly()
    {
        // Arrange
        var transaction = CreateTestTransaction("tx_special-123!@#", 1.0, 250);
        var filePath = Path.Combine(_testDirectory, "special_chars.json");
        WriteTransactionToFile(transaction, filePath);

        // Act
        var result = _reader.ReadTransaction(filePath);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo("tx_special-123!@#"));
    }

    [Test]
    public void ReadTransaction_WithVeryLargeValues_ReturnsCorrectly()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Fee = 999999.99,
            Size = int.MaxValue
        };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(double.MaxValue / 2, "pubKeyOut1"));

        var filePath = Path.Combine(_testDirectory, "large_values.json");
        WriteTransactionToFile(transaction, filePath);

        // Act
        var result = _reader.ReadTransaction(filePath);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Fee, Is.EqualTo(999999.99));
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

    private void WriteTransactionToFile(TransactionEntry transaction, string filePath)
    {
        var json = JsonSerializer.Serialize(transaction, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
}