using Application.MiningApplication.Abstractions;
using Application.MiningApplication.Commands;
using CommandType = Application.QueryHandler.Command.CommandType;

namespace Application.MiningApplication.Dispatching;

public class CommandHandlerRegistry
{
    private readonly DifficultyCommand _difficultyApplication;
    private readonly TransactionCommand _transactionApplication;
    private readonly EvictCommand _evictApplication;
    private readonly BlockCommand _blockApplication;

    public CommandHandlerRegistry(DifficultyCommand difficultyApplication,
        TransactionCommand transactionApplication, EvictCommand evictApplication,
        BlockCommand blockApplication)
    {
        _difficultyApplication = difficultyApplication;
        _transactionApplication = transactionApplication;
        _evictApplication = evictApplication;
        _blockApplication = blockApplication;
    }

    public void Registry(Dictionary<CommandType, ICommand> handlersByType)
    {
        handlersByType.Add(CommandType.SETDIFFICULTY, _difficultyApplication);
        handlersByType.Add(CommandType.ADDTRANSACTIONTOMEMPOOL, _transactionApplication);
        handlersByType.Add(CommandType.EVICTMEMPOOL, _evictApplication);
        handlersByType.Add(CommandType.MINEBLOCK, _blockApplication);
    }

}