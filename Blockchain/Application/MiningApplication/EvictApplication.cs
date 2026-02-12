using Domain.Contracts;
using Domain.Interfaces;
using DomainService;

namespace Application.MiningApplication;

public sealed class EvictApplication
{
    public void EvictMempool(int count, Mempool mempool, IResultWriter resultWriter)
    {
        mempool.EvictHighestPriorityTransaction(count);
        resultWriter.WriteMempool(new MempoolDto(mempool.GetAllTransactions(true)), true);
    }
}