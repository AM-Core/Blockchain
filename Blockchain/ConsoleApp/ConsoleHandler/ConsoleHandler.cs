using Application.MiningApplication;
using Application.MiningApplication.Dispatching;

namespace ConsoleApp.ConsoleHandler;

public class ConsoleHandler
{
    public void Run(ApplicationHandler application)
    {
        ConfigureSetting();
        Console.WriteLine("Blockchain CLI - Enter command (type 'exit' or press Ctrl+C to quit):");

        var command = ReadLine.Read(">> ");
        while (command != "exit" && command != null)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                command = ReadLine.Read(">> ");
                continue;
            }

            if (command.ToUpper().StartsWith("HELP"))
            {
                ShowHelp();
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

    private void ConfigureSetting()
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("\nExiting application...");
            Environment.Exit(0);
        };

        ReadLine.HistoryEnabled = true;
        ReadLine.AutoCompletionHandler = new CommandAutoCompletion();
    }

    private void ShowHelp()
    {
        Console.WriteLine("\n=== Blockchain CLI Commands ===\n");
        Console.WriteLine("SetDifficulty {number}");
        Console.WriteLine("  Set the mining difficulty level");
        Console.WriteLine("  Example: SetDifficulty 5\n");

        Console.WriteLine("AddTransactionToMempool {filepath}");
        Console.WriteLine("  Add a transaction from a JSON file to the mempool");
        Console.WriteLine("  Example: AddTransactionToMempool tx.json \n");

        Console.WriteLine("EvictMempool {count}");
        Console.WriteLine("  Evict specified number of transactions from mempool");
        Console.WriteLine("  Example: EvictMempool 10\n");

        Console.WriteLine("MineBlock");
        Console.WriteLine("  Mine a new block with transactions from mempool");
        Console.WriteLine("  Example: MineBlock\n");

        Console.WriteLine("Help");
        Console.WriteLine("  Display this help message\n");

        Console.WriteLine("exit");
        Console.WriteLine("  Exit the application\n");

        Console.WriteLine("=== Keyboard Shortcuts ===");
        Console.WriteLine("arrow keys: Navigate command history");
        Console.WriteLine("Tab: Auto-complete commands");
        Console.WriteLine("Ctrl+C: Exit application\n");
    }
}