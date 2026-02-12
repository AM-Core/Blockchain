using Application.MiningApplication;
using Application.QueryHandler;
using Domain;
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
        services.AddTransient<IQueryParser, QueryParser>();
        
        services.AddSingleton<ApplicationHandler>();
        services.AddSingleton<Mempool>();
        services.AddSingleton<BlockMiner>();
        services.AddSingleton<MiningConfig>();
        
        return services.BuildServiceProvider();
    }
}