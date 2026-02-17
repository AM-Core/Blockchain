using Application.MiningApplication;
using Application.MiningApplication.Commands;
using Application.MiningApplication.Dispatching;
using Application.QueryHandler;
using Domain;
using Domain.Interfaces;
using DomainService;
using IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ConsoleApp.Bootstrap;

public static class DependencyBootstrapper
{
    public static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<ConsoleHandler.ConsoleHandler>();
        
        services.AddSingleton<IResultWriter, ResultWriter>();
        services.AddSingleton<ITransactionReader, TransactionReader>();
        services.AddSingleton<IQueryParser, QueryParser>();
        
        services.AddSingleton<TransactionCommand>();
        services.AddSingleton<BlockCommand>();
        services.AddSingleton<EvictCommand>();
        services.AddSingleton<DifficultyCommand>();

        services.AddSingleton<HashingHandler>();
        services.AddSingleton<NonceRunner>();
        
        services.AddSingleton<CommandHandlerRegistry>();
        services.AddSingleton<ApplicationHandler>();
        services.AddSingleton<Mempool>();
        services.AddSingleton<BlockMiner>();
        services.AddSingleton<MiningConfig>();
        services.AddSingleton<LoadConfiguration>();
        services.AddSingleton<TransactionSizeCalculator>();
        services.AddSingleton<FeeRateCalculator>();
        
        return services.BuildServiceProvider();
    }
}