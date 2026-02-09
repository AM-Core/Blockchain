using Application;
using Application.MiningApplication;
using Domain.Interfaces;
using DomainService;
using IO;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Bootstrap;

public static class DependencyBootstrapper
{
    public static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddTransient<IResultWriter, ResultWriter>();
        services.AddTransient<ITransactionReader, TransactionReader>();

        services.AddSingleton<Handler>();
        services.AddSingleton<Mempool>();
        return services.BuildServiceProvider();
    }
}