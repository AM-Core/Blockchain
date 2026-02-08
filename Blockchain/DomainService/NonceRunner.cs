using Domain;
using Domain.Interfaces;

namespace DomainService;

public class NonceRunner : INonceRunner
{
    private readonly IHashingHandler _hashingHandler;

    public NonceRunner(IHashingHandler hashingHandler)
    {
        _hashingHandler = hashingHandler;
    }

    public long FindValidNonce(Block block, int difficulty, IHashingHandler hasher)
    {
        throw new NotImplementedException();
    }

    public Task<long> FindValidNonceAsync(Block block, int difficulty, IHashingHandler hasher)
    {
        throw new NotImplementedException();
    }

    public bool IsNonceValid(Block block, long nonce, int difficulty)
    {
        throw new NotImplementedException();
    }
}