using Domain;
using Domain.Interfaces;
using DomainService;

namespace Application.MiningApplication;

public sealed class BlockApplication
{
    public void MineBlock(IResultWriter resultWriter,
        MiningConfig miningConfig, BlockMiner blockMiner, Mempool mempool)
    {
        var transactions = mempool.GetTransactionsSortedToCreateBlock();
        var block = new Block(miningConfig.Difficulty, transactions);
        block = blockMiner.MineBlock(block);
        resultWriter.WriteBlock(block);
    }
}
