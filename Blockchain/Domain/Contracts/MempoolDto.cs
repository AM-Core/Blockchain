namespace Domain.Contracts;

public class MempoolDto
{
    public List<TransactionDto> Transactions { get; set; }

    public MempoolDto(List<string> transacstions)
    {
        Transactions = transacstions.Select(x => new TransactionDto(x)).ToList();
    }
}