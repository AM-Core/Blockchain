using DataStructures;

namespace Domain;
using Domain.Transaction;
public class Block
{
    public string BlockHost { get; private set; }
    public string PrevBlockHost { get; private set; }
    public int Difficulty { get; set; }
    public int Nonce { get; set; }
    public DAG<Transaction.Transaction> Transactions { get;private set; }
    public string MerkleRoot{ get; set; }

    public Block(string blockHost, string prevBlockHost, int difficulty, int nonce)
    {
        BlockHost = blockHost;
        PrevBlockHost = prevBlockHost;
        Difficulty = difficulty;
        Nonce = nonce;
        Transactions = new DAG<Transaction.Transaction>();
    }
}