namespace Domain.Contracts;

public class BlockDto
{
    public BlockDto()
    {
        Header = new HeaderDto();
        Transactions = new List<TransactionDto>();
    }

    public BlockDto(Block block)
    {
        Transactions = new List<TransactionDto>();
        Header = new HeaderDto(block.BlockHash, block.PrevBlockHash, block.Difficulty, block.Nonce);
        Transactions = block.Transactions.Select(x => new TransactionDto(x.Id)).ToList();
    }

    public HeaderDto Header { get; set; }

    public List<TransactionDto> Transactions { get; set; }
}