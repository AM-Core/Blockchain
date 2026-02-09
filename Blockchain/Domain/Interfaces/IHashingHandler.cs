using Domain.Transaction;

namespace Domain.Interfaces;

public interface IHashingHandler
{
    string ComputeHash(string data);
    string ComputeBlockHash(Block block);
    bool VerifyHash(string data, string hash);
    string ComputeTransactionHash(TransactionEntry transactionEntry);
    string ComputeMerkleRoot(List<TransactionEntry> transactions);
}