namespace Domain.Contracts;

public class HeaderDto
{
    public HeaderDto()
    {
    }

    public HeaderDto(string blockHash, string prevBlockHash, long difficulty, long nonce)
    {
        BlockHash = blockHash;
        PrevBlockHash = prevBlockHash;
        Difficulty = difficulty;
        Nonce = nonce;
    }

    public string BlockHash { get; set; }
    public string PrevBlockHash { get; set; }
    public long Difficulty { get; set; }
    public long Nonce { get; set; }
}