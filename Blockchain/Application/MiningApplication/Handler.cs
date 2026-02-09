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
        ITransactionReader transactionReader)
    {
        _resultWriter = resultWriter;
        _transactionReader = transactionReader;
        _queryParser = new QueryParser();
        _miningConfig = new MiningConfig();
        _blockMiner = new BlockMiner();
        _mempool = new Mempool();
    }

    public void Handle(string query)
    {
        Command command = _queryParser.Parse(query);
        switch (command.Type)
        {
            case CommandType.ADD_TRANSACTION_TO_MEMPOOL:
                var transactionApplication = new TransactionApplication();
                transactionApplication
                    .AddTransactionToMempool(command.Argument, _transactionReader, _mempool);

                break;

            case CommandType.EVICT_MEMPOOL:
                var evictApplication = new EvictApplication();
                evictApplication.EvictMempool(Convert.ToInt32(command.Argument), _mempool);
                break;

            case CommandType.MINE_BLOCK:
                var blockApplication = new BlockApplication();
                blockApplication.MineBlock(_resultWriter, _miningConfig, _blockMiner, _mempool);
                break;

            case CommandType.SET_DIFFICULTY:
                var difficultyApplication = new DifficultyApplication();
                difficultyApplication.SetDifficulty(Convert.ToInt32(command.Argument), _miningConfig);
                break;
        }
    }
}