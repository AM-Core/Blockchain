using Domain.Transaction;

namespace DomainService;

public class NonceRunner
{
    private readonly HashingHandler _hashingHandler;

    public NonceRunner(HashingHandler hashingHandler)
    {
        _hashingHandler = hashingHandler;
    }

    public long FindValidNonce(List<TransactionEntry> transactions, long difficulty)
    {
        var transactionString = transactions.ToString();
        
        for (long nonce = 0;; nonce++)
        {
            var hash = _hashingHandler.ComputeHash(nonce + transactionString);
            
            if (GetLeadingZeroCount(hash) > difficulty)
                return nonce;
        }
    }

    private long GetLeadingZeroCount(string value)
    {
        return value.TakeWhile(c => c == '0').Count();
    }
}