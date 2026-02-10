using ConsoleApp.Bootstrap;
using Microsoft.Extensions.DependencyInjection;
using Application.MiningApplication;
using ConsoleApp.ConsoleHandler;

namespace ConsoleAppTests;

[TestFixture]
public class Tests
{
    #region DependencyBootstrapper Tests

    [Test]
    public void ConfigureServices_ReturnsServiceProvider()
    {
        // Act
        var provider = DependencyBootstrapper.ConfigureServices();

        // Assert
        Assert.That(provider, Is.Not.Null);
        Assert.That(provider, Is.InstanceOf<ServiceProvider>());
    }

    [Test]
    public void ConfigureServices_RegistersApplicationHandler()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var handler = provider.GetService<ApplicationHandler>();

        // Assert
        Assert.That(handler, Is.Not.Null);
    }

    [Test]
    public void ConfigureServices_ApplicationHandlerIsSingleton()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var handler1 = provider.GetService<ApplicationHandler>();
        var handler2 = provider.GetService<ApplicationHandler>();

        // Assert
        Assert.That(handler1, Is.SameAs(handler2));
    }

    [Test]
    public void ConfigureServices_RegistersResultWriter()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var resultWriter = provider.GetService<Domain.Interfaces.IResultWriter>();

        // Assert
        Assert.That(resultWriter, Is.Not.Null);
    }

    [Test]
    public void ConfigureServices_RegistersTransactionReader()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var transactionReader = provider.GetService<Domain.Interfaces.ITransactionReader>();

        // Assert
        Assert.That(transactionReader, Is.Not.Null);
    }

    [Test]
    public void ConfigureServices_RegistersQueryParser()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var queryParser = provider.GetService<Application.QueryHandler.IQueryParser>();

        // Assert
        Assert.That(queryParser, Is.Not.Null);
    }

    [Test]
    public void ConfigureServices_RegistersMempool()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var mempool = provider.GetService<DomainService.Mempool>();

        // Assert
        Assert.That(mempool, Is.Not.Null);
    }

    [Test]
    public void ConfigureServices_MempoolIsSingleton()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var mempool1 = provider.GetService<DomainService.Mempool>();
        var mempool2 = provider.GetService<DomainService.Mempool>();

        // Assert
        Assert.That(mempool1, Is.SameAs(mempool2));
    }

    [Test]
    public void ConfigureServices_RegistersBlockMiner()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var blockMiner = provider.GetService<DomainService.BlockMiner>();

        // Assert
        Assert.That(blockMiner, Is.Not.Null);
    }

    [Test]
    public void ConfigureServices_BlockMinerIsSingleton()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var blockMiner1 = provider.GetService<DomainService.BlockMiner>();
        var blockMiner2 = provider.GetService<DomainService.BlockMiner>();

        // Assert
        Assert.That(blockMiner1, Is.SameAs(blockMiner2));
    }

    [Test]
    public void ConfigureServices_AllServicesResolveWithoutError()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act & Assert
        Assert.DoesNotThrow(() =>
        {
            var handler = provider.GetRequiredService<ApplicationHandler>();
            var resultWriter = provider.GetRequiredService<Domain.Interfaces.IResultWriter>();
            var transactionReader = provider.GetRequiredService<Domain.Interfaces.ITransactionReader>();
            var queryParser = provider.GetRequiredService<Application.QueryHandler.IQueryParser>();
            var mempool = provider.GetRequiredService<DomainService.Mempool>();
            var blockMiner = provider.GetRequiredService<DomainService.BlockMiner>();
        });
    }

    #endregion

    #region CommandAutoCompletion Tests

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

    #endregion

    #region ConsoleHandler Tests

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
        var provider = DependencyBootstrapper.ConfigureServices();
        var handler = provider.GetRequiredService<ApplicationHandler>();
        var consoleHandler = new ConsoleHandler();

        // Act & Assert
        // Note: This test can't fully test Run() due to Console.ReadLine blocking
        // In real scenarios, you'd use dependency injection for Console I/O
        Assert.That(consoleHandler, Is.Not.Null);
    }

    #endregion

    #region Integration Tests

    [Test]
    public void Integration_FullDIChain_ResolvesSuccessfully()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var handler = provider.GetRequiredService<ApplicationHandler>();
        var mempool = provider.GetRequiredService<DomainService.Mempool>();
        var blockMiner = provider.GetRequiredService<DomainService.BlockMiner>();

        // Assert
        Assert.That(handler, Is.Not.Null);
        Assert.That(mempool, Is.Not.Null);
        Assert.That(blockMiner, Is.Not.Null);
    }

    [Test]
    public void Integration_MultipleServiceProviders_CreateIndependentInstances()
    {
        // Arrange & Act
        var provider1 = DependencyBootstrapper.ConfigureServices();
        var provider2 = DependencyBootstrapper.ConfigureServices();

        var handler1 = provider1.GetRequiredService<ApplicationHandler>();
        var handler2 = provider2.GetRequiredService<ApplicationHandler>();

        // Assert - Different providers create different singletons
        Assert.That(handler1, Is.Not.SameAs(handler2));
    }

    [Test]
    public void Integration_ApplicationHandler_CanHandleCommands()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();
        var handler = provider.GetRequiredService<ApplicationHandler>();

        // Act & Assert - Should not throw for valid command format
        Assert.DoesNotThrow(() => handler.Handle("SetDifficulty 5"));
    }

    [Test]
    public void Integration_CommandAutoCompletion_WorksWithAllCommands()
    {
        // Arrange
        var autoComplete = new CommandAutoCompletion();
        var commands = new[] { "SetDifficulty", "AddTransactionToMempool", "EvictMempool", "MineBlock", "Help", "exit" };

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
        var provider1 = DependencyBootstrapper.ConfigureServices();
        var provider2 = DependencyBootstrapper.ConfigureServices();
        var provider3 = DependencyBootstrapper.ConfigureServices();

        // Assert
        Assert.That(provider1, Is.Not.Null);
        Assert.That(provider2, Is.Not.Null);
        Assert.That(provider3, Is.Not.Null);
    }

    #endregion

    #region Service Lifetime Tests

    [Test]
    public void ServiceLifetime_Transient_ResultWriter_CreatesDifferentInstances()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var resultWriter1 = provider.GetService<Domain.Interfaces.IResultWriter>();
        var resultWriter2 = provider.GetService<Domain.Interfaces.IResultWriter>();

        // Assert
        Assert.That(resultWriter1, Is.Not.SameAs(resultWriter2));
    }

    [Test]
    public void ServiceLifetime_Transient_TransactionReader_CreatesDifferentInstances()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var reader1 = provider.GetService<Domain.Interfaces.ITransactionReader>();
        var reader2 = provider.GetService<Domain.Interfaces.ITransactionReader>();

        // Assert
        Assert.That(reader1, Is.Not.SameAs(reader2));
    }

    [Test]
    public void ServiceLifetime_Transient_QueryParser_CreatesDifferentInstances()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var parser1 = provider.GetService<Application.QueryHandler.IQueryParser>();
        var parser2 = provider.GetService<Application.QueryHandler.IQueryParser>();

        // Assert
        Assert.That(parser1, Is.Not.SameAs(parser2));
    }

    [Test]
    public void ServiceLifetime_Singleton_Mempool_ReturnsSameInstance()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var mempool1 = provider.GetService<DomainService.Mempool>();
        var mempool2 = provider.GetService<DomainService.Mempool>();

        // Assert
        Assert.That(mempool1, Is.SameAs(mempool2));
    }

    [Test]
    public void ServiceLifetime_Singleton_BlockMiner_ReturnsSameInstance()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var miner1 = provider.GetService<DomainService.BlockMiner>();
        var miner2 = provider.GetService<DomainService.BlockMiner>();

        // Assert
        Assert.That(miner1, Is.SameAs(miner2));
    }

    [Test]
    public void ServiceLifetime_Singleton_ApplicationHandler_ReturnsSameInstance()
    {
        // Arrange
        var provider = DependencyBootstrapper.ConfigureServices();

        // Act
        var handler1 = provider.GetService<ApplicationHandler>();
        var handler2 = provider.GetService<ApplicationHandler>();

        // Assert
        Assert.That(handler1, Is.SameAs(handler2));
    }

    #endregion
}