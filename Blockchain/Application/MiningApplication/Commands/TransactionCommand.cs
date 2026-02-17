using Application.MiningApplication.Abstractions;
using Application.QueryHandler.Command;
using Domain.Contracts;
using Domain.Interfaces;
using DomainService;

namespace Application.MiningApplication.Commands;

public class TransactionCommand : ICommand
{
    private readonly Mempool _mempool;
    private readonly ITransactionReader _transactionReader;
    private readonly IResultWriter _resultWriter;
    
    public TransactionCommand(Mempool mempool, ITransactionReader transactionReader,
        IResultWriter resultWriter)
    {
        _mempool = mempool;
        _transactionReader = transactionReader;
        _resultWriter = resultWriter;
    }
    public void Execute(Command command)
    {
        var filePath = command.Argument;
        var transactionEntry = _transactionReader.ReadTransaction(filePath);
        _mempool.AddTransaction(transactionEntry);
        _resultWriter.WriteMempool(new MempoolDto(_mempool.GetAllTransactions()));
    }
}