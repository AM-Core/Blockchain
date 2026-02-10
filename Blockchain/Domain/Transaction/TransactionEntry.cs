namespace Domain.Transaction;

public class TransactionEntry
{
    public TransactionEntry(string id)
    {
        this.Id = id;
        Inputs = new List<Input>();
        Outputs = new List<Output>();
    }

    public string Id { get; private set; }
    public List<Input> Inputs { get; set; }
    public List<Output> Outputs { get; set; }
    public double Fee { get; set; } = 0;
    public int Size { get; set; } = 0;
}