namespace Application.QueryHandler;

public interface IQueryParser
{
    Command.Command Parse(string query);
}