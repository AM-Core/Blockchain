namespace Domain.Contracts;

public class MempoolDto
{
    public MempoolDto()
    {
        Transactions = new List<TransactionDto>();
    }

    public MempoolDto(List<string> transacstions)
    {
        Transactions = transacstions.Select(x => new TransactionDto(x)).ToList();
    }

    public List<TransactionDto> Transactions { get; set; }
}