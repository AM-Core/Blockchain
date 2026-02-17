using Application.QueryHandler.Command;

namespace Application.MiningApplication;

public interface ICommand
{
    void Execute(Command command);
}