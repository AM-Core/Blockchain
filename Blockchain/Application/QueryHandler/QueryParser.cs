using Application.Exceptions;
using Domain.Command;

namespace Application.QueryHandler;

public class QueryParser : IQueryParser
{
    public Command Parse(string query)
    {
        if (query == "")
        {
            throw new InvalidCommandException();
        }

        string type = query;
        string arg = "";
        if (query.Contains('('))
        {
            type = query.Substring(0, query.IndexOf('('));
            arg = query.Substring(query.IndexOf('(') + 1, query.IndexOf(')') - query.IndexOf('(') - 1);
        }

        if (Enum.TryParse(type.ToUpper(), out CommandType commandType))
            return new Command(commandType, arg);
        
        throw new InvalidCommandException();
    }
}