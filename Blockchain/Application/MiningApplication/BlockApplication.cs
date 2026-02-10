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
        var block = blockMiner.MineBlock(miningConfig);
        resultWriter.WriteBlock(block);
    }
}