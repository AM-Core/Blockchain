namespace Domain.Transaction;

public class TransactionEntry
{
    public TransactionEntry(string txid)
    {
        this.txid = txid;
        inputs = new List<Input>();
        outputs = new List<Output>();
    }

    public string txid { get; private set; }
    public List<Input> inputs { get; set; }
    public List<Output> outputs { get; set; }
    public double ParentFee { get; set; } = 0;
    public int ParentSize { get; set; } = 0;
    public double Fee { get; set; } = 0;
    public int Size { get; set; } = 0;
}