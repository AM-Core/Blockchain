using Application.MiningApplication.Abstractions;
using Application.QueryHandler.Command;
using Domain;

namespace Application.MiningApplication.Commands;

public sealed class DifficultyCommand : ICommand
{
    private readonly MiningConfig _miningConfig;

    public DifficultyCommand(MiningConfig miningConfig)
    {
        _miningConfig = miningConfig;
    }

    public void Execute(Command command)
    {
        _miningConfig.Difficulty = long.Parse(command.Argument);
    }
}