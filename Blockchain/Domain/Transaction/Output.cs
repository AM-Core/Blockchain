namespace Domain.Transaction;

public class Output
{
    public Output(double value, string publicKey)
    {
        Value = value;
        PublicKey = publicKey;
    }

    public double Value { get; private set; }
    public string PublicKey { get; private set; }
}