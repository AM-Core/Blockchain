using Application.QueryHandler.Command;
using Domain.Contracts;
using Domain.Interfaces;
using DomainService;

namespace Application.MiningApplication;

public sealed class EvictApplication : ICommand
{
    private readonly Mempool _mempool;
    private readonly IResultWriter _resultWriter;
    
    public EvictApplication(Mempool mempool, IResultWriter resultWriter)
    {
        _mempool = mempool;
        _resultWriter = resultWriter;
    }
    public void Execute(Command command)
    {
        var count = int.Parse(command.Argument);
        _mempool.EvictHighestPriorityTransaction(count);
        _resultWriter.WriteMempool(new MempoolDto(_mempool.GetAllTransactions(true)), true);
    }
}