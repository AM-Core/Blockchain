namespace Domain.Transaction;

public class Output
{
    public double Value { get;private set; }
    public string PublicKey { get;private set; }

    public Output(double value, string publicKey)
    {
        Value = value;
        PublicKey = publicKey;
    }
}