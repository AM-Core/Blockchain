using Application.MiningApplication;
using ConsoleApp.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        var provider = DependencyBootstrapper.ConfigureServices();
        var application = provider.GetRequiredService<ApplicationHandler>();
        var consoleHandler = new ConsoleHandler.ConsoleHandler();

        consoleHandler.Run(application);
    }
}