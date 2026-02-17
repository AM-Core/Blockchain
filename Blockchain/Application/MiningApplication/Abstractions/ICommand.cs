using Application.QueryHandler.Command;

namespace Application.MiningApplication.Abstractions;

public interface ICommand
{
    void Execute(Command command);
}