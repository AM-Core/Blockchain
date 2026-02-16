using Domain;

namespace DomainService;

public class NonceRunner
{
    private readonly HashingHandler _hashingHandler;

    public NonceRunner(HashingHandler hashingHandler)
    {
        _hashingHandler = hashingHandler;
    }

    public long FindValidNonce(Block block)
    {
        for (long nonce = 0;; nonce++)
        {
            block.Nonce = nonce;
            var hash = _hashingHandler.ComputeBlockHash(block);
            if (GetLeadingZeroCount(hash) > block.Difficulty)
                return nonce;
        }
    }

    private long GetLeadingZeroCount(string value)
    {
        return value.TakeWhile(c => c == '0').Count();
    }
}