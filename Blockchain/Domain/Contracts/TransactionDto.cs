namespace Domain.Contracts;

public class TransactionDto
{
    public TransactionDto()
    {
    }

    public TransactionDto(string transactionId)
    {
        TxId = transactionId;
    }

    public string TxId { get; set; }
}