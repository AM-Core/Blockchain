using Application;
using ConsoleApp.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var provider = DependencyBootstrapper.ConfigureServices();
        var application = provider.GetRequiredService<MiningApplication>();
        string command = Console.ReadLine()!;
        
        while (command != "exit")
        {
            application.GetCommand(command);
            command = Console.ReadLine()!;
        }
    }
}