namespace Domain.Command;

public class Command
{
    public CommandType Type { get; set; }
    public object? Payload { get; set; }

    public int? GetIntPayload()
    {
        return Payload is int i ? i : null;
    }

    public string? GetStringPayload()
    {
        return Payload is string s ? s : null;
    }
}