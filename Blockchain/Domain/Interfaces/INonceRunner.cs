using Domain;
using Domain.Interfaces;

namespace DomainService;

public interface INonceRunner
{
    long FindValidNonce(Block block);
    Task<long> FindValidNonceAsync(Block block);
    bool IsNonceValid(Block block);
}
