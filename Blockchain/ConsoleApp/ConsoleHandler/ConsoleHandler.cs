using Application.MiningApplication;

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
}