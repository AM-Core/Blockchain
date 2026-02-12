using Application.QueryHandler;
using Application.QueryHandler.Command;
using Domain;
using Domain.Interfaces;
using DomainService;

namespace Application.MiningApplication;

public class ApplicationHandler
{
    private readonly BlockMiner _blockMiner;
    private readonly Mempool _mempool;
    private readonly MiningConfig _miningConfig;
    private readonly IQueryParser _queryParser;
    private readonly IResultWriter _resultWriter;
    private readonly ITransactionReader _transactionReader;

    public ApplicationHandler(IResultWriter resultWriter,
        ITransactionReader transactionReader, IQueryParser queryParser, Mempool mempool,
        BlockMiner blockMiner)
    {
        _resultWriter = resultWriter;
        _transactionReader = transactionReader;
        _queryParser = queryParser;
        _mempool = mempool;
        _blockMiner = blockMiner;
        _miningConfig = MiningConfig.Instance;
    }

    public void Handle(string query)
    {
        var command = _queryParser.Parse(query);
        switch (command.Type)
        {
            case CommandType.ADDTRANSACTIONTOMEMPOOL:
                var transactionApplication = new TransactionApplication();
                transactionApplication
                    .AddTransactionToMempool(command.Argument, _transactionReader, _mempool, _resultWriter);

                break;

            case CommandType.EVICTMEMPOOL:
                var evictApplication = new EvictApplication();
                evictApplication.EvictMempool(Convert.ToInt32(command.Argument), _mempool, _resultWriter);
                break;

            case CommandType.MINEBLOCK:
                var blockApplication = new BlockApplication();
                blockApplication.MineBlock(_resultWriter, _miningConfig, _blockMiner, _mempool);
                break;

            case CommandType.SETDIFFICULTY:
                var difficultyApplication = new DifficultyApplication();
                difficultyApplication.SetDifficulty(Convert.ToInt32(command.Argument), _miningConfig);
                break;
        }
    }
}