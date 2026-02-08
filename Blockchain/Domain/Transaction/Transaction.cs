namespace Domain.Transaction;

public class Transaction
{
    public string Id { get; private set; }
    public List<Input> Inputs { get; set; }
    public List<Output> Outputs { get; set; }
    public double Fee { get; set; } = 0;
    public Transaction(string id)
    {
        Id = id;
        Inputs = new List<Input>();
        Outputs = new List<Output>();
    }
}