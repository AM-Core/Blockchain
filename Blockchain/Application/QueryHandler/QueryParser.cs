using Application.Exceptions;
using Application.QueryHandler.Command;

namespace Application.QueryHandler;

public class QueryParser : IQueryParser
{
    public Command.Command Parse(string query)
    {
        if (query == "") throw new InvalidCommandException();

        var type = query;
        var arg = "";
        if (query.Contains(' '))
        {
            type = query.Substring(0, query.IndexOf(' '));
            arg = query.Substring(query.IndexOf(' ') + 1, query.Length - query.IndexOf(' ') - 1);
        }

        if (Enum.TryParse(type.ToUpper(), out CommandType commandType))
            return new Command.Command(commandType, arg);

        throw new InvalidCommandException();
    }
}