using Application.QueryHandler;
using Domain;
using Domain.Command;
using Domain.Interfaces;
using DomainService;

namespace Application.MiningApplication;

public class Handler
{
    private readonly IResultWriter _resultWriter;
    private readonly ITransactionReader _transactionReader;
    private readonly IQueryParser _queryParser;
    private readonly MiningConfig _miningConfig;
    private readonly BlockMiner _blockMiner;
    private readonly Mempool _mempool;

    public Handler(IResultWriter resultWriter,
        ITransactionReader transactionReader,IQueryParser queryParser)
    {
        _resultWriter = resultWriter;
        _transactionReader = transactionReader;
        _queryParser = queryParser;
        _miningConfig = new MiningConfig();
        _blockMiner = new BlockMiner();
        _mempool = new Mempool();
    }

    public void Handle(string query)
    {
        Command command = _queryParser.Parse(query);
        switch (command.Type)
        {
            case CommandType.ADDTRANSACTIONTOMEMPOOL:
                var transactionApplication = new TransactionApplication();
                transactionApplication
                    .AddTransactionToMempool(command.Argument, _transactionReader, _mempool);

                break;

            case CommandType.EVICTMEMPOOL:
                var evictApplication = new EvictApplication();
                evictApplication.EvictMempool(Convert.ToInt32(command.Argument), _mempool);
                break;

            case CommandType.MINEBLOCK:
                var blockApplication = new BlockApplication();
                blockApplication.MineBlock(_resultWriter, _miningConfig, _blockMiner, _mempool);
                break;

            case CommandType.SETDIFFICULTY:
                var difficultyApplication = new DifficultyApplication();
                difficultyApplication.SetDifficulty(Convert.ToInt32(command.Argument), _miningConfig);
                break;
            case CommandType.HELP:
                ShowHelp();
                break;
        }
    }
    private void ShowHelp()
    {
        Console.WriteLine("\n=== Blockchain CLI Commands ===\n");
        Console.WriteLine("SetDifficulty(number)");
        Console.WriteLine("  Set the mining difficulty level");
        Console.WriteLine("  Example: SetDifficulty(5)\n");

        Console.WriteLine("AddTransactionToMempool(filepath)");
        Console.WriteLine("  Add a transaction from a JSON file to the mempool");
        Console.WriteLine("  Example: AddTransactionToMempool(tx.json)\n");

        Console.WriteLine("EvictMempool(count)");
        Console.WriteLine("  Evict specified number of transactions from mempool");
        Console.WriteLine("  Example: EvictMempool(10)\n");

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