namespace ConsoleApp.ConsoleHandler;

public class CommandAutoCompletion : IAutoCompleteHandler
{
    private readonly string[] _commands =
    {
        "SetDifficulty(",
        "AddTransactionToMempool(",
        "EvictMempool(",
        "MineBlock",
        "Help",
        "exit"
    };

    public char[] Separators { get; set; } = { ' ', '.', '/' };

    public string[] GetSuggestions(string text, int index)
    {
        if (string.IsNullOrWhiteSpace(text))
            return _commands;

        return _commands.Where(cmd => cmd.StartsWith
            (text, StringComparison.OrdinalIgnoreCase)).ToArray();
    }
}