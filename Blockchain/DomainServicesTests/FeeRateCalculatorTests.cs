using Domain.Exceptions;
using Domain.Transaction;
using DomainService;

namespace DomainServicesTests;

[TestFixture]
public class FeeRateCalculatorTests
{
    private FeeRateCalculator _calculator;

    [SetUp]
    public void Setup()
    {
        _calculator = new FeeRateCalculator();
    }

    #region CalculateFee Tests

    [Test]
    public void CalculateFee_ValidTransaction_CalculatesCorrectFee()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        // Input: 10 BTC
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1", 10.0));
        // Output: 9.9 BTC (0.1 BTC fee)
        transaction.Outputs.Add(new Output(9.9, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0.1));
    }

    [Test]
    public void CalculateFee_MultipleInputsAndOutputs_CalculatesCorrectly()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 300
        };
        // Inputs: 5 + 3 + 2 = 10 BTC
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1", 5.0));
        transaction.Inputs.Add(new Input("prevTx2", 1, "pubKey2", "sig2", 3.0));
        transaction.Inputs.Add(new Input("prevTx3", 0, "pubKey3", "sig3", 2.0));
        
        // Outputs: 4 + 3 + 2.5 = 9.5 BTC (0.5 BTC fee)
        transaction.Outputs.Add(new Output(4.0, "pubKeyOut1"));
        transaction.Outputs.Add(new Output(3.0, "pubKeyOut2"));
        transaction.Outputs.Add(new Output(2.5, "pubKeyOut3"));

        // Act
        _calculator.CalculateFee(transaction);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0.5));
    }

    [Test]
    public void CalculateFee_ZeroFeeTransaction_SetsZeroFee()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        // Input: 10 BTC, Output: 10 BTC (0 fee)
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1", 10.0));
        transaction.Outputs.Add(new Output(10.0, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0.0));
    }

    [Test]
    public void CalculateFee_HighFeeTransaction_CalculatesCorrectly()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 200
        };
        // Input: 100 BTC, Output: 50 BTC (50 BTC fee)
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1", 100.0));
        transaction.Outputs.Add(new Output(50.0, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(50.0));
    }

    [Test]
    public void CalculateFee_NegativeFee_ThrowsInvalidFeeException()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        // Input: 5 BTC, Output: 10 BTC (negative fee - invalid!)
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1", 5.0));
        transaction.Outputs.Add(new Output(10.0, "pubKeyOut1"));

        // Act & Assert
        var ex = Assert.Throws<InvalidFeeException>(() => _calculator.CalculateFee(transaction));
        Assert.That(ex.Message, Does.Contain("negative fee"));
        Assert.That(ex.Message, Does.Contain("tx1"));
    }

    [Test]
    public void CalculateFee_SmallFractionsFee_CalculatesPrecisely()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        // Input: 1.123456 BTC, Output: 1.023456 BTC (0.1 BTC fee)
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1", 1.123456));
        transaction.Outputs.Add(new Output(1.023456, "pubKeyOut1"));

        // Act
        _calculator.CalculateFee(transaction);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0.1).Within(0.000001));
    }

    [Test]
    public void CalculateFee_NoInputs_ThrowsInvalidFeeException()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        // No inputs, only output
        transaction.Outputs.Add(new Output(10.0, "pubKeyOut1"));

        // Act & Assert
        Assert.Throws<InvalidFeeException>(() => _calculator.CalculateFee(transaction));
    }

    [Test]
    public void CalculateFee_NoOutputs_FeesEqualInputs()
    {
        // Arrange
        var transaction = new TransactionEntry("tx1")
        {
            Size = 250
        };
        // Input: 10 BTC, No outputs (all goes to fees - unusual but valid)
        transaction.Inputs.Add(new Input("prevTx1", 0, "pubKey1", "sig1", 10.0));

        // Act
        _calculator.CalculateFee(transaction);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(10.0));
    }

    #endregion

    #region Integration Example

    [Test]
    public void CalculateFee_RealWorldExample_WorksCorrectly()
    {
        // Arrange - Typical Bitcoin transaction
        var transaction = new TransactionEntry("real_tx_001")
        {
            Size = 250
        };
        
        // Spending from 2 previous outputs
        transaction.Inputs.Add(new Input("prev_tx_1", 0, "sender_pubkey_1", "signature_1", 0.5));
        transaction.Inputs.Add(new Input("prev_tx_2", 1, "sender_pubkey_2", "signature_2", 0.3));
        
        // Sending to 1 recipient, 1 change address
        transaction.Outputs.Add(new Output(0.6, "recipient_pubkey"));      // Payment
        transaction.Outputs.Add(new Output(0.19, "change_pubkey"));         // Change
        // Fee: 0.5 + 0.3 - 0.6 - 0.19 = 0.01 BTC

        // Act
        _calculator.CalculateFee(transaction);

        // Assert
        Assert.That(transaction.Fee, Is.EqualTo(0.01).Within(0.0001));
    }

    #endregion
}
