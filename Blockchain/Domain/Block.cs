using Domain.Transaction;

namespace Domain;

public class Block
{
    public Block(long difficulty, List<TransactionEntry> transactions)
    {
        Difficulty = difficulty;
        Transactions = transactions;
        PrevBlockHash = new string('0', 64);
    }

    private const int LimitSize = 20;
    public string BlockHash { get; set; }
    public string PrevBlockHash { get; private set; }
    public long Difficulty { get; private set; }
    public long Nonce { get; set; }
    public List<TransactionEntry> Transactions { get; private set; }
    public string MerkleRoot { get; set; }
}