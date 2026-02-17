using Application.MiningApplication.Abstractions;
using Application.QueryHandler;
using Application.QueryHandler.Command;
using Domain;
using Domain.Interfaces;
using DomainService;

namespace Application.MiningApplication.Dispatching;

public class ApplicationHandler
{
    private readonly BlockMiner _blockMiner;
    private readonly Mempool _mempool;
    private readonly MiningConfig _miningConfig;
    private readonly IQueryParser _queryParser;
    private readonly IResultWriter _resultWriter;
    private readonly ITransactionReader _transactionReader;
    private readonly CommandHandlerRegistry _commandHandlerRegistry;
    private readonly Dictionary<CommandType, ICommand> _handlersByType;

    public ApplicationHandler(IResultWriter resultWriter,
        ITransactionReader transactionReader, IQueryParser queryParser, Mempool mempool,
        BlockMiner blockMiner, MiningConfig miningConfig, CommandHandlerRegistry commandHandlerRegistry)
    {
        _resultWriter = resultWriter;
        _transactionReader = transactionReader;
        _queryParser = queryParser;
        _mempool = mempool;
        _blockMiner = blockMiner;
        _miningConfig = miningConfig;
        _commandHandlerRegistry = commandHandlerRegistry;
        _handlersByType = new Dictionary<CommandType, ICommand>();
        commandHandlerRegistry.Registry(_handlersByType);
    }

    public void Handle(string query)
    {
        var command = _queryParser.Parse(query);
        _handlersByType[command.Type].Execute(command);
    }
}