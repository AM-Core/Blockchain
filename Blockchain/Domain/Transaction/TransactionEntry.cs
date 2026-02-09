namespace Domain.Transaction;

public class TransactionEntry
{
    public string Id { get; private set; }
    public List<Input> Inputs { get; set; }
    public List<Output> Outputs { get; set; }
    public double ParentFee { get; set; } = 0;
    public int ParentSize { get; set; } = 0;
    public double Fee { get; set; } = 0;
    public int Size { get; set; } = 0;

    public TransactionEntry(string id)
    {
        Id = id;
        Inputs = new List<Input>();
        Outputs = new List<Output>();
    }
}