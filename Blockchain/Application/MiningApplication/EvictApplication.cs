using DomainService;

namespace Application.MiningApplication;

public sealed class EvictApplication
{
    public void EvictMempool(int count, Mempool mempool)
    {
        mempool.EvictHighestPriorityTransaction(count);
    }
}
