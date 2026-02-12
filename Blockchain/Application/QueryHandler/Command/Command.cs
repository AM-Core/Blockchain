namespace Application.QueryHandler.Command;

public class Command
{
    public Command(CommandType type, string argument)
    {
        Type = type;
        Argument = argument;
    }

    public CommandType Type { get; private set; }
    public string Argument { get; private set; }
}