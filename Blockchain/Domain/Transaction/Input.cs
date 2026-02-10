namespace Domain.Transaction;

public class Input
{
    public Input(string prevId, int prevIndex, string publicKey, string signature)
    {
        PrevId = prevId;
        PrevIndex = prevIndex;
        PublicKey = publicKey;
        Signature = signature;
    }

    public string PrevId { get; private set; }
    public int PrevIndex { get; private set; }
    public string PublicKey { get; private set; }
    public string Signature { get; private set; }
}