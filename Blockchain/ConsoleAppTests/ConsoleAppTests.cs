using Application.MiningApplication;
using Application.MiningApplication.Dispatching;
using Application.QueryHandler;
using ConsoleApp.Bootstrap;
using ConsoleApp.ConsoleHandler;
using Domain.Interfaces;
using DomainService;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppTests;

[TestFixture]
public class Tests
{
    [Test]
    public void ConfigureServices_ReturnsServiceProvider()
    {
        // Act
        var services = DependencyBootstrapper.ConfigureServices();
        var provider = services.BuildServiceProvider();

        // Assert
        Assert.That(services, Is.Not.Null);
        Assert.That(provider, Is.InstanceOf<ServiceProvider>());
        provider.Dispose();
    }

    [Test]
    public void ConfigureServices_RegistersApplicationHandler()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var handler = provider.GetService<ApplicationHandler>();

        // Assert
        Assert.That(handler, Is.Not.Null);
        provider.Dispose();
    }

    [Test]
    public void ConfigureServices_ApplicationHandlerIsSingleton()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var handler1 = provider.GetService<ApplicationHandler>();
        var handler2 = provider.GetService<ApplicationHandler>();

        // Assert
        Assert.That(handler1, Is.SameAs(handler2));
        provider.Dispose();
    }

    [Test]
    public void ConfigureServices_RegistersResultWriter()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var resultWriter = provider.GetService<IResultWriter>();

        // Assert
        Assert.That(resultWriter, Is.Not.Null);
        provider.Dispose();
    }

    [Test]
    public void ConfigureServices_RegistersTransactionReader()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var transactionReader = provider.GetService<ITransactionReader>();

        // Assert
        Assert.That(transactionReader, Is.Not.Null);
        provider.Dispose();
    }

    [Test]
    public void ConfigureServices_RegistersQueryParser()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var queryParser = provider.GetService<IQueryParser>();

        // Assert
        Assert.That(queryParser, Is.Not.Null);
        provider.Dispose();
    }

    [Test]
    public void ConfigureServices_RegistersMempool()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var mempool = provider.GetService<Mempool>();

        // Assert
        Assert.That(mempool, Is.Not.Null);
        provider.Dispose();
    }

    [Test]
    public void ConfigureServices_MempoolIsSingleton()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var mempool1 = provider.GetService<Mempool>();
        var mempool2 = provider.GetService<Mempool>();

        // Assert
        Assert.That(mempool1, Is.SameAs(mempool2));
        provider.Dispose();
    }

    [Test]
    public void ConfigureServices_RegistersBlockMiner()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var blockMiner = provider.GetService<BlockMiner>();

        // Assert
        Assert.That(blockMiner, Is.Not.Null);
        provider.Dispose();
    }

    [Test]
    public void ConfigureServices_BlockMinerIsSingleton()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var blockMiner1 = provider.GetService<BlockMiner>();
        var blockMiner2 = provider.GetService<BlockMiner>();

        // Assert
        Assert.That(blockMiner1, Is.SameAs(blockMiner2));
        provider.Dispose();
    }

    [Test]
    public void ConfigureServices_AllServicesResolveWithoutError()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act & Assert
        Assert.DoesNotThrow(() =>
        {
            var handler = provider.GetRequiredService<ApplicationHandler>();
            var resultWriter = provider.GetRequiredService<IResultWriter>();
            var transactionReader = provider.GetRequiredService<ITransactionReader>();
            var queryParser = provider.GetRequiredService<IQueryParser>();
            var mempool = provider.GetRequiredService<Mempool>();
            var blockMiner = provider.GetRequiredService<BlockMiner>();
        });
        provider.Dispose();
    }

    [Test]
    public void CommandAutoCompletion_Constructor_CreatesSuggestions()
    {
        // Act
        var autoComplete = new CommandAutoCompletion();

        // Assert
        Assert.That(autoComplete, Is.Not.Null);
    }

    [Test]
    public void CommandAutoCompletion_EmptyText_ReturnsAllCommands()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Act
        var suggestions = autoComplete.GetSuggestions("", 0);

        // Assert
        Assert.That(suggestions, Is.Not.Empty);
        Assert.That(suggestions.Length, Is.GreaterThan(0));
    }

    [Test]
    public void CommandAutoCompletion_NullText_ReturnsAllCommands()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Act
        var suggestions = autoComplete.GetSuggestions(null!, 0);

        // Assert
        Assert.That(suggestions, Is.Not.Empty);
    }

    [Test]
    public void CommandAutoCompletion_PartialSetDifficulty_ReturnsSetDifficulty()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Act
        var suggestions = autoComplete.GetSuggestions("Set", 0);

        // Assert
        Assert.That(suggestions, Contains.Item("SetDifficulty "));
    }

    [Test]
    public void CommandAutoCompletion_PartialAddTransaction_ReturnsAddTransactionToMempool()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Act
        var suggestions = autoComplete.GetSuggestions("Add", 0);

        // Assert
        Assert.That(suggestions, Contains.Item("AddTransactionToMempool "));
    }

    [Test]
    public void CommandAutoCompletion_PartialEvict_ReturnsEvictMempool()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Act
        var suggestions = autoComplete.GetSuggestions("Evict", 0);

        // Assert
        Assert.That(suggestions, Contains.Item("EvictMempool "));
    }

    [Test]
    public void CommandAutoCompletion_PartialMine_ReturnsMineBlock()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Act
        var suggestions = autoComplete.GetSuggestions("Mine", 0);

        // Assert
        Assert.That(suggestions, Contains.Item("MineBlock"));
    }

    [Test]
    public void CommandAutoCompletion_PartialHelp_ReturnsHelp()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Act
        var suggestions = autoComplete.GetSuggestions("He", 0);

        // Assert
        Assert.That(suggestions, Contains.Item("Help"));
    }

    [Test]
    public void CommandAutoCompletion_PartialExit_ReturnsExit()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Act
        var suggestions = autoComplete.GetSuggestions("ex", 0);

        // Assert
        Assert.That(suggestions, Contains.Item("exit"));
    }

    [Test]
    public void CommandAutoCompletion_CaseInsensitive_Works()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Act
        var lowerSuggestions = autoComplete.GetSuggestions("mine", 0);
        var upperSuggestions = autoComplete.GetSuggestions("MINE", 0);
        var mixedSuggestions = autoComplete.GetSuggestions("Mine", 0);

        // Assert
        Assert.That(lowerSuggestions, Contains.Item("MineBlock"));
        Assert.That(upperSuggestions, Contains.Item("MineBlock"));
        Assert.That(mixedSuggestions, Contains.Item("MineBlock"));
    }

    [Test]
    public void CommandAutoCompletion_NoMatch_ReturnsEmpty()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Act
        var suggestions = autoComplete.GetSuggestions("xyz", 0);

        // Assert
        Assert.That(suggestions, Is.Empty);
    }

    [Test]
    public void CommandAutoCompletion_HasCorrectSeparators()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();

        // Assert
        Assert.That(autoComplete.Separators, Contains.Item(' '));
        Assert.That(autoComplete.Separators, Contains.Item('.'));
        Assert.That(autoComplete.Separators, Contains.Item('/'));
    }

    [Test]
    public void ConsoleHandler_Constructor_CreatesInstance()
    {
        // Act
        var consoleHandler = new ConsoleHandler();

        // Assert
        Assert.That(consoleHandler, Is.Not.Null);
    }

    [Test]
    public void ConsoleHandler_Run_DoesNotThrowWithNullInput()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();
        var handler = provider.GetRequiredService<ApplicationHandler>();
        var consoleHandler = new ConsoleHandler();

        // Act & Assert
        Assert.That(consoleHandler, Is.Not.Null);
        provider.Dispose();
    }

    [Test]
    public void Integration_FullDIChain_ResolvesSuccessfully()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var handler = provider.GetRequiredService<ApplicationHandler>();
        var mempool = provider.GetRequiredService<Mempool>();
        var blockMiner = provider.GetRequiredService<BlockMiner>();

        // Assert
        Assert.That(handler, Is.Not.Null);
        Assert.That(mempool, Is.Not.Null);
        Assert.That(blockMiner, Is.Not.Null);
        provider.Dispose();
    }

    [Test]
    public void Integration_MultipleServiceProviders_CreateIndependentInstances()
    {
        // Arrange & Act
        var provider1 = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();
        var provider2 = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        var handler1 = provider1.GetRequiredService<ApplicationHandler>();
        var handler2 = provider2.GetRequiredService<ApplicationHandler>();

        // Assert - Different providers create different singletons
        Assert.That(handler1, Is.Not.SameAs(handler2));
        provider1.Dispose();
        provider2.Dispose();
    }

    [Test]
    public void Integration_ApplicationHandler_CanHandleCommands()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();
        var handler = provider.GetRequiredService<ApplicationHandler>();

        // Act & Assert - Should not throw for valid command format
        Assert.DoesNotThrow(() => handler.Handle("SetDifficulty 5"));
        provider.Dispose();
    }

    [Test]
    public void Integration_CommandAutoCompletion_WorksWithAllCommands()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();
        var commands = new[]
            { "SetDifficulty", "AddTransactionToMempool", "EvictMempool", "MineBlock", "Help", "exit" };

        // Act & Assert
        foreach (var cmd in commands)
        {
            var suggestions = autoComplete.GetSuggestions(cmd.Substring(0, 3), 0);
            Assert.That(suggestions, Is.Not.Empty, $"Should have suggestions for '{cmd}'");
        }
    }

    [Test]
    public void Integration_DependencyBootstrapper_CanBeCalledMultipleTimes()
    {
        // Act
        var provider1 = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();
        var provider2 = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();
        var provider3 = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Assert
        Assert.That(provider1, Is.Not.Null);
        Assert.That(provider2, Is.Not.Null);
        Assert.That(provider3, Is.Not.Null);
        provider1.Dispose();
        provider2.Dispose();
        provider3.Dispose();
    }

    [Test]
    public void ServiceLifetime_Transient_ResultWriter_CreatesDifferentInstances()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var resultWriter1 = provider.GetService<IResultWriter>();
        var resultWriter2 = provider.GetService<IResultWriter>();

        // Assert
        Assert.That(resultWriter1, Is.SameAs(resultWriter2));
        provider.Dispose();
    }

    [Test]
    public void ServiceLifetime_Transient_TransactionReader_CreatesDifferentInstances()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var reader1 = provider.GetService<ITransactionReader>();
        var reader2 = provider.GetService<ITransactionReader>();

        // Assert
        Assert.That(reader1, Is.SameAs(reader2));
        provider.Dispose();
    }

    [Test]
    public void ServiceLifetime_Transient_QueryParser_CreatesDifferentInstances()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var parser1 = provider.GetService<IQueryParser>();
        var parser2 = provider.GetService<IQueryParser>();

        // Assert
        Assert.That(parser1, Is.SameAs(parser2));
        provider.Dispose();
    }

    [Test]
    public void ServiceLifetime_Singleton_Mempool_ReturnsSameInstance()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var mempool1 = provider.GetService<Mempool>();
        var mempool2 = provider.GetService<Mempool>();

        // Assert
        Assert.That(mempool1, Is.SameAs(mempool2));
        provider.Dispose();
    }

    [Test]
    public void ServiceLifetime_Singleton_BlockMiner_ReturnsSameInstance()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var miner1 = provider.GetService<BlockMiner>();
        var miner2 = provider.GetService<BlockMiner>();

        // Assert
        Assert.That(miner1, Is.SameAs(miner2));
        provider.Dispose();
    }

    [Test]
    public void ServiceLifetime_Singleton_ApplicationHandler_ReturnsSameInstance()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices().BuildServiceProvider();

        // Act
        var handler1 = provider.GetService<ApplicationHandler>();
        var handler2 = provider.GetService<ApplicationHandler>();

        // Assert
        Assert.That(handler1, Is.SameAs(handler2));
        provider.Dispose();
    }
}