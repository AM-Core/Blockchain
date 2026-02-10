using Application.MiningApplication;
using ConsoleApp.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("\nExiting application...");
            Environment.Exit(0);
        };

        var provider = DependencyBootstrapper.ConfigureServices();
        var application = provider.GetRequiredService<Handler>();

        Console.WriteLine("Blockchain CLI - Enter command (type 'exit' or press Ctrl+C to quit):");

        ReadLine.HistoryEnabled = true;
        ReadLine.AutoCompletionHandler = new CommandAutoCompletion();

        string? command = ReadLine.Read(">> ");
        while (command != "exit" && command != null)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                command = ReadLine.Read(">> ");
                continue;
            }

            try
            {
                ReadLine.AddHistory(command); 
                application.Handle(command);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
            command = ReadLine.Read(">> ");
        }

        Console.WriteLine("Goodbye!");
    }
}

// Optional: Add auto-completion for commands
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

        return _commands.Where(cmd => cmd.StartsWith(text, StringComparison.OrdinalIgnoreCase)).ToArray();
    }
}

