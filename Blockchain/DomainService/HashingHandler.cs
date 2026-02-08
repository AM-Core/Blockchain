using Domain;
using Domain.Interfaces;
using Domain.Transaction;

namespace DomainService;

public class HashingHandler : IHashingHandler
{
    public string ComputeHash(string data)
    {
        throw new NotImplementedException();
    }

    public string ComputeBlockHash(Block block)
    {
        throw new NotImplementedException();
    }

    public bool VerifyHash(string data, string hash)
    {
        throw new NotImplementedException();
    }

    public string ComputeTransactionHash(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public string ComputeMerkleRoot(List<Transaction> transactions)
    {
        throw new NotImplementedException();
    }
}