using Application.QueryHandler.Command;
using Domain;

namespace Application.MiningApplication;

public sealed class DifficultyApplication : ICommand
{
    private readonly MiningConfig _miningConfig;

    public DifficultyApplication(MiningConfig miningConfig)
    {
        _miningConfig = miningConfig;
    }

    public void Execute(Command command)
    {
        _miningConfig.Difficulty = long.Parse(command.Argument);
    }
}