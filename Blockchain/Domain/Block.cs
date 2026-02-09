namespace Domain;

using Transaction;

public class Block
{
    public string BlockHash { get; private set; }
    public string PrevBlockHash { get; private set; }
    public int Difficulty { get; set; }
    public long Nonce { get; set; }
    public List<TransactionEntry> Transactions { get; private set; }
    public string MerkleRoot { get; set; }

    public Block(int difficulty, List<TransactionEntry> transactions)
    {
        PrevBlockHash = new string('0', 64);
        Difficulty = difficulty;
        Transactions = transactions;
    }
}