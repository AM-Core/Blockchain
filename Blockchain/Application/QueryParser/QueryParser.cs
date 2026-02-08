using Domain.Command;

namespace Application.QueryParser;

public class QueryParser : IQueryParser
{
    public Command Parse(string command)
    {
        return new Command();
    }

}