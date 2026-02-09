namespace Domain;

using Transaction;

public class Block
{
    public string BlockHash { get; private set; }
    public string PrevBlockHash { get; private set; }
    public int Difficulty { get; set; }
    public int Nonce { get; set; }
    public List<TransactionEntry> Transactions { get; private set; }
    public string MerkleRoot { get; set; }

    public Block(string blockHash, string prevBlockHash, int difficulty)
    {
        BlockHash = blockHash;
        PrevBlockHash = prevBlockHash;
        Difficulty = difficulty;
        Transactions = new();
    }
}