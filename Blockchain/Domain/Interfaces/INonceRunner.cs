using Domain;
using Domain.Interfaces;

namespace DomainService;

public interface INonceRunner
{
    long FindValidNonce(Block block, int difficulty, IHashingHandler hasher);
    Task<long> FindValidNonceAsync(Block block, int difficulty, IHashingHandler hasher);
    bool IsNonceValid(Block block, long nonce, int difficulty);
}
