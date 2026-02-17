using System.Data;
using CommandType = Application.QueryHandler.Command.CommandType;

namespace Application.MiningApplication;

public class CommandHandlerRegistry
{
    private readonly DifficultyApplication _difficultyApplication;
    private readonly TransactionApplication _transactionApplication;
    private readonly EvictApplication _evictApplication;
    private readonly BlockApplication _blockApplication;

    public CommandHandlerRegistry(DifficultyApplication difficultyApplication,
        TransactionApplication transactionApplication, EvictApplication evictApplication,
        BlockApplication blockApplication)
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