using Domain.Command;

namespace Application.QueryParser;

public interface IQueryParser
{
    Command Parse(string query);
}