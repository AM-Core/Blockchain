namespace Domain.Command;

public class Command
{

    public CommandType Type { get; private set; }
    public string Argument { get; private set; }

    public Command(CommandType type, string argument)
    {
        Type = type;
        Argument = argument;
    }
}