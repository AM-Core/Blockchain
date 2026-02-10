using DataStructures;
using Domain.Exceptions;
using Domain.Transaction;
using DomainService;

namespace DomainServicesTests;

[TestFixture]
public class FeeRateCalculatorTests
{
    private FeeRateCalculator _calculator;
    private HashMap<string, TransactionEntry> _map;

    [SetUp]
    public void Setup()
    {
        _calculator = new FeeRateCalculator();
        _map = new HashMap<string, TransactionEntry>();
    }

    #region CalculateFee Tests

    [Test]
    public void CalculateFee_ValidTransaction_CalculatesCorrectFee()
    {
        // Arrange
        // Create parent transaction with output
        var parentTx = new TransactionEntry("prevTx1")
        {
            Fee = 0,
            Size = 200
        };
        parentTx.Outputs.Add(new Output(10.0, "pubKey1"));
        _map.Put("prevTx1", parentTx);

        // Create transaction spending from parent
        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(9.9, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0.1));
    }

    [Test]
    public void CalculateFee_MultipleInputsAndOutputs_CalculatesCorrectly()
    {
        // Arrange
        // Create parent transactions
        var parentTx1 = new TransactionEntry("prevTx1");
        parentTx1.Outputs.Add(new Output(5.0, "pubKey1"));
        _map.Put("prevTx1", parentTx1);

        var parentTx2 = new TransactionEntry("prevTx2");
        parentTx2.Outputs.Add(new Output(2.0, "pubKey2"));
        parentTx2.Outputs.Add(new Output(3.0, "pubKey2b"));
        _map.Put("prevTx2", parentTx2);

        var parentTx3 = new TransactionEntry("prevTx3");
        parentTx3.Outputs.Add(new Output(2.0, "pubKey3"));
        _map.Put("prevTx3", parentTx3);

        // Create transaction spending from multiple parents
        var transaction = new TransactionEntry("tx1")
        {
            Size = 300
        };
        // Inputs: 5 + 3 + 2 = 10 BTC
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Inputs.Add(new Input("prevTx2", 1, "pubKey2", "sig2"));
        transaction.Inputs.Add(new Input("prevTx3", 0, "pubKey3", "sig3"));
        
        // Outputs: 4 + 3 + 2.5 = 9.5 BTC (0.5 BTC fee)
        transaction.Outputs.Add(new Output(4.0, "pubKeyOut1"));
        transaction.Outputs.Add(new Output(3.0, "pubKeyOut2"));
        transaction.Outputs.Add(new Output(2.5, "pubKeyOut3"));

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0.5));
    }

    [Test]
    public void CalculateFee_ZeroFeeTransaction_SetsZeroFee()
    {
        // Arrange
        var parentTx = new TransactionEntry("prevTx1");
        parentTx.Outputs.Add(new Output(10.0, "pubKey1"));
        _map.Put("prevTx1", parentTx);

        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(10.0, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0.0));
    }

    [Test]
    public void CalculateFee_HighFeeTransaction_CalculatesCorrectly()
    {
        // Arrange
        var parentTx = new TransactionEntry("prevTx1");
        parentTx.Outputs.Add(new Output(100.0, "pubKey1"));
        _map.Put("prevTx1", parentTx);

        var transaction = new TransactionEntry("tx1")
        {
            Size = 200
        };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(50.0, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(50.0));
    }

    [Test]
    public void CalculateFee_NegativeFee_ThrowsInvalidFeeException()
    {
        // Arrange
        var parentTx = new TransactionEntry("prevTx1");
        parentTx.Outputs.Add(new Output(5.0, "pubKey1"));
        _map.Put("prevTx1", parentTx);

        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(10.0, "pubKeyOut1"));

        // Act & Assert
        var ex = Assert.Throws<InvalidFeeException>(() => _calculator.CalculateFee(transaction, _map));
        Assert.That(ex.Message, Does.Contain("negative fee"));
        Assert.That(ex.Message, Does.Contain("tx1"));
    }

    [Test]
    public void CalculateFee_SmallFractionsFee_CalculatesPrecisely()
    {
        // Arrange
        var parentTx = new TransactionEntry("prevTx1");
        parentTx.Outputs.Add(new Output(1.123456, "pubKey1"));
        _map.Put("prevTx1", parentTx);

        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(1.023456, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert - Rounded to 2 decimal places
        Assert.That(transaction.Fee, Is.EqualTo(0.1).Within(0.01));
    }

    [Test]
    public void CalculateFee_NoInputs_SetsZeroFee()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 250,
            Fee = 0
        };
        transaction.Outputs.Add(new Output(10.0, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0));
    }

    [Test]
    public void CalculateFee_NoOutputs_FeesEqualInputs()
    {
        // Arrange
        var parentTx = new TransactionEntry("prevTx1");
        parentTx.Outputs.Add(new Output(10.0, "pubKey1"));
        _map.Put("prevTx1", parentTx);

        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(10.0));
    }

    [Test]
    public void CalculateFee_ParentTransactionNotFound_SetsZeroFeeAndReturns()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        transaction.Inputs.Add(new Input("nonexistent", 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(5.0, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert - Fee should remain 0 since parent not found
        Assert.That(transaction.Fee, Is.EqualTo(0));
    }

    [Test]
    public void CalculateFee_NullPrevId_SetsZeroFeeAndReturns()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        transaction.Inputs.Add(new Input(null, 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(5.0, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0));
    }

    [Test]
    public void CalculateFee_InvalidOutputIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var parentTx = new TransactionEntry("prevTx1");
        parentTx.Outputs.Add(new Output(10.0, "pubKey1"));
        _map.Put("prevTx1", parentTx);

        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        // Referencing output index 5, but parent only has 1 output
        transaction.Inputs.Add(new Input("prevTx1", 5, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(5.0, "pubKeyOut1"));

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => _calculator.CalculateFee(transaction, _map));
    }

    [Test]
    public void CalculateFee_RoundsToTwoDecimalPlaces()
    {
        // Arrange
        var parentTx = new TransactionEntry("prevTx1");
        parentTx.Outputs.Add(new Output(10.123456789, "pubKey1"));
        _map.Put("prevTx1", parentTx);

        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1"));
        transaction.Outputs.Add(new Output(9.876543210, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert - Should be rounded to 2 decimal places
        // 10.123456789 - 9.876543210 = 0.246913579 -> rounds to 0.25
        Assert.That(transaction.Fee, Is.EqualTo(0.25));
    }

    #endregion

    #region Integration Example

    [Test]
    public void CalculateFee_RealWorldExample_WorksCorrectly()
    {
        // Arrange - Typical Bitcoin transaction
        // Create parent transactions
        var parentTx1 = new TransactionEntry("prev_tx_1");
        parentTx1.Outputs.Add(new Output(0.5, "sender_pubkey_1"));
        _map.Put("prev_tx_1", parentTx1);

        var parentTx2 = new TransactionEntry("prev_tx_2");
        parentTx2.Outputs.Add(new Output(0.1, "sender_pubkey_2a"));
        parentTx2.Outputs.Add(new Output(0.3, "sender_pubkey_2b"));
        _map.Put("prev_tx_2", parentTx2);

        var transaction = new TransactionEntry("real_tx_001")
        {
            Size = 250
        };
        
        // Spending from 2 previous outputs
        transaction.Inputs.Add(new Input("prev_tx_1", 0, "sender_pubkey_1", "signature_1"));
        transaction.Inputs.Add(new Input("prev_tx_2", 1, "sender_pubkey_2", "signature_2"));
        
        // Sending to 1 recipient, 1 change address
        transaction.Outputs.Add(new Output(0.6, "recipient_pubkey"));      // Payment
        transaction.Outputs.Add(new Output(0.19, "change_pubkey"));         // Change
        // Fee: 0.5 + 0.3 - 0.6 - 0.19 = 0.01 BTC

        // Act
        _calculator.CalculateFee(transaction, _map);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0.01));
    }

    [Test]
    public void CalculateFee_ChainedTransactions_CalculatesCorrectly()
    {
        // Arrange
        // Transaction 1 (creates outputs)
        var tx1 = new TransactionEntry("tx1");
        tx1.Outputs.Add(new Output(10.0, "pubKey1"));
        tx1.Outputs.Add(new Output(5.0, "pubKey2"));
        _map.Put("tx1", tx1);

        // Transaction 2 (spends from tx1)
        var tx2 = new TransactionEntry("tx2")
        {
            Size = 250
        };
        tx2.Inputs.Add(new Input("tx1", 0, "pubKey1", "sig1"));
        tx2.Outputs.Add(new Output(9.5, "pubKey3"));
        _calculator.CalculateFee(tx2, _map);
        _map.Put("tx2", tx2);

        // Transaction 3 (spends from tx2)
        var tx3 = new TransactionEntry("tx3")
        {
            Size = 250
        };
        tx3.Inputs.Add(new Input("tx2", 0, "pubKey3", "sig2"));
        tx3.Outputs.Add(new Output(9.0, "pubKey4"));

        // Act
        _calculator.CalculateFee(tx3, _map);

        // Assert
        Assert.That(tx2.Fee, Is.EqualTo(0.5));
        Assert.That(tx3.Fee, Is.EqualTo(0.5));
    }

    #endregion
}
