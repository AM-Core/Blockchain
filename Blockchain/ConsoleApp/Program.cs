using Application;
using Application.MiningApplication;
using ConsoleApp.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var provider = DependencyBootstrapper.ConfigureServices();
        var application = provider.GetRequiredService<Handler>();
        string command = Console.ReadLine()!;
        
        while (command != "exit")
        {
            application.Handle(command);
            command = Console.ReadLine()!;
        }
    }
}

