using DataStructures;

namespace Domain;
using Domain.Transaction;
public class Block
{
    public string BlockHost { get; set; }
    public string PrevBlockHost { get; set; }
    public int Difficulty { get; set; }
    public int Nonce { get; set; }
    private DAG<Transaction.Transaction> Transactions { get; set; }

    public Block(string blockHost, string prevBlockHost, int difficulty, int nonce)
    {
        BlockHost = blockHost;
        PrevBlockHost = prevBlockHost;
        Difficulty = difficulty;
        Nonce = nonce;
        Transactions = new DAG<Transaction.Transaction>();
    }
}