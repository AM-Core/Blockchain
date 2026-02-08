namespace Domain.Interfaces;

public interface IHashingHandler
{
    string ComputeHash(string data);
    string ComputeBlockHash(Block block);
    bool VerifyHash(string data, string hash);
    string ComputeTransactionHash(Transaction.Transaction transaction);
    string ComputeMerkleRoot(List<Transaction.Transaction> transactions);
}