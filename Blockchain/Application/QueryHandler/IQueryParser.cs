using Domain.Command;

namespace Application.QueryHandler;

public interface IQueryParser
{
    Command Parse(string query);
}